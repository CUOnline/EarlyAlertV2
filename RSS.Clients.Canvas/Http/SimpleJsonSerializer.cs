using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using RSS.Clients.Canvas.Interfaces.Http;
using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Reflection;

namespace RSS.Clients.Canvas.Http
{
    public class SimpleJsonSerializer : IJsonSerializer
    {
        static readonly CanvasSerializerStrategy _serializationStrategy = new CanvasSerializerStrategy();

        public string Serialize(object item)
        {
            return SimpleJson.SerializeObject(item, _serializationStrategy);
        }

        public T Deserialize<T>(string json)
        {
            return SimpleJson.DeserializeObject<T>(json, _serializationStrategy);
        }

        internal static string SerializeEnum(Enum value)
        {
            return _serializationStrategy.SerializeEnumHelper(value).ToString();
        }

        internal static object DeserializeEnum(string value, Type type)
        {
            return _serializationStrategy.DeserializeEnumHelper(value, type);
        }

        class CanvasSerializerStrategy : PocoJsonSerializerStrategy
        {
            readonly List<string> _membersWhichShouldPublishNull = new List<string>();
            ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> _cachedEnums = new ConcurrentDictionary<Type, ConcurrentDictionary<object, object>>();

            protected override string MapClrMemberToJsonFieldName(MemberInfo member)
            {
                return member.GetJsonFieldName();
            }

            internal override IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
            {
                var propertiesAndFields = type.GetPropertiesAndFields().Where(p => p.CanSerialize).ToList();

                foreach (var property in propertiesAndFields.Where(p => p.SerializeNull))
                {
                    var key = type.FullName + "-" + property.JsonFieldName;

                    _membersWhichShouldPublishNull.Add(key);
                }

                return propertiesAndFields
                    .ToDictionary(
                        p => p.JsonFieldName,
                        p => p.GetDelegate);
            }

            // This is overridden so that null values are omitted from serialized objects.
            protected override bool TrySerializeUnknownTypes(object input, out object output)
            {
                var type = input.GetType();
                var getters = GetCache[type];

                if (ReflectionUtils.IsStringEnumWrapper(type))
                {
                    // Handle StringEnum<T> by getting the underlying enum value, then using the enum serializer
                    // Note this will throw if the StringEnum<T> was initialised using a string that is not a valid enum member
                    var inputEnum = (getters["value"](input) as Enum);
                    if (inputEnum != null)
                    {
                        output = SerializeEnum(inputEnum);
                        return true;
                    }
                }

                var jsonObject = new JsonObject();
                foreach (var getter in getters)
                {
                    if (getter.Value != null)
                    {
                        var value = getter.Value(input);
                        if (value == null)
                        {
                            var key = type.FullName + "-" + getter.Key;
                            if (!_membersWhichShouldPublishNull.Contains(key))
                                continue;
                        }
                        jsonObject.Add(getter.Key, value);
                    }
                }
                output = jsonObject;
                return true;
            }

            internal object SerializeEnumHelper(Enum p)
            {
                return SerializeEnum(p);
            }

            protected override object SerializeEnum(Enum p)
            {
                return p.ToParameter();
            }

            internal object DeserializeEnumHelper(string value, Type type)
            {
                var cachedEnumsForType = _cachedEnums.GetOrAdd(type, t =>
                {
                    var enumsForType = new ConcurrentDictionary<object, object>();

                    // Try to get all custom attributes, this happens only once per type
                    var fields = type.GetRuntimeFields();
                    foreach (var field in fields)
                    {
                        if (field.Name == "value__")
                            continue;
                        var attribute = (ParameterAttribute)field.GetCustomAttribute(typeof(ParameterAttribute));
                        if (attribute != null)
                        {
                            enumsForType.GetOrAdd(attribute.Value, _ => field.GetValue(null));
                        }
                    }

                    return enumsForType;
                });

                // If type cache does not contain enum value and has no custom attribute, add it for future loops
                return cachedEnumsForType.GetOrAdd(value, v => Enum.Parse(type, value, ignoreCase: true));
            }

            //private string _type;

            // Overridden to handle enums.
            public override object DeserializeObject(object value, Type type)
            {
                var stringValue = value as string;
                var jsonValue = value as JsonObject;

                if (stringValue != null)
                {
                    // If it's a nullable type, use the underlying type
                    if (ReflectionUtils.IsNullableType(type))
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }

                    var typeInfo = ReflectionUtils.GetTypeInfo(type);

                    if (typeInfo.IsEnum)
                    {
                        return DeserializeEnumHelper(stringValue, type);
                    }

                    if (ReflectionUtils.IsTypeGenericeCollectionInterface(type))
                    {
                        // OAuth tokens might be a string of comma-separated values
                        // we should only try this if the return array is a collection of strings
                        var innerType = ReflectionUtils.GetGenericListElementType(type);
                        if (innerType.IsAssignableFrom(typeof(string)))
                        {
                            return stringValue.Split(',');
                        }
                    }

                    if (ReflectionUtils.IsStringEnumWrapper(type))
                    {
                        return Activator.CreateInstance(type, stringValue);
                    }
                }
                //else if (jsonValue != null)
                //{
                //    if (type == typeof(Activity))
                //    {
                //        _type = jsonValue["type"].ToString();
                //    }
                //}

                //if (type == typeof(ActivityPayload))
                //{
                //    var payloadType = GetPayloadType(_type);
                //    return base.DeserializeObject(value, payloadType);
                //}

                return base.DeserializeObject(value, type);
            }

            internal override IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
            {
                return type.GetPropertiesAndFields()
                    .Where(p => p.CanDeserialize)
                    .ToDictionary(
                        p => p.JsonFieldName,
                        p => new KeyValuePair<Type, ReflectionUtils.SetDelegate>(p.Type, p.SetDelegate));
            }

            //private static Type GetPayloadType(string activityType)
            //{
            //    switch (activityType)
            //    {
            //        case "CommitCommentEvent":
            //            return typeof(CommitCommentPayload);
            //        case "ForkEvent":
            //            return typeof(ForkEventPayload);
            //        case "IssueCommentEvent":
            //            return typeof(IssueCommentPayload);
            //        case "IssuesEvent":
            //            return typeof(IssueEventPayload);
            //        case "PullRequestEvent":
            //            return typeof(PullRequestEventPayload);
            //        case "PullRequestReviewEvent":
            //            return typeof(PullRequestReviewEventPayload);
            //        case "PullRequestReviewCommentEvent":
            //            return typeof(PullRequestCommentPayload);
            //        case "PushEvent":
            //            return typeof(PushEventPayload);
            //        case "StatusEvent":
            //            return typeof(StatusEventPayload);
            //        case "WatchEvent":
            //            return typeof(StarredEventPayload);
            //    }
            //    return typeof(ActivityPayload);
            //}
        }
    }
}

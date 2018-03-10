using RSS.Clients.Canvas.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RSS.Clients.Canvas.Helpers
{
    internal static class ReflectionExtensions
    {
        public static string GetJsonFieldName(this MemberInfo memberInfo)
        {
            var memberName = memberInfo.Name;
            var paramAttr = memberInfo.GetCustomAttribute<ParameterAttribute>();

            if (paramAttr != null && !string.IsNullOrEmpty(paramAttr.Key))
            {
                memberName = paramAttr.Key;
            }

            return memberName.ToRubyCase();
        }

        public static IEnumerable<PropertyOrField> GetPropertiesAndFields(this Type type)
        {
            return ReflectionUtils.GetProperties(type).Select(property => new PropertyOrField(property))
                .Union(ReflectionUtils.GetFields(type).Select(field => new PropertyOrField(field)))
                .Where(p => (p.IsPublic || p.HasParameterAttribute) && !p.IsStatic);
        }

        public static bool IsDateTimeOffset(this Type type)
        {
            return type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?);
        }

        public static bool IsNullable(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public static bool IsEnumeration(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
    }
}

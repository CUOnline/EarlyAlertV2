var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
((window) => {
    let $ = window.jQuery;
    let Vue = window.Vue;
    let axios = window.axios;
    let _ = window._;
    let webApiUrl = window.webApiUrl;
    let reportId = window.location.href.substr(window.location.href.lastIndexOf('=') + 1);
    let app = new Vue({
        el: '#app',
        data: {
            users: [],
            refreshProgress: 0
        },
        mounted: function () {
            return __awaiter(this, void 0, void 0, function* () {
                // Get the users.
                this.users = (yield axios.get('/Reports/GetStudents?reportId=' + reportId)).data;
                // Get user risk indicies
                for (let i = 0; i < this.users.length; ++i) {
                    this.users[i].riskIndex = (yield axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[i].id)).data;
                }
            });
        },
        methods: {
            RefreshData: function () {
                return __awaiter(this, void 0, void 0, function* () {
                    this.refreshProgress = 0;
                    var pleaseWait = $('#pleaseWaitDialog');
                    pleaseWait.modal('show');
                    var minProgress = 0;
                    var totalProgress = this.users.length * 6;
                    for (let i = 0; i < this.users.length; ++i) {
                        var courses = yield axios.get('/Reports/UpdateStudentCourses?studentId=' + this.users[i].id);
                        this.refreshProgress = (minProgress++ / totalProgress) * 100;
                        for (let j = 0; j < courses.data.length; ++j) {
                            yield axios.get('/Reports/UpdateAssignmentsForCourse?courseId=' + courses.data[j].id);
                        }
                        this.refreshProgress = (minProgress++ / totalProgress) * 100;
                        yield axios.get('/Reports/UpdateStudentSubmissionsForCourse?studentId=' + this.users[i].id);
                        this.refreshProgress = (minProgress++ / totalProgress) * 100;
                        yield axios.get('/Reports/UpdateStudentGrades?studentId=' + this.users[i].id);
                        this.refreshProgress = (minProgress++ / totalProgress) * 100;
                        yield axios.get('/Reports/UpdateStudentActivity?studentId=' + this.users[i].id);
                        this.refreshProgress = (minProgress++ / totalProgress) * 100;
                    }
                    // Get user risk indicies
                    for (let i = 0; i < this.users.length; ++i) {
                        this.users[i].riskIndex = (yield axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[i].id)).data;
                        this.refreshProgress = (minProgress++ / totalProgress) * 100;
                    }
                    pleaseWait.modal('hide');
                });
            },
            viewStudentProfile(id) {
                window.location.href = "/Reports/StudentProfile/" + id;
            }
        },
        computed: {
            orderedUsers: function () {
                return _.orderBy(this.users, 'riskIndex', 'desc');
            }
        },
        filters: {
            'formatDate': function (value) {
                if (!value)
                    return '';
                let date = new Date(value);
                // get time
                let hours = date.getHours();
                let minutes = date.getMinutes();
                hours = hours % 12;
                hours = hours ? hours : 12;
                let strTime = `${hours}:${minutes < 10 ? '0' + minutes : minutes}${hours >= 12 ? 'pm' : 'am'}`;
                return `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()} ${strTime}`;
            },
            'formatRiskIndex': function (value) {
                return value.toFixed(2);
            }
        }
    });
})(window);
//# sourceMappingURL=RiskIndex.js.map
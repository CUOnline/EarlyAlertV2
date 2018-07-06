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
            users: [
                { id: 'loading...' }
            ],
            refreshProgress: 0,
            iterProgress: 0,
            maxProgress: 0,
            loading: false
        },
        mounted: function () {
            return __awaiter(this, void 0, void 0, function* () {
                // Get the users.
                this.users = (yield axios.get('/Reports/GetStudents?reportId=' + reportId)).data;
                // Get user risk indicies
                for (let i = 0; i < this.users.length; ++i) {
                    try {
                        this.users[i].riskIndex = (yield axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[i].id)).data;
                    }
                    catch (error) {
                        this.users[i].hasError = true;
                        this.users[i].error = "There was an error loading data for this user.  Clicking the reload button on the left of this row may clear up the error. \n" + error;
                    }
                }
            });
        },
        methods: {
            refreshData: function () {
                return __awaiter(this, void 0, void 0, function* () {
                    this.loading = true;
                    this.refreshProgress = 0;
                    var pleaseWait = $('#pleaseWaitDialog');
                    pleaseWait.modal('show');
                    this.iterProgress = 0;
                    this.maxProgress = this.users.length * 6;
                    for (let i = 0; i < this.users.length; ++i) {
                        yield this.refreshUserData(this.users[i].id, true);
                    }
                    pleaseWait.modal('hide');
                    this.loading = false;
                });
            },
            refreshUserData: function (userId, supressModal) {
                return __awaiter(this, void 0, void 0, function* () {
                    var index = _.findIndex(this.users, x => x.id == userId);
                    if (!supressModal) {
                        this.loading = true;
                        this.refreshProgress = 0;
                        this.iterProgress = 0;
                        this.maxProgress = 6;
                        var pleaseWait = $('#pleaseWaitDialog');
                        pleaseWait.modal('show');
                    }
                    try {
                        var courses = yield axios.get('/Reports/UpdateStudentCourses?studentId=' + this.users[index].id);
                        this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;
                        for (let j = 0; j < courses.data.length; ++j) {
                            yield axios.get('/Reports/UpdateAssignmentsForCourse?courseId=' + courses.data[j].id);
                        }
                        this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;
                        yield axios.get('/Reports/UpdateStudentSubmissionsForCourse?studentId=' + this.users[index].id);
                        this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;
                        yield axios.get('/Reports/UpdateStudentGrades?studentId=' + this.users[index].id);
                        this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;
                        yield axios.get('/Reports/UpdateStudentActivity?studentId=' + this.users[index].id);
                        this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;
                        this.users[index].riskIndex = (yield axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[index].id)).data;
                        this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;
                    }
                    catch (error) {
                        console.log(error);
                        this.users[index].hasError = true;
                        this.users[index].error = "There was an error loading data for this user.  Clicking the reload button on the left of this row may clear up the error. \n" + error;
                    }
                    if (!supressModal) {
                        pleaseWait.modal('hide');
                        this.loading = false;
                    }
                });
            },
            viewProgress() {
                var pleaseWait = $('#pleaseWaitDialog');
                pleaseWait.modal('show');
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
                if (!value)
                    return '';
                return value.toFixed(2);
            }
        }
    });
})(window);

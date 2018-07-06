((window: any) => {
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
        mounted: async function () {
            // Get the users.
            this.users = (await axios.get('/Reports/GetStudents?reportId=' + reportId)).data;

            // Get user risk indicies
            for (let i = 0; i < this.users.length; ++i) {
                try {
                    this.users[i].riskIndex = (await axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[i].id)).data;
                }
                catch (error) {
                    this.users[i].hasError = true;
                    this.users[i].error = "There was an error loading data for this user.  Clicking the reload button on the left of this row may clear up the error. \n" + error;
                }
            }
        },
        methods: {
            refreshData: async function () {
                this.loading = true;
                this.refreshProgress = 0;
                var pleaseWait = $('#pleaseWaitDialog'); 
                pleaseWait.modal('show');

                this.iterProgress = 0;
                this.maxProgress = this.users.length * 6;

                for (let i = 0; i < this.users.length; ++i) {
                    await this.refreshUserData(this.users[i].id, true);
                }

                pleaseWait.modal('hide');
                this.loading = false;
            },
            refreshUserData: async function (userId, supressModal) {
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
                    var courses = await axios.get('/Reports/UpdateStudentCourses?studentId=' + this.users[index].id);
                    this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;

                    for (let j = 0; j < courses.data.length; ++j) {
                        await axios.get('/Reports/UpdateAssignmentsForCourse?courseId=' + courses.data[j].id);
                    }
                    this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;

                    await axios.get('/Reports/UpdateStudentSubmissionsForCourse?studentId=' + this.users[index].id);
                    this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;

                    await axios.get('/Reports/UpdateStudentGrades?studentId=' + this.users[index].id);
                    this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;

                    await axios.get('/Reports/UpdateStudentActivity?studentId=' + this.users[index].id);
                    this.refreshProgress = (this.iterProgress++ / this.maxProgress) * 100;

                    this.users[index].riskIndex = (await axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[index].id)).data;
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
                if (!value) return '';
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
                if (!value) return '';
                return value.toFixed(2);
            }
        }
    });
})(window);
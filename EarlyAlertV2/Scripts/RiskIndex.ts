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
            users: [],
            refreshProgress: 0
        },
        mounted: async function () {

            // Get the users.
            this.users = (await axios.get('/Reports/GetStudents?reportId=' + reportId)).data;

            // Get user risk indicies
            for (let i = 0; i < this.users.length; ++i) {
                this.users[i].riskIndex = (await axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[i].id)).data;
            }
        },
        methods: {
            RefreshData: async function () {

                this.refreshProgress = 0;
                var pleaseWait = $('#pleaseWaitDialog'); 
                pleaseWait.modal('show');

                var minProgress = 0;
                var totalProgress = this.users.length * 6;

                for (let i = 0; i < this.users.length; ++ i) {
                    var courses = await axios.get('/Reports/UpdateStudentCourses?studentId=' + this.users[i].id);
                    this.refreshProgress = (minProgress++ / totalProgress) * 100;

                    for (let j = 0; j < courses.data.length; ++j) {
                        await axios.get('/Reports/UpdateAssignmentsForCourse?courseId=' + courses.data[j].id);
                    }
                    this.refreshProgress = (minProgress++ / totalProgress) * 100;

                    await axios.get('/Reports/UpdateStudentSubmissionsForCourse?studentId=' + this.users[i].id);
                    this.refreshProgress = (minProgress++ / totalProgress) * 100;

                    await axios.get('/Reports/UpdateStudentGrades?studentId=' + this.users[i].id);
                    this.refreshProgress = (minProgress++ / totalProgress) * 100;

                    await axios.get('/Reports/UpdateStudentActivity?studentId=' + this.users[i].id);
                    this.refreshProgress = (minProgress++ / totalProgress) * 100;
                }

                // Get user risk indicies
                for (let i = 0; i < this.users.length; ++i) {
                    this.users[i].riskIndex = (await axios.get('/Reports/GetStudentRiskIndex?userId=' + this.users[i].id)).data;
                    this.refreshProgress = (minProgress++ / totalProgress) * 100;
                }

                pleaseWait.modal('hide');
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
                return value.toFixed(2);
            }
        }
    });
})(window);
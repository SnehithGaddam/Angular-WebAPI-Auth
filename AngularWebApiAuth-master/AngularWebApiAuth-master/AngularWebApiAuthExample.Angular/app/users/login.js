(function () {
    'use strict';

    var controllerId = 'login';

    angular
        .module('app')
        .controller(controllerId, login);

    login.$inject = ['authenticator', 'common'];

    function login(authenticator, common) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'login';
        vm.loginData = {
            userName: '',
            password: ''
        };
        vm.message = '';
        vm.loginUser = loginUser;
        vm.useAdmin = useAdmin;
        vm.useUser = useUser;

        activate();

        function activate() {
            common.activateController([], controllerId).then(function () {
            });
        }

        function useAdmin() {
            vm.loginData.userName = 'admin@example.com';
            vm.loginData.password = 'Admin@123456';
            loginUser();
        }

        function useUser() {
            vm.loginData.userName = 'user@example.com';
            vm.loginData.password = 'User@123456';
            loginUser();
        }

        function loginUser() {
            authenticator.login(vm.loginData)
                .then(function (response) {
                    common.logger.logSuccess('Welcome to our world ' + authenticator.authData.userName, true);
                    common.$location.path('/');
                }, function (error) {
                    vm.message = error.error_description;
                });
        }
    }
})();

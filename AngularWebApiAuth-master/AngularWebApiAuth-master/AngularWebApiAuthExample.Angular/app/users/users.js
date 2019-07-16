(function () {
    'use strict';

    var controllerId = 'users';
    angular
        .module('app')
        .controller(controllerId, users);

    users.$inject = ['dataservice', 'common'];

    function users(dataservice, common) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'users';
        vm.users = [];
        vm.viewDetail = viewDetail;

        activate();

        function activate() {
            var promises = [getPeople()];
            common.activateController(promises, controllerId)
                .then(function (data) {

                });
        }

        function getPeople() {
            return dataservice.getPeople().then(function(response) {
                vm.users = response.data;
                return response;
            }, function (error) {
                common.logger.logError('Could not get the people. ' + common.jsonMessage(error), true);
                return false;
            });
        }

        function viewDetail(person) {
            if (person && person.id) {
                common.$location.path('/users/detail/' + person.id);
            }
        }
    }
})();

(function () {
    'use strict';

    var controllerId = 'userdetail';

    angular
        .module('app')
        .controller(controllerId, userdetail);

    userdetail.$inject = ['$routeParams', 'common', 'dataservice']; 

    function userdetail($routeParams, common, dataservice) {
        var personId = $routeParams.id || 0;

        /* jshint validthis:true */
        var vm = this;
        vm.title = 'userdetail';
        vm.person = undefined;
        

        activate();

        function activate() {
            common.activateController([getPerson()], controllerId).then(function () {

            });
        }

        function getPerson() {
            return dataservice.getPerson(personId).then(function (response) {
                vm.person = response.data;
                return response;
            },function(error) {
                common.logger.logError('Could not get the person. ' + common.jsonMessage(error), true);
                return false;
            });
        }
    }
})();

(function () {
    'use strict';

    var controllerId = 'about';

    angular
        .module('app')
        .controller(controllerId, about);

    about.$inject = ['dataservice', 'common']; 

    function about(dataservice, common) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'about';
        vm.getPeople = getPeople;

        activate();

        function activate() {
            common.activateController([], controllerId).then(function () {
            });
        }

        function getPeople() {
            return dataservice.getPeople().then(function (response) {
                vm.users = response.data;
                return response;
            }, function (error) {
                common.logger.logError('Could not get the people. ' + common.jsonMessage(error), true);
                return false;
            });
        }
    }
})();

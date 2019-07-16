(function () {
    'use strict';

    var controllerId = 'notauthorized';
    angular
        .module('app')
        .controller(controllerId, notauthorized);

    notauthorized.$inject = ['common'];

    function notauthorized(common) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'notauthorized';

        activate();

        function activate() {
            common.activateController([], controllerId).then(function () {
                
            });
        }
    }
})();

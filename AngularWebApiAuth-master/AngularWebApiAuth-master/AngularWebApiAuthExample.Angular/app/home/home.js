(function () {
    'use strict';

    var controllerId = 'home';

    angular
        .module('app')
        .controller(controllerId, home);

    home.$inject = ['common']; 

    function home(common) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'home';

        activate();

        function activate() {
            common.activateController([], controllerId).then(function () {
            });
        }
    }
})();

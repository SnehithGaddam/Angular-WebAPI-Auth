(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());

    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);

    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            //$routeProvider.when(r.url, r.config);
            setRoute(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });

        function setRoute(url, config) {
            //Sets resolver for all the routes
            //by extending any existing resolvers (or create a new one.)
            config.resolve = angular.extend(config.resolve || {},
            {
                checkSecurity: checkSecurity
            });

            $routeProvider.when(url, config);
            return $routeProvider;
        }
    }

    checkSecurity.$inject = ['$route', 'authenticator', 'common'];
    function checkSecurity($route, authenticator, common) {
        var deferred = common.$q.defer();
        authenticator.fillData().then(function() {
            var settings = $route.current.settings;
            var loginRequired = settings.loginRequired || false;
            var roles = settings.roles || [];
            if (loginRequired) {
                if (!authenticator.authData.isAuth) {
                    common.$location.path('/login');
                } else {
                    if (roles.length > 0) {
                        if (!common.checkRole(authenticator.authData.roles, roles)) {
                            common.$location.path('/notauthorized').replace();
                        }
                    }
                }
            }
            deferred.resolve(true); //We want to return just true even if we have to re-route. 
                                    //If we returned an reject, the the global handler will re-route us to home
        }, function(error) {
            deferred.reject(error);
        });

        return deferred.promise;               
    }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/home/home.html',
                    title: 'home',
                    settings: {
                        nav: 1,
                        loginRequired: false,
                        roles: [],
                        content: '<i class="fa fa-home"></i> Home'
                    }
                }
            }, {
                url: '/about',
                config: {
                    templateUrl: 'app/home/about.html',
                    title: 'about',
                    settings: {
                        nav: 2,
                        loginRequired: false,
                        roles: [],
                        content: '<i class="fa fa-building"></i> About'
                    }
                }
            }, {
                url: '/users',
                config: {
                    title: 'users',
                    templateUrl: 'app/users/users.html',
                    settings: {
                        nav: 3,
                        loginRequired: true,
                        roles: ['Admin', 'User'],
                        content: '<i class="fa fa-users"></i> People'
                    }
                }
            }, {
                url: '/users/detail/:id',
                config: {
                    title: 'user detail',
                    templateUrl: 'app/users/userdetail.html',
                    settings: {
                        loginRequired: true,
                        roles: ['User']
                    }
                }
            }, {
                url: '/login',
                config: {
                    title: 'login',
                    templateUrl: 'app/users/login.html',
                    settings: {                        
                    }
                }
            }, {
                url: '/notauthorized',
                config: {
                    title: 'not authorized',
                    templateUrl: 'app/home/notauthorized.html',
                    settings: {
                    }
                }
            }
        ];
    }
})();
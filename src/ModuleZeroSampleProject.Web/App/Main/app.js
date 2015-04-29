(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'ngMaterial',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'
    ]);

    //Configuration for Angular UI routing.
    app.config([
        '$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise('/questions');
            $stateProvider
                .state('questions', {
                    url: '/questions',
                    templateUrl: abp.appPath + 'App/Main/views/questions/index.cshtml',
                    menu: 'Questions' //Matches to name of 'Questions' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('questionDetail', {
                    url: '/questions/:id',
                    templateUrl: abp.appPath + 'App/Main/views/questions/detail.cshtml',
                    menu: 'Questions' //Matches to name of 'Questions' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('users', {
                    url: '/users',
                    templateUrl: abp.appPath + 'App/Main/views/users/index.cshtml',
                    menu: 'Users' //Matches to name of 'Users' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('tenants', {
                    url: '/tenants',
                    templateUrl: abp.appPath + 'App/Main/views/tenants/index.cshtml',
                    menu: 'Tenants' //Matches to name of 'Users' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('tenants/create', {
                    url: '/tenants/create',
                    templateUrl: abp.appPath + 'App/Main/views/tenants/create.cshtml',
                    menu: 'Tenants' //Matches to name of 'Users' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('tenants/update', {
                    url: '/tenants/update/:tenantId',
                    templateUrl: abp.appPath + 'App/Main/views/tenants/update.cshtml',
                    menu: 'Tenants' //Matches to name of 'Users' menu in ModuleZeroSampleProjectNavigationProvider
                })
                .state('tenants/delete', {
                    url: '/tenants/delete/:tenantId',
                    templateUrl: abp.appPath + 'App/Main/views/tenants/delete.cshtml',
                    menu: 'Tenants' //Matches to name of 'Users' menu in ModuleZeroSampleProjectNavigationProvider
                });
        }
    ]);
})();
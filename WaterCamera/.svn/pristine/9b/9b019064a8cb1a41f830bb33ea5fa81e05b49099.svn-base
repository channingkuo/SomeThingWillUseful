/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function (win) {
    'use strict';
    require.config({
        packages: [
            {
                name: 'echarts',
                location: './lib/echarts',
                main: 'echarts'
            },
            {
                name: 'zrender',
                location: './lib/zrender',
                main: 'zrender'
            }
        ]
    });

    win.xrmApp = angular.module('xrmApp', ['ngIOS9UIWebViewPatch','ionic'])
        .config([
            '$locationProvider', function ($locationProvider) {
                //$locationProvider.html5Mode(false);
                //$locationProvider.hashPrefix('!');
            }
        ])
        .config([
            "$httpProvider", function ($httpProvider) {
                $httpProvider.defaults.useXDomain = true;
                delete $httpProvider.defaults.headers.common['X-Requested-With'];
            }
        ])
        .config([
            "$ionicConfigProvider",function($ionicConfigProvider) {
                $ionicConfigProvider.backButton.previousTitleText(false).text('');
                $ionicConfigProvider.templates.maxPrefetch(100);
            }
        ])
        .config([
            '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
                $stateProvider
                    //.state('xrmApp', {
                    //    url: '/',
                    //    templateUrl: 'module/home/blankView.html',
                    //    controller: 'blankController'
                    //})
                    .state('xrmApp', {
                        url: '/',
                        templateUrl: 'module/home/loginView.html',
                        controller: 'LoginController'
                    })
                    .state('app-close',{
                        url:'/app/close',
                        templateUrl:'module/home/loginView.html',
                        controller:'LoginController'
                    })
                    .state('app-exit',{
                        url:'/app/exit',
                        templateUrl:'module/home/loginView.html',
                        controller:'LoginController'
                    });

                $urlRouterProvider.otherwise("/");
            }]);
})(window);

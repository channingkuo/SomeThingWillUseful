angular.module('xrmApp')
    .directive('rtEmpty', function () {
        return {
            restrict: 'EA',
            scope: {
                rtMessage: "=",
                data: "=rtData"
            },
            template: '<div ng-if="!data || data.length==0"><div class="row"><div class="col text-center"><img style="width:20%;height:auto;font-size: small;" src="./asset/img/empty.png" />' +
            '</div></div><div class="row" style="padding-top: 0px;"><div class="col text-center"  style="padding-top: 0px;"><h4 style="color:#dadadf;margin-top: 0px;"><b>{{rtMessage}}</b></h4></div></div></div>'
        };
    });
angular.module('xrmApp')
    .directive('focusOn', function () {
        return function (scope, elem, attr) {
            scope.$on('focusOn', function (e, name) {
                if (name === attr.focusOn) {
                    elem[0].focus();
                }
            });
        };

    }).factory('focus', function ($rootScope, $timeout) {
        return function (name) {
            $timeout(function () {
                $rootScope.$broadcast('focusOn', name);
            });
        };
    });

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .filter('getViewFieldValue', function () {
            /**
             * 根据传入的对象返回视图字段值
             * @param input 包含Type、Value和Name的属性的对象
             * @returns {string} 如果是lookup类型则返回Value.Text，否则直接返回Value
             */
            return function (input) {
                switch (input.Type) {
                    case "lookup":
                        var json = input.Value;
                        if (typeof json === 'string' && json.length > 0) {
                            json = JSON.parse(input.Value);
                        }
                        else if (typeof json === 'object') {
                            return json.Text;
                        }
                        else {
                            return "";
                        }

                        return json.Text;
                    default :
                        return input.Value;
                }
            };
        }).filter('getFriendlyDateString', function () {

            function formatDateTime(d, format) {
                var o = {
                    "M+": d.getMonth() + 1, //month
                    "d+": d.getDate(), //day
                    "h+": d.getHours(), //hour
                    "m+": d.getMinutes(), //minute
                    "s+": d.getSeconds(), //second
                    "q+": Math.floor((d.getMonth() + 3) / 3), //quarter
                    "S": d.getMilliseconds() //millisecond
                };

                if (/(y+)/.test(format)) {
                    format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
                }

                for (var k in o) {
                    if (new RegExp("(" + k + ")").test(format)) {
                        format = format.replace(RegExp.$1, RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                    }
                }
                return format;
            }

            return function (input) {
                var d;
                if (typeof input === 'string') {
                    d = new Date(Date.parse(input.replace(/-/g, "/")));
                } else {
                    d = input;
                }

                var today = new Date();
                today.setHours(23, 59, 59, 999);

                var friendlyDateString;

                var internalDays = parseInt((today.getTime() - d.getTime()) / (1000 * 60 * 60 * 24));
                switch (internalDays) {
                    case -2:
                        friendlyDateString = "后天" + formatDateTime(d, " hh:mm");
                        break;
                    case -1:
                        friendlyDateString = "明天" + formatDateTime(d, " hh:mm");
                        break;
                    case 0:
                        friendlyDateString = "今天" + formatDateTime(d, " hh:mm");
                        break;
                    case 1:
                        friendlyDateString = "昨天" + formatDateTime(d, " hh:mm");
                        break;
                    case 2:
                        friendlyDateString = "前天" + formatDateTime(d, " hh:mm");
                        break;
                    default :
                        friendlyDateString = formatDateTime(d, "yyyy-MM-dd hh:mm");
                        break;
                }

                return friendlyDateString;
            };
        });
})();

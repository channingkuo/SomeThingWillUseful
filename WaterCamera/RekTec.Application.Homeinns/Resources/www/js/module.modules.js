/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('attendance-map', {
                url: '/attendance/map',
                templateUrl: 'module/attendance/attendanceMapView.html',
                controller: 'AttendanceMapController'
            })
            .state('attendance-detail',{
                url:'/attendance/detail/:id',
                templateUrl:'module/attendance/attendanceDetailView.html',
                controller:'AttendanceDetailController'
            })
            .state('attendance-manager',{
                url:'/attendance/manager/:attendanceInfo',
                templateUrl:'module/attendance/attendanceManagerView.html',
                controller:'AttendanceManagerController'
            })
            .state('attendance-main',{
                url: "/attendance/main/",
                abstract: true,
                templateUrl: 'module/attendance/attendanceMainView.html',
                controller: 'AttendanceMainController'
            })
            .state('attendance-main.week', {
                url: "/week",
                views: {
                    'attendanceWeek-tab': {
                        templateUrl: "module/attendance/attendanceWeekListView.html",
                        controller: 'AttendanceWeekListController'
                    }
                }
            })
            .state('attendance-main.month', {
                url: "/month",
                views: {
                    'attendanceMonth-tab': {
                        templateUrl: "module/attendance/attendanceMonthListView.html",
                        controller: 'AttendanceMonthListController'
                    }
                }
            });


        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .config([
            '$stateProvider', function ($stateProvider) {
                $stateProvider
                    .state('cityVisit-main', {
                        url: '/cityVisit/main/:id',
                        templateUrl: 'module/cityVisit/cityVisitFormView.html',
                        controller: 'CityVisitFormController'
                    })
                    .state('pro-form', {
                        url: '/cityVisit/proForm/:id/:cityVisitId/:isEnd',
                        templateUrl: 'module/cityVisit/cityVisitProView.html',
                        controller: 'CityVisitProController'
                    })
                    .state('pro-deal', {
                        url: '/cityVisit/proDeal/:id/:cityVisitId/:isEnd',
                        templateUrl: 'module/cityVisit/cityVisitProDealView.html',
                        controller: 'CityVisitProDealController'
                    })
                    .state('cityVisit-end', {
                        url: '/cityVisit/end/:id',
                        templateUrl: 'module/cityVisit/cityVisitEndView.html',
                        controller: 'CityVisitEndController'
                    });
            }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('complaint-list', {
                url: '/complaint/list',
                templateUrl: 'module/complaint/complaintListView.html',
                controller: 'ComplaintListController'
            })
            .state('complaint-read',{
                url: '/complaint/read/:id',
                templateUrl: 'module/complaint/complaintReadOnlyView.html',
                controller: 'ComplaintReadOnlyController'
            })
            .state('complaint-edit',{
                url: '/complaint/edit/:id',
                templateUrl: 'module/complaint/ComplaintEditView.html',
                controller: 'ComplaintEditController'
            })
            .state('complaint-hotel',{
                url: '/complaint/hotel/:hotelId',
                templateUrl: 'module/complaint/hotelComplaintDetailView.html',
                controller: 'HotelComplaintDetailController'
            })
            .state('appraise-read',{
                url: '/appraise/read/:id',
                templateUrl: 'module/complaint/appraiseReadOnlyView.html',
                controller: 'AppraiseReadOnlyController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('daily-list', {
                url: '/daily/list',
                templateUrl: 'module/daily/dailyListView.html',
                controller: 'DailyListController'
            })
            .state('daily-edit',{
                url: '/daily/edit/:id',
                templateUrl: 'module/daily/dailyEditView.html',
                controller: 'DailyEditController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .config([
            '$stateProvider', function ($stateProvider) {
                $stateProvider
                    .state('choose-hotel', {
                        url: '/choose/hotel',
                        templateUrl: 'module/dialog/chooseHotelView.html',
                        controller: 'ChooseHotelController'
                    })
                    .state('image-view', {
                        url: '/preview/image',
                        templateUrl: 'module/dialog/previewImageView.html',
                        controller: 'PreviewImageController'
                    });
            }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('home-login', {
                url: '/home/login',
                templateUrl: 'module/home/loginView.html',
                controller: 'LoginController'
            })
            .state('home-login2', {
                url: '/login2',
                templateUrl: 'module/home/login2View.html',
                controller: 'Login2Controller'
            })
            .state('home-serveraddressedit', {
                url: '/serveraddressedit',
                templateUrl: 'module/home/serverAddressEditView.html',
                controller: 'ServerAddressEditController'
            })
            .state('home-app', {
                url: '/home/app',
                templateUrl: 'module/home/applicationView.html',
                controller: 'ApplicationController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('homeinns-list', {
                url: '/homeinns/list',
                templateUrl: 'module/homeinns/homeinnsListView.html',
                controller: 'HomeinnsListController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .config([
            '$stateProvider', function ($stateProvider) {
                $stateProvider
                    .state('hotel-map', {
                        url: '/hotel/map',
                        templateUrl: 'module/hotel/hotelMapView.html',
                        controller: 'HotelMapController'
                    })
                    .state('hotel-detail', {
                        url: '/hotel/detail/:id',
                        templateUrl: 'module/hotel/hotelDetailView.html',
                        controller: 'HotelDetailController'
                    })
                    .state('qc-score', {
                        url: '/hotel/score/:id',
                        templateUrl: 'module/hotel/qcScoreView.html',
                        controller: 'QcScoreController'
                    })
                    .state('room-state', {
                        url: '/hotel/room/:id',
                        templateUrl: 'module/hotel/pmsRoomStateView.html',
                        controller: 'PmsRoomStateController'
                    })
                    .state('hotel-form', {
                        url: '/hotel/form/:id/:hotelid',
                        templateUrl: 'module/hotel/hotelVisitFormView.html',
                        controller: 'HotelVisitFormController'
                    })
                    .state('hotel-visit', {
                        url: '/hotel/visit/:id',
                        templateUrl: 'module/hotel/hotelVisitDetailView.html',
                        controller: 'HotelVisitDetailController'
                    })
                ;
            }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .config([
            '$stateProvider', function ($stateProvider) {
                $stateProvider
                    .state('manage-list', {
                        url: '/manageVisit/list',
                        templateUrl: 'module/manageVisit/manageVisitListView.html',
                        controller: 'ManageVisitListController'
                    })
                    .state('manage-form', {
                        url: '/manageVisit/form/:id',
                        templateUrl: 'module/manageVisit/manageVisitFormView.html',
                        controller: 'ManageVisitFormController'
                    })
                    .state('manage-detail', {
                        url: '/manageVisit/detail/:id',
                        templateUrl: 'module/manageVisit/manageVisitDetailView.html',
                        controller: 'ManageVisitDetailController'
                    })
                    .state('manage-end', {
                        url: '/manageVisit/end/:id',
                        templateUrl: 'module/manageVisit/endManageVisitView.html',
                        controller: 'EndManageVisitController'
                    });
            }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .config([
            '$stateProvider', function ($stateProvider) {
                $stateProvider
                    .state('myhotel-detail', {
                        url: '/myhotel/detail',
                        templateUrl: 'module/myHotel/myHotelDetailView.html',
                        controller: 'MyHotelDetailController'
                    })
                    .state('myqc-score', {
                        url: '/myhotel/score/:id',
                        templateUrl: 'module/myHotel/myQcScoreView.html',
                        controller: 'MyQcScoreController'
                    })
                    .state('myroom-state', {
                        url: '/myhotel/room/:id',
                        templateUrl: 'module/myHotel/myPmsRoomStateView.html',
                        controller: 'MyPmsRoomStateController'
                    });
            }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('notice-list', {
                url: '/notice/noticeView',
                templateUrl: 'module/notice/noticeListView.html',
                controller: 'NoticeListController'
            }
        )
            .state('notice-readonly', {
                url: '/notice/noticeRead/:id',
                templateUrl: 'module/notice/noticeReadOnlyView.html',
                controller: 'NoticeReadController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('password-forget', {
                url: '/password/forget',
                templateUrl: 'module/passwordForget/passwordView.html',
                controller: 'PasswordController'
            }
        ).state('password-reset', {
                url: '/password/reset/:userCode/:emailTypeId',
                templateUrl: 'module/passwordForget/resetPasswordView.html',
                controller: 'ResetPasswordController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('question-list', {
                url: '/question/list',
                templateUrl: 'module/question/questionListView.html',
                controller: 'QuestionListController'
            })
            .state('question-edit',{
                url: '/question/edit/:id/:hotelid/:isEdit/:states',
                templateUrl: 'module/question/questionEditView.html',
                controller: 'QuestionEditController'
            });

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('revenue-card', {
                url: '/revenue/card',
                templateUrl: 'module/revenue/revenueCardView.html',
                controller: 'RevenueCardController'
            }
        ).state('flow-list', {
                url: '/flow/list',
                templateUrl: 'module/revenue/flowListView.html',
                controller: 'FlowListController'
            }
        ).state('flow-room', {
                url: '/flow/room/:hotelId/:hotelName',
                templateUrl: 'module/revenue/flowRoomTypeView.html',
                controller: 'FlowRoomTypeController'
            }
        ).state('price-room', {
                url: '/price/room/:hotelId',
                templateUrl: 'module/revenue/priceRoomTypeView.html',
                controller: 'PriceRoomTypeController'
            }
        ).state('threshold-edit', {
                url: '/threshold/edit',
                templateUrl: 'module/revenue/thresholdEditView.html',
                controller: 'ThresholdEditController'
            }
        ).state('lock-room', {
                url: '/lock/room',
                templateUrl: 'module/revenue/lockRoomView.html',
                controller: 'LockRoomController'
            }
        );

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('roomControl-card', {
                url: '/roomControl/card',
                templateUrl: 'module/roomControl/roomControlCardView.html',
                controller: 'RoomControlCardController'
            })
            .state('roomControl-clean', {
                url: '/roomControl/clean',
                templateUrl: 'module/roomControl/roomCleanView.html',
                controller: 'RoomCleanController'
            })
            .state('roomControl-count', {
                url: '/roomControl/count',
                templateUrl: 'module/roomControl/roomCleanCountView.html',
                controller: 'RoomCleanCountController'
            })
            .state('roomControl-unclean', {
                url: '/roomControl/unclean/:num',
                templateUrl: 'module/roomControl/roomUncleanView.html',
                controller: 'RoomUncleanController'
            })
            .state('roomControl-check', {
                url: '/roomControl/check',
                templateUrl: 'module/roomControl/roomCheckView.html',
                controller: 'RoomCheckController'
            })
            .state('roomControl-state', {
                url: '/roomControl/state/:roomNum/:roomState',
                templateUrl: 'module/roomControl/roomStateChangeView.html',
                controller: 'RoomStateChangeController'
            })
            .state('roomControl-cleaned', {
                url: '/roomControl/cleaned/:num/:roomType',
                templateUrl: 'module/roomControl/roomCleanedListView.html',
                controller: 'RoomCleanedListController'
            })
            .state('roomControl-redivide', {
                url: '/roomControl/redivide',
                templateUrl: 'module/roomControl/roomReDivideView.html',
                controller: 'RoomReDivideController'
            })
            .state('guest-supplies', {
                url: '/guest/supplies',
                templateUrl: 'module/roomControl/guestSuppliesView.html',
                controller: 'GuestSuppliesController'
            })

            .state('roomControl-manage', {
                url: '/roomControl/manage',
                templateUrl: 'module/roomControl/manageCheckedCountView.html',
                controller: 'ManageCheckedCountController'
            })
            .state('roomControl-checked', {
                url: '/roomControl/checked/:num/:roomType',
                templateUrl: 'module/roomControl/roomCheckedListView.html',
                controller: 'RoomCheckedListController'
            })
            .state('roomControl-unchecked', {
                url: '/roomControl/unchecked/:num',
                templateUrl: 'module/roomControl/roomUncheckedView.html',
                controller: 'RoomUncheckedController'
            })
        ;

        $urlRouterProvider.otherwise("/");
    }]);
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
xrmApp.config([
    '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        'use strict';
        $stateProvider
            .state('task-list', {
                url: '/task/list',
                templateUrl: 'module/task/taskListView.html',
                controller: 'TaskListController'
            })
            .state('task-edit', {
                url: '/task/edit',
                templateUrl: 'module/task/taskEditView.html',
                controller: 'TaskEditController'
            })
            .state('task-read', {
                url: '/task/read/:id',
                templateUrl: 'module/task/taskReadOnlyView.html',
                controller: 'TaskReadOnlyController'
            })
            .state('task-choose',{
                url:'/task/chooseUser',
                templateUrl: 'module/task/chooseUser.html',
                controller: 'ChooseUserController'
            })
            .state('task-handler',{
                url:'/task/handler/:id',
                templateUrl: 'module/task/taskHandlerView.html',
                controller: 'TaskHandlerController'
            })
            .state('task-system',{
                url:'/task/system/:id',
                templateUrl: 'module/task/taskSystemHandlerView.html',
                controller: 'TaskSystemHandlerController'
            })

            .state('task-main', {
                url: '/task/main/:id',
                templateUrl: 'module/task/taskVisitFormView.html',
                controller: 'TaskVisitFormController'
            })
            .state('task-form', {
                url: '/task/proForm/:id/:cityVisitId/:isEnd',
                templateUrl: 'module/task/taskVisitProView.html',
                controller: 'TaskVisitProController'
            })
            .state('task-deal', {
                url: '/task/proDeal/:id/:cityVisitId/:isEnd',
                templateUrl: 'module/task/cityVisitProDealView.html',
                controller: 'TaskVisitProDealController'
            })
        ;

        $urlRouterProvider.otherwise("/");
    }]);
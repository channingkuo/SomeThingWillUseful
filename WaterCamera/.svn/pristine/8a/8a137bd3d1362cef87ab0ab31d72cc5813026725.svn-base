/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function AttendanceDetailController($scope, $state, $stateParams,$ionicHistory, rt, AttendanceService) {
        function init() {
            //返回事件
            $scope.goBack=_goBack;
            //获取考勤记录的明细
            AttendanceService.getAttendanceDetail($stateParams.id)
                .success(function (data) {
                    $scope.vm = data;
                    if ($scope.vm.AttendanceTime === "0001-01-01T00:00:00") {
                        $scope.vm.AttendanceTime = "";
                    }
                })
                .error(function (data) {
                    rt.showErrorToast(data);
                });
        }
        /*
        * 返回函数
        * */
        function _goBack(){
            //返回上一个界面
            $ionicHistory.goBack();
        }

        init();
    }

    xrmApp.controller("AttendanceDetailController", ['$scope', '$state', '$stateParams','$ionicHistory', 'rt', 'AttendanceService', AttendanceDetailController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function AttendanceMainController($scope, $state, $stateParams,$ionicHistory, rt, AttendanceService) {
        function init() {
            $scope.goWeekList=_goWeekList;
            $scope.goMonthList=_goMonthList;

        }
        /*
         * 返回函数
         * */
        function _goWeekList(){
            $state.go("attendance-main.week");
        }

        function _goMonthList(){
            $state.go("attendance-main.month");
        }

        init();
    }

    xrmApp.controller("AttendanceMainController", ['$scope', '$state', '$stateParams','$ionicHistory', 'rt', 'AttendanceService', AttendanceMainController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
/*global MediaStreamTrack:false */
(function () {
    'use strict';

    function AttendanceManagerController($scope, $state, $stateParams, $ionicNavBarDelegate, $ionicHistory, $timeout, rt, $window, AttendanceService) {
        var attendance;
        var canvas;
        var video;

        function init() {
            //考勤对象
            $scope.attendanceInfo = JSON.parse($stateParams.attendanceInfo);
            //考勤Id
            $scope.attendanceInfo.Id = rt.newGuid();
            //提交事件
            $scope.submitClick = _submitClick;
            //拍照事件
            $scope.takePhotoClick = _takePhotoClick;
            //照片高度
            $scope.canvasHeight = screen.height * 0.35;

            $scope.isTakePhoto = false;
            $scope.XrmImageData = false;
        }

        /**
         * 提交事件
         */
        function _submitClick() {
            if ($scope.isTakePhoto) {
                var imageElement = document.getElementById("canvas");
                if (imageElement === null) {
                    return;
                }
                //考勤类型赋值
                $scope.attendanceInfo.AttendanceType={Value:$scope.attendanceInfo.AType,Name:$scope.attendanceInfo.AType===0?"签到":"签退"};

                //照片
                $scope.attendanceInfo.Photoe={Id:rt.newGuid(),FileBase64Content:imageElement.src.substr(22, imageElement.src.length)};
               // $scope.attendanceInfo.Photoe={Id:rt.newGuid(),FileBase64Content:"11111"};
                rt.showLoadingToast("正在提交...");
                    AttendanceService.attendanceCheck($scope.attendanceInfo)
                        .success(function (data) {
                            rt.hideLoadingToast();
                            rt.showSuccessToast($scope.attendanceInfo.AType===0?"签到成功":"签退成功");
                            $ionicNavBarDelegate.back();
                            $ionicHistory.clearCache();
                        })
                        .error(function (data) {
                            rt.hideLoadingToast();
                            rt.showErrorToast(data);
                        });

            } else {
                rt.showErrorToast("照片为空");
            }

        }

        /**
         * 拍照事件
         * @private
         */
        //function _takePhotoClick() {
        //    rt.takePhoto(800, 80, function (base64Image) {
        //        try {
        //            //var obj=new Object();
        //            //obj.Id=rt.newGuid();
        //            //obj.FileBase64Content=base64Image;
        //            $scope.attendanceInfo.Photoe={Id:rt.newGuid(),FileBase64Content:base64Image};
        //        }
        //        catch (e) {
        //            rt.showErrorToast(e.message);
        //        }
        //    });
        //}


        function _takePhotoClick() {
            location.href = "app:take-photo?px=800&kb=80";
            _checkHasXrmDevieImageData();
        }

        function _checkHasXrmDevieImageData() {
            if (window.XrmImageData === undefined) {
                $scope.XrmImageData = true;
            }
            if (window.XrmImageData && window.XrmImageData.getXrmImageData && window.XrmImageData.getXrmImageData() !== "") {
                $scope.isTakePhoto = true;

                var imageElement = document.getElementById("canvas");
                if (imageElement === null)
                    return true;
                var imageData = window.XrmImageData.getXrmImageData();
                imageElement.src = "data:image/png;base64," + imageData;
                if ($scope.XrmImageData) {
                    window.XrmImageData = undefined;
                }
                window.XrmImageData.clearImageData();
                return;
            }
            $timeout(_checkHasXrmDevieImageData, 1500);
        }



        init();
    }

    xrmApp.controller("AttendanceManagerController", ['$scope', '$state', '$stateParams', '$ionicNavBarDelegate', '$ionicHistory', '$timeout', 'rt', '$window', 'AttendanceService', AttendanceManagerController]);
})();
/*global UIMenu:false */
/*global xrmApp:false */
/*global BMap:false */
(function () {
    'use strict';
    xrmApp.controller("AttendanceMapController", ['$scope', '$state', 'rt', 'AttendanceService', AttendanceMapController]);
    function AttendanceMapController($scope, $state, rt, AttendanceService) {
        //定义百度地图的地图对象
        var _map;
        //百度地图的缩放级别，大多数情况下使用14
        var _mapScale = 14;
        //显示百度地图的div的id
        var _mapDivId = "baiduMap";

        var _pageIndex = 1;
        var _pageSize = rt.getPaginationSize();
        //位置信息
        var newPosition={};
        //考勤记录列表
        var attendanceList = [];
        //今日签到对象
        var attendanceON = {};
        //今日签退对象
        var attendanceOFF = {};
        init();
        function init() {

            //签到/签退事件
            $scope.attendanceClick = _attendanceClick;
            //返回事件
            $scope.backClick = _backClick;
            //考勤记录按钮点击事件
            $scope.attendanceListClick=_attendanceListClick;

            //获取当前位置
            getLocation(true);
        }
        /**
         * 获取考勤记录列表数据
         * @private
         */
        function _getAttendancesList(success) {
            AttendanceService.getAttendancesWeekList(_pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                })
                .error(function (data) {
                    rt.showErrorToast(data);
                });
        }
        /*
        * 获取今日考勤记录
        * */
        function _getAttendanceInfo(){
            //判断今日是否已有考勤
            var myDate=new Date();
            var today=rt.formatDateTime(myDate,"yyyy-MM-dd");

            for(var i=0;i<attendanceList.length;i++){
                var d=attendanceList[i].AttendanceTime;
                var dd= d.substring (0,10);
                if(dd==today){
                    //如果考勤类型为签到，给签到对象赋值
                    if(attendanceList[i].AttendanceType.Value===0){
                        attendanceON=attendanceList[i];
                    }
                    //如果考勤类型为签退，给签退对象赋值
                    if(attendanceList[i].AttendanceType.Value===1){
                        attendanceOFF=attendanceList[i];
                    }
                }
            }
        }
        /**
         * 考勤点击事件
         * @param tabIndex 0签到，1签退
         * @private
         */
        //检测对象是否为空
        function isOwnEmpty(obj)
        {
            for(var name in obj)
            {
                if(obj.hasOwnProperty(name))
                {
                    return false;
                }
            }
            return true;
        }
        function _attendanceClick(tabIndex) {
            rt.showLoadingToast("正在跳转...");
            if(isOwnEmpty(newPosition)){
                rt.showErrorToast("当前位置获取失败，请稍后再试");
                return ;
            }

            //获取考勤记录
            _getAttendancesList(function (data) {
                attendanceList=data;
                //今日考勤记录赋值
                _getAttendanceInfo();
                if(tabIndex===0)
                {
                    rt.hideLoadingToast();
                    //判断今日考勤状况
                    if(isOwnEmpty(attendanceON)){
                        attendanceON.Address=newPosition.address;
                        attendanceON.Longitude=newPosition.longitude;
                        attendanceON.Latitude=newPosition.latitude;
                        //类型
                        attendanceON.AType=0;
                        $state.go("attendance-manager", {attendanceInfo: JSON.stringify(attendanceON)});
                        return ;
                    }
                    rt.showErrorToast("今日已签到！");
                    return ;
                }
                if(tabIndex===1)
                {
                    rt.hideLoadingToast();
                    //判断今日考勤状况
                    if(isOwnEmpty(attendanceOFF)){
                        attendanceOFF.Address=newPosition.address;
                        attendanceOFF.Longitude=newPosition.longitude;
                        attendanceOFF.Latitude=newPosition.latitude;
                        //类型
                        attendanceOFF.AType=1;
                        //$scope.attendanceOFF.AttendanceType.Name="签退";

                        $state.go("attendance-manager", {attendanceInfo: JSON.stringify(attendanceOFF)});
                        return ;
                    }
                    rt.showErrorToast("今日已签退！");
                    return ;
                }

            });
        }
        /**
         * 获取当前位置
         */
        function getLocation(reLoadMap) {
            rt.getGeolocation(function (position) {
                handlePosition(position,reLoadMap);
                //赋值全局变量位置信息
                newPosition=position;
            },function (error){
                rt.showErrorToast(error);
            });
        }

        /**
         * 处理当前获取到的位置
         * @param position 位置信息
         */
        function handlePosition(position,reLoadMap) {
            if (position === null) {
                return;
            }
            if(reLoadMap) {
               _map = new BMap.Map(_mapDivId);
               _map.centerAndZoom(new BMap.Point(position.longitude, position.latitude), _mapScale);
               var zoomControl = new BMap.ZoomControl();
               _map.addControl(zoomControl);//添加缩放控件
               var scaleControl = new BMap.ScaleControl();
               _map.addControl(scaleControl);//添加比例尺控件
               //创建当前位置的点
               var marker = new BMap.Marker(new BMap.Point(position.longitude, position.latitude));
               //添加到地图中
               _map.addOverlay(marker);
            }
        }

        /**
         * 返回事件
         * @private
         */
        function _backClick() {
            $state.go("app-close");
        }
        /*
        * 考勤记录tab点击事件
        * */
        function _attendanceListClick(){
            $state.go("attendance-main.week");
        }

    }

})();




/*global UIMenu:false */
/*global xrmApp:false */
/*global BMap:false */
(function () {
    'use strict';
    xrmApp.controller("AttendanceMonthListController", ['$scope', '$state','$ionicHistory', 'rt', 'AttendanceService', AttendanceMonthListController]);
    function AttendanceMonthListController($scope, $state,$ionicHistory, rt, AttendanceService) {
        var _pageIndex = 1;
        var _pageSize = rt.getPaginationSize();
        init();
        function init() {
            //考勤记录列表
            $scope.attendanceList = [];
            //返回事件
            $scope.backClick = _backClick;
            //考勤记录加载更多事件
            $scope.loadMore = _loadMore;
            //考勤记录下拉刷新事件
            $scope.viewRefresh = _viewRefresh;


            //默认加载考勤列表
            _getAttendancesList(function (data) {
                $scope.attendanceList=data;
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }
        /**
         * 获取考勤记录列表数据
         * @private
         */
        function _getAttendancesList(success) {
            AttendanceService.getAttendancesMonthList(_pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length >= _pageSize) {
                        $scope.isLoadMore = true;
                    } else {
                        $scope.isLoadMore = false;
                    }
                })
                .error(function (data) {
                    rt.showErrorToast(data);
                });
        }
        /**
         * 返回事件
         * @private
         */
        function _backClick() {
            $state.go("attendance-map");
        }
        /**
         * 加载更多
         * @private
         */
        function _loadMore() {
            _pageIndex++;
            _getAttendancesList(function (data) {
                $scope.attendanceList=data;
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _getAttendancesList(function (data) {
                $scope.attendanceList = data;
                $scope.$broadcast('scroll.refreshComplete');
            });
        }
    }

})();




/*global UIMenu:false */
/*global xrmApp:false */
/*global BMap:false */
(function () {
    'use strict';
    xrmApp.controller("AttendanceWeekListController", ['$scope', '$state','$ionicHistory', 'rt', 'AttendanceService', AttendanceWeekListController]);
    function AttendanceWeekListController($scope, $state,$ionicHistory, rt, AttendanceService) {
        var _pageIndex = 1;
        var _pageSize = rt.getPaginationSize();
        init();
        function init() {
            //考勤记录列表
            $scope.attendanceList = [];
            //返回事件
            $scope.backClick = _backClick;
            //考勤记录加载更多事件
            $scope.loadMore = _loadMore;
            //考勤记录下拉刷新事件
            $scope.viewRefresh = _viewRefresh;


            //默认加载考勤列表
            _getAttendancesList(function (data) {
                $scope.attendanceList=data;
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }
        /**
         * 获取考勤记录列表数据
         * @private
         */
        function _getAttendancesList(success) {
            AttendanceService.getAttendancesWeekList(_pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length >= _pageSize) {
                        $scope.isLoadMore = true;
                    } else {
                        $scope.isLoadMore = false;
                    }
                })
                .error(function (data) {
                    rt.showErrorToast(data);
                });
        }
        /**
         * 返回事件
         * @private
         */
        function _backClick() {
            $state.go("attendance-map");
        }
        /**
         * 加载更多
         * @private
         */
        function _loadMore() {
            _pageIndex++;
            _getAttendancesList(function (data) {
                $scope.attendanceList=data;
            });
            $scope.$broadcast('scroll.infiniteScrollComplete');
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _getAttendancesList(function (data) {
                $scope.attendanceList = data;
            });
            $scope.$broadcast('scroll.refreshComplete');
        }


    }

})();




(function () {
    'use strict';

    xrmApp.controller("CityVisitEndController", ['$scope', '$state', '$stateParams', '$ionicHistory','rt', 'CityVisitService', CityVisitEndController]);

    function CityVisitEndController($scope, $state, $stateParams,$ionicHistory, rt, CityVisitService) {
        //评价数组
        $scope.appriseArray=[];

        function init() {
            $scope.chooseClick=_chooseClick;
            $scope.endClick=_endClick;
            $scope.visit={};
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            CityVisitService.getCityVisit($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (data.StartTime === "0001-01-01T00:00:00"||!data.StartTime) {
                        data.StartTime = "";
                    }
                    if (data.EndTime === "0001-01-01T00:00:00"||!data.EndTime) {
                        data.EndTime = "";
                    }
                    $scope.visit=data;
                    for(var i=0;i<data.Appraise.Options.length;i++){
                        var option={};
                        option.Name=data.Appraise.Options[i].Name;
                        option.Value=data.Appraise.Options[i].Value;
                        if($scope.visit.Appraise.Value===i+1){
                            option.Checked=true;
                        }else{
                            option.Checked=false;
                        }
                        $scope.appriseArray[i]=option;
                    }
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 评价按钮点击事件
         * @param apprise
         * @param index
         * @private
         */
        function _chooseClick(apprise,index){
            for(var i=0;i<$scope.appriseArray.length;i++){
                if(i==index){
                    $scope.appriseArray[i].Checked=true;
                }else{
                    $scope.appriseArray[i].Checked=false;
                }
            }
            $scope.visit.Appraise.Value=apprise.Value;
            $scope.visit.Appraise.Name=apprise.Name;
        }

        function _endClick(){
            if($scope.visit.Appraise.Value<1||$scope.visit.Appraise.Value>3){
                rt.showErrorToast("评价没有选择！");
                return;
            }
            rt.showLoadingToast("正在保存...");
            CityVisitService.endCityVisit($scope.visit)
                .success(function(data){
                    rt.hideLoadingToast();
                    $ionicHistory.goBack();
                    rt.showSuccessToast("结束巡店成功！");
                })
                .error(function(error){
                    alert(error);
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        init();
    }
})();

(function () {
    'use strict';

    xrmApp.controller("CityVisitFormController", ['$scope', '$state', '$stateParams', '$ionicHistory', 'rt', 'CityVisitService', CityVisitFormController]);

    function CityVisitFormController($scope, $state, $stateParams, $ionicHistory, rt, CityVisitService) {
        function init() {
            $scope.backClick = _backClick;
            $scope.addProClick = _addProClick;
            $scope.endVisitClick = _endVisitClick;
            $scope.onItemClick = _onItemClick;
            $scope.isEnd = false;
            _loadData();

        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            CityVisitService.getCityVisit($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (data.StartTime === "0001-01-01 00:00:00" || !data.StartTime) {
                        data.StartTime = "";
                    }
                    /*else {
                     data.StartTime = new Date(data.StartTime);
                     }*/
                    if (data.EndTime === "0001-01-01 00:00:00" || !data.EndTime) {
                        data.EndTime = "";
                    }
                    /*else {
                     data.EndTime = new Date(data.EndTime);
                     }*/
                    $scope.visit = data;
                    $scope.isEnd = data.VisitState.Value === 2 ? true : false;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        function _backClick() {
            $ionicHistory.goBack();

            //如果没有添加问题清单，则返回时自己删除此巡店单
            if (!rt.isNullOrEmptyString($stateParams.id)
                & $scope.visit !== null
                & $scope.visit.IssuesList !== null
                & $scope.visit.IssuesList.length === 0) {

                CityVisitService.cancelCityVisit($stateParams.id);
            }
        }

        /**
         * 添加问题按钮点击事件
         * @private
         */
        function _addProClick() {
            $state.go("pro-form", {id: "", cityVisitId: $stateParams.id, isEnd: false});
        }

        /**
         * 结束巡店
         * @private
         */
        function _endVisitClick() {
            $state.go("cityVisit-end", {id: $stateParams.id});
        }

        /**
         *列表项按钮点击事件
         * @param issue
         * @private
         */
        function _onItemClick(issue) {
            if ($scope.isEnd) {
                $state.go("pro-deal", {id: issue.Id, cityVisitId: $stateParams.id, isEnd: $scope.isEnd});
            } else {
                $state.go("pro-form", {id: issue.Id, cityVisitId: $stateParams.id, isEnd: $scope.isEnd});
            }
        }

        init();
    }
})();

/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("CityVisitProController", ['$scope', '$rootScope', '$state', '$stateParams', '$ionicHistory', 'rt', 'CityVisitService', CityVisitProController]);
    function CityVisitProController($scope, $rootScope, $state, $stateParams, $ionicHistory, rt, CityVisitService) {
        //构建所有问题类型数组
        $scope.typeArray = [];
        $scope.array = [];

        function _init() {
            $scope.previewIndex = 0;

            $scope.choosePicture = _choosePicture;
            $scope.deletePicture = _deletePicture;
            $scope.previewPicture = _previewPicture;
            $scope.chooseClick = _chooseClick;
            $scope.completeClick = _completeClick;
            $scope.addNewClick = _addNewClick;

            _loadData($stateParams.id);

            $scope.$on("rt-reload", function () {
                _loadData("");
            });
        }

        function _loadData(id) {
            rt.showLoadingToast("正在加载数据...");
            CityVisitService.getIssuesById(id, $stateParams.cityVisitId)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.Issues = data;
                    for (var i = 0; i < data.TypeList.length; i++) {
                        var issueType = {};
                        issueType.Name = data.TypeList[i].Name;
                        issueType.Id = data.TypeList[i].Id;
                        if (data.Type.Id === data.TypeList[i].Id) {
                            issueType.Checked = true;
                        } else {
                            issueType.Checked = false;
                        }
                        $scope.array[i] = issueType;
                    }
                    _dealProType($scope.array);
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });

        }

        /**
         * 将问题类型集合拆分成一行三个的二维数组
         * @param types
         * @private
         */
        function _dealProType(types) {
            var lines = types.length % 3 === 0 ? types.length / 3 : parseInt(types.length / 3) + 1;
            for (var i = 0; i < lines; i++) {
                $scope.typeArray[i] = [];
                for (var j = 0; j < 3; j++) {
                    $scope.typeArray[i][j] = types[3 * i + j] === undefined ? {} : types[3 * i + j];
                }
            }
        }

        /**
         * 选中当前类型的按钮
         * @param type
         * @param child
         * @param parent
         * @private
         */
        function _chooseClick(type, child, parent) {
            //设置当前按钮选中状态
            for (var i = 0; i < $scope.array.length; i++) {
                if (i === child + parent * 3) {
                    $scope.array[i].Checked = true;
                } else {
                    $scope.array[i].Checked = false;
                }
            }

            $scope.Issues.Type.Id = type.Id;
            $scope.Issues.Type.Text = type.Name;
        }

        /**
         * 完成点击事件
         * @private
         */
        function _completeClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Content) && $scope.Issues.Photos.length === 0) {
                rt.showErrorToast("问题照片或问题描述必须提供一项！");
                return;
            }

            if (rt.isNullOrEmptyString($scope.Issues.Type.Id)) {
                rt.showErrorToast("问题类型没有选择！");
                return;
            }

            if (rt.isNullOrEmptyString($scope.Issues.Id)) {
                $scope.Issues.Id = rt.newGuid();
            }

            rt.showLoadingToast("正在保存...");

            CityVisitService.saveCityVisitIssue($scope.Issues)
                .success(function (data) {
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功！");
                    $ionicHistory.goBack();
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 完成并新建
         * @private
         */
        function _addNewClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Content) && $scope.Issues.Photos.length === 0) {
                rt.showErrorToast("问题照片或问题描述必须提供一项！");
                return;
            }
            if (rt.isNullOrEmptyString($scope.Issues.Type.Id)) {
                rt.showErrorToast("问题类型没有选择！");
                return;
            }

            if (rt.isNullOrEmptyString($scope.Issues.Id)) {
                $scope.Issues.Id = rt.newGuid();
            }
            rt.showLoadingToast("正在保存...");
            CityVisitService.saveCityVisitIssue($scope.Issues)
                .success(function (data) {
                    rt.hideLoadingToast();

                    $scope.$emit("rt-reload");
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 选择照片
         * @private
         */
        function _choosePicture() {
            /*
            rt.takePhoto(800, 80, function (base64Image) {
                try {
                    $scope.Issues.Photos.push({
                        "Id": rt.newGuid(),
                        "FileBase64Content": base64Image
                    });
                }
                catch (e) {
                    alert(e.message);
                }
            });*/

            rt.chooseImage({
                count: 1,
                px: 800,
                kb: 80,
                sourceType: ['album', 'camera'],
                success: function (base64Image) {
                    try {
                        $scope.Issues.Photos.push({
                            "Id": rt.newGuid(),
                            "FileBase64Content": base64Image
                        });
                    }
                    catch (e) {
                        rt.showErrorToast(e.message);
                    }
                }
            });
        }

        /**
         * 预览照片
         * @param index
         * @private
         */
        function _previewPicture(photo) {
            $scope.previewPhoto = photo;
            rt.createDialog("module/dialog/previewImageView.html", $scope, null)
                .then(function (d) {
                    d.show();
                });
        }

        /**
         * 删除照片
         * @param fileid
         * @private
         */
        function _deletePicture(fileId) {
            //检查是否已经添加过了
            var index = _.findIndex($scope.Issues.Photos, {Id: fileId});
            if (index >= 0) {
                $scope.Issues.Photos.splice(index, 1);
            }
        }

        _init();
    }


})();
/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("CityVisitProDealController", ['$scope', '$rootScope', '$state', '$stateParams', '$ionicHistory', 'rt', 'CityVisitService', CityVisitProDealController]);
    function CityVisitProDealController($scope, $rootScope, $state, $stateParams, $ionicHistory, rt, CityVisitService) {

        function _init() {
            $scope.previewIndex = 0;

            $scope.editable = false;

            $scope.previewPicture = _previewPicture;
            $scope.choosePicture = _choosePicture;
            $scope.deletePicture = _deletePicture;

            $scope.completeClick = _completeClick;

            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast("正在加载数据...");
            CityVisitService.getIssuesById($stateParams.id, $stateParams.cityVisitId)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.Issues = data;

                    if (data.Owner.Id.toLowerCase() === rt.getUserId().toLowerCase() && data.Status.Value === 1) {
                        $scope.editable = true;
                    }
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });

        }

        /**
         * 完成点击事件
         * @private
         */
        function _completeClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Method)) {
                rt.showErrorToast("问题解决方案没有填写！");
                return;
            }
            rt.showLoadingToast("正在保存...");
            CityVisitService.saveCityVisitIssue($scope.Issues)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $ionicHistory.goBack();
                    rt.showSuccessToast("保存成功！");
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 预览照片
         * @param index
         * @private
         */
        function _previewPicture(photo) {
            $scope.previewPhoto = photo;
            rt.createDialog("module/dialog/previewImageView.html", $scope, null)
                .then(function (d) {
                    d.show();
                });
        }

        /**
         * 选择照片
         * @private
         */
        function _choosePicture() {
            /*
            rt.takePhoto(800, 80, function (base64Image) {
                try {
                    $scope.Issues.HandlePhotos.push({
                        "Id": rt.newGuid(),
                        "FileBase64Content": base64Image
                    });
                }
                catch (e) {
                    alert(e.message);
                }
            });*/

            rt.chooseImage({
                count: 1,
                px: 800,
                kb: 80,
                sourceType: ['album', 'camera'],
                success: function (base64Image) {
                    try {
                        $scope.Issues.HandlePhotos.push({
                            "Id": rt.newGuid(),
                            "FileBase64Content": base64Image
                        });
                    }
                    catch (e) {
                        rt.showErrorToast(e.message);
                    }
                }
            });
        }

        /**
         * 删除照片
         * @param fileid
         * @private
         */
        function _deletePicture(fileId) {
            //检查是否已经添加过了
            var index = _.findIndex($scope.Issues.HandlePhotos, {Id: fileId});
            if (index >= 0) {
                $scope.Issues.HandlePhotos.splice(index, 1);
            }
        }

        _init();
    }


})();
/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function AppraiseReadOnlyController($scope, $timeout, $ionicHistory, $state, $stateParams,rt, ComplaintService) {

        function init() {
            $scope.btnGoRun = _btnGoRun;
            _loadData();
        }

        /**
         * 加载表单数据
         * @private
         */
        function _loadData() {
            rt.showLoadingToast("正在加载数据...");
            ComplaintService.getAppraiseDetail($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.appraise = data;
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 回复按钮的事件
         * @private
         */
        function _btnGoRun(){
            rt.showLoadingToast("正在提交...");
            if(rt.isNullOrEmptyString($scope.appraise.Replay)){
                rt.showErrorToast("请填写回复内容再提交!");
                return;
            }
            ComplaintService.replayAppraise($scope.appraise.Id, $scope.appraise.Replay)
                .success(function(data){
                    rt.hideLoadingToast();
                    if(data > 0){
                        rt.showSuccessToast("回复成功!");
                        $timeout(function () {
                            $ionicHistory.goBack();
                        }, 2000);
                    }
                    else{
                        rt.showErrorToast("提交回复失败!");
                    }
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        init();
    }

    xrmApp.controller("AppraiseReadOnlyController", ['$scope', '$timeout', '$ionicHistory', '$state', '$stateParams','rt','ComplaintService', AppraiseReadOnlyController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function ComplaintEditController($scope, $state, $stateParams,rt,$timeout,$ionicHistory,ComplaintService) {

       function init() {
            $scope.vm = {};
            $scope.getSrc = _getSrc;
            $scope.photoClick = _photoClick;
            $scope.btnOkClick = _btnOkClick;
            _getComplaintData();
        }

        /**
         * 获取投诉单信息
         * @private
         */
        function _getComplaintData(){
            ComplaintService.getComplaint($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.vm = data;
                    if (!$scope.vm.Photos){
                        $scope.vm.Photos = [];
                    }
                    $scope.vm.Photos.push({FileBase64Content:"add"});
                })
                .error(function(data){
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载数据...");
        }

        /**
         * 照片点击事件
         * @param index 照片列表下标
         * @private
         */
        function _photoClick(index) {
            if (index == $scope.vm.Photos.length - 1) {
                _takePhotoClick();
            }
        }

        /**
         * 获取图片的src
         * @param index 图片的下标
         * @returns {*}
         * @private
         */
        function _getSrc(index){
            if (index == $scope.vm.Photos.length - 1){
                //如果是最后一张就返回拍照图片
                return "././asset/img/photo1.png";
            }else{
                return 'data:image/png;base64,'+$scope.vm.Photos[index].FileBase64Content;
            }
        }

        /**
         * 拍照事件
         * @private
         */
        function _takePhotoClick() {
            location.href = "app:take-photo?px=800&kb=80";
            _checkHasXrmDevieImageData();
        }

        /**
         * 查看是否拍照完成
         * @private
         */
        function _checkHasXrmDevieImageData() {
            if (window.XrmImageData === undefined) {
                $scope.XrmImageData = true;
            }
            if (window.XrmImageData && window.XrmImageData.getXrmImageData && window.XrmImageData.getXrmImageData() !== "") {
                $scope.isTakePhoto = true;

                var imageData = window.XrmImageData.getXrmImageData();
                //保存拍照的图片
                $scope.vm.Photos[$scope.vm.Photos.length - 1] = {FileBase64Content:imageData};
                //在最后的位置上添加拍照图标图片
                $scope.vm.Photos.push({FileBase64Content:""});

                if ($scope.XrmImageData) {
                    window.XrmImageData = undefined;
                }
                window.XrmImageData.clearImageData();
                return;
            }
            $timeout(_checkHasXrmDevieImageData, 1500);
        }

        /**
         * 确定按钮点击事件
         * @private
         */
        function _btnOkClick(){
            if(rt.isNullOrEmptyString($scope.vm.Result))
            {
                rt.showErrorToast("请输入处理结果后再确定");
                return;
            }
            $scope.vm.Photos.pop();

            $scope.vm.handledstatus = 20;
            ComplaintService.saveComplaint($scope.vm)
                .success(function (data){
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功");
                    $ionicHistory.goBack();
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载数据...");
        }

        init();
    }

    xrmApp.controller("ComplaintEditController", ['$scope', '$state', '$stateParams','rt','$timeout','$ionicHistory','ComplaintService', ComplaintEditController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function ComplaintListController($scope, $state, rt,$ionicActionSheet,ComplaintService) {
        //当前的页数
        var _pageIndex = 1;
        //每页显示条数`
        var _pageSize = rt.getPaginationSize();

        function _init() {
            $scope.searchClick = _searchClick;
            $scope.itemClick = _itemClick;
            $scope.appraiseClick = _appraiseClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.loadMore = _loadMore;
            $scope.selectViewMenu = _selectViewMenu;
            $scope.chooseType = _chooseType;
            $scope.type = 0;
            //返回按钮点击事件
            $scope.goBackClick = _goBackClick;

            $scope.views = _getViewList();
            $scope.viewType = $scope.views[ComplaintService.status];
            $scope.txt = {};
            $scope.txt.queryValue = "";

            _getComplaintList(function(data){
                $scope.vm = data;
            });
        }

        /**
         * 获取投诉信息列表
         * @private
         */
        function _getComplaintList(success)
        {
            ComplaintService.getComplaintList($scope.viewType.value,$scope.txt.queryValue,_pageIndex, _pageSize)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (success) {
                        success(data);
                    }
                    //判断是否还有数据
                    if (data.length >= _pageSize) {
                        $scope.isLoadMore = true;
                    } else {
                        $scope.isLoadMore = false;
                    }
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载数据...");
        }

        /**
         * 获取点评数据
         * @private
         */
        function _loadAppraiseList(success){
            ComplaintService.getAppraiseList(_pageIndex, _pageSize)
                .success(function(data){
                    if (success) {
                        success(data);
                    }
                    //判断是否还有数据
                    if (data.length >= _pageSize) {
                        $scope.isLoadMore = true;
                    } else {
                        $scope.isLoadMore = false;
                    }
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 查询按钮点击事件
         * @private
         */
        function _searchClick()
        {
            _viewRefresh();
        }

        /**
         * 下拉刷新事件
         * @private
         */
        function _viewRefresh()
        {
            _pageIndex = 1;
            // 投诉信息下拉刷新
            if($scope.type === 0){
                _getComplaintList(function (data) {
                    $scope.vm = data;
                });
            }
            // 点评信息下拉信息
            if($scope.type === 1){
                _loadAppraiseList(function(data){
                    $scope.appraiseList = data;
                });
            }
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMore()
        {
            _pageIndex++;
            // 投诉信息加载更多
            if($scope.type === 0){
                _getComplaintList(function (data) {
                    $scope.vm.push.apply($scope.vm, data);
                });
            }
            // 点评信息加载更多
            if($scope.type === 1){
                _loadAppraiseList(function(data){
                    $scope.appraiseList.push.apply($scope.appraiseList, data);
                });
            }
            $scope.$broadcast('scroll.infiniteScrollComplete');
        }

        /**
         * 列表点击事件
         * @private
         */
        function _itemClick(id)
        {
            //跳转到只读界面
            $state.go('complaint-read', {id: id});
        }

        /**
         * 列表点击事件
         * @param id
         * @private
         */
        function _appraiseClick(id){
            $state.go('appraise-read', {id: id});
        }

        function _chooseType(index){
            $scope.type = index;
            ComplaintService.status = index;
            $scope.viewType = $scope.views[index];
            _viewRefresh();
        }

        /**
         * 选择视图
         * @private
         */
        function _selectViewMenu()
        {
            var selectedButtons = [
                {text: $scope.views[0].name},
                {text: $scope.views[1].name}
            ];
            _showActionSheet(selectedButtons);
        }

        /**
         * 显示视图切换的actionSheet
         */
        function _showActionSheet(selectedButtons) {
            $ionicActionSheet.show({
                buttons: selectedButtons,
                titleText: '选择视图',
                cancelText: '取消',
                cancel: function () {
                },
                buttonClicked: function (index) {
                    ComplaintService.status = index;
                    $scope.viewType = $scope.views[index];
                    _viewRefresh();
                    return true;
                }
            });
        }
        /**
         * 初始化视图
         * @private
         */
        function _getViewList() {
            var views = [
                {
                    name:"我的投诉",
                    value:10
                },
                {
                    name:"我的点评",
                    value:20
                }
            ];
            return views;
        }
        _init();
    }

    xrmApp.controller("ComplaintListController", ['$scope', '$state', 'rt','$ionicActionSheet','ComplaintService', ComplaintListController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function ComplaintReadOnlyController($scope, $state, $stateParams,rt, ComplaintService) {

        function init() {
            $scope.btnGoRun = _btnGoRun;
            $scope.getSrc = _getSrc;
            _loadData();
        }

        /**
         * 加载表单数据
         * @private
         */
        function _loadData()
        {
            ComplaintService.getComplaint($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.vm = data;
                })
                .error(function(data){
                    rt.hideLoadingToast();
                });
            rt.showLoadingToast("正在加载数据...");
        }

        function _getSrc(index){
            return "data:image/png;base64,"+$scope.vm.Photos[index].FileBase64Content;
        }

        /**
         * 去处理按钮的事件
         * @param id 投诉信息的id
         * @private
         */
        function _btnGoRun(id){
            $state.go('complaint-edit',{id:id});
        }

        init();
    }

    xrmApp.controller("ComplaintReadOnlyController", ['$scope', '$state', '$stateParams','rt','ComplaintService', ComplaintReadOnlyController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function HotelComplaintDetailController($scope, $state, rt,$stateParams,$ionicActionSheet,ComplaintService) {
        //当前的页数
        var _pageIndex = 1;
        //每页显示条数`
        var _pageSize = rt.getPaginationSize();

        function _init() {
            $scope.viewRefresh = _viewRefresh;
            $scope.loadMore = _loadMore;
            //返回按钮点击事件
            $scope.goBackClick = _goBackClick;

            _getComplaintList(function(data){
                $scope.vm = data;
            });
        }

        /**
         * 获取投诉信息列表
         * @private
         */
        function _getComplaintList(success)
        {
            ComplaintService.getComplaintListByHotelId($stateParams.hotelId,_pageIndex, _pageSize)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (success) {
                        success(data);
                    }
                    //判断是否还有数据
                    if (data.length >= _pageSize) {
                        $scope.isLoadMore = true;
                    } else {
                        $scope.isLoadMore = false;
                    }
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载数据...");
        }


        /**
         * 下拉刷新事件
         * @private
         */
        function _viewRefresh()
        {
            _pageIndex = 1;
            _getComplaintList(function (data) {
                $scope.vm = data;
            });
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMore()
        {
            _pageIndex++;
            _getComplaintList(function (data) {
                $scope.vm.push.apply($scope.vm, data);
            });
            $scope.$broadcast('scroll.infiniteScrollComplete');
        }


        _init();
    }

    xrmApp.controller("HotelComplaintDetailController", ['$scope', '$state', 'rt','$stateParams','$ionicActionSheet','ComplaintService', HotelComplaintDetailController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function DailyEditController($scope, $state, $stateParams, rt, $filter, $ionicHistory, $ionicPlatform, DailyService) {

        function init() {
            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 2;
            $scope.dp.smallHeight = 40;

            $scope.labelClick = _labelClick;
            $scope.btnSaveClick = _btnSaveClick;

            $scope.vm = {};
            if (rt.isNullOrEmptyString($stateParams.id)) {
                $scope.vm.Id = rt.newGuid();
                $scope.vm.Time = new Date();
                _loadLabel();
            } else {
                _loadLabelAndDailyLog();
            }

            $scope.isAndroid = $ionicPlatform.is("android");
            $scope.isIOS = $ionicPlatform.is("ios");
        }

        /**
         * 获取工作日志的信息
         * @private
         */
        function _loadLabelAndDailyLog() {
            rt.showLoadingToast("正在加载...");

            DailyService.getDailyLog($stateParams.id)
                .then(function (response) {
                    var data = response.data;
                    if (!rt.isNull(data.Time)) {
                        data.Time = new Date(data.Time);
                    }

                    $scope.vm = data;

                    return DailyService.getDailyLogLabel();
                })
                .then(function (response) {
                    var data = response.data;

                    $scope.labels = data;
                    //获取标签的长度
                    var length = $scope.labels.length;
                    for (var i = 0; i < length; i++) {
                        //设置表单标签选中
                        if ($scope.labels[i].Id == $scope.vm.LabelId.Id) {
                            $scope.labels[i].choose = 1;
                        }
                    }

                    rt.hideLoadingToast();
                }, function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 加载日志信息的标签
         * @private
         */
        function _loadLabel() {
            rt.showLoadingToast("正在加載...");
            DailyService.getDailyLogLabel()
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.labels = data;
                    //获取标签的长度
                    var length = $scope.labels.length;
                    for (var i = 0; i < length; i++) {
                        //设置表单标签选中
                        if ($scope.labels[i].Id == $scope.vm.LabelId.Id) {
                            $scope.labels[i].choose = 1;
                        }
                    }
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
        }

        /**
         * 标签点击事件
         * @param index
         * @private
         */
        function _labelClick(index) {
            var length = $scope.labels.length;
            for (var i = 0; i < length; i++) {
                $scope.labels[i].choose = 0;
            }
            $scope.labels[index].choose = 1;
            $scope.vm.LabelId = {
                Id: $scope.labels[index].Id,
                Name: $scope.labels[index].Name
            };
        }

        /**
         * 保存按钮点击事件
         * @private
         */
        function _btnSaveClick() {

            if (!$scope.vm.LabelId) {
                rt.showErrorToast("请选择日志标签");
                return;
            }

            rt.showLoadingToast("正在保存...");
            DailyService.save($scope.vm)
                .success(function (data) {
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功");
                    $ionicHistory.goBack();
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast();
        }

        init();
    }

    xrmApp.controller("DailyEditController", ['$scope', '$state', '$stateParams', 'rt', '$filter', '$ionicHistory', '$ionicPlatform', 'DailyService', DailyEditController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function DailyListController($scope, $state,rt, $stateParams,DailyService) {
        //当前的页数
        var _pageIndex = 1;
        //每页显示条数
        var _pageSize = rt.getPaginationSize();

        function init() {
            $scope.chooseTimeSpace = _chooseTimeSpace;
            $scope.addClick = _addClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.loadMore = _loadMore;
            $scope.itemClick = _itemClick;
            //返回按钮点击事件
            $scope.goBackClick = _goBackClick;
            //默认选中今天
            $scope.timeSpace = DailyService.timeSpace;
            $scope.isLoadMore = false;

            _getDailyList(function(data){
                $scope.vm = data;
            });

        }

        /**
         * 列表项点击事件
         * @param id 工作日志的id
         * @private
         */
        function _itemClick(id){
            $state.go('daily-edit',{id:id});
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 切换日期间隔
         * @param timeSpace
         * @private
         */
        function _chooseTimeSpace(timeSpace){
            $scope.timeSpace = timeSpace;
            DailyService.timeSpace = timeSpace;
            _viewRefresh();
        }

        /**
         * 添加按钮点击事件
         * @private
         */
        function _addClick() {
            $state.go("daily-edit");
        }

        /**
         * 获取日志列表数据
         * @param success 成功回掉函数
         * @private
         */
        function _getDailyList(success){
            DailyService.getDailyList($scope.timeSpace,_pageIndex,_pageSize)
                .success(function(data){
                    rt.hideLoadingToast();
                    if (success){
                        success(data);
                    }

                    if (data.length < _pageSize){
                        $scope.isLoadMore = false;
                    }else{
                        $scope.isLoadMore = true;
                    }
                })
                .error(function(data){
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast();
        }

        /**
         * 下拉刷新事件
         * @private
         */
        function _viewRefresh()
        {
            _pageIndex = 1;
            _getDailyList(function (data) {
                $scope.vm = data;
                $scope.$broadcast('scroll.refreshComplete');
            });
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMore()
        {
            _pageIndex++;
            _getDailyList(function (data) {
                $scope.vm.push.apply($scope.vm, data);
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }

        init();
    }

    xrmApp.controller("DailyListController", ['$scope', '$state', 'rt', '$stateParams','DailyService', DailyListController]);
})();



/*global xrmApp:false*/
(function () {
    'use strict';

    xrmApp.controller("ChooseHotelController", ['$scope', '$stateParams', 'rt', '$timeout', 'DialogService', ChooseHotelController]);
    function ChooseHotelController($scope, $stateParams, rt, $timeout, DialogService) {
        var _pageIndex = 1;
        var _pageSize = rt.getPaginationSize();

        //是否需要加载更多
        var _isLoadMore = false;

        function _init() {
            $scope.vm = {};
            $scope.vm.queryValue = "";

            $scope.vm.data = [];
            //加载更多按钮点击事件
            $scope.loadMoreClick = _loadMoreClick;
            //下拉刷新
            $scope.viewRefresh = _viewRefresh;
            $scope.search = _search;
            $scope.selectData = _selectData;
            $scope.removeValue = _removeValue;

            rt.showLoadingToast("正在加载数据...");
            _loadData(function (data) {
                rt.hideLoadingToast();
                $scope.vm.data = data;
            }, function (error) {
                rt.hideLoadingToast();
            });
        }

        /**
         * 加载数据
         * @private
         */
        function _loadData(success, msg) {
            DialogService.getOwnerHotel($scope.vm.queryValue, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        _isLoadMore = false;
                    }
                    else {
                        _isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (msg) {
                        msg();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _loadData(function (data) {
                $scope.vm.data = data;
            }, null);
            //Stop the ion-refresher from spinning
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMoreClick() {
            if (_isLoadMore) {
                _pageIndex += 1;
                _loadData(function (data) {
                    $scope.vm.data.push.apply($scope.vm.data, data);
                }, null);
                //Stop the ion-refresher from spinning
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
            else {
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
        }

        /**
         * 搜索框搜索按钮点击事件
         * @private
         */
        function _search() {
            _pageIndex = 1;
            rt.showLoadingToast("正在加载数据...");
            _loadData(function (data) {
                rt.hideLoadingToast();
                $scope.vm.data = data;
            }, function (error) {
                rt.hideLoadingToast();
            });
        }

        /**
         * 选择
         */
        function _selectData(hotel) {
            $scope.closeDialog();
            if (rt.isFunction($scope.onDataSelected) && !rt.isNull(hotel)) {
                $scope.onDataSelected(hotel);
            }
        }

        /**
         * 移除值
         * @param user
         * @private
         */
        function _removeValue(acc) {
            $scope.closeDialog();
            $scope.onDataSelected(acc);
        }

        _init();
    }
})();
/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("PreviewImageController", ['$scope',PreviewImageController]);

    function PreviewImageController($scope) {
        function _init() {
            $scope.imageWidth = (document.documentElement.clientWidth || document.body.clientWidth);
            $scope.imageHeight = (document.documentElement.clientHeight || document.body.clientHeight);
        }
        _init();
    }
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */

(function () {
    'use strict';

    function ApplicationController($scope, $state, rt) {
        $scope.vm = {};
        $scope.vm.menus = [];
        function init() {
            _loadMenuData();
        }

        function _loadMenuData(){
            var apiUrl = "api/SystemMenu/GetMobileSystemMenu?lastQueryTime=";
            rt.get(apiUrl)
                .success(function(data){
                    var menus = data.SystemMenuList;
                    for(var i=0;i<menus.length;i++){
                        var pMenu = menus[i];

                        for(var j=0;j<pMenu.Children.length;j++){
                            var cMenu = pMenu.Children[j];
                            $scope.vm.menus.push(cMenu);
                        }
                    }
                });
        }
        
        init();
    }

    xrmApp.controller("ApplicationController", ['$scope', '$state', 'rt', ApplicationController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function Login2Controller($scope, $state, $stateParams, $timeout, rt, $window) {

        function init() {
            $scope.logoWidth = (document.documentElement.clientWidth || document.body.clientWidth);

            $scope.vm = {};
            $scope.vm.isSavePassword = !!localStorage.XrmLoginIsSavePassword;
            $scope.vm.userName = localStorage.XrmLoginUserName;
            $scope.vm.password = localStorage.XrmLoginPassword;

            $scope.savePassword = function () {
                $scope.vm.isSavePassword = !$scope.vm.isSavePassword;
            };

            $scope.setServerAddress = function () {
                $state.go("home-serveraddressedit");
            };

            $scope.refreshCode = _refreshCheckcode;
            $scope.login = _login;

            _refreshCheckcode();

            //_data();
        }


        function _refreshCheckcode() {
            var url = "api/Authentication/GetCheckCode?seed=" + Math.random();
            rt.get(url, true, true)
                .then(function (resp) {
                    $scope.vm.verifyStr0 = resp.data[0];
                    $scope.vm.verifyStr1 = resp.data[1];
                    $scope.vm.verifyStr2 = resp.data[2];
                });
        }

        /**
         * 登陆
         * @private
         */
        function _login() {
            if(rt.isNullOrEmptyString($scope.vm.userName) || rt.isNullOrEmptyString($scope.vm.password)){
                return;
            }

            localStorage.XrmLoginIsSavePassword = $scope.vm.isSavePassword;
            localStorage.XrmLoginUserName = $scope.vm.userName;
            localStorage.XrmLoginPassword = $scope.vm.password;

            rt.showLoadingToast("登录中...");
            rt.post("api/Authentication/login2", {
                uid: $scope.vm.userName,
                pwd: $scope.vm.password,
                checkCode: $scope.vm.checkcode,
                verifyStr: $scope.vm.verifyStr2
            })
                .success(function (u) {
                    rt.hideLoadingToast();
                    localStorage.XrmAuthToken = u.AuthToken;
                    localStorage.UserId = u.SystemUserId;
                    localStorage.UserName = u.UserCode;
                    localStorage.HotelId = "053128";
                    $state.go("home-app");
                })
                .error(function (msg) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(msg);
                });
        }

        function _data() {
            rt.queryData("getmenus",
                {'@fullname': 'BJ肖云'},
                'FullName',
                1,
                function (data) {
                    rt.showErrorToast(data);
                },
                function (msg) {
                    rt.showErrorToast(msg);
                });

            rt.saveData("new_notice", rt.newGuid(), {
                "new_name": "joesong",
                "new_buid": "B432747F-26BE-E411-80B9-000C293C0BE1",
                "new_ispublish": 1,
                "new_publishdate": '2015-5-1 15:13:14',
                "new_type": 4
            });

            rt.filteredGetData("new_notice", '779270EC-2E4D-1227-837F-3F90D40DBD12',
                function (data) {
                    rt.showErrorToast(data);
                },
                function (msg) {
                    rt.showErrorToast(msg);
                });

            rt.getData("new_notice", '779270EC-2E4D-1227-837F-3F90D40DBD12',
                function (data) {
                    rt.showErrorToast(data);
                },
                function (msg) {
                    rt.showErrorToast(msg);
                });
        }

        init();
    }

    xrmApp.controller("Login2Controller", ['$scope', '$state', '$stateParams', '$timeout', 'rt', '$window', Login2Controller]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function LoginController($scope, $state, $stateParams, $timeout, rt, $window) {

        function init() {
        }

        init();
    }

    xrmApp.controller("LoginController", ['$scope', '$state', '$stateParams', '$timeout', 'rt', '$window', LoginController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';
    xrmApp.controller("ServerAddressEditController", ['$scope','$ionicHistory', ServerAddressEditController]);

    function ServerAddressEditController($scope,$ionicHistory) {
        _init();

        function _init() {
            var ch = (document.documentElement.clientWidth || document.body.clientWidth);
            $scope.logoWidth = ch;

            $scope.save = _save;
            $scope.vm = {};
            $scope.vm.address = localStorage.XrmBaseUrl;
        }

        function _save() {
            localStorage.XrmBaseUrl = $scope.vm.address;
            $ionicHistory.goBack();
        }
    }
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function HomeinnsListController($scope, $state,rt, $stateParams, HomeinnsService) {

        function init() {
            $scope.goBackClick = _goBackClick();

            window.open("app:openHeader@" + "http://t1.faqrobot.org/robot/mobileFree.html?sysNum=142059471003130");
        }

        function _getK2Token(){
            HomeinnsService.getK2Token()
                .success(function(data){
                    if(data.errCode !== "0"){
                        rt.showErrorToast(data.errMsg);
                        return;
                    }
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        init();
    }

    xrmApp.controller("HomeinnsListController", ['$scope', '$state', 'rt', '$stateParams','HomeinnsService', HomeinnsListController]);
})();



(function () {
    'use strict';

    xrmApp.controller("HotelDetailController", ['$scope', '$state', '$stateParams', 'rt', 'HotelService', HotelDetailController]);

    function HotelDetailController($scope, $state, $stateParams, rt, HotelService) {
        //当前页数
        var _pageIndex = 1;
        //每页显示数目
        var _pageSize = rt.getPaginationSize();

        function init() {
            $scope.qcScoreClick = _qcScoreClick;
            $scope.pmsRoomStateClick = _pmsRoomStateClick;
            $scope.startVisit = _startVisit;
            $scope.vm = {};
            $scope.vm.isShowManageOrNot = false;//是否显示店长巡店数据，用于显示巡店记录的分类（城总/区总，店长）
            $scope.onItemClick = _onItemClick;
            $scope.visitedHistoryClick = _visitedHistoryClick;
            $scope.complaintClick = _complaintClick;
            $scope.loadMoreClick = _loadMoreClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.chooseViewClick = _chooseViewClick;

            rt.showLoadingToast('正在加载数据...');
            _loadData(function (data) {
                rt.hideLoadingToast();
                $scope.hotel = data;
            }, function () {
                rt.hideLoadingToast();
            });

            //判断当前人员是否有开始巡查的权限
            HotelService.canStartVisit()
                .success(function (data) {
                    $scope.canStartVisit = data;
                })
                .error(function (error) {
                    rt.showErrorToast(error);
                });
        }

        /**
         * 巡店历史
         * @private
         */
        function _visitedHistoryClick() {
            alert("未开放!");
        }

        /**
         *加载数据
         * @param success
         * @param failure
         * @private
         */
        function _loadData(success, failure) {
            HotelService.getHotelDetailById($stateParams.id, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.VisitList.length < _pageSize) {
                        $scope.isLoadMore = false;
                    }
                    else {
                        $scope.isLoadMore = true;
                    }
                    $scope.vm.hotelIdCode = data.Id;
                    var hotelId = "[" + "\"" + data.Code + "\"" + "]";
                    HotelService.getHotelCurrentDayFlow(hotelId)
                        .success(function(data){
                            rt.hideLoadingToast();
                            if(data.HotelFlowList.length <= 0){
                                rt.showErrorToast("没有当前流量数据！");
                            }
                            var flow = (((data.HotelFlowList[0].SumRoomCount - data.HotelFlowList[0].BookableRoomCount) / data.HotelFlowList[0].SumRoomCount) * 100).toFixed(2);
                            $scope.currentFlow = isNaN(flow) ? 0 : flow;
                        })
                        .error(function(err){
                            rt.hideLoadingToast();
                            rt.showErrorToast(err);
                        });
                })
                .error(function (error) {
                    if (failure) {
                        failure();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 跳转qc成绩
         * @param $stateParams
         * @private
         */
        function _qcScoreClick() {
            $state.go("qc-score", {id: $stateParams.id});
        }

        /**
         * 跳转pms房态
         * @private
         */
        function _pmsRoomStateClick() {
            $state.go("room-state", {id: $stateParams.id});
        }

        /**
         * 投诉信息点击事件
         * @private
         */
        function _complaintClick() {
            $state.go("complaint-hotel", {hotelId: $stateParams.id});
        }

        /**
         * 开始拜访
         * @private
         */
        function _startVisit() {
            //新建的时候需要传id
            if (rt.isNullOrEmptyString($scope.vm.Id)) {
                $scope.vm.Id = rt.newGuid();
            }

            HotelService.startCityVisit($stateParams.id, $scope.vm.Id)
                .success(function (data) {
                    $state.go("cityVisit-main", {id: data});
                })
                .error(function (error) {
                    rt.showErrorToast(error);
                });
        }

        /**
         * 加载更多事件
         * @private
         */
        function _loadMoreClick() {
            _pageIndex += 1;
            if(!$scope.vm.isShowManageOrNot){//城总/区总巡店记录
                _loadData(function (data) {
                    $scope.hotel.VisitList.push.apply($scope.hotel.VisitList, data.VisitList);
                    $scope.$broadcast('scroll.infiniteScrollComplete');
                });
            }else{//店长巡店记录
                _loadDataByManage(function (data) {
                    $scope.vm.visitList.push.apply($scope.vm.visitList, data);
                    $scope.$broadcast('scroll.infiniteScrollComplete');
                });
            }
        }

        /**'
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            if(!$scope.vm.isShowManageOrNot){
                _loadData(function (data) {
                    $scope.hotel.VisitList = data.VisitList;
                }, null);
            }else{
                _loadDataByManage(function (data) {
                    $scope.vm.visitList = data;
                    rt.hideLoadingToast();
                }, function () {
                    rt.hideLoadingToast();
                });
            }
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 列表项的点击事件
         * @param id
         * @private
         */
        function _onItemClick(visit) {
            if(!$scope.vm.isShowManageOrNot){
                $state.go("cityVisit-main", {id: visit.Id});
            }else{
                $state.go("hotel-form", {id: visit.Id, hotelid: $scope.vm.hotelIdCode});
            }
        }


        /**
         * 视图切换
         * @param type
         * @private
         */
        function _chooseViewClick(type) {
            if (type === 1) {//城总巡店记录
                _pageIndex = 1;
                $scope.vm.isShowManageOrNot = false;
                rt.showLoadingToast("正在加载数据...");
                _loadData(function (data) {
                    rt.hideLoadingToast();
                    $scope.hotel = data;
                }, function () {
                    rt.hideLoadingToast();
                });
            } else if (type === 2) {//店长巡店记录
                _pageIndex = 1;
                $scope.vm.isShowManageOrNot = true;
                rt.showLoadingToast("正在加载数据...");
                _loadDataByManage(function (data) {
                    $scope.vm.visitList = data;
                    rt.hideLoadingToast();
                }, function () {
                    rt.hideLoadingToast();
                });
            } else {
                return;
            }
        }

        /**
         * 新增店长巡店记录，显示店长门店的巡店日志
         * @param success
         * @param failure
         * @private
         */
        function _loadDataByManage(success, failure) {
            HotelService.getManageVisit($scope.vm.hotelIdCode, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        $scope.isLoadMore = false;
                    }
                    else {
                        $scope.isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (failure) {
                        failure();
                    }
                    rt.showErrorToast(error);
                });
        }

        init();
    }
})();

/*global BMap:false */
/*global BMAP_ANCHOR_BOTTOM_RIGHT*/
(function () {
    'use strict';

    xrmApp.controller("HotelMapController", ['$scope', '$state', '$compile', 'rt', 'HotelService', HotelMapController]);

    function HotelMapController($scope, $state, $compile, rt, HotelService) {
        //定义百度地图的地图对象
        var _map;
        //百度地图的缩放级别，大多数情况下使用14
        var _mapScale = 14;
        //显示百度地图的div的id
        var _mapDivId = "addressMap";
        $scope.vm = [];
        //搜索框绑定查询字段
        $scope.vm.queryValue = "";


        function _init() {
            $scope.hotelList = [];
            $scope.hotelNames = [];

            $scope.setQueryValue = _setQueryValue;

            $scope.searchClick = _searchClick;
            $scope.goBackClick = _goBackClick;
            $scope.detailClick = _detailClick;

            $scope.retrieveHotelsByKeyword = _retrieveHotelsByKeyword;

            _locateAndLoadHotelList();
        }

        function _retrieveHotelsByKeyword() {
            if (rt.isNullOrEmptyString($scope.vm.queryValue)) {
                $scope.hotelNames = [];
                return;
            }

            HotelService.getTopNHotelList($scope.vm.queryValue)
                .success(function (data) {
                    $scope.hotelNames = data;
                })
                .error(function (errorMessage) {
                    rt.showErrorToast(errorMessage);
                });
        }

        function _setQueryValue(queryValue) {
            $scope.vm.queryValue = queryValue;
            $scope.hotelNames = [];

            _searchClick();
        }

        /**
         * 获取当前位置一定范围内的门店
         */
        function _locateAndLoadHotelList() {
            rt.getGeolocation(function (position) {
                if (position === null) {
                    return;
                }

                _map = new BMap.Map(_mapDivId);
                _map.centerAndZoom(new BMap.Point(position.longitude, position.latitude), _mapScale);
                var zoomControl = new BMap.ZoomControl();
                _map.addControl(zoomControl);//添加缩放控件
                var scaleControl = new BMap.ScaleControl();
                _map.addControl(scaleControl);//添加比例尺控件

                // 创建控件实例
                var myZoomCtrl = new ZoomControl();
                // 添加到地图当中
                _map.addControl(myZoomCtrl);

                //创建当前位置的点
                var marker = new BMap.Marker(new BMap.Point(position.longitude, position.latitude));
                //添加到地图中
                _map.addOverlay(marker);

                _loadHotelList("",position);
            });
        }

        /**
         * 根据点获取一定范围内的门店
         * @param position
         * @private
         */
        function _loadHotelList(queryValue,position) {
            HotelService.getHotelList(queryValue, position.longitude, position.latitude)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.vm.hotelList = data;

                    if (data === null || data.HotelList === null) {
                        return;
                    }

                    _addPoint(data.PositionList);
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 地图上添加点并显示
         * @param points
         * @private
         */
        function _addPoint(points) {
            _map.clearOverlays();
            var myIcon = new BMap.Icon("asset/img/bluemark.png", new BMap.Size(32, 45), {anchor: new BMap.Size(32, 45)});
            for (var i = 0; i < points.length; i++) {
                //构造地图显示的点
                var marker = new BMap.Marker(new BMap.Point(points[i].Longitude, points[i].Latitude), {icon: myIcon});
                _map.addOverlay(marker);
                setInfoWindow(marker, points[i]);
            }
        }


        /**
         * 设置标注的信息窗口
         * @param marker
         * @param title
         * @param address
         */
        function setInfoWindow(marker, point) {
            var content = '<div>';
            for (var i = 0; i < $scope.vm.hotelList.HotelList.length; i++) {
                if ($scope.vm.hotelList.HotelList[i].Longitude === point.Longitude &&
                    $scope.vm.hotelList.HotelList[i].Latitude === point.Latitude) {
                    var hotelId = $scope.vm.hotelList.HotelList[i].Id;
                    content += '<input ng-click="detailClick(' + i + ')" type="radio" name="hotel"/>' + $scope.vm.hotelList.HotelList[i].Name + '<br/>';
                }
            }
            content += '</div>';
            _setInfo(marker, content);
        }

        function _setInfo(marker, content) {
            var compiled = $compile(content)($scope);
            var infoWindow = new BMap.InfoWindow(compiled[0], null);
            infoWindow.addEventListener("clickclose", function (e) {
                infoWindow.hide();
            });
            marker.addEventListener("click", function (e) {
                this.openInfoWindow(infoWindow);
            });
        }

        /**
         * 跳转门店详情页面
         * @private
         */
        function _detailClick(index) {
            var id = $scope.vm.hotelList.HotelList[index].Id;
            document.activeElement.checked = true;
            $state.go("hotel-detail", {id: id});
        }

        /**
         * 搜索
         * @private
         */
        function _searchClick() {
            if (rt.isNullOrEmptyString($scope.vm.queryValue)) {
                rt.showErrorToast("请输入酒店名称或详细地址");
                return;
            }

            rt.showLoadingToast("正在搜索...");
            HotelService.getHotelList($scope.vm.queryValue, "0", "0")
                .success(function (data) {
                    rt.hideLoadingToast();

                    //如果根据酒店名称能够从服务器端查出数据，则直接在界面上显示
                    if (data !== null && data.PositionList !== null && data.PositionList.length > 0) {
                        $scope.vm.hotelList = data;

                        _addPoint(data.PositionList);

                        _map.centerAndZoom(new BMap.Point(data.PositionList[0].Longitude, data.PositionList[0].Latitude), _mapScale);
                        return;
                    }

                    //否则根据输入的位置进行定位，再查找
                    rt.showLoadingToast("正在定位...");

                    var myGeo = new BMap.Geocoder();
                    myGeo.getPoint($scope.vm.queryValue, function (point) {
                        rt.hideLoadingToast();
                        if (point) {
                            if (_map === null) {
                                _map = new BMap.Map(_mapDivId);
                            }
                            _map.centerAndZoom(point, _mapScale);

                            _loadHotelList("",{"latitude":point.lat,"longitude":point.lng});
                        } else {
                            rt.showErrorToast("未找到您输入的位置，请输入更精确的地址");
                        }
                    });

                })
                .error(function (errorMessage) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(errorMessage);
                });
        }

        /**
         * 退出
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        // 定义一个控件类，即function
        //定义一个刷新位置的控件
        function ZoomControl() {
            // 设置默认停靠位置和偏移量
            this.defaultAnchor = BMAP_ANCHOR_BOTTOM_RIGHT;
            this.defaultOffset = new BMap.Size(9, 160);
        }

        // 通过JavaScript的prototype属性继承于BMap.Control
        ZoomControl.prototype = new BMap.Control();

        // 自定义控件必须实现initialize方法，并且将控件的DOM元素返回
        // 在本方法中创建个div元素作为控件的容器，并将其添加到地图容器中
        ZoomControl.prototype.initialize = function (map) {
            // 创建一个DOM元素
            var div = document.createElement("div");
            // 添加文字说明
            div.innerHTML = '<img  style="width: 35px;height: 35px;" src="././asset/img/map_refresh.png" />';
            // 设置样式
            //div.style.cursor = "pointer";
            //div.style.border = "1px solid gray";
            div.style.backgroundColor = "white";
            div.style.height = "35px";
            div.style.width = "35px";
            // 绑定事件，点击一次放大两级
            div.onclick = function (e) {
                rt.showLoadingToast("正在加载数据...");
                var newCenterPoint = _map.getCenter();

                _map = new BMap.Map(_mapDivId);
                _map.centerAndZoom(new BMap.Point(newCenterPoint.lng, newCenterPoint.lat), _mapScale);
                var zoomControl = new BMap.ZoomControl();
                _map.addControl(zoomControl);//添加缩放控件
                var scaleControl = new BMap.ScaleControl();
                _map.addControl(scaleControl);//添加比例尺控件

                // 创建控件实例
                var myZoomCtrl = new ZoomControl();
                // 添加到地图当中
                _map.addControl(myZoomCtrl);

                //创建当前位置的点
                var marker = new BMap.Marker(new BMap.Point(newCenterPoint.lng, newCenterPoint.lat));
                //添加到地图中
                _map.addOverlay(marker);

                var position = {};
                position.longitude = newCenterPoint.lng;
                position.latitude = newCenterPoint.lat;
                _loadHotelList($scope.vm.queryValue,position);
            };
            // 添加DOM元素到地图中
            map.getContainer().appendChild(div);
            // 将DOM元素返回
            return div;
        };

        _init();
    }
})();



/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("HotelVisitDetailController", ['$scope', '$rootScope', '$state', '$stateParams', '$ionicHistory', 'rt','HotelService',HotelVisitDetailController]);
    function HotelVisitDetailController($scope, $rootScope, $state, $stateParams, $ionicHistory, rt, HotelService) {

        function _init() {
            $scope.previewIndex = 0;
            $scope.previewPicture = _previewPicture;
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast("正在加载数据...");
            HotelService.getManageIssueById($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.Issues=data;
                    //
                    if(rt.isNull($scope.Issues.Photos) || $scope.Issues.Photos.length <= 0){
                        $scope.Issues.Photos = rt.isNull(JSON.parse(localStorage.getItem($scope.Issues.Id))) ? [] : JSON.parse(localStorage.getItem($scope.Issues.Id));
                    }
                    $scope.photoes = $scope.Issues.Photos;
                    //
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });

        }

        /**
         * 预览照片
         * @param index
         * @private
         */
        function _previewPicture(photo) {
            $scope.previewPhoto = photo;
            rt.createDialog("module/dialog/previewImageView.html", $scope, null)
                .then(function (d) {
                    d.show();
                });
        }

        _init();
    }


})();
(function () {
    'use strict';

    xrmApp.controller("HotelVisitFormController", ['$scope', '$state', '$stateParams', 'rt', 'HotelService', HotelVisitFormController]);

    function HotelVisitFormController($scope, $state, $stateParams, rt, HotelService) {

        var _manageVisitId;
        var _hotelId;

        function init() {
            $scope.onItemClick = _onItemClick;
            $scope.isEnd = false;

            _manageVisitId = $stateParams.id;
            _hotelId = $stateParams.hotelid;

            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');

            HotelService.getManageVisitById(_manageVisitId, _hotelId)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (data.StartTime === "0001-01-01 00:00:00" || !data.StartTime) {
                        data.StartTime = "";
                    }
                    /*else {
                     data.StartTime = new Date(data.StartTime);
                     }*/
                    if (data.EndTime === "0001-01-01 00:00:00" || !data.EndTime) {
                        data.EndTime = "";
                    }
                    /*else {
                     data.EndTime = new Date(data.EndTime);
                     }*/

                    _manageVisitId = data.Id;

                    $scope.visit = data;
                    $scope.isEnd = data.Status.Value === 2;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         *列表项按钮点击事件
         * @param issue
         * @private
         */
        function _onItemClick(issue) {
            $state.go("hotel-visit", {id: issue.Id, isEnd: $scope.isEnd});
        }

        init();
    }
})();

(function () {
    'use strict';
    xrmApp.controller("PmsRoomStateController", ['$scope', '$stateParams', 'rt', 'HotelService', PmsRoomStateController]);

    function PmsRoomStateController($scope, $stateParams, rt, HotelService) {

        //所有
        var All=0;
        //脏房
        var Dirty = 1;
        //干净房
        var Clean = 2;
        //入住房
        var CheckIn=3;

        //构建所有房间数组
        $scope.roomArray=[];

        function init() {
            $scope.viewType=HotelService.getViewType();
            $scope.dirtyClick=_dirtyClick;
            $scope.cleanClick=_cleanClick;
            $scope.checkInClick=_checkInClick;
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            HotelService.getRoomList($stateParams.id,$scope.viewType.value)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.stateCount = {};
                    $scope.stateCount.VD = 0;
                    $scope.stateCount.VC = 0;
                    $scope.stateCount.OD = 0;
                    if(data.length > 0 && data !== null){
                        for(var i = 0; i < data.length; i++){
                            if(data[i].RoomState === 1){//脏房
                                $scope.stateCount.VD += 1;
                            }
                            else if(data[i].RoomState === 2){//干净房
                                $scope.stateCount.VC += 1;
                            }
                            else {//入住房
                                $scope.stateCount.OD += 1;
                            }
                        }
                    }
                    $scope.roomArray=[];
                    _dealRoomArray(data);
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 将接口返回数据拆分成一行四个
         * @param array
         * @private
         */
        function _dealRoomArray(array){
            var lines=array.length%4===0?array.length/4:parseInt(array.length/4)+1;
            for(var i=0;i<lines;i++){
                $scope.roomArray[i]=[];
                for(var j=0;j<4;j++){
                    $scope.roomArray[i][j]=array[i*4+j]===undefined?{}:array[i*4+j];
                }
            }
        }

        /**
         * 脏房点击
         * @private
         */
        function _dirtyClick(){
            $scope.viewType={name:'脏房',value:Dirty};
            _loadData();
        }

        /**
         * 干净房点击
         * @private
         */
        function _cleanClick(){
            $scope.viewType={name:'干净房',value:Clean};
            _loadData();
        }

        /**
         * 入住房点击
         * @private
         */
        function _checkInClick(){
            $scope.viewType={name:'入住房',value:CheckIn};
            _loadData();
        }
        init();
    }
})();

(function () {
    'use strict';
    xrmApp.controller("QcScoreController", ['$scope', '$stateParams', 'rt', 'HotelService', QcScoreController]);

    function QcScoreController($scope, $stateParams, rt, HotelService) {

        function init() {
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            HotelService.getQcScoreById($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.score = data;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }
        init();
    }
})();

(function () {
    'use strict';

    xrmApp.controller("EndManageVisitController", ['$scope', '$state', '$stateParams', '$ionicHistory', 'rt', 'ManageVisitService', EndManageVisitController]);

    function EndManageVisitController($scope, $state, $stateParams, $ionicHistory, rt, ManageVisitService) {
        //评价数组
        $scope.appriseArray = [];

        function init() {
            $scope.chooseClick = _chooseClick;
            $scope.endClick = _endClick;
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            ManageVisitService.getManageVisitById($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.visit = data;
                    for (var i = 0; i < data.Appraise.Options.length; i++) {
                        var option = {};
                        option.Name = data.Appraise.Options[i].Name;
                        option.Value = data.Appraise.Options[i].Value;
                        option.Checked = false;
                        $scope.appriseArray[i] = option;
                    }
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 评价按钮点击事件
         * @param apprise
         * @param index
         * @private
         */
        function _chooseClick(apprise, index) {
            for (var i = 0; i < $scope.appriseArray.length; i++) {
                if (i == index) {
                    $scope.appriseArray[i].Checked = true;
                } else {
                    $scope.appriseArray[i].Checked = false;
                }
            }
            $scope.visit.Appraise.Value = apprise.Value;
            $scope.visit.Appraise.Name = apprise.Name;
        }

        function _endClick() {
            if ($scope.visit.Appraise.Value !== 1 && $scope.visit.Appraise.Value !== 2 && $scope.visit.Appraise.Value !== 3) {
                rt.showErrorToast("评价没有选择！");
                return;
            }
            rt.showLoadingToast("正在处理...");

            //从本地加载图片，加载完成后上传至服务器，上传成功后，清空本地图片
            _loadVisitIssuePhotos(function (issues) {
                $scope.visit.IssuesList = issues;
                //
                ManageVisitService.endManageVisit($scope.visit)
                    .success(function () {
                        rt.hideLoadingToast();

                        // 照片上传成功后把本地的照片删除
                        for (var i = 0; i < $scope.visit.IssuesList.length; i++) {
                            cache.removeItem($scope.visit.IssuesList[i].Id, null, null);
                        }
                        //
                        $ionicHistory.goBack();
                        rt.showSuccessToast("结束巡店成功！");
                    })
                    .error(function (error) {
                        rt.hideLoadingToast();
                        rt.showErrorToast(error);
                    });

            }, function (errorMessage) {
                rt.hideLoadingToast();
                rt.showErrorToast(errorMessage);
            });
        }

        function _loadVisitIssuePhotos(success, failure) {
            var issues = [];

            _loadIssuePhotos(issues, 0, success, failure);
        }

        function _loadIssuePhotos(issues, index, success, failure) {
            var issue = $scope.visit.IssuesList[index];

            cache.getItem(issue.Id, function (data) {
                if (!rt.isNullOrEmptyString(data)) {
                    issue.Photos = JSON.parse(data);
                    issues.push(issue);
                }

                //继续处理下一个问题
                if (index >= $scope.visit.IssuesList.length - 1) {
                    success(issues);
                } else {
                    _loadIssuePhotos(issues, index + 1, success, failure);
                }
            }, function (error) {
                //没有照片，继续处理下一个问题
                if (error.code === 6) {
                    if (index >= $scope.visit.IssuesList.length - 1) {
                        success(issues);
                    } else {
                        _loadIssuePhotos(issues, index + 1, success, failure);
                    }
                    return;
                }
                failure(error.msg);
                return;
            });
        }

        init();
    }
})();

/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("ManageVisitDetailController", ['$scope', '$rootScope', '$state', '$stateParams', '$ionicHistory', 'rt', 'ManageVisitService', ManageVisitDetailController]);
    function ManageVisitDetailController($scope, $rootScope, $state, $stateParams, $ionicHistory, rt, ManageVisitService) {

        function _init() {
            $scope.previewIndex = 0;
            $scope.choosePicture = _choosePicture;
            $scope.deletePicture = _deletePicture;
            $scope.previewPicture = _previewPicture;
            $scope.completeClick = _completeClick;

            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast("正在加载数据...");
            ManageVisitService.getManageIssueById($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.Issues = data;
                    //如果服务器端没有照片，就从本地获取
                    if (rt.isNull($scope.Issues.Photos) || $scope.Issues.Photos.length <= 0) {
                        cache.getItem($scope.Issues.Id, function (data) {
                            var photos = JSON.parse(data);
                            $scope.photoes = $scope.Issues.Photos = rt.isNull(photos) ? [] : photos;
                        }, function (error) {
                            if (error.code === 6) {
                                return;
                            }
                            rt.showErrorToast(JSON.stringify(error));
                        });
                    }
                    else {
                        $scope.photoes = $scope.Issues.Photos;
                        //如果服务器端有照片，就把本地的照片清除，取服务器端的照片
                        cache.removeItem($scope.Issues.Id, null, null);
                    }
                    //
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });

        }

        /**
         * 完成点击事件
         * @private
         */
        function _completeClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Content)) {
                rt.showErrorToast("问题描述没有填写！");
                return;
            }

            try {
                cache.setItem($scope.Issues.Id, JSON.stringify($scope.Issues.Photos), null,
                    function () {
                        $scope.Issues.Photos = [];

                        rt.showLoadingToast("正在保存...");
                        ManageVisitService.saveManageVisitIssue($scope.Issues)
                            .success(function (data) {
                                rt.hideLoadingToast();
                                rt.showSuccessToast("保存成功！");
                                $ionicHistory.goBack();
                            })
                            .error(function (error) {
                                rt.hideLoadingToast();
                                rt.showErrorToast(error);
                            });

                    }, function (errorMessage) {
                        rt.showErrorToast(errorMessage);
                    });
            }
            catch (e) {
                rt.showErrorToast(e.message);
            }
        }

        /**
         * 选择照片
         * @private
         */
        function _choosePicture() {

            rt.takePhoto(800, 80, function (base64Image) {
                $scope.Issues.Photos.push({
                    "Id": rt.newGuid(),
                    "FileBase64Content": base64Image
                });
            });

            //rt.chooseImage({
            //    count: 1,
            //    px: 800,
            //    kb: 80,
            //    sourceType: ['album', 'camera'],
            //    success: function (base64Image) {
            //        try {
            //            $scope.Issues.Photos.push({
            //                "Id": rt.newGuid(),
            //                "FileBase64Content": base64Image
            //            });
            //        }
            //        catch (e) {
            //            rt.showErrorToast(e.message);
            //        }
            //    }
            //});
            $scope.photoes = $scope.Issues.Photos;
        }

        /**
         * 预览照片
         * @param index
         * @private
         */
        function _previewPicture(photo) {
            $scope.previewPhoto = photo;
            rt.createDialog("module/dialog/previewImageView.html", $scope, null)
                .then(function (d) {
                    d.show();
                });
        }

        /**
         * 删除照片
         * @param fileid
         * @private
         */
        function _deletePicture(fileId) {

            //检查是否已经添加过了
            var index = _.findIndex($scope.Issues.Photos, {Id: fileId});
            if (index < 0) {
                return;
            }

            $scope.Issues.Photos.splice(index, 1);
            $scope.photoes = $scope.Issues.Photos;

            cache.setItem($scope.Issues.Id, JSON.stringify($scope.Issues.Photos), null,
                function () {
                    //
                }, function (errorMessage) {
                    rt.showErrorToast(errorMessage);
                });
        }

        _init();
    }


})();
(function () {
    'use strict';

    xrmApp.controller("ManageVisitFormController", ['$scope', '$state', '$stateParams', 'rt', 'ManageVisitService', ManageVisitFormController]);

    function ManageVisitFormController($scope, $state, $stateParams, rt, ManageVisitService) {

        var _manageVisitId;

        function init() {
            $scope.endVisitClick = _endVisitClick;
            $scope.onItemClick = _onItemClick;
            $scope.isEnd = false;

            _manageVisitId = $stateParams.id;

            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');

            ManageVisitService.getManageVisitById(_manageVisitId)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (data.StartTime === "0001-01-01 00:00:00" || !data.StartTime) {
                        data.StartTime = "";
                    }
                    /*else {
                     data.StartTime = new Date(data.StartTime);
                     }*/
                    if (data.EndTime === "0001-01-01 00:00:00" || !data.EndTime) {
                        data.EndTime = "";
                    }
                    /*else {
                     data.EndTime = new Date(data.EndTime);
                     }*/

                    _manageVisitId = data.Id;

                    $scope.visit = data;
                    $scope.isEnd = data.Status.Value === 2;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 结束巡店
         * @private
         */
        function _endVisitClick() {
            $state.go("manage-end", {id: _manageVisitId});
        }

        /**
         *列表项按钮点击事件
         * @param issue
         * @private
         */
        function _onItemClick(issue) {
            $state.go("manage-detail", {id: issue.Id, isEnd: $scope.isEnd});
        }

        init();
    }
})();

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */

(function () {
    'use strict';
    xrmApp.controller("ManageVisitListController", ['$scope', '$state', '$ionicListDelegate', '$stateParams', 'rt', 'ManageVisitService', ManageVisitListController]);

    function ManageVisitListController($scope, $state, $ionicListDelegate, $stateParams, rt, ManageVisitService) {
        //本周
        var Week = 1;
        //本月
        var Month = 2;
        //当前页数
        var _pageIndex = 1;
        //每页显示数目
        var _pageSize = rt.getPaginationSize();

        function init() {
            $scope.vm = [];
            $scope.onItemClick = _onItemClick;
            $scope.loadMore = _loadMoreClick;
            $scope.goBack = _goBackClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.viewType = ManageVisitService.getViewType();
            $scope.chooseViewClick = _chooseViewClick;
            $scope.addClick = _addClick;
            $scope.cancelManageVisit = _cancelManageVisit;

            rt.showLoadingToast('正在加载数据...');
            _loadData(function (data) {
                $scope.vm.visitList = data;
                rt.hideLoadingToast();
            }, function () {
                rt.hideLoadingToast();
            });

        }

        /**
         *加载数据
         * @param success
         * @param failure
         * @private
         */
        function _loadData(success, failure) {
            ManageVisitService.getManageVisit($scope.viewType.value, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        $scope.isLoadMore = false;
                    }
                    else {
                        $scope.isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (failure) {
                        failure();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 加载更多事件
         * @private
         */
        function _loadMoreClick() {
            _pageIndex += 1;
            _loadData(function (data) {
                $scope.vm.visitList.push.apply($scope.vm.visitList, data);
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }

        /**'
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _loadData(function (data) {
                $scope.vm.visitList = data;
            }, null);
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 列表项的点击事件
         * @param id
         * @private
         */
        function _onItemClick(visit) {
            $state.go("manage-form", {id: visit.Id});
        }

        /**
         * 返回按钮点击事件
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 添加按钮点击事件
         * @private
         */
        function _addClick() {
            $state.go("manage-form", {id: rt.newGuid()});
        }

        /**
         * 视图切换
         * @param type
         * @private
         */
        function _chooseViewClick(type) {
            if (type === 1) {
                $scope.viewType = {name: "本周", value: Week};
                _pageIndex = 1;
                _loadData(function (data) {
                    $scope.vm.visitList = data;
                }, null);
            } else if (type === 2) {
                $scope.viewType = {name: "本月", value: Month};
                _pageIndex = 1;
                rt.showLoadingToast("正在加载数据...");
                _loadData(function (data) {
                    $scope.vm.visitList = data;
                    rt.hideLoadingToast();
                }, null);
            } else {
                return;
            }
        }

        function _cancelManageVisit(deletedIndex) {
            rt.showLoadingToast("正在删除...");

            var manageVisitId = $scope.vm.visitList[deletedIndex].Id;
            ManageVisitService.cancelManageVisit(manageVisitId)
                .success(function () {
                    rt.hideLoadingToast();

                    $scope.vm.visitList.splice(deletedIndex, 1);

                    $ionicListDelegate.closeOptionButtons();
                })
                .error(function (errorMessage) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(errorMessage);
                });
        }

        init();
    }

})();



(function () {
    'use strict';

    xrmApp.controller("MyHotelDetailController", ['$scope', '$state', '$stateParams', 'rt', 'MyHotelService', MyHotelDetailController]);

    function MyHotelDetailController($scope, $state, $stateParams, rt, MyHotelService) {
        //当前页数
        var _pageIndex = 1;
        //每页显示数目
        var _pageSize = rt.getPaginationSize();

        var hotelIdByCurUser = "";

        function init() {
            $scope.qcScoreClick = _qcScoreClick;
            $scope.pmsRoomStateClick = _pmsRoomStateClick;
            $scope.startVisit = _startVisit;
            $scope.vm = {};
            $scope.vm.isShowManageOrNot = false;//是否显示店长巡店数据，用于显示巡店记录的分类（城总/区总，店长）
            $scope.onItemClick = _onItemClick;
            $scope.visitedHistoryClick = _visitedHistoryClick;
            $scope.complaintClick = _complaintClick;
            $scope.loadMoreClick = _loadMoreClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.goBackClick = _goBackClick;
            $scope.chooseViewClick = _chooseViewClick;

            rt.showLoadingToast('正在加载数据...');

            var promise = MyHotelService.getHotelNameByUserId()
                .then(function(resp){
                    if(resp.data === null || resp.data === undefined){
                        rt.showErrorToast("根据当前用户没有找到酒店！");
                    }
                    hotelIdByCurUser = resp.data[0].hotelId;

                    _loadData(function (data) {
                        rt.hideLoadingToast();
                        $scope.hotel = data;
                    }, function () {
                        rt.hideLoadingToast();
                    });

                    //判断当前人员是否有开始巡查的权限
                    MyHotelService.canStartVisit()
                        .success(function (data) {
                            $scope.canStartVisit = data;
                        })
                        .error(function (error) {
                            rt.showErrorToast(error);
                        });
                }, function(err) {
                    rt.showErrorToast(err);
                });
        }

        /**
         * 退出
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 巡店历史
         * @private
         */
        function _visitedHistoryClick() {
            alert("未开放!");
        }

        /**
         *加载数据
         * @param success
         * @param failure
         * @private
         */
        function _loadData(success, failure) {
            MyHotelService.getHotelDetailById(hotelIdByCurUser, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.VisitList.length < _pageSize) {
                        $scope.isLoadMore = false;
                    }
                    else {
                        $scope.isLoadMore = true;
                    }
                    $scope.vm.hotelIdCode = data.Id;
                    var hotelId = "[" + "\"" + data.Code + "\"" + "]";
                    MyHotelService.getHotelCurrentDayFlow(hotelId)
                        .success(function(data){
                            rt.hideLoadingToast();
                            if(data.HotelFlowList.length <= 0){
                                rt.showErrorToast("没有当前流量数据！");
                            }
                            var flow = (((data.HotelFlowList[0].SumRoomCount - data.HotelFlowList[0].BookableRoomCount) / data.HotelFlowList[0].SumRoomCount) * 100).toFixed(2);
                            $scope.currentFlow = isNaN(flow) ? 0 : flow;
                        })
                        .error(function(err){
                            rt.hideLoadingToast();
                            rt.showErrorToast(err);
                        });
                })
                .error(function (error) {
                    if (failure) {
                        failure();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 跳转qc成绩
         * @param $stateParams
         * @private
         */
        function _qcScoreClick() {
            $state.go("qc-score", {id: hotelIdByCurUser});
        }

        /**
         * 跳转pms房态
         * @private
         */
        function _pmsRoomStateClick() {
            $state.go("room-state", {id: hotelIdByCurUser});
        }

        /**
         * 投诉信息点击事件
         * @private
         */
        function _complaintClick() {
            $state.go("complaint-hotel", {hotelId: hotelIdByCurUser});
        }

        /**
         * 开始拜访
         * @private
         */
        function _startVisit() {
            //新建的时候需要传id
            if (rt.isNullOrEmptyString($scope.vm.Id)) {
                $scope.vm.Id = rt.newGuid();
            }

            MyHotelService.startCityVisit(hotelIdByCurUser, $scope.vm.Id)
                .success(function (data) {
                    $state.go("cityVisit-main", {id: data});
                })
                .error(function (error) {
                    rt.showErrorToast(error);
                });
        }

        /**
         * 加载更多事件
         * @private
         */
        function _loadMoreClick() {
            _pageIndex += 1;
            if(!$scope.vm.isShowManageOrNot){//城总/区总巡店记录
                _loadData(function (data) {
                    $scope.hotel.VisitList.push.apply($scope.hotel.VisitList, data.VisitList);
                    $scope.$broadcast('scroll.infiniteScrollComplete');
                });
            }else{//店长巡店记录
                _loadDataByManage(function (data) {
                    $scope.vm.visitList.push.apply($scope.vm.visitList, data);
                    $scope.$broadcast('scroll.infiniteScrollComplete');
                });
            }
        }

        /**'
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            if(!$scope.vm.isShowManageOrNot){
                _loadData(function (data) {
                    $scope.hotel.VisitList = data.VisitList;
                }, null);
            }else{
                _loadDataByManage(function (data) {
                    $scope.vm.visitList = data;
                    rt.hideLoadingToast();
                }, function () {
                    rt.hideLoadingToast();
                });
            }
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 列表项的点击事件
         * @param id
         * @private
         */
        function _onItemClick(visit) {
            if(!$scope.vm.isShowManageOrNot){
                $state.go("cityVisit-main", {id: visit.Id});
            }else{
                $state.go("manage-form", {id: visit.Id});
            }
        }

        /**
         * 视图切换
         * @param type
         * @private
         */
        function _chooseViewClick(type) {
            if (type === 1) {//城总巡店记录
                _pageIndex = 1;
                $scope.vm.isShowManageOrNot = false;
                rt.showLoadingToast("正在加载数据...");
                _loadData(function (data) {
                    rt.hideLoadingToast();
                    $scope.hotel = data;
                }, function () {
                    rt.hideLoadingToast();
                });
            } else if (type === 2) {//店长巡店记录
                _pageIndex = 1;
                $scope.vm.isShowManageOrNot = true;
                rt.showLoadingToast("正在加载数据...");
                _loadDataByManage(function (data) {
                    $scope.vm.visitList = data;
                    rt.hideLoadingToast();
                }, function () {
                    rt.hideLoadingToast();
                });
            } else {
                return;
            }
        }

        /**
         * 新增店长巡店记录，显示店长门店的巡店日志
         * @param success
         * @param failure
         * @private
         */
        function _loadDataByManage(success, failure) {
            MyHotelService.getManageVisit($scope.vm.hotelIdCode, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        $scope.isLoadMore = false;
                    }
                    else {
                        $scope.isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (failure) {
                        failure();
                    }
                    rt.showErrorToast(error);
                });
        }


        init();
    }
})();

(function () {
    'use strict';
    xrmApp.controller("MyPmsRoomStateController", ['$scope', '$stateParams', 'rt', 'MyHotelService', MyPmsRoomStateController]);

    function MyPmsRoomStateController($scope, $stateParams, rt, MyHotelService) {

        //所有
        var All=0;
        //脏房
        var Dirty = 1;
        //干净房
        var Clean = 2;
        //入住房
        var CheckIn=3;

        //构建所有房间数组
        $scope.roomArray=[];

        function init() {
            $scope.viewType=MyHotelService.getViewType();
            $scope.dirtyClick=_dirtyClick;
            $scope.cleanClick=_cleanClick;
            $scope.checkInClick=_checkInClick;
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            MyHotelService.getRoomList($stateParams.id,$scope.viewType.value)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.stateCount = {};
                    $scope.stateCount.VD = 0;
                    $scope.stateCount.VC = 0;
                    $scope.stateCount.OD = 0;
                    if(data.length > 0 && data !== null){
                        for(var i = 0; i < data.length; i++){
                            if(data[i].RoomState === 1){//脏房
                                $scope.stateCount.VD += 1;
                            }
                            else if(data[i].RoomState === 2){//干净房
                                $scope.stateCount.VC += 1;
                            }
                            else {//入住房
                                $scope.stateCount.OD += 1;
                            }
                        }
                    }
                    $scope.roomArray=[];
                    _dealRoomArray(data);
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 将接口返回数据拆分成一行四个
         * @param array
         * @private
         */
        function _dealRoomArray(array){
            var lines=array.length%4===0?array.length/4:parseInt(array.length/4)+1;
            for(var i=0;i<lines;i++){
                $scope.roomArray[i]=[];
                for(var j=0;j<4;j++){
                    $scope.roomArray[i][j]=array[i*4+j]===undefined?{}:array[i*4+j];
                }
            }
        }

        /**
         * 脏房点击
         * @private
         */
        function _dirtyClick(){
            $scope.viewType={name:'脏房',value:Dirty};
            _loadData();
        }

        /**
         * 干净房点击
         * @private
         */
        function _cleanClick(){
            $scope.viewType={name:'干净房',value:Clean};
            _loadData();
        }

        /**
         * 入住房点击
         * @private
         */
        function _checkInClick(){
            $scope.viewType={name:'入住房',value:CheckIn};
            _loadData();
        }
        init();
    }
})();

(function () {
    'use strict';
    xrmApp.controller("MyQcScoreController", ['$scope', '$stateParams', 'rt', 'MyHotelService', MyQcScoreController]);

    function MyQcScoreController($scope, $stateParams, rt, MyHotelService) {

        function init() {
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            MyHotelService.getQcScoreById($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.score = data;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }
        init();
    }
})();

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2015-12-14 */
/*comment:公告通知列表界面 */

(function () {
    'use strict';

    xrmApp.controller("NoticeListController", ['$scope', '$state', 'rt', 'NoticeService', NoticeListController]);

    function NoticeListController($scope, $state, rt, NoticeService) {
        $scope.vm = [];
        //搜索框绑定查询字段
        $scope.vm.queryValue = "";
        //当前页数
        var _pageIndex = 1;
        //每页显示数目
        var _pageSize = rt.getPaginationSize();

        function init() {
            $scope.noticeList = [];
            $scope.search = _search;
            $scope.onItemClick = _onItemClick;
            $scope.loadMore = _loadMoreClick;
            $scope.goBackClick = _goBackClick;
            $scope.viewRefresh = _viewRefresh;

            rt.showLoadingToast('正在加载数据...');
            _loadNoticeList(function (data) {
                $scope.noticeList = data;
                rt.hideLoadingToast();
            }, function () {
                rt.hideLoadingToast();
            });
        }

        /**
         * 公告通知列表s
         * @private
         */
        function _loadNoticeList(success, failure){
            NoticeService.getNoticeList($scope.vm.queryValue, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        $scope.isLoadMore = false;
                    }
                    else {
                        $scope.isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (failure) {
                        failure();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 搜索
         * @private
         */
        function _search() {
            _pageIndex = 1;
            $scope.noticeList = [];
            rt.showLoadingToast('正在加载数据...');
            _loadNoticeList(function (data) {
                $scope.noticeList = data;
                rt.hideLoadingToast();
            }, function () {
                rt.hideLoadingToast();
            });
        }

        /**
         * 加载更多事件
         * @private
         */
        function _loadMoreClick() {
            _pageIndex += 1;
            _loadNoticeList(function (data) {
                $scope.noticeList.push.apply($scope.noticeList, data);
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });

        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _loadNoticeList(function (data) {
                $scope.noticeList = data;
            }, null);
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 列表项的点击事件
         * @param id
         * @private
         */
        function _onItemClick(notice) {
            notice.IsRead = true;
            $state.go("notice-readonly", {id: notice.ReadId});
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        init();
    }
})();



/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2015-12-14 */
/*comment:公告通知只读界面 */
(function () {
    'use strict';

    xrmApp.controller("NoticeReadController", ['$scope', '$stateParams', 'rt', 'NoticeService', NoticeReadController]);

    function NoticeReadController($scope, $stateParams, rt, NoticeService) {

        function init() {
            $scope.downLoadAttachment = _downLoadAttachmentClick;
            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            NoticeService.getFormData($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.noticeForm = data;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        function _downLoadAttachmentClick(id, name) {
            rt.showPopupDialog("下载附件", "是否下载附件？", function () {
                var baseUrl;
                if (window.XrmDeviceData && window.XrmDeviceData.getXrmBaseUrl) {
                    baseUrl = window.XrmDeviceData.getXrmBaseUrl();
                }
                else if (!rt.isNullOrEmptyString(localStorage.XrmBaseUrl)) {
                    baseUrl = localStorage.XrmBaseUrl;
                }

                var fileext = name.substring(name.lastIndexOf(".") + 1);

                var url = baseUrl + 'FileDownloadHandler.ashx?moduleType=notice&fileid=' + id + "&fileext=" + fileext;
                window.open(url, "下载附件", 'toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no', true);
            });
        }

        init();
    }
})();

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-04-01 */
/*comment:忘记密码 */

(function () {
    'use strict';

    xrmApp.controller("PasswordController", ['$scope', '$state', 'rt', 'PasswordService', PasswordController]);

    function PasswordController($scope, $state, rt, PasswordService) {

        function init() {
            $scope.goBackClick = _goBackClick;
            $scope.gotoResetPasswordView = _gotoResetPasswordView;

            $scope.vm = {};
            $scope.vm.userCode = "";

            _loadEmailTypes();
        }

        function _loadEmailTypes(){
            rt.showLoadingToast("正在加载...");
            PasswordService.getEmailTypeList()
                .success(function(emailTypes){
                    rt.hideLoadingToast();

                    $scope.emailTypes = emailTypes;
                    if(emailTypes !== null && emailTypes.length > 0){
                        $scope.vm.emailTypeId = emailTypes[0].EmailTypeId;
                    }
                })
                .error(function(errorMessage){
                    rt.hideLoadingToast();
                    rt.showErrorToast(errorMessage);
                });
        }

        function _gotoResetPasswordView() {
            if (rt.isNullOrEmptyString($scope.vm.userCode)) {
                rt.showErrorToast("请输入用户名");
                return;
            }

            if($scope.vm.userCode.toUpperCase() === 'HMS'){
                rt.showErrorToast("该账户不可以修改");
                return;
            }

            var userCode = $scope.vm.userCode;
            var emailTypeId = $scope.vm.emailTypeId;

            $state.go('password-reset', {"userCode":userCode,"emailTypeId":emailTypeId});
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-04-01 */
/*comment:重置密码 */

(function () {
    'use strict';

    xrmApp.controller("ResetPasswordController", ['$scope', '$state', '$stateParams', 'rt', 'PasswordService', ResetPasswordController]);

    function ResetPasswordController($scope, $state, $stateParams, rt, PasswordService) {

        var _userCode;
        var _emailTypeId;

        function init() {
            _userCode = $stateParams.userCode;
            _emailTypeId = $stateParams.emailTypeId;

            $scope.resetPassword = _resetPassword;

            _getUserInfo();
        }

        function _getUserInfo() {
            rt.showLoadingToast("正在加载...");

            PasswordService.getUserInfo(_userCode,_emailTypeId)
                .success(function (user) {
                    rt.hideLoadingToast();

                    $scope.user = user;
                })
                .error(function (errorMessage) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(errorMessage);
                });
        }

        /**
         * 发送邮件
         * @private
         */
        function _resetPassword() {
            rt.showLoadingToast("正在处理...");
            PasswordService.resetPassword(_userCode,_emailTypeId)
                .success(function (data) {
                    rt.hideLoadingToast();

                    rt.showSuccessToast("重置成功");

                    $state.go("app-close");
                })
                .error(function (err) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function QuestionEditController($scope, $filter,rt, $stateParams,$ionicHistory,QuestionService) {

        function init(){
            $scope.isEdit =  $filter('date')(new Date(), 'yyyy-MM-dd') < $filter('date')(new Date($stateParams.isEdit), 'yyyy-MM-dd');
            $scope.canEdit = true;
            $scope.states = $stateParams.states;
            $scope.vm = {};
            $scope.an = [];
            $scope.btnOkClick = _btnOkClick;
            $scope.singleRadioClick = _singleRadioClick;
            _loadData();
        }

        function _btnOkClick(){
            for (var i = 0;i<$scope.vm.length;i++){
                var subject = $scope.vm[i].Subject;
                /*var answerId = subject.Answers[0].Answer.SubjectOptionid.Id;
                if(subject.Type.Value == 10){
                    for(var a = 0; a < subject.Options.length; a++){
                        // 单选题中的其他选项
                        if(subject.Options[a].Type.Value != 20 && subject.Options[a].Id == answerId){
                            subject.Answers[0].Answer = "";
                        }
                    }
                }*/
                for (var j = 0;j<subject.Answers.length;j++){
                    if (subject.Answers[j] ){
                        if ($scope.vm[i].Subject.Type.Value == 20 && subject.Answers[j].IsCheck){
                            subject.Answers[j].Id = subject.Options[j].Id;
                        }
                        if (rt.isNullOrEmptyString(subject.Answers[j].Id)){
                            subject.Answers[j].Id = rt.newGuid();
                        }
                    }
                }
            }
            QuestionService.save($scope.vm,$stateParams.hotelid)
                .success(function(data){
                    rt.showSuccessToast("保存成功");
                    $ionicHistory.goBack();
                })
                .error(function(data){
                    rt.showErrorToast(data);
                });
        }

        function _loadData(){
            QuestionService.getQuestionSubject($stateParams.id)
                .success(function(data){
                    $scope.vm = data;

                    for (var i = 0;i<$scope.vm.length;i++){
                        if ($scope.vm[i].Subject.Type.Value == 20){
                            var subject = $scope.vm[i].Subject;
                            var answer = [];
                            for (var j = 0;j<subject.Options.length;j++){
                                answer[j] = {Id:subject.Options[j].Id,IsCheck : false};
                                for (var z = 0;z<subject.Answers.length;z++){
                                    if(subject.Options[j].Id == subject.Answers[z].Id){
                                        answer[j].IsCheck = true;
                                    }
                                }
                            }
                            subject.Answers = answer;
                        }
                    }
                })
                .error(function(data){
                    rt.showErrorToast(data);
                });
        }

        function _singleRadioClick(id, seq){
            for(var i = 0; i < $scope.vm[seq - 1].Subject.Options.length; i++){
                if($scope.vm[seq - 1].Subject.Options[i].Id == id && $scope.vm[seq - 1].Subject.Options[i].Type.Value == 20){
                    $scope.canEdit = false;
                }
                else{
                    $scope.vm[seq - 1].Subject.Answers[0].Answer = "";
                    $scope.canEdit = true;
                }
            }
            /*for (var m = 0; m < $scope.vm.length; m++) {
                if ($scope.vm[m].Type.Value === 10) {
                    $scope.vm[seq-1].Answers = option;
                }
            }*/
        }

        init();
    }

    xrmApp.controller("QuestionEditController", ['$scope', '$filter', 'rt', '$stateParams','$ionicHistory','QuestionService', QuestionEditController]);
})();
/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function QuestionListController($scope, $state,rt, $stateParams,QuestionService) {
        //当前的页数
        var _pageIndex = 1;
        //每页显示条数
        var _pageSize = rt.getPaginationSize();

        function init() {
            $scope.addClick = _addClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.loadMore = _loadMore;
            $scope.itemClick = _itemClick;
            $scope.getStateName = _getStateName;
            $scope.getStateColor = _getStateColor;
            //返回按钮点击事件
            $scope.goBackClick = _goBackClick;

            $scope.isLoadMore = false;

            _getQuestionList(function(data){
                $scope.vm = data;
            });
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 列表项点击事件
         * @param id 问卷调查的id
         * @private
         */
        function _itemClick(id, hotelid, time, state){
            $state.go('question-edit', {id: id, hotelid: hotelid, isEdit: time, states: state});
        }

        function _getStateName(time,state){
            if (state == 20){
                return "已参与";
            }
            var nowDate = new Date();
            time = Date.parse(time);
            if (nowDate < time){
                return "未参与";
            }else{
                return "已过期";
            }
        }

        function _getStateColor(time,state){
            if (state == 20){
                return {"color" : "green"};
            }
            var nowDate = new Date();
            time = Date.parse(time);
            if (nowDate < time){
                return {"color" : "red"};
            }
            return {"color" : "black"};
        }

        /**
         * 添加按钮点击事件
         * @private
         */
        function _addClick() {
            $state.go("question-edit");
        }

        /**
         * 获取问卷调查数据
         * @param success 成功回掉函数
         * @private
         */
        function _getQuestionList(success){
            QuestionService.getQuestionList(_pageIndex,_pageSize)
                .success(function(data){
                    rt.hideLoadingToast();
                    if (success){
                        success(data);
                    }

                    if (data.length < _pageSize){
                        $scope.isLoadMore = false;
                    }else{
                        $scope.isLoadMore = true;
                    }
                })
                .error(function(data){
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast();
        }

        /**
         * 下拉刷新事件
         * @private
         */
        function _viewRefresh()
        {
            _pageIndex = 1;
            _getQuestionList(function (data) {
                $scope.vm = data;
                $scope.$broadcast('scroll.refreshComplete');
            });
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMore()
        {
            _pageIndex++;
            _getQuestionList(function (data) {
                //$scope.vm = data;
                $scope.vm.push.apply($scope.vm, data);
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }

        init();
    }

    xrmApp.controller("QuestionListController", ['$scope', '$state', 'rt', '$stateParams','QuestionService', QuestionListController]);
})();
/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2015-12-15 */
/*comment:酒店流量界面 */
(function () {
    'use strict';

    xrmApp.controller("FlowListController", ['$scope', '$state', 'rt', 'RevenueService', FlowListController]);

    function FlowListController($scope, $state, rt, RevenueService) {

        function init() {
            $scope.vm = {};
            $scope.viewRefresh = _viewRefresh;
            $scope.goToRoomTypeFlow = _goToRoomTypeFlow;
            $scope.search = _search;

            _loadHotelData();
        }

        /**
         * 加载酒店流量数据
         * @private
         */
        function _loadHotelData(){
            rt.showLoadingToast("正在加载数据...");
            RevenueService.getHotelCurrentDayFlow()
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.hotelList = data.HotelFlowList;
                    if($scope.hotelList.length > 0){
                        if($scope.hotelList[0].HotelName.length > 9){
                            $scope.HotelName = $scope.hotelList[0].HotelName.substr(0, 9) + '...';
                        }else{
                            $scope.HotelName = $scope.hotelList[0].HotelName;
                        }
                    }
                })
                .error(function(err){
                    rt.hideLoadingToast();
                   rt.showErrorToast(err);
                });
        }

        /**
         * 搜索
         * @private
         */
        function _search(){
            alert("暂不支持搜索!");
            //_loadHotelData();
        }

        /**
         * itemClick  进入房型流量界面
         * @private
         */
        function _goToRoomTypeFlow(hotelId, hotelName){
            $state.go("flow-room", {hotelId: hotelId, hotelName: hotelName});
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh(){
            $scope.hotelList = [];
            _loadHotelData();
            $scope.$broadcast('scroll.refreshComplete');
        }

        init();
    }
})();

/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2015-12-15 */
/*comment:房型流量界面 */
(function () {
    'use strict';

    xrmApp.controller("FlowRoomTypeController", ['$scope', '$stateParams', 'rt', 'RevenueService', FlowRoomTypeController]);

    function FlowRoomTypeController($scope, $stateParams, rt, RevenueService) {

        function init() {
            $scope.viewRefresh = _viewRefresh;
            $scope.hotelName = $stateParams.hotelName;

            _loadRoomTypeFlow();
        }

        /**
         * 获取门店房型当日流量及房价
         * @private
         */
        function _loadRoomTypeFlow(){
            RevenueService.getHotelRoomCurrentDayFlowAndPrice($stateParams.hotelId)
                .success(function(data){
                    $scope.roomTypeFlow = data.HotelRoomFlowList;
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh(){
            _loadRoomTypeFlow();
            $scope.$broadcast('scroll.refreshComplete');
        }

        init();
    }
})();

/*global UIMenu:false */
/*global xrmApp:false */

(function () {
    'use strict';

    xrmApp.controller("LockRoomController", ['$scope', '$state', '$stateParams', 'rt', 'RevenueService', LockRoomController]);

    function LockRoomController($scope, $state, $stateParams, rt, RevenueService) {

        function init() {
            $scope.viewRefresh = _viewRefresh;
            $scope.submitChange = _submitChange;
            $scope.hotelName = $stateParams.hotelName;
        }

        /**
         * 提交房价修改
         * @private
         */
        function _submitChange(){
            rt.showLoadingToast("正在提交修改...");
            setTimeout(function(){rt.hideLoadingToast();rt.showSuccessToast("修改成功!");}, 2000);
        }

        /**
         * 下拉刷新
         * @privat
         */
        function _viewRefresh(){
            $scope.$broadcast('scroll.refreshComplete');
        }

        init();
    }
})();

/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2015-12-15 */
/*comment:房价设置界面 */
(function () {
    'use strict';

    xrmApp.controller("PriceRoomTypeController", ['$scope', '$filter', '$state', '$stateParams', 'rt', 'RevenueService', PriceRoomTypeController]);

    function PriceRoomTypeController($scope, $filter, $state, $stateParams, rt, RevenueService) {

        function init() {
            $scope.vm = {};
            $scope.flowData = [];
            $scope.roomPrice = [];
            $scope.viewRefresh = _viewRefresh;
            $scope.goToThresholdEditView = _goToThresholdEditView;
            $scope.submitChange = _submitChange;
            $scope.checkApi = _checkApi;

            $scope.vm.shouldDisableSubmit = false;

            _loadHotelFlowData();
            _loadHotelRoomTypePrice();

            if(rt.isNullOrEmptyString(localStorage.isFirstLoad) && rt.isNullOrEmptyString(localStorage.date)){
                localStorage.isFirstLoad = '0';
                localStorage.date = $filter('date')(new Date(), 'yyyy-MM-dd');
            }
            else if(localStorage.date !== $filter('date')(new Date(), 'yyyy-MM-dd')){
                localStorage.isFirstLoad = '0';
                localStorage.date = $filter('date')(new Date(), 'yyyy-MM-dd');
            }
            else{ }
        }

        /**
         * 获取房价调整界面中的流量信息
         * @private
         */
        function _loadHotelFlowData(){
            RevenueService.getPriceChangeHotelFlow($stateParams.hotelId)
                .success(function(data){
                    $scope.flowData = data.HotelFlowList;
                })
                .error(function(err){
                   rt.showErrorToast(err);
                    $scope.flowData[0] = {};
                    $scope.flowData[0].SumRoomCount = $scope.flowData[0].BookableRoomCount = 1;
                });
        }

        /**
         * 获取房价调整界面中的房型门市价
         * @private
         */
        function _loadHotelRoomTypePrice(){
            RevenueService.getPriceChangeRoomTypePrice($stateParams.hotelId)
                .success(function(data){
                    $scope.roomTypePrice = data.HotelRoomFlowList;
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        /**
         * 检查所选房型是否已经申请
         * @private
         */
        function _checkApi(roomType, index){
            var newPrice =  $scope.roomPrice[index];
            if(rt.isNullOrEmptyString(newPrice)){
                return;
            }

            var dataModel = {};
            dataModel.HotelCd = $scope.flowData[0].HotelID;
            dataModel.HotelNm = $scope.flowData[0].HotelName;
            dataModel.Rmtp = $scope.roomTypePrice[index].RoomTypeCode;
            dataModel.RmtpDesp = $scope.roomTypePrice[index].RoomTypeName;
            dataModel.RmNum = $scope.roomTypePrice[index].SumRoomCount;
            dataModel.OldPrice = $scope.roomTypePrice[index].YearRoomPrice;
            dataModel.NewPrice = $scope.roomPrice[index];
            dataModel.PriBal = $scope.roomPrice[index] - $scope.roomTypePrice[index].YearRoomPrice;
            dataModel.StDate = $filter('date')(new Date(), 'yyyy-MM-dd');
            dataModel.EndDate = $filter('date')(new Date(), 'yyyy-MM-dd');
            dataModel.ValidDay = (new Date()).getDay() + 1;
            dataModel.Balance = $scope.roomPrice[index] - $scope.roomTypePrice[index].YearRoomPrice;
            dataModel.TotRmNum = $scope.flowData[0].SumRoomCount;
            dataModel.ApplyMode = "0";
            dataModel.TypeTj = "1";

            RevenueService.checkApiIsUsed(dataModel)
                .success(function(data){
                    if(data.ErrorCode === 0){
                        //成功
                    }
                    else if(data.ErrorCode === 2){
                        // 需确认是否覆盖
                        //rt.showErrorToast(data.Message);
                        rt.showConfirmDialog("是否继续执行后续操作？确认将覆盖之前已生效的调价！", "")
                            .then(function(res){
                                if(res){
                                    return;
                                }
                                else{
                                    $scope.roomPrice[index] = "";
                                    return;
                                }
                            });
                    }
                    else if(data.ErrorCode === -1){
                        // 不管确认是否覆盖都不允许更改
                        rt.showErrorToast(data.Message);
                        //$scope.roomPrice[index] = "";
                    }
                    else{
                        rt.showErrorToast(data.Message);
                    }

                    $scope.vm.shouldDisableSubmit = false;
                })
                .error(function(err){
                   rt.showErrorToast(err);

                    $scope.vm.shouldDisableSubmit = false;
                });
        }

        /**
         * 提交房价修改
         * @private
         */
        function _submitChange(){

            rt.showLoadingToast("正在处理...");
            RevenueService.lessThanThridSubmit()
                .success(function(data){
                    if(data < 4){
                        var dataModel = {};
                        dataModel.Id = "";
                        dataModel.ApplyUser = $scope.flowData[0].HotelID;
                        dataModel.CheckSeq = "";
                        dataModel.SPuser = "";
                        dataModel.Remarks = "";
                        dataModel.NeedCoo = "0";
                        dataModel.TerminateAll = "";
                        dataModel.ConfirmList = [];
                        _setDataModel(dataModel.ConfirmList);

                        RevenueService.changeRoomPriceTwo(dataModel)
                            .success(function(data){
                                rt.hideLoadingToast();
                                if(!rt.isNullOrEmptyString(data)){
                                    rt.showSuccessToast(data);
                                }
                                _loadHotelRoomTypePrice();
                            })
                            .error(function(err){
                                rt.hideLoadingToast();
                                rt.showErrorToast(err);
                            });
                    }else{
                        rt.hideLoadingToast();
                        rt.showErrorToast("当日提交房价不可超过3次！");
                        return;
                    }
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 设置改房价的详细信息
         * @private
         */
        function _setDetailsInfo(Details){
            var j = 0;
            for(var i = 0; i < $scope.roomTypePrice.length; i++){
                if(!rt.isNullOrEmptyString($scope.roomPrice[i]) && $scope.roomPrice[i] >= 0){
                    Details[j] = {};
                    Details[j].Id = j + 1;
                    Details[j].RoomType = $scope.roomTypePrice[i].RoomTypeCode;
                    Details[j].RoomTypeDesp = $scope.roomTypePrice[i].RoomTypeName;
                    Details[j].RoomCount = $scope.roomTypePrice[i].SumRoomCount;
                    Details[j].ChangeType = "";
                    Details[j].BeginDate = $filter('date')(new Date(), 'yyyy-MM-dd');
                    Details[j].EndDate = $filter('date')(new Date(), 'yyyy-MM-dd');
                    Details[j].BeginDateFlow = "";
                    Details[j].BeginDatePrice = $scope.roomTypePrice[i].YearRoomPrice;
                    Details[j].BeginDateNewPrice = $scope.roomPrice[i];
                    Details[j].PriceRange = $scope.roomPrice[i] - $scope.roomTypePrice[i].YearRoomPrice;
                    Details[j].WeekOn = (new Date()).getDay() + 1;
                    Details[j].TotRoomCount = $scope.flowData[0].SumRoomCount;
                    j++;
                }
            }
        }

        function _setDataModel(Details){
            var j = 0;
            for(var i = 0; i < $scope.roomTypePrice.length; i++){
                if(!rt.isNullOrEmptyString($scope.roomPrice[i]) && $scope.roomPrice[i] >= 0){
                    Details[j] = {};
                    Details[j].Id = j + 1;
                    Details[j].ApplyID = "";
                    Details[j].HotelCd = $scope.flowData[0].HotelID;
                    Details[j].HotelNm = "";//酒店Name后台取
                    Details[j].Rmtp = $scope.roomTypePrice[i].RoomTypeCode;
                    Details[j].RmtpDesp = $scope.roomTypePrice[i].RoomTypeName;
                    Details[j].RmNum = $scope.roomTypePrice[i].SumRoomCount;
                    Details[j].OldPrice = $scope.roomTypePrice[i].YearRoomPrice;
                    Details[j].NewPrice = $scope.roomPrice[i];
                    Details[j].PriBal = $scope.roomPrice[i] - $scope.roomTypePrice[i].YearRoomPrice;
                    Details[j].StDate = $filter('date')(new Date(), 'yyyy-MM-dd');
                    Details[j].EndDate = $filter('date')(new Date(), 'yyyy-MM-dd');
                    Details[j].ValidDay = (new Date()).getDay() + 1;
                    Details[j].CheckSeq = "";
                    Details[j].Terminated = "";
                    Details[j].GoPriceRmNum = "0";
                    Details[j].TotRmNum = $scope.flowData[0].SumRoomCount;
                    Details[j].ApplyMode = 0;
                    Details[j].TypeTj = "";
                    j++;
                }
            }
        }

        /**
         * 调整阀值
         * @private
         */
        function _goToThresholdEditView(){
            $state.go("threshold-edit");
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh(){
            _loadHotelFlowData();
            _loadHotelRoomTypePrice();
            $scope.$broadcast('scroll.refreshComplete');
        }

        init();
    }
})();

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2015-12-15 */
/*comment:收益管理主界面 */

(function () {
    'use strict';

    xrmApp.controller("RevenueCardController", ['$scope', '$state', 'rt', 'RevenueService', RevenueCardController]);

    function RevenueCardController($scope, $state, rt, RevenueService) {

        function init() {
            $scope.goBack = _goBackClick;
            $scope.goToNextView = _goToNextView;
        }

        /**
         * goTo 房价调整, 锁房, 流量   界面
         * @param index
         * @private
         */
        function _goToNextView(index){
            switch(index){
                //房价调整
                case 0: $state.go("price-room", {hotelId: ""});break;
                //锁房
                //case 1: $state.go("lock-room");break;
                case 1: alert("此功能暂未开放!");break;
                //流量
                case 2: $state.go("flow-list");break;
                default :break;
            }
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        init();
    }
})();



/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2015-12-15 */
/*comment:阀值调整界面 */
(function () {
    'use strict';

    xrmApp.controller("ThresholdEditController", ['$scope', '$ionicHistory', 'rt', 'RevenueService', ThresholdEditController]);

    function ThresholdEditController($scope, $ionicHistory, rt, RevenueService) {

        function init() {
            $scope.rightLimit = [];
            $scope.leftLimit = [];
            $scope.submit = _submit;

            _loadFlowThreshold();
        }

        /**
         * 获取流量阀值
         * @private
         */
        function _loadFlowThreshold(){
            rt.showLoadingToast("正在加载数据...");
            RevenueService.getTheFlowThreshold()
                .success(function(data){
                    rt.hideLoadingToast();
                    if(!rt.isNull(data)){
                        if(!rt.isNull(data.Spell)){
                            for(var i = 0; i < data.Spell.length; i++){
                                if(data.Spell[i].StartTime === "10:00"){
                                    $scope.rightLimit.push(data.Spell[i].DownLimit);
                                    $scope.leftLimit.push(data.Spell[i].UpLimit);
                                }
                                if(data.Spell[i].StartTime === "12:00"){
                                    $scope.rightLimit.push(data.Spell[i].DownLimit);
                                    $scope.leftLimit.push(data.Spell[i].UpLimit);
                                }
                                if(data.Spell[i].StartTime === "14:00"){
                                    $scope.rightLimit.push(data.Spell[i].DownLimit);
                                    $scope.leftLimit.push(data.Spell[i].UpLimit);
                                }
                                if(data.Spell[i].StartTime === "16:00"){
                                    $scope.rightLimit.push(data.Spell[i].DownLimit);
                                    $scope.leftLimit.push(data.Spell[i].UpLimit);
                                }
                                if(data.Spell[i].StartTime === "18:00"){
                                    $scope.rightLimit.push(data.Spell[i].DownLimit);
                                    $scope.leftLimit.push(data.Spell[i].UpLimit);
                                }
                            }
                        }
                    }
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 确认修改阀值
         * @private
         */
        function _submit(){
            var threshold = {};
            threshold.hotelId = "";
            threshold.Spell = [];
            _setThreshold(threshold.Spell);
            rt.showLoadingToast("正在提交修改值...");
            RevenueService.saveTheFlowThreshold(threshold)
                .success(function(data){
                    rt.hideLoadingToast();
                    $ionicHistory.goBack();
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 设置阀值
         * @private
         */
        function _setThreshold(spell){
            var startTime = ["10:00", "12:00", "14:00", "16:00", "18:00"];
            var endTime = ["12:00", "14:00", "16:00", "18:00", "20:00"];
            for(var i = 0; i < 5; i++){
                /*spell[i].push({
                    StartTime : startTime[i],
                    EndTime : endTime[i],
                    UpLimit : $scope.leftLimit[i],
                    DownLimit : $scope.rightLimit[i]
                });*/
                spell[i] = {};
                spell[i].StartTime = startTime[i];
                spell[i].EndTime = endTime[i];
                spell[i].UpLimit = $scope.leftLimit[i];
                spell[i].DownLimit = $scope.rightLimit[i];
            }
        }

        init();
    }
})();

/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2016-01-13 */
/*comment:房间清扫异常Dialog界面 */
(function () {
    'use strict';
    xrmApp.controller("DialogController", ['$scope', 'rtUtils', 'rt', DialogController]);

    function DialogController($scope, rtUtils, rt) {

        _init();

        function _init() {
            $scope.selectData = _selectData;
        }

        function _selectData(u){
            rt.showConfirmDialog("是否确认" + getDescribe(u) + "?", "")
                .then(function(res){
                    if(res){
                        $scope.closeDialog();
                        if(rtUtils.isFunction($scope.onDataSelected) && !rtUtils.isNull(u))
                        {
                            $scope.onDataSelected(u);
                        }
                    }else{
                        return;
                    }
                });
        }

        function getDescribe(u){
            switch (u){
                case 10: return "DND";
                case 20: return "房态异常";
                case 30: return "NNS";
                default :break;
            }
        }
    }
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-02-29 */
/*comment:可用品 */

(function () {
    'use strict';

    xrmApp.controller("GuestSuppliesController", ['$scope', '$ionicHistory', 'rt', 'RoomControlService', GuestSuppliesController]);

    function GuestSuppliesController($scope, $ionicHistory, rt, RoomControlService) {

        function init() {
            /*$scope.standardNum = [];
            $scope.remainingNum = [];*/

            $scope.saveClick = _saveSuppliesList;

            /*for (var i = 0; i < 10; i++) {
                $scope.standardNum[i] = "";
                $scope.remainingNum[i] = "";
            }*/
            //_loadNumData();
            //_loadStandardNum();
            _loadSuppliesList();
        }

        /**
         * 获取当前酒店品牌下的客用品+数量
         * @private
         */
        function _loadSuppliesList(){
            RoomControlService.getGuestSupplies()
                .success(function(data){
                    $scope.guestSuppliesList = data;
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 加載數據
         * @private
         */
        function _loadNumData() {
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.getGuestSupplies()
                .success(function (data) {
                    rt.hideLoadingToast();

                    if (rt.isNull(data)) {
                        return;
                    }

                    if (rt.isNull(data.Item)) {
                        return;
                    }

                    for (var i = 0; i < data.Item.length; i++) {
                        if (data.Item[i].ItemId === 10) {
                            //$scope.standardNum[0] = data.Item[i].StandardNum;
                            $scope.remainingNum[0] = data.Item[i].RemainingNum;
                        }
                        if (data.Item[i].ItemId === 11) {
                            //$scope.standardNum[1] = data.Item[i].StandardNum;
                            $scope.remainingNum[1] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 20) {
                            //$scope.standardNum[2] = data.Item[i].StandardNum;
                            $scope.remainingNum[2] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 30) {
                            //$scope.standardNum[3] = data.Item[i].StandardNum;
                            $scope.remainingNum[3] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 40) {
                            //$scope.standardNum[4] = data.Item[i].StandardNum;
                            $scope.remainingNum[4] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 41) {
                            //$scope.standardNum[5] = data.Item[i].StandardNum;
                            $scope.remainingNum[5] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 50) {
                            //$scope.standardNum[6] = data.Item[i].StandardNum;
                            $scope.remainingNum[6] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 60) {
                            //$scope.standardNum[7] = data.Item[i].StandardNum;
                            $scope.remainingNum[7] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 70) {
                            //$scope.standardNum[8] = data.Item[i].StandardNum;
                            $scope.remainingNum[8] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 80) {
                            //$scope.standardNum[9] = data.Item[i].StandardNum;
                            $scope.remainingNum[9] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 90) {
                            //$scope.standardNum[10] = data.Item[i].StandardNum;
                            $scope.remainingNum[10] = data.Item[i].RemainingNum;
                        }
                        else if (data.Item[i].ItemId === 100) {
                            //$scope.standardNum[11] = data.Item[i].StandardNum;
                            $scope.remainingNum[11] = data.Item[i].RemainingNum;
                        }
                    }
                })
                .error(function (err) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 获取客用品标配数量
         * @private
         */
        function _loadStandardNum(){
            RoomControlService.getStandardNum()
                .success(function(data){
                    if (rt.isNull(data)) {
                        return;
                    }

                    if (rt.isNull(data.Item)) {
                        return;
                    }

                    for (var i = 0; i < data.Item.length; i++) {
                        if (data.Item[i].ItemId === 10) {
                            $scope.standardNum[0] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 11) {
                            $scope.standardNum[1] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 20) {
                            $scope.standardNum[2] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 30) {
                            $scope.standardNum[3] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 40) {
                            $scope.standardNum[4] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 41) {
                            $scope.standardNum[5] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 50) {
                            $scope.standardNum[6] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 60) {
                            $scope.standardNum[7] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 70) {
                            $scope.standardNum[8] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 80) {
                            $scope.standardNum[9] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 90) {
                            $scope.standardNum[10] = data.Item[i].StandardNum;
                        }
                        if (data.Item[i].ItemId === 100) {
                            $scope.standardNum[11] = data.Item[i].StandardNum;
                        }
                    }
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        function _saveSuppliesList(){
            for (var i = 0; i < $scope.guestSuppliesList.length; i++) {
                if ($scope.guestSuppliesList[i].RemainingNum < 0 || $scope.guestSuppliesList[i].RemainingNum === undefined) {
                    rt.showErrorToast("有剩余数量没有填写,请检查!");
                    return;
                }
                if($scope.guestSuppliesList[i].RemainingNum > $scope.guestSuppliesList[i].StandardNum){
                    rt.showErrorToast("剩余数量不可以大于标配数量!");
                    return;
                }
            }
            rt.showLoadingToast("正在保存...");
            RoomControlService.saveGuestSupplies($scope.guestSuppliesList)
                .success(function(data){
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功!");
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 保存數據
         * @private
         */
        function _saveClick() {
            var guestSupplies = {};
            guestSupplies.HotelId = "";
            guestSupplies.AuntId = "";
            guestSupplies.Item = [];
            var j = _setGuestSupplies(guestSupplies.Item);
            if (j !== 0) {
                rt.showErrorToast("有标配数量或剩余数量没有填写,请检查!");
                return;
            }

            var i = _checkInputNumber();
            if (i !== 0) {
                rt.showErrorToast("有标配数量或剩余数量填写有误,请检查!");
                return;
            }

            rt.showLoadingToast("正在保存...");
            RoomControlService.saveGuestSupplies(guestSupplies)
                .success(function (data) {
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功!");
                    $ionicHistory.goBack();
                })
                .error(function (err) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 检查输入
         * @private
         */
        function _checkInputNumber() {
            var j = 0;
            for (var i = 0; i < 12; i++) {
                if (!check($scope.standardNum[i]) || !check($scope.remainingNum[i])) {
                    j++;
                    return j;
                }
            }
            return j;
        }

        function check(s) {
            if (typeof s === "string" && rt.isNullOrEmptyString(s)) {
                return false;
            }

            var n = /^\d+$/;
            return n.test(s);
        }

        /**
         * 设置数量
         * @param Item
         * @private
         */
        function _setGuestSupplies(Item) {
            var ItemId = [10, 11, 20, 30, 40, 41, 50, 60, 70, 80, 90, 100];
            var j = 0;
            for (var i = 0; i < 12; i++) {
                Item[i] = {};
                Item[i].ItemId = ItemId[i];
                Item[i].StandardNum = $scope.standardNum[i];
                Item[i].RemainingNum = $scope.remainingNum[i];
                if (!check(Item[i].RemainingNum) || !check(Item[i].StandardNum)) {
                    j++;
                }
            }
            return j;
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-14 */
/*comment:房间查房统计 */

(function () {
    'use strict';

    xrmApp.controller("ManageCheckedCountController", ['$scope', '$state', 'rt', 'RoomControlService', ManageCheckedCountController]);

    function ManageCheckedCountController($scope, $state, rt, RoomControlService) {

        function init() {
            $scope.vdNum = 0;
            $scope.odNum = 0;
            $scope.vcNum = 0;
            $scope.gcNum = 0;
            $scope.unCleanNum = 0;
            $scope.unCheckedNum = 0;

            $scope.goToDetailView = _goToDetailView;
            $scope.goToCleanedView = _goToCleanedView;
            $scope.goToUnCleanedDetailView = _goToUnCleanedDetailView;

            _countRoomCleaned();
            _getCleanedRoom();
            _getCheckedRoom();
        }

        /**
         * 进入已查房的房间列表
         * @private
         */
        function _goToCleanedView(num, type){
            $state.go('roomControl-checked', {num: num, roomType: type});
        }

        /**
         * 进入未查房的房间列表
         * @private
         */
        function _goToDetailView(num) {
            $state.go("roomControl-unchecked", {num: num});
        }

        /**
         * 进入未打扫的房间列表
         * @private
         */
        function _goToUnCleanedDetailView(num) {
            $state.go("roomControl-unclean", {num: num});
        }

        /**
         * 获取所有已分房未打扫的房间,并计算各类房态的房间数
         * @private
         */
        function _countRoomCleaned() {
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.countRoomUnCheckedUnCleaned("")
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.roomCleanedList = data;

                    if (!rt.isNull($scope.roomCleanedList)) {
                        _countTypeNum($scope.roomCleanedList);
                    }
                })
                .error(function (err) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        /**
         * 获取所有已打扫未查房的房间,并计算各类房态的房间数
         * @private
         */
        function _getCleanedRoom(){
            RoomControlService.countRoomUnCheckedCleaned("")
                .success(function(data){
                    $scope.roomList = data;

                    if (!rt.isNull($scope.roomList)) {
                        //_countTypeNumTwo($scope.roomList);
                        $scope.unCheckedNum = $scope.roomList.length;
                    }else{
                        $scope.unCheckedNum = 0;
                    }
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 获取所有已查房的房间,并计算各类房态的房间数
         * @private
         */
        function _getCheckedRoom(){
            RoomControlService.countRoomChecked("")
                .success(function(data){
                    $scope.roomListChecked = data;

                    if (!rt.isNull($scope.roomListChecked)) {
                        _countTypeNumTwo($scope.roomListChecked);
                    }
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 计算各类房态的房间数
         * @private
         */
        function _countTypeNum(roomCleanedList) {
            for (var i = 0; i < roomCleanedList.length; i++) {
                // 未打扫
                if (roomCleanedList[i].cleanstate.Value === 10) {
                    $scope.unCleanNum += 1;
                }
            }
        }

        /**
         * 计算各类房态的房间数
         * @private
         */
        function _countTypeNumTwo(roomCleanedList) {
            for (var i = 0; i < roomCleanedList.length; i++) {
                // 退房 VD
                if (roomCleanedList[i].roomState === 1 && roomCleanedList[i].cleantype.Value === -1) {
                    $scope.vdNum += 1;
                }
                // 需抹尘的干净房 VC      如果房间异常为:房间有人住, 房间有行李时统计进入VD房
                if (roomCleanedList[i].roomState === 2) {
                    if (roomCleanedList[i].roomexception === true && (roomCleanedList[i].exceptionstr.Value === 21 || roomCleanedList[i].exceptionstr.Value === 22)) {
                        $scope.vdNum += 1;
                    }
                    else if (roomCleanedList[i].cleantype.Value === 10){
                        //$scope.gcNum += 1;  大清房间数会在下面的判断中增加
                    }
                    else{
                        $scope.vcNum += 1;
                    }
                }
                // 住客房 OD
                if ((roomCleanedList[i].roomState === 3 || roomCleanedList[i].roomState === 4) && roomCleanedList[i].cleantype.Value !== 10) {
                    $scope.odNum += 1;
                }
                // 大清房 GC
                if (roomCleanedList[i].cleantype.Value === 10) {
                    $scope.gcNum += 1;
                }
            }
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-14 */
/*comment:查房主界面 */

(function () {
    'use strict';

    xrmApp.controller("RoomCheckController", ['$scope', '$state', 'rt', 'RoomControlService', RoomCheckController]);

    function RoomCheckController($scope, $state, rt, RoomControlService) {

        function init() {
            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = 80;

            $scope.setStateOrException = _setStateOrException;

            _getTheRoomList();
        }

        /**
         * 设置异常或更改房态
         * @private
         */
        function _setStateOrException(room){
            // 设置异常
            if(room.roomexception && (room.exceptionstr.Value === 10 || room.exceptionstr.Value === 20 || room.exceptionstr.Value === 30)){
                _setRoomException(room);
            }
            if(!room.roomexception || room.exceptionstr.Value == 21 || room.exceptionstr.Value == 22 || room.exceptionstr.Value == 23){
                // 更改房态
                _updateRoomState(room);
            }
        }

        /**
         * 设置异常
         * @param room
         * @private
         */
        function _setRoomException(room){
            if(room.exceptionstr.Value === 20){
                rt.createDialog('module/roomControl/roomCheckDialogView.html', $scope, function (FieldValue) {
                    if (rt.isNull(FieldValue)) {
                        rt.showErrorToast("请选择房间异常类型!");
                        return;
                    }
                    // 取得从dialog上取得的值,调用接口把异常更新到数据库
                    rt.showLoadingToast("正在提交房间异常...");
                     RoomControlService.setRoomExceptionNote(room.roomno, FieldValue)
                     .success(function(data){
                        rt.hideLoadingToast();
                        _getTheRoomList();
                     })
                     .error(function(err){
                        rt.hideLoadingToast();
                        rt.showErrorToast(err);
                     });
                }).then(function (d) {
                    d.show();
                });
            }
            else{
                rt.showConfirmDialog("确认解除 " + room.roomno + "房间的" + room.exceptionstr.Name + " 异常?","")
                    .then(function(res){
                        if(res){
                            // 调接口,解除异常(DND  NNS)
                            rt.showLoadingToast("正在解除房间异常...");
                            RoomControlService.setRoomExceptionNote(room.roomno, -1)
                                .success(function(data){
                                    rt.hideLoadingToast();
                                    _getTheRoomList();
                                })
                                .error(function(err){
                                    rt.hideLoadingToast();
                                    rt.showErrorToast(err);
                                });
                        } else{
                            return;
                        }
                    });
            }
        }

        /**
         * 更改房态
         * @param room
         * @private
         */
        function _updateRoomState(room){
            $state.go("roomControl-state", {roomNum: room.roomno, roomState: room.roomstate});
        }

        /**
         * 获取所有房间
         * @private
         */
        function _getTheRoomList(){
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.CountRoomCleaned("")
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.roomList = data;

                    if(!rt.isNull($scope.roomList)){
                        // 每日单项
                        $scope.dailyItem = JSON.parse($scope.roomList[0].dailyitemsid);
                    }
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2016-01-14 */
/*comment:查房Dialog界面 */
(function () {
    'use strict';
    xrmApp.controller("RoomCheckDialogController", ['$scope', 'rtUtils', 'rt', RoomCheckDialogController]);

    function RoomCheckDialogController($scope, rtUtils, rt) {

        _init();

        function _init() {
            $scope.selectData = _selectData;
        }

        function _selectData(u){
            $scope.closeDialog();
            if(rtUtils.isFunction($scope.onDataSelected) && !rtUtils.isNull(u))
            {
                $scope.onDataSelected(u);
            }
        }
    }
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-18 */
/*comment:已查房房间明细 */

(function () {
    'use strict';

    xrmApp.controller("RoomCheckedListController", ['$scope', '$state',  '$stateParams', 'rt', 'RoomControlService', RoomCheckedListController]);

    function RoomCheckedListController($scope, $state, $stateParams, rt, RoomControlService) {

        function init() {
            $scope.roomType = $stateParams.roomType;
            $scope.cleanNum = $stateParams.num;

            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = 80;

            _countRoomCleaned();
        }

        /**
         * 获取所有房间
         * @private
         */
        function _countRoomCleaned(){
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.countRoomChecked($scope.roomType)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.roomList = data;
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global UIMenu:false */
/*global xrmApp:false */
/*author:郭伟 */
/*date:2016-01-12 */
/*comment:房间清理界面 */
(function () {
    'use strict';

    xrmApp.controller("RoomCleanController", ['$scope', '$filter', 'rt', 'RoomControlService', RoomCleanController]);

    var hour;
    var minute;
    var timePoint;

    function RoomCleanController($scope, $filter, rt, RoomControlService) {

        $scope.todayTime = $filter('date')(new Date(), 'yyyy-MM-dd');

        // 存放界面上被选择的房间
        $scope.roomTemp = "";
        // 存放界面上被选择的房间的房态
        $scope.roomState = 0;
        // 存放所有房间的选择状态
        $scope.state = [];

        function init() {
            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = $scope.dp.documentHeight / 5.3;

            $scope.viewRefresh = _viewRefresh;
            $scope.startClean = _startClean;
            $scope.doException = _doException;
            $scope.viewRefresh = _viewRefresh;

            /*RoomControlService.getTimePoint()
                .success(function(data){
                    if(rt.isNull(data)){
                        rt.showErrorToast("没有获得白中班时间！");
                        return;
                    }
                    hour = data.strhour.substr(0, 1) !== '0' ? data.strhour : data.strhour.substr(1, 1);
                    minute = data.strminute.substr(0, 1) !== '0' ? data.strminute : data.strminute.substr(1, 1);

                    timePoint = new Date($scope.todayTime + " " + hour + ":" + minute);

                    if(new Date() > timePoint){
                        //调中班的接口
                        _getRoomListMid();
                    }
                    else{
                        //调白班的接口
                        _getTheRoomList();
                    }
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });*/

            //_getTheRoomList();

            var promise = RoomControlService.getIsMid().then(function (resp) {
                //获取当前人员是否为中班人员
                $scope.isMid = resp.data;
                return RoomControlService.getTheRoomList();
            }).then(function (resp) {
                $scope.dividedRoomList = resp.data;

                if(!rt.isNull($scope.dividedRoomList)){
                    // 每日单项
                    $scope.dailyItem = JSON.parse($scope.dividedRoomList[0].dailyitemsid);

                    // 初始化所有房间的选择状态
                    for(var i = 0; i < $scope.dividedRoomList.length; i++){
                        $scope.state[i] = 0;
                    }
                }
            }, function (err) {
                rt.showErrorToast(err);
            });
        }

        /**
         * 打扫开始、完成
         * @param room
         * @private
         */
        function _startClean(room, index){
            if(room.roomno === $scope.roomTemp && !$scope.isMid){
                $scope.state[index] = 0;
                // 弹出确认对话框,确认是否完成打扫
                //alert(roomTemp + "房间打扫完成!");
                rt.showConfirmDialog(room.roomno + "房间, 是否确认打扫完成？", "")
                    .then(function(res){
                        if(res){
                            rt.showLoadingToast("正在提交...");
                            // 调接口,把打扫好的房间更新到数据库中
                            RoomControlService.saveTheCleanedRoom(room.roomno)
                                .success(function(data){
                                    rt.hideLoadingToast();
                                    $scope.roomTemp = "";
                                    // 打扫完一间房后刷新界面
                                    _getTheRoomList();
                                })
                                .error(function(err){
                                    rt.hideLoadingToast();
                                    rt.showErrorToast(err);
                                });
                        } else{
                            $scope.roomTemp = "";
                            return;
                        }
                    });
                $scope.roomTemp = "";
                $scope.roomState = 0;
            }
            else if(room.roomno !== $scope.roomTemp && !$scope.isMid){
                // 记录下选择的房间信息
                $scope.roomTemp = room.roomno;
                $scope.roomState = room.roomstate;
                // 改变选择的房间的状态(或是可以新添个状态给选择的房间)
                for(var i = 0; i < $scope.dividedRoomList.length; i++){
                    if(i === index){
                        $scope.state[i] = 1;
                    }
                    else{
                        $scope.state[i] = 0;
                    }
                }
                // 添加一个开始打扫时间，更新到实体中去
                RoomControlService.startCleanRoom(room.roomno)
                    .success(function (data) {
                    })
                    .error(function(err){
                        rt.showErrorToast(err);
                    });
            }
            else if(room.roomno === $scope.roomTemp && $scope.isMid){
                $scope.state[index] = 0;
                // 弹出确认对话框,确认是否完成打扫
                //alert(roomTemp + "房间打扫完成!");
                rt.showConfirmDialog(room.roomno + "房间, 是否确认打扫完成？", "")
                    .then(function(res){
                        if(res){
                            rt.showLoadingToast("正在提交...");
                            // 调接口,把打扫好的房间更新到数据库中
                            RoomControlService.saveTheCleanedRoom(room.roomno)
                                .success(function(data){
                                    rt.hideLoadingToast();
                                    $scope.roomTemp = "";
                                    // 打扫完一间房后刷新界面
                                    _getRoomListMid();
                                })
                                .error(function(err){
                                    rt.hideLoadingToast();
                                    rt.showErrorToast(err);
                                });
                        } else{
                            $scope.roomTemp = "";
                            return;
                        }
                    });
                $scope.roomTemp = "";
                $scope.roomState = 0;
            }else if(room.roomno !== $scope.roomTemp && $scope.isMid && $scope.roomTemp !== ""){
                rt.showErrorToast(room.roomno + "房间还未完成打扫！请先把" + room.roomno + "房间打扫完成！");
            }
            else if($scope.roomTemp === "" && $scope.isMid){
                // 记录下选择的房间信息
                $scope.roomTemp = room.roomno;
                $scope.roomState = room.roomstate;
                // 改变选择的房间的状态(或是可以新添个状态给选择的房间)
                for(var j = 0; j < $scope.dividedRoomList.length; j++){
                    if(j === index){
                        $scope.state[j] = 1;
                    }
                    else{
                        $scope.state[j] = 0;
                    }
                }
                // 赋值阿姨给房间
                RoomControlService.setAuntToRoom(room.roomno)
                    .success(function(data){
                    })
                    .error(function(err){
                        rt.showErrorToast(err);
                    });
            }
            /*if(room.roomno === $scope.roomTemp && new Date() <= timePoint){
                $scope.state[index] = 0;
                // 弹出确认对话框,确认是否完成打扫
                //alert(roomTemp + "房间打扫完成!");
                rt.showConfirmDialog(room.roomno + "房间, 是否确认打扫完成？", "")
                    .then(function(res){
                        if(res){
                            rt.showLoadingToast("正在提交...");
                            // 调接口,把打扫好的房间更新到数据库中
                            RoomControlService.saveTheCleanedRoom(room.roomno)
                                .success(function(data){
                                    rt.hideLoadingToast();
                                    $scope.roomTemp = "";
                                    // 打扫完一间房后刷新界面
                                    _getTheRoomList();
                                })
                                .error(function(err){
                                    rt.hideLoadingToast();
                                    rt.showErrorToast(err);
                                });
                        } else{
                            $scope.roomTemp = "";
                            return;
                        }
                    });
                $scope.roomTemp = "";
                $scope.roomState = 0;
            }
            else if(room.roomno !== $scope.roomTemp && new Date() <= timePoint){
                // 记录下选择的房间信息
                $scope.roomTemp = room.roomno;
                $scope.roomState = room.roomstate;
                // 改变选择的房间的状态(或是可以新添个状态给选择的房间)
                for(var i = 0; i < $scope.dividedRoomList.length; i++){
                    if(i === index){
                        $scope.state[i] = 1;
                    }
                    else{
                        $scope.state[i] = 0;
                    }
                }
                // 添加一个开始打扫时间，更新到实体中去
                RoomControlService.startCleanRoom(room.roomno)
                    .success(function (data) {
                    })
                    .error(function(err){
                        rt.showErrorToast(err);
                    });
            }
            else if(room.roomno === $scope.roomTemp && new Date() > timePoint){
                $scope.state[index] = 0;
                // 弹出确认对话框,确认是否完成打扫
                //alert(roomTemp + "房间打扫完成!");
                rt.showConfirmDialog(room.roomno + "房间, 是否确认打扫完成？", "")
                    .then(function(res){
                        if(res){
                            rt.showLoadingToast("正在提交...");
                            // 调接口,把打扫好的房间更新到数据库中
                            RoomControlService.saveTheCleanedRoom(room.roomno)
                                .success(function(data){
                                    rt.hideLoadingToast();
                                    $scope.roomTemp = "";
                                    // 打扫完一间房后刷新界面
                                    _getRoomListMid();
                                })
                                .error(function(err){
                                    rt.hideLoadingToast();
                                    rt.showErrorToast(err);
                                });
                        } else{
                            $scope.roomTemp = "";
                            return;
                        }
                    });
                $scope.roomTemp = "";
                $scope.roomState = 0;
            }else if(room.roomno !== $scope.roomTemp && new Date() > timePoint && $scope.roomTemp !== ""){
                rt.showErrorToast(room.roomno + "房间还未完成打扫！请先把" + room.roomno + "房间打扫完成！");
            }
            else if($scope.roomTemp === "" && new Date() > timePoint){
                // 记录下选择的房间信息
                $scope.roomTemp = room.roomno;
                $scope.roomState = room.roomstate;
                // 改变选择的房间的状态(或是可以新添个状态给选择的房间)
                for(var j = 0; j < $scope.dividedRoomList.length; j++){
                    if(j === index){
                        $scope.state[j] = 1;
                    }
                    else{
                        $scope.state[j] = 0;
                    }
                }
                // 赋值阿姨给房间
                RoomControlService.setAuntToRoom(room.roomno)
                    .success(function(data){
                    })
                    .error(function(err){
                       rt.showErrorToast(err);
                    });
            }*/
        }

        /**
         * 异常处理
         * @private
         */
        function _doException(){
            if($scope.roomTemp !== ""){
                rt.createDialog('module/roomControl/dialogView.html', $scope, function (FieldValue) {
                    if (rt.isNull(FieldValue)) {
                        rt.showErrorToast("请选择房间异常项!");
                        return;
                    }
                    //alert(FieldValue);
                    // 取得从dialog上取得的值,调用接口把异常更新到数据库
                    rt.showLoadingToast("正在提交房间异常...");
                    RoomControlService.setRoomException($scope.roomTemp, FieldValue)
                        .success(function(data){
                            rt.hideLoadingToast();
                            $scope.roomTemp = "";
                            if(new Date() > timePoint){
                                //调中班的接口
                                _getRoomListMid();
                            }
                            else{
                                //调白班的接口
                                _getTheRoomList();
                            }
                        })
                        .error(function(err){
                            rt.hideLoadingToast();
                            rt.showErrorToast(err);
                            $scope.roomTemp = "";
                        });
                }).then(function (d) {
                    d.show();
                });
            }
            else{
                rt.showErrorToast("请先选择存在异常的房间!");
            }
        }

        /**
         * 获取当前用户所负责的房间
         * @private
         */
        function _getTheRoomList(){
            RoomControlService.getTheRoomList()
                .success(function(data){
                    $scope.dividedRoomList = data;

                    if(!rt.isNull($scope.dividedRoomList)){
                        // 每日单项
                        $scope.dailyItem = JSON.parse($scope.dividedRoomList[0].dailyitemsid);

                        // 初始化所有房间的选择状态
                        for(var i = 0; i < $scope.dividedRoomList.length; i++){
                            $scope.state[i] = 0;
                        }
                    }
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        /**
         * 获取中班房间
         * @private
         */
        function _getRoomListMid(){
            RoomControlService.getRoomListMid()
                .success(function(data){
                    $scope.dividedRoomList = data;

                    if(!rt.isNull($scope.dividedRoomList)){
                        // 每日单项
                        $scope.dailyItem = JSON.parse($scope.dividedRoomList[0].dailyitemsid);

                        // 初始化所有房间的选择状态
                        for(var i = 0; i < $scope.dividedRoomList.length; i++){
                            $scope.state[i] = 0;

                            // 更新每条记录，如果房间又所属阿姨就把阿姨信息去掉
                            if(!rt.isNullOrEmptyString($scope.dividedRoomList[i].auntidName)){
                                RoomControlService.updateRoomInfo($scope.dividedRoomList[i].roomno, hour + ":" + minute + ":00")
                                    .success(function(data){
                                    })
                                    .error(function(err){
                                        rt.showErrorToast(err);
                                    });
                            }
                        }
                    }
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh(){
            $scope.dividedRoomList = [];
            /*if(timePoint === undefined){
                rt.showErrorToast("没有获取到白中班时间!");
                $scope.$broadcast('scroll.refreshComplete');
                return;
            }*/
            /*if(new Date() > timePoint){
                //调中班的接口
                _getRoomListMid();
            }
            else{
                //调白班的接口
                _getTheRoomList();
            }*/
            _getTheRoomList();
            $scope.$broadcast('scroll.refreshComplete');
        }

        init();
    }
})();

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-14 */
/*comment:房间清洁统计 */

(function () {
    'use strict';

    xrmApp.controller("RoomCleanCountController", ['$scope', '$state', 'rt', 'RoomControlService', RoomCleanCountController]);

    function RoomCleanCountController($scope, $state, rt, RoomControlService) {

        function init() {
            $scope.vdNum = 0;
            $scope.odNum = 0;
            $scope.vcNum = 0;
            $scope.gcNum = 0;
            $scope.unCleanNum = 0;

            $scope.goToDetailView = _goToDetailView;
            $scope.goToCleanedView = _goToCleanedView;

            _countRoomCleaned();
            _getCleanedRoom();
        }

        /**
         * 进入已打扫的房间列表
         * @private
         */
        function _goToCleanedView(num, type){
            $state.go('roomControl-cleaned', {num: num, roomType: type});
        }

        /**
         * 进入未打扫的房间列表
         * @private
         */
        function _goToDetailView(num) {
            $state.go("roomControl-unclean", {num: num});
        }

        /**
         * 获取所有已打扫的房间,并计算各类房态的房间数
         * @private
         */
        function _countRoomCleaned() {
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.CountRoomCleaned("")
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.roomCleanedList = data;

                    if (!rt.isNull($scope.roomCleanedList)) {
                        _countTypeNum($scope.roomCleanedList);
                    }
                })
                .error(function (err) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        function _getCleanedRoom(){
            RoomControlService.getCleanedRooms("")
                .success(function(data){
                    $scope.roomList = data;

                    if (!rt.isNull($scope.roomList)) {
                        _countTypeNumTwo($scope.roomList);
                    }
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        /**
         * 计算各类房态的房间数
         * @private
         */
        function _countTypeNumTwo(roomCleanedList) {
            for (var i = 0; i < roomCleanedList.length; i++) {
                // 退房 VD
                if (roomCleanedList[i].roomState === 1 && roomCleanedList[i].cleantype.Value === -1) {
                    $scope.vdNum += 1;
                }
                // 需抹尘的干净房 VC      如果房间异常为:房间有人住, 房间有行李时统计进入VD房
                if (roomCleanedList[i].roomState === 2) {
                    if (roomCleanedList[i].roomexception === true && (roomCleanedList[i].exceptionstr.Value === 21 || roomCleanedList[i].exceptionstr.Value === 22)) {
                        $scope.vdNum += 1;
                    }
                    else if (roomCleanedList[i].cleantype.Value === 10){
                        //$scope.gcNum += 1;  大清房间数会在下面的判断中增加
                    }
                    else{
                        $scope.vcNum += 1;
                    }
                }
                // 住客房 OD
                if ((roomCleanedList[i].roomState === 3 || roomCleanedList[i].roomState === 4) && roomCleanedList[i].cleantype.Value !== 10) {
                    $scope.odNum += 1;
                }
                // 大清房 GC
                if (roomCleanedList[i].cleantype.Value === 10) {
                    $scope.gcNum += 1;
                }
            }
        }

        /**
         * 计算各类房态的房间数
         * @private
         */
        function _countTypeNum(roomCleanedList) {
            for (var i = 0; i < roomCleanedList.length; i++) {
                // 退房 VD
                /*if (roomCleanedList[i].roomstate === 1 && roomCleanedList[i].cleanstate.Value === 20 && roomCleanedList[i].cleantype.Value === -1) {
                    $scope.vdNum += 1;
                }
                // 需抹尘的干净房 VC      如果房间异常为:房间有人住, 房间有行李时统计进入VD房
                if (roomCleanedList[i].roomstate === 2 && roomCleanedList[i].cleanstate.Value === 20) {
                    if (roomCleanedList[i].roomexception === true && (roomCleanedList[i].exceptionstr.Value === 21 || roomCleanedList[i].exceptionstr.Value === 22)) {
                        $scope.vdNum += 1;
                    }
                    else if (roomCleanedList[i].cleantype.Value === 10){
                        //$scope.gcNum += 1;  大清房间数会在下面的判断中增加
                    }
                    else{
                        $scope.vcNum += 1;
                    }
                }
                // 住客房 OD
                if ((roomCleanedList[i].roomstate === 3 || roomCleanedList[i].roomstate === 4) && roomCleanedList[i].cleanstate.Value === 20 && roomCleanedList[i].cleantype.Value !== 10) {
                    $scope.odNum += 1;
                }
                // 大清房 GC
                if (roomCleanedList[i].cleanstate.Value === 20 && roomCleanedList[i].cleantype.Value === 10) {
                    $scope.gcNum += 1;
                }*/
                // 未打扫
                if (roomCleanedList[i].cleanstate.Value === 10) {
                    $scope.unCleanNum += 1;
                }
            }
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-18 */
/*comment:已打扫房间明细 */

(function () {
    'use strict';

    xrmApp.controller("RoomCleanedListController", ['$scope', '$state',  '$stateParams', 'rt', 'RoomControlService', RoomCleanedListController]);

    function RoomCleanedListController($scope, $state, $stateParams, rt, RoomControlService) {

        function init() {
            $scope.roomType = $stateParams.roomType;
            $scope.cleanNum = $stateParams.num;

            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = 80;

            //_countRoomCleaned();
            _getCleanedRoom();
        }

        /**
         * 获取所有房间
         * @private
         */
        function _countRoomCleaned(){
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.CountRoomCleaned($scope.roomType)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.roomList = data;
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        function _getCleanedRoom(){
            RoomControlService.getCleanedRooms($scope.roomType)
                .success(function(data){
                    $scope.roomList = data;
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-12 */
/*comment:客房房控主界面 */

(function () {
    'use strict';

    xrmApp.controller("RoomControlCardController", ['$scope', '$state', 'rt', 'RoomControlService', RoomControlCardController]);

    function RoomControlCardController($scope, $state, rt, RoomControlService) {

        function init() {
            $scope.goBackClick = _goBackClick;
            $scope.goToNextView = _goToNextView;
            $scope.exitApplication = _exitApplication;
            _loadData();
        }

        /**
         * goTo 清洁, 客用品统计, 查房, 房间清洁统计, 重新分配   界面
         * @param index
         * @private
         */
        function _goToNextView(index){
            switch(index){
                //清洁
                case 0: $state.go("roomControl-clean");break;
                //客用品统计
                case 1: $state.go("guest-supplies");break;
                //查房
                case 2: $state.go("roomControl-check");break;
                //房间清洁统计
                case 3: $state.go("roomControl-count");break;
                //重新分配
                case 4: $state.go("roomControl-redivide");break;
                //查房统计
                case 5: $state.go("roomControl-manage");break;
                default :break;
            }
        }

        function _loadData(){
            RoomControlService.getPrivilege()
                .success(function(data){
                    $scope.roleName = data;
                })
                .error(function(err){
                    rt.showErrorToast(err);
                });
        }

        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }

        /**
         * 退出应用
         * @private
         */
        function _exitApplication(){
            $state.go("app-exit");
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-18 */
/*comment:重新分配 */

(function () {
    'use strict';

    xrmApp.controller("RoomReDivideController", ['$scope', '$ionicHistory', 'rt', 'RoomControlService', RoomReDivideController]);

    function RoomReDivideController($scope, $ionicHistory, rt, RoomControlService) {
        // 存储本地数据
        var localData;

        function init() {
            $scope.vm = {};

            $scope.isSure = _isSure;
            $scope.isCancel = _isCancel;

            _getAuntList();
            _getLocalStorage();
        }

        /**
         * 取消重新分配
         */
        function _isCancel(){
            $ionicHistory.goBack();
        }

        /**
         * 确认提交重新分配
         * @private
         */
        function _isSure(){
            if(rt.isNull($scope.vm.roomNo)){
                rt.showErrorToast("请输入房间号!");
                return;
            }
            if(rt.isNull($scope.vm.aunt)){
                rt.showErrorToast("请选择服务员!");
                return;
            }
            if(!_isNumber($scope.vm.roomNo)){
                rt.showErrorToast("输入的房间号中有非数字!");
                return;
            }
            _updateLocalData();
            _deleteTheRoomDataByRoomNo();
            var saveData = {};
            saveData.localAuntid = JSON.stringify($scope.auntid);
            saveData.localDailyitemid = JSON.stringify($scope.dailyItemid);
            saveData.localDividedRoom = JSON.stringify($scope.dividedRoom);
            // 调用接口重新分配
            RoomControlService.reDivideRoomToAunt($scope.vm.roomNo, $scope.vm.aunt)
                .success(function(data){
                    rt.showSuccessToast(data);
                    // 需要把最新的分房结果存一份到缓存中,已便在PC端看到重新分配的结果
                    // 保存最新的缓存
                    RoomControlService.saveLocalStorage(saveData)
                        .success(function(resp){ })
                        .error(function(err){
                            rt.showErrorToast(err);
                        });
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        /**
         * 检查输入的是否为数字
         * @param num
         * @private
         */
        function _isNumber(num){
            var reg = /^(0|[1-9]\d*)$/;
            return reg.test(num);
        }

        /**
         * 重新分配后,更新缓存中的阿姨与房间的对应关系
         * @private
         */
        function _updateLocalData(){
            _findRoomDataByRoomNo();
            for(var i = 0; i < $scope.dividedRoom.length; i++){
                for(var j = 0; j < $scope.dividedRoom[i].length; j++){
                    if($scope.dividedRoom[i][j].auntid === $scope.vm.aunt){
                        $scope.dividedRoom[i].push($scope.roomData);
                        $scope.dividedRoom[i][$scope.dividedRoom[i].length - 1].auntid = $scope.vm.aunt;
                        return;
                    }
                }
            }
        }

        /**
         * 删除重新分配前的房间数据
         * @private
         */
        function _deleteTheRoomDataByRoomNo(){
            for(var i = 0; i < $scope.dividedRoom.length; i++){
                for(var j = 0; j < $scope.dividedRoom[i].length; j++){
                    if($scope.dividedRoom[i][j].roomno === $scope.vm.roomNo.toString()){
                        $scope.dividedRoom[i].splice(j, 1);
                        return;
                    }
                }
            }
            rt.showErrorToast($scope.vm.roomNo + "房间,在分房好的房间中不存在!");
        }

        /**
         * 查找重新分配房间的房间数据
         * @private
         */
        function _findRoomDataByRoomNo(){
            for(var i = 0; i < $scope.dividedRoom.length; i++){
                for(var j = 0; j < $scope.dividedRoom[i].length; j++){
                    if($scope.dividedRoom[i][j].roomno === $scope.vm.roomNo.toString()){
                        $scope.roomData = $scope.dividedRoom[i][j];
                        return;
                    }
                }
            }
            rt.showErrorToast($scope.vm.roomNo + "房间,在分房好的房间中不存在!");
        }

        /**
         * 获取阿姨列表
         * @private
         */
        function _getAuntList(){
            RoomControlService.getAuntList()
                .success(function(data){
                    $scope.auntList = data;
                    $scope.vm.aunt = data[0].AuntId;
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        /**
         * 读取缓存
         * @private
         */
        function _getLocalStorage(){
            RoomControlService.getLocalStorage()
                .success(function(data){
                    localData = data;
                    if(!rt.isNull(localData)){
                        $scope.auntid = JSON.parse(localData.localAuntid);
                        $scope.dailyItemid = JSON.parse(localData.localDailyitemid);
                        $scope.dividedRoom = JSON.parse(localData.localDividedRoom);
                    }
                })
                .error(function(err){
                   rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-15 */
/*comment:变更房态 */

(function () {
    'use strict';

    xrmApp.controller("RoomStateChangeController", ['$scope', '$timeout', '$ionicHistory', '$stateParams', 'rt', 'RoomControlService', RoomStateChangeController]);

    function RoomStateChangeController($scope, $timeout, $ionicHistory, $stateParams, rt, RoomControlService) {

        function init() {
            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = 50;

            $scope.dp.roomStateChangeTo = 2;
            if($stateParams.roomState === "1"){
                $scope.dp.roomStateChangeTo = 2;
            }
            if($stateParams.roomState === "3"){
                $scope.dp.roomStateChangeTo = 4;
            }
            $scope.roomState = [{id: 1, name: '空脏房'}, {id: 2, name: '空净房'}, {id: 3, name: '在住脏房'}, {id: 4, name: '在住干净房'}];

            $scope.setScoreToRoom = _setScoreToRoom;
        }

        /**
         * 房间打分,更新房态
         * @param index
         * @private
         */
        function _setScoreToRoom(index){
            rt.showConfirmDialog("是否确认提交?", "")
                .then(function(res){
                    if(res){
                        // 调接口,把更新后的房态和房间评分设置到对应的房间上(还需要把房态更改回写到PMS)
                        RoomControlService.checkRoom($stateParams.roomNum, index, $stateParams.roomState, $scope.dp.roomStateChangeTo)
                            .success(function(data){
                                rt.showSuccessToast(data);
                                $timeout(function () {
                                    $ionicHistory.goBack();
                                }, 2000);
                            })
                            .error(function(err){
                                rt.showErrorToast(err);
                            });
                    }
                });
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-14 */
/*comment:未查房房间明细 */

(function () {
    'use strict';

    xrmApp.controller("RoomUncheckedController", ['$scope', '$state',  '$stateParams', 'rt', 'RoomControlService', RoomUncheckedController]);

    function RoomUncheckedController($scope, $state, $stateParams, rt, RoomControlService) {

        function init() {
            $scope.unCleanNum = $stateParams.num;

            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = 80;

            _countRoomCleaned();
        }

        /**
         * 获取所有房间,并计算未打扫房态的房间
         * @private
         */
        function _countRoomCleaned(){
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.countRoomUnCheckedCleaned("")
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.roomUncleanList = data;
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global angular:false */
/*global _:false */
/*global xrmApp:false */
/*global UITaskView:false */
/*author:郭伟 */
/*date:2016-01-14 */
/*comment:未打扫房间明细 */

(function () {
    'use strict';

    xrmApp.controller("RoomUncleanController", ['$scope', '$state',  '$stateParams', 'rt', 'RoomControlService', RoomUncleanController]);

    function RoomUncleanController($scope, $state, $stateParams, rt, RoomControlService) {

        function init() {
            $scope.unCleanNum = $stateParams.num;

            $scope.dp = {};
            $scope.dp.documentWidth = rt.getDocumentWidth();
            $scope.dp.documentHeight = rt.getDocumentHeight();
            //最小的Grid块的高度和宽度
            $scope.dp.smallWidth = $scope.dp.documentWidth / 3;
            $scope.dp.smallHeight = 80;

            _countRoomCleaned();
        }

        /**
         * 获取所有房间,并计算未打扫房态的房间
         * @private
         */
        function _countRoomCleaned(){
            rt.showLoadingToast("正在加载数据...");
            RoomControlService.CountRoomCleaned("")
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.roomUncleanList = data;
                })
                .error(function(err){
                    rt.hideLoadingToast();
                    rt.showErrorToast(err);
                });
        }

        init();
    }
})();



/*global xrmApp:false*/
(function () {
    'use strict';
    function ChooseRoleController($scope, $stateParams, rt, $timeout, TaskService) {
        var _pageIndex = 1;
        var _pageSize = rt.getPaginationSize();

        //是否需要加载更多
        var _isLoadMore = false;

        function _init() {
            $scope.vm = {};
            $scope.vm.queryValue = "";

            $scope.vm.data = [];
            //加载更多按钮点击事件
            $scope.loadMoreClick = _loadMoreClick;
            //下拉刷新
            $scope.viewRefresh = _viewRefresh;
            $scope.btnSaveClick = _btnSaveClick;
            $scope.search = _search;
            $scope.selectData = _selectData;
            $scope.removeValue = _removeValue;

            _loadData(function (data) {
                rt.hideLoadingToast();
                $scope.vm.data = data;
            }, function (error) {
                rt.hideLoadingToast();
            });
        }

        /**
         * 加载数据
         * @private
         */
        function _loadData(success, msg) {
            TaskService.getRoleList($scope.vm.queryValue, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        _isLoadMore = false;
                    }
                    else {
                        _isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (msg) {
                        msg();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _loadData(function (data) {
                $scope.vm.data = data;
            }, null);
            //Stop the ion-refresher from spinning
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMoreClick() {
            if (_isLoadMore) {
                _pageIndex += 1;
                _loadData(function (data) {
                    $scope.vm.data.push.apply($scope.vm.data, data);
                }, null);
                //Stop the ion-refresher from spinning
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
            else {
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
        }

        /**
         * 搜索框搜索按钮点击事件
         * @private
         */
        function _search() {
            _pageIndex = 1;
            rt.showLoadingToast("正在加载数据...");
            _loadData(function (data) {
                rt.hideLoadingToast();
                $scope.vm.data = data;
            }, function (error) {
                rt.hideLoadingToast();
            });
        }

        /**
         * 选择
         */
        function _selectData(u) {
            $scope.closeDialog();
            if (rt.isFunction($scope.onDataSelected) && !rt.isNull(u)) {
                $scope.onDataSelected(u);
            }
        }

        function _btnSaveClick(){
            var u = [];

            for(var i = 0;i<$scope.vm.data.length;i++)
            {
                if ($scope.vm.data[i].check){
                    u.push($scope.vm.data[i]);
                }
            }

            if (u.length === 0){
                rt.showErrorToast("请选择酒店");
                return;
            }

            $scope.closeDialog();
            if (rt.isFunction($scope.onDataSelected) && !rt.isNull(u)) {
                $scope.onDataSelected(u);
            }
        }

        /**
         * 移除值
         * @param user
         * @private
         */
        function _removeValue(user) {
            $scope.closeDialog();
            $scope.onDataSelected(user);
        }

        _init();
    }

    xrmApp.controller("ChooseRoleController", ['$scope', '$stateParams', 'rt', '$timeout', 'TaskService', ChooseRoleController]);
})();
/*global xrmApp:false*/
(function () {
    'use strict';
    function ChooseUserController($scope, $stateParams, rt, $timeout, TaskService) {
        var _pageIndex = 1;
        var _pageSize = rt.getPaginationSize();

        //是否需要加载更多
        var _isLoadMore = false;

        function _init() {
            $scope.vm = {};
            $scope.vm.queryValue = "";
            $scope.vm.ismanagedself = 0;

            $scope.vm.roleData = [];
            $scope.vm.userData = [];
            //加载更多按钮点击事件
            $scope.loadMoreClick = _loadMoreClick;
            //下拉刷新
            $scope.viewRefresh = _viewRefresh;
            $scope.btnSaveClick = _btnSaveClick;
            $scope.search = _search;
            $scope.selectData = _selectData;
            $scope.removeValue = _removeValue;

            rt.showLoadingToast("正在加载数据...");
            TaskService.getHotelScreen()
                .success(function(ret){
                    $scope.vm.HotelScreen = ret;

                    TaskService.getRoleList($scope.vm.queryValue)
                        .success(function(data){
                            $scope.vm.roleData = data;
                            _loadData(function (data) {
                                rt.hideLoadingToast();
                                $scope.vm.userData = data;
                            }, function (error) {
                                rt.hideLoadingToast(error);
                            });
                        })
                        .error(function(error){
                            rt.hideLoadingToast();
                            rt.showErrorToast(error);
                        });

                })
                .error(function(error){
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });


        }

        /**
         * 加载数据
         * @private
         */
        function _loadData(success, msg) {
            TaskService.getUserList($scope.vm.queryValue,$scope.vm.HotelScreen, _pageIndex, _pageSize)
                .success(function (data) {
                    if (success) {
                        success(data);
                    }
                    if (data.length < _pageSize) {
                        _isLoadMore = false;
                    }
                    else {
                        _isLoadMore = true;
                    }
                })
                .error(function (error) {
                    if (msg) {
                        msg();
                    }
                    rt.showErrorToast(error);
                });
        }

        /**
         * 下拉刷新
         * @private
         */
        function _viewRefresh() {
            _pageIndex = 1;
            _loadData(function (data) {
                $scope.vm.userData = data;
            }, null);
            //Stop the ion-refresher from spinning
            $scope.$broadcast('scroll.refreshComplete');
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMoreClick() {
            if (_isLoadMore) {
                _pageIndex += 1;
                _loadData(function (data) {
                    $scope.vm.userData.push.apply($scope.vm.userData, data);
                }, null);
                //Stop the ion-refresher from spinning
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
            else {
                $scope.$broadcast('scroll.infiniteScrollComplete');
            }
        }

        /**
         * 搜索框搜索按钮点击事件
         * @private
         */
        function _search() {
            _pageIndex = 1;
            rt.showLoadingToast("正在加载数据...");
            _loadData(function (data) {
                rt.hideLoadingToast();
                $scope.vm.userData = data;
            }, function (error) {
                rt.hideLoadingToast();
            });
        }

        /**
         * 选择
         */
        function _selectData(u) {
            $scope.closeDialog();
            if (rt.isFunction($scope.onDataSelected) && !rt.isNull(u)) {
                $scope.onDataSelected(u);
            }
        }

        function _btnSaveClick(){
            var data = {};
            data.userList = [];
            data.roleList = [];

            for(var i = 0;i<$scope.vm.userData.length;i++)
            {
                if ($scope.vm.userData[i].check){
                    data.userList.push($scope.vm.userData[i]);
                }
            }

            for(var j = 0;j<$scope.vm.roleData.length;j++)
            {
                if ($scope.vm.roleData[j].check){
                    data.roleList.push($scope.vm.roleData[j]);
                }
            }

            if (data.roleList.length === 0 && data.userList.length === 0){
                rt.showErrorToast("请选择发布对象");
                return;
            }

            $scope.closeDialog();
            if (rt.isFunction($scope.onDataSelected) && !rt.isNull(data)) {
                $scope.onDataSelected(data);
            }
        }

        /**
         * 移除值
         * @param user
         * @private
         */
        function _removeValue(user) {
            $scope.closeDialog();
            $scope.onDataSelected(user);
        }

        _init();
    }

    xrmApp.controller("ChooseUserController", ['$scope', '$stateParams', 'rt', '$timeout', 'TaskService', ChooseUserController]);
})();
/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function TaskEditController($scope, $state, $stateParams,rt,$ionicHistory,$ionicActionSheet,TaskService) {

        function init() {
            $scope.vm = {};
            //初始化负责人为空
            $scope.vm.HotelId = {};

            if (rt.isNullOrEmptyString($stateParams.id)){
                $scope.vm.Id = rt.newGuid();
                //$scope.vm.Data = new Date();
                _getLevelOptions();
            }else{
                _getTaskData();
            }
            $scope.btnSaveClick = _btnSaveClick;
            $scope.chooseOwner = _chooseOwner;
        }

        /**
         * 获取任务信息
         * @private
         */
        function _getTaskData(){

        }

        /**
         * 获取重要级别选项信息
         * @private
         */
        function _getLevelOptions(){
            TaskService.getLevelOptions()
                .success(function(data){
                    $scope.vm.Level = data;
                    rt.hideLoadingToast();
                })
                .error(function(data){
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载数据...");
        }

        /**
         * 选择用户信息
         * @private
         */
        function _chooseOwner(){
            rt.createDialog('module/task/chooseUser.html', $scope, function (user) {
                if (rt.isNull(user)) {
                    $scope.vm.RoleList = [];
                    $scope.vm.UserList = [];
                    return;
                }

                $scope.vm.RoleList = user.roleList;
                $scope.vm.UserList = user.userList;

                TaskService.save($scope.vm)
                    .success(function(data){
                        rt.hideLoadingToast();
                        rt.showSuccessToast("发布成功");
                        $ionicHistory.goBack();
                    })
                    .error(function(data){
                        rt.hideLoadingToast();
                        rt.showErrorToast(data);
                    });
                rt.showLoadingToast("正在发布任务...");
            }).then(function (d) {
                d.show();
            });
        }

        /**
         * 保存按钮点击事件
         * @private
         */
        function _btnSaveClick(){
            if (rt.isNullOrEmptyString($scope.vm.Title)){
                rt.showErrorToast("标题没有填写");
                return;
            }

            if($scope.vm.Data === null || $scope.vm.Data === undefined){
                rt.showErrorToast("计划完成时间没有填写");
                return;
            }

            if (rt.isNullOrEmptyString($scope.vm.Description)){
                rt.showErrorToast("任务描述没有填写");
                return;
            }

            _chooseOwner();
        }

        init();
    }

    xrmApp.controller("TaskEditController", ['$scope', '$state', '$stateParams','rt','$ionicHistory','$ionicActionSheet','TaskService', TaskEditController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function TaskHandlerController($scope, $state, $stateParams,rt,$timeout,$ionicHistory,TaskService) {

        function init() {
            $scope.vm = {};
            $scope.vm.Id = $stateParams.id;
            $scope.getSrc = _getSrc;
            $scope.photoClick = _photoClick;
            $scope.btnOkClick = _btnOkClick;
            $scope.takePhotoClick = _takePhotoClick;
            $scope.vm.Photos = [];
        }

        /**
         * 照片点击事件
         * @param index 照片列表下标
         * @private
         */
        function _photoClick(index) {

        }

        /**
         * 获取图片的src
         * @param index 图片的下标
         * @returns {*}
         * @private
         */
        function _getSrc(index){
            return 'data:image/png;base64,'+$scope.vm.Photos[index].FileBase64Content;
        }

        /**
         * 拍照事件
         * @private
         */
        function _takePhotoClick() {
            location.href = "app:take-photo?px=800&kb=80";
            _checkHasXrmDevieImageData();
        }

        /**
         * 查看是否拍照完成
         * @private
         */
        function _checkHasXrmDevieImageData() {
            if (window.XrmImageData === undefined) {
                $scope.XrmImageData = true;
            }
            if (window.XrmImageData && window.XrmImageData.getXrmImageData && window.XrmImageData.getXrmImageData() !== "") {
                $scope.isTakePhoto = true;

                var imageData = window.XrmImageData.getXrmImageData();
                //保存拍照的图片
                $scope.vm.Photos.push({FileBase64Content:imageData});

                if ($scope.XrmImageData) {
                    window.XrmImageData = undefined;
                }
                window.XrmImageData.clearImageData();
                return;
            }
            $timeout(_checkHasXrmDevieImageData, 1500);
        }

        /**
         * 确定按钮点击事件
         * @private
         */
        function _btnOkClick(){
            rt.showLoadingToast("正在处理...");
            TaskService.handlerTask($scope.vm)
                .success(function (data){
                    rt.hideLoadingToast();
                    rt.showSuccessToast("处理成功");
                    $ionicHistory.goBack();
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast();
        }

        init();
    }

    xrmApp.controller("TaskHandlerController", ['$scope', '$state', '$stateParams','rt','$timeout','$ionicHistory','TaskService', TaskHandlerController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function TaskListController($scope, $state,rt, $stateParams,TaskService) {
        //当前的页数
        var _pageIndex = 1;
        //每页显示条数
        var _pageSize = rt.getPaginationSize();

        function init() {
            $scope.chooseType = _chooseType;
            $scope.addClick = _addClick;
            $scope.viewRefresh = _viewRefresh;
            $scope.loadMore = _loadMore;
            $scope.itemClick = _itemClick;
            //返回按钮点击事件
            $scope.goBackClick = _goBackClick;
            $scope.titleClick = _titleClick;
            //默认选中我处理的
            $scope.type = 0;
            $scope.isLoadMore = false;

            _getTaskList(function(data){
                $scope.vm = data;
            });

            TaskService.getPrivilege()
                .success(function(privilege){
                    $scope.privilege = privilege;
                })
                .error(function(error){
                    rt.showErrorToast(error);
                });
        }

        /**
         * 列表项点击事件
         * @param id 工作日志的id
         * @private
         */
        function _itemClick(task){
            if (!task.HmsCityvisitId.Id) {
                if(task.Type === 4){
                    $state.go('task-main', {id: task.Id});
                }else{
                    $state.go('task-read', {id: task.Id});
                }
            }else{
                $state.go("cityVisit-main", {id: task.HmsCityvisitId.Id});
            }
        }
        /**
         * 返回
         * @private
         */
        function _goBackClick() {
            $state.go("app-close");
        }
        /**
         * 切换任务类型
         * @param type
         * @private
         */
        function _chooseType(type){
            $scope.type = type;
            _viewRefresh();
        }

        /**
         * 添加按钮点击事件
         * @private
         */
        function _addClick() {
            $state.go("task-edit");
        }

        /**
         * 获取日志列表数据
         * @param success 成功回掉函数
         * @private
         */
        function _getTaskList(success){
            if ($scope.type === 0 ){
                TaskService.getTaskList($scope.type,_pageIndex,_pageSize)
                    .success(function(data){
                        rt.hideLoadingToast();
                        if (success){
                            success(data);
                        }

                        if (data.length < _pageSize){
                            $scope.isLoadMore = false;
                        }else{
                            $scope.isLoadMore = true;
                        }
                    })
                    .error(function(data){
                        rt.hideLoadingToast();
                        rt.showErrorToast(data);
                    });
                rt.showLoadingToast("正在加载数据...");
            }else{
                TaskService.getSponsorTask(_pageIndex,_pageSize)
                    .success(function(data){
                        rt.hideLoadingToast();


                        for(var i =0;i<data.length;i++){
                            var count = 0;
                            for(var j = 0;j<data[i].TaskModelList.length;j++){
                                if (data[i].TaskModelList[j].Status.Value == 2){
                                    count++;
                                }
                            }
                            data[i].isShow = false;
                            data[i].num = data[i].OverCount + "/"+data[i].Count;
                        }


                        if (success){
                            success(data);
                        }

                        if (data.length < _pageSize){
                            $scope.isLoadMore = false;
                        }else{
                            $scope.isLoadMore = true;
                        }
                    })
                    .error(function(data){
                        rt.hideLoadingToast();
                        rt.showErrorToast(data);
                    });
                rt.showLoadingToast("正在加载数据...");
            }

        }

        function _titleClick(index){
            $scope.vm[index].isShow = !$scope.vm[index].isShow;

            if (!$scope.vm[index].isLoad){
                TaskService.getSponsorTaskList($scope.vm[index].Id)
                    .success(function(data){
                        rt.hideLoadingToast();
                        $scope.vm[index].TaskModelList = data;
                        $scope.vm[index].isLoad = true;
                    })
                    .error(function(error){
                        rt.hideLoadingToast();
                        rt.showErrorToast(error);
                    });
                rt.showLoadingToast("正在加载数据...");
            }
        }

        /**
         * 下拉刷新事件
         * @private
         */
        function _viewRefresh()
        {
            _pageIndex = 1;
            _getTaskList(function (data) {
                $scope.vm = data;
                $scope.$broadcast('scroll.refreshComplete');
            });
        }

        /**
         * 加载更多数据
         * @private
         */
        function _loadMore()
        {
            _pageIndex++;
            _getTaskList(function (data) {
                $scope.vm.push.apply($scope.vm, data);
                $scope.$broadcast('scroll.infiniteScrollComplete');
            });
        }

        init();
    }

    xrmApp.controller("TaskListController", ['$scope', '$state', 'rt', '$stateParams','TaskService', TaskListController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function TaskReadOnlyController($scope, $state, $stateParams,rt, TaskService) {

        function init() {
            $scope.btnGoRun = _btnGoRun;
            $scope.getSrc = _getSrc;
            $scope.getTaskTypeSrc = _getTaskTypeSrc;
            $scope.userId = rt.getUserId();
            _loadData();
        }

        function _getSrc(index){
            return "data:image/png;base64,"+$scope.vm.Photos[index].FileBase64Content;
        }

        function _getTaskTypeSrc(id,index){
            for(var i = 0;i<$scope.taskTypes.length;i++){
                if($scope.taskTypes[i].Id == id){
                    return "data:image/png;base64,"+$scope.taskTypes[i].Photos[index].FileBase64Content;
                }
            }
        }

        /**
         * 加载表单数据
         * @private
         */
        function _loadData()
        {
            TaskService.getTaskModel($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.vm = data;

                    if($scope.vm.Type == 2 && $scope.vm.Status.Value == 2){
                        _loadTaskSystemData();
                    }
                })
                .error(function(data){
                    rt.hideLoadingToast();
                });
            rt.showLoadingToast("正在加载数据...");
        }

        function _loadTaskSystemData(){
            TaskService.getTaskTypes($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.taskTypes = data;
                })
                .error(function(data){
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载数据...");
        }

        /**
         * 去处理按钮的事件
         * @param id 投诉信息的id
         * @private
         */
        function _btnGoRun(){
            if($scope.vm.Type == 1) {
                $state.go('task-handler',{id:$stateParams.id});
            }else if($scope.vm.Type == 2){
                $state.go('task-system',{id:$stateParams.id});
            }
        }

        init();
    }

    xrmApp.controller("TaskReadOnlyController", ['$scope', '$state', '$stateParams','rt','TaskService', TaskReadOnlyController]);
})();



/*global UIMenu:false */
/*global xrmApp:false */
(function () {
    'use strict';

    function TaskSystemHandlerController($scope, $state, $stateParams,rt,$timeout,$ionicHistory,TaskService) {

        var chooseIndex = 0;

        function init() {
            $scope.vm = {};
            $scope.vm.Id = $stateParams.id;
            $scope.getSrc = _getSrc;
            $scope.photoClick = _photoClick;
            $scope.btnOkClick = _btnOkClick;

            _getTaskTypes();

            //$scope.vm.Photos = [];
            //$scope.vm.Photos.push({FileBase64Content:"add"});
        }

        /**
         * 获取系统任务类型信息
         * @private
         */
        function _getTaskTypes(){
            TaskService.getTaskTypes($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $scope.vm = data;
                    if($scope.vm.length === 0){
                        rt.showErrorToast("没有配置拍照类型");
                    }

                    for(var i = 0;i<$scope.vm.length;i++){
                        if($scope.vm[i].Photos.length === 0){
                            $scope.vm[i].Photos.push({FileBase64Content:"add"});
                        }
                    }
                })
                .error(function(data){
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在加载...");
        }

        /**
         * 照片点击事件
         * @param index 照片列表下标
         * @private
         */
        function _photoClick(id,index) {
            for (var i=0;i<$scope.vm.length;i++) {
                if ($scope.vm[i].Id == id) {
                    if (index == $scope.vm[i].Photos.length - 1) {
                        chooseIndex = i;
                        _takePhotoClick();
                    }
                }
            }
        }

        /**
         * 获取图片的src
         * @param index 图片的下标
         * @returns {*}
         * @private
         */
        function _getSrc(id,index){
            for (var i=0;i<$scope.vm.length;i++){
                if ($scope.vm[i].Id == id){
                    if (index == $scope.vm[i].Photos.length - 1){
                        //如果是最后一张就返回拍照图片
                        return "././asset/img/photo1.png";
                    }else{
                        return 'data:image/png;base64,'+$scope.vm[i].Photos[index].FileBase64Content;
                    }
                }
            }
        }

        /**
         * 拍照事件
         * @private
         */
        function _takePhotoClick() {
            location.href = "app:take-photo?px=800&kb=80";
            _checkHasXrmDevieImageData();
        }

        /**
         * 查看是否拍照完成
         * @private
         */
        function _checkHasXrmDevieImageData() {
            if (window.XrmImageData === undefined) {
                $scope.XrmImageData = true;
            }
            if (window.XrmImageData && window.XrmImageData.getXrmImageData && window.XrmImageData.getXrmImageData() !== "") {
                $scope.isTakePhoto = true;

                var imageData = window.XrmImageData.getXrmImageData();
                //保存拍照的图片
                $scope.vm[chooseIndex].Photos[$scope.vm[chooseIndex].Photos.length - 1] = {FileBase64Content:imageData};
                //在最后的位置上添加拍照图标图片
                $scope.vm[chooseIndex].Photos.push({FileBase64Content:""});

                if ($scope.XrmImageData) {
                    window.XrmImageData = undefined;
                }
                window.XrmImageData.clearImageData();
                return;
            }
            $timeout(_checkHasXrmDevieImageData, 1500);
        }

        /**
         * 确定按钮点击事件
         * @private
         */
        function _btnOkClick(){
            if ($scope.vm.length ===0){
                rt.showErrorToast("没有配置拍照类型");
                return;
            }

            for (var i = 0;i<$scope.vm.length;i++){
                if ($scope.vm[i].Photos.length == 1){
                    rt.showErrorToast($scope.vm[i].Name + "没有拍照");
                    return;
                }
                $scope.vm[i].Photos.pop();
            }

            TaskService.handlerTaskSystem($stateParams.id,$scope.vm)
                .success(function (data){
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功");
                    $ionicHistory.goBack();
                })
                .error(function (data) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(data);
                });
            rt.showLoadingToast("正在保存...");
        }

        init();
    }

    xrmApp.controller("TaskSystemHandlerController", ['$scope', '$state', '$stateParams','rt','$timeout','$ionicHistory','TaskService', TaskSystemHandlerController]);
})();



(function () {
    'use strict';

    xrmApp.controller("TaskVisitFormController", ['$scope', '$state', '$stateParams', '$ionicHistory', 'rt', 'TaskService', TaskVisitFormController]);

    function TaskVisitFormController($scope, $state, $stateParams, $ionicHistory, rt, TaskService) {
        function init() {
            $scope.backClick = _backClick;
            $scope.addProClick = _addProClick;
            $scope.endVisitClick = _endVisitClick;
            $scope.onItemClick = _onItemClick;
            $scope.isEnd = false;
            _loadData();

        }

        function _loadData() {
            rt.showLoadingToast('正在加载数据...');
            TaskService.getCityVisit($stateParams.id)
                .success(function (data) {
                    rt.hideLoadingToast();
                    if (data.DataList[0].data === "0001-01-01 00:00:00" || !data.DataList[0].data) {
                        data.DataList[0].data = "";
                    }
                    $scope.visit = data;
                    $scope.isEnd = data.DataList[0].status === "2" ? true : false;
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        function _backClick() {
            $ionicHistory.goBack();

            //如果没有添加问题清单，则返回时自己删除此巡店单
            if (!rt.isNullOrEmptyString($stateParams.id)
                & $scope.visit !== null
                & $scope.visit.IssuesList !== null
                & $scope.visit.IssuesList.length === 0) {

                TaskService.cancelCityVisit($stateParams.id);
            }
        }

        /**
         * 添加问题按钮点击事件
         * @private
         */
        function _addProClick() {
            $state.go("task-form", {id: "", cityVisitId: $stateParams.id, isEnd: $scope.isEnd});
        }

        /**
         * 结束巡店
         * @private
         */
        function _endVisitClick() {
            //$state.go("cityVisit-end", {id: $stateParams.id});
            rt.showLoadingToast("正在保存...");
            TaskService.endCityVisit($stateParams.id)
                .success(function(data){
                    rt.hideLoadingToast();
                    $ionicHistory.goBack();
                    rt.showSuccessToast("结束巡店成功！");
                })
                .error(function(error){
                    alert(error);
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         *列表项按钮点击事件
         * @param issue
         * @private
         */
        function _onItemClick(issue) {
            //if ($scope.isEnd) {
            //    $state.go("task-deal", {id: issue.Id, cityVisitId: $stateParams.id, isEnd: $scope.isEnd});
            //} else {
            //    $state.go("task-form", {id: issue.Id, cityVisitId: $stateParams.id, isEnd: $scope.isEnd});
            //}
            $state.go("task-form", {id: issue.Id, cityVisitId: $stateParams.id, isEnd: $scope.isEnd});
        }

        init();
    }
})();

/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("TaskVisitProController", ['$scope', '$rootScope', '$state', '$stateParams', '$ionicHistory', 'rt', 'TaskService', TaskVisitProController]);
    function TaskVisitProController($scope, $rootScope, $state, $stateParams, $ionicHistory, rt, TaskService) {
        //构建所有问题类型数组
        $scope.typeArray = [];
        $scope.array = [];

        function _init() {
            $scope.previewIndex = 0;

            $scope.choosePicture = _choosePicture;
            $scope.deletePicture = _deletePicture;
            $scope.previewPicture = _previewPicture;
            $scope.chooseClick = _chooseClick;
            $scope.completeClick = _completeClick;
            $scope.addNewClick = _addNewClick;

            $scope.isEnd = $stateParams.isEnd === "true" ? true : false;

            _loadData($stateParams.id);

            $scope.$on("rt-reload", function () {
                _loadData("");
            });
        }

        function _loadData(id) {
            rt.showLoadingToast("正在加载数据...");
            //id为hms_cityvisit_issuesId，cityVisitId为taskroleId
            TaskService.getIssuesById(id, $stateParams.cityVisitId)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.Issues = data;
                    for (var i = 0; i < data.TypeList.length; i++) {
                        var issueType = {};
                        issueType.Name = data.TypeList[i].Name;
                        issueType.Id = data.TypeList[i].Id;
                        if (data.Type.Id === data.TypeList[i].Id) {
                            issueType.Checked = true;
                        } else {
                            issueType.Checked = false;
                        }
                        $scope.array[i] = issueType;
                    }
                    _dealProType($scope.array);
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });

        }

        /**
         * 将问题类型集合拆分成一行三个的二维数组
         * @param types
         * @private
         */
        function _dealProType(types) {
            var lines = types.length % 3 === 0 ? types.length / 3 : parseInt(types.length / 3) + 1;
            for (var i = 0; i < lines; i++) {
                $scope.typeArray[i] = [];
                for (var j = 0; j < 3; j++) {
                    $scope.typeArray[i][j] = types[3 * i + j] === undefined ? {} : types[3 * i + j];
                }
            }
        }

        /**
         * 选中当前类型的按钮
         * @param type
         * @param child
         * @param parent
         * @private
         */
        function _chooseClick(type, child, parent) {
            //设置当前按钮选中状态
            for (var i = 0; i < $scope.array.length; i++) {
                if (i === child + parent * 3) {
                    $scope.array[i].Checked = true;
                } else {
                    $scope.array[i].Checked = false;
                }
            }

            $scope.Issues.Type.Id = type.Id;
            $scope.Issues.Type.Text = type.Name;
        }

        /**
         * 完成点击事件
         * @private
         */
        function _completeClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Content) && $scope.Issues.Photos.length === 0) {
                rt.showErrorToast("问题照片或问题描述必须提供一项！");
                return;
            }

            if (rt.isNullOrEmptyString($scope.Issues.Type.Id)) {
                rt.showErrorToast("问题类型没有选择！");
                return;
            }

            if (rt.isNullOrEmptyString($scope.Issues.Id)) {
                $scope.Issues.Id = rt.newGuid();
            }

            rt.showLoadingToast("正在保存...");

            TaskService.saveCityVisitIssue($scope.Issues)
                .success(function (data) {
                    rt.hideLoadingToast();
                    rt.showSuccessToast("保存成功！");
                    $ionicHistory.goBack();
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 完成并新建
         * @private
         */
        function _addNewClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Content) && $scope.Issues.Photos.length === 0) {
                rt.showErrorToast("问题照片或问题描述必须提供一项！");
                return;
            }
            if (rt.isNullOrEmptyString($scope.Issues.Type.Id)) {
                rt.showErrorToast("问题类型没有选择！");
                return;
            }

            if (rt.isNullOrEmptyString($scope.Issues.Id)) {
                $scope.Issues.Id = rt.newGuid();
            }
            rt.showLoadingToast("正在保存...");
            TaskService.saveCityVisitIssue($scope.Issues)
                .success(function (data) {
                    rt.hideLoadingToast();

                    $scope.$emit("rt-reload");
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 选择照片
         * @private
         */
        function _choosePicture() {

            rt.takePhoto(800, 80, function (base64Image) {
                try {
                    $scope.Issues.Photos.push({
                        "Id": rt.newGuid(),
                        "FileBase64Content": base64Image
                    });
                }
                catch (e) {
                    alert(e.message);
                }
            });
        }

        /**
         * 预览照片
         * @param index
         * @private
         */
        function _previewPicture(photo) {
            $scope.previewPhoto = photo;
            rt.createDialog("module/dialog/previewImageView.html", $scope, null)
                .then(function (d) {
                    d.show();
                });
        }

        /**
         * 删除照片
         * @param fileid
         * @private
         */
        function _deletePicture(fileId) {
            //检查是否已经添加过了
            var index = _.findIndex($scope.Issues.Photos, {Id: fileId});
            if (index >= 0) {
                $scope.Issues.Photos.splice(index, 1);
            }
        }

        _init();
    }


})();
/*global xrmApp:false*/
(function () {
    'use strict';
    xrmApp.controller("TaskVisitProDealController", ['$scope', '$rootScope', '$state', '$stateParams', '$ionicHistory', 'rt', 'TaskService', TaskVisitProDealController]);
    function TaskVisitProDealController($scope, $rootScope, $state, $stateParams, $ionicHistory, rt, TaskService) {

        function _init() {
            $scope.previewIndex = 0;

            $scope.editable = false;

            $scope.previewPicture = _previewPicture;
            $scope.choosePicture = _choosePicture;
            $scope.deletePicture = _deletePicture;

            $scope.completeClick = _completeClick;

            _loadData();
        }

        function _loadData() {
            rt.showLoadingToast("正在加载数据...");
            TaskService.getIssuesById($stateParams.id, $stateParams.cityVisitId)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $scope.Issues = data;

                    if (data.Owner.Id.toLowerCase() === rt.getUserId().toLowerCase() && data.Status.Value === 1) {
                        $scope.editable = true;
                    }
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });

        }

        /**
         * 完成点击事件
         * @private
         */
        function _completeClick() {
            if (rt.isNullOrEmptyString($scope.Issues.Method)) {
                rt.showErrorToast("问题解决方案没有填写！");
                return;
            }
            rt.showLoadingToast("正在保存...");
            CityVisitService.saveCityVisitIssue($scope.Issues)
                .success(function (data) {
                    rt.hideLoadingToast();
                    $ionicHistory.goBack();
                    rt.showSuccessToast("保存成功！");
                })
                .error(function (error) {
                    rt.hideLoadingToast();
                    rt.showErrorToast(error);
                });
        }

        /**
         * 预览照片
         * @param index
         * @private
         */
        function _previewPicture(photo) {
            $scope.previewPhoto = photo;
            rt.createDialog("module/dialog/previewImageView.html", $scope, null)
                .then(function (d) {
                    d.show();
                });
        }

        /**
         * 选择照片
         * @private
         */
        function _choosePicture() {
            rt.takePhoto(800, 80, function (base64Image) {
                try {
                    $scope.Issues.HandlePhotos.push({
                        "Id": rt.newGuid(),
                        "FileBase64Content": base64Image
                    });
                }
                catch (e) {
                    alert(e.message);
                }
            });
        }

        /**
         * 删除照片
         * @param fileid
         * @private
         */
        function _deletePicture(fileId) {
            //检查是否已经添加过了
            var index = _.findIndex($scope.Issues.HandlePhotos, {Id: fileId});
            if (index >= 0) {
                $scope.Issues.HandlePhotos.splice(index, 1);
            }
        }

        _init();
    }


})();
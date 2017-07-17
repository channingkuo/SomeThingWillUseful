/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rtData', ['$http', 'rtRestClient', 'rtUtils',function ($http, rtRestClient,rtUtils) {

            /**
             * 根据配置条件查询实体的数据
             * @param getDataName 配置文件中配置的查询节点的名字
             * @param paramsList 传入后端的查询条件
             * @param orderby 排序字段
             * @param pageIndex 当前页码
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             * @returns DataListModel的对象
             */
            this.queryPagingData = function(getDataName,paramsList,orderby,pageIndex,success,error){
                rtRestClient.post("api/DataService/Query",{
                    GetDataName:getDataName,
                    Paramslist:paramsList,
                    Orderby:orderby,
                    PageSize:rtUtils.getPaginationSize(),
                    PageIndex:pageIndex
                }).success(function(dataString){
                    var data = JSON.parse(dataString);
                    if(success){
                        success(data);
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };

            /**
             * 根据配置条件查询实体的数据(过滤权限)
             * @param getDataName 配置文件中配置的查询节点的名字
             * @param paramsList 传入后端的查询条件
             * @param orderby 排序字段
             * @param pageIndex 当前页码
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             * @returns DataListModel的对象
             */
            this.filteredQueryPagingData = function(getDataName,paramsList,orderby,pageIndex,success,error){
                rtRestClient.post("api/DataService/FilteredQuery",{
                    GetDataName:getDataName,
                    Paramslist:paramsList,
                    Orderby:orderby,
                    PageSize:rtUtils.getPaginationSize(),
                    PageIndex:pageIndex
                }).success(function(dataString){
                    var data = JSON.parse(dataString);
                    if(success){
                        success(data);
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };

            /**
             * 根据配置条件查询实体的数据
             * @param getDataName 配置文件中配置的查询节点的名字
             * @param paramsList 传入后端的查询条件
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             * @returns DataListModel的对象
             */
            this.queryData = function(getDataName,paramsList,success,error){
                rtRestClient.post("api/DataService/Query",{
                    GetDataName:getDataName,
                    Paramslist:paramsList
                }).success(function(dataString){
                    var data = JSON.parse(dataString);
                    if(success){
                        success(data);
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };

            /**
             * 根据配置条件查询实体的数据(过滤权限)
             * @param getDataName 配置文件中配置的查询节点的名字
             * @param paramsList 传入后端的查询条件
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             * @returns DataListModel的对象
             */
            this.filteredQueryData = function(getDataName,paramsList,success,error){
                rtRestClient.post("api/DataService/FilteredQuery",{
                    GetDataName:getDataName,
                    Paramslist:paramsList
                }).success(function(dataString){
                    var data = JSON.parse(dataString);
                    if(success){
                        success(data);
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };

            /**
             * 根据实体的Id获取实体的数据
             * @param entityName 实体的名字
             * @param id 主键字段
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             */
            this.getData = function(entityName,id,success,error){
                rtRestClient.get("api/DataService/Get?entityName="+entityName + "&id=" + id)
                    .success(function(dataString){
                    var data = JSON.parse(dataString);
                    if(success){
                        success(data);
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };

            /**
             * 根据实体的Id获取实体的数据(过滤权限)
             * @param entityName 实体的名字
             * @param id 主键字段
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             */
            this.filteredGetData = function(entityName,id,success,error){
                rtRestClient.get("api/DataService/FilteredGet?entityName="+entityName + "&id=" + id)
                    .success(function(dataString){
                        var data = JSON.parse(dataString);
                        if(success){
                            success(data);
                        }
                    }).error(function(msg){
                        if(error){
                            error(msg);
                        }
                        else{
                            rtUtils.showErrorToast(msg);
                        }
                    });
            };

            /**
             * 插入或者更新实体数据
             * @param entityName 实体的名字
             * @param id 主键字段
             * @param obj 要插入数据的对象
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             */
            this.saveData = function(entityName,id,obj,success,error){
                rtRestClient.post("api/DataService/Save",{
                    EntityName:entityName,
                    Fields:obj,
                    Id:id
                }).success(function(){
                    if(success){
                        success();
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };

            /**
             * 删除实体数据
             * @param entityName 实体的名字
             * @param id 主键字段
             * @param success 成功后的回调函数
             * @param error 失败后的回调函数
             */
            this.deleteData = function(entityName,id,success,error){
                rtRestClient.post("api/DataService/Delete",{
                    EntityName:entityName,
                    Id:id
                }).success(function(){
                    if(success){
                        success();
                    }
                }).error(function(msg){
                    if(error){
                        error(msg);
                    }
                    else{
                        rtUtils.showErrorToast(msg);
                    }
                });
            };


        }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rtFlow', ['$http', 'rtRestClient', function ($http, rtRestClient) {

            /**
             * 获取表单签核按钮
             * @param {string} etn 实体名称
             * @param {string} id 单据名称
             * @returns {HttpPromise}
             */
            this.getFormFlowStatus = function(etn,id){
                var url = "api/flow/GetFormFlowStatus?etn=" + etn + "&id=" + id;
                return rtRestClient.get(url);
            };

            /**
             * 获取实体签核列表
             * @param {string} etn 实体名称
             * @returns {HttpPromise}
             */
            this.getWorkFlow = function(etn){
                var url = "api/flowextension/workflow?entity=" + etn;
                return rtRestClient.get(url);
            };

            /**
             * 获取实体签核步骤列表
             * @param {string} flowid 签核流程标识
             * @param {string} id 记录标识
             * @param {string} etn 实体名称
             * @returns {HttpPromise}
             */
            this.getFlowUser = function(flowid,id,etn){
                var url = "api/flowextension/flowuser?flowid=" + flowid + "&etn=" + etn + "&id=" + id;
                return rtRestClient.get(url);
            };

            /**
             * 提交签核
             * @param {*} 签核用户列表
             * @param {string} id 记录标识
             * @param {string} etn 实体名称
             * @param {string} flowsetpid 签核流程标识
             * @returns {HttpPromise}
             */
            this.submitFlow = function(approveUserList, etn, id, flowsetpid){
                var url = "api/flowextension/flowsubmit?approveUserList=" + angular.toJson(approveUserList) +
                    "&etn=" + etn + "&id=" + id + "&flowsetpid=" + flowsetpid;
                return rtRestClient.get(url);
            };

            /**
             * 撤回签核
             * @param {string} id 记录标识
             * @param {string} etn 实体名称
             * @returns {HttpPromise}
             */
            this.revokeFlow = function(etn,id){
                var url = "api/flowextension/revoke?etn=" + etn + "&id=" + id;
                return rtRestClient.get(url);
            };

            /*
            * 执行签核动作
            * @param {string} flowid 流程标识
            * @param {string} memo 意见
            * @param {string} actionType 执行动作
            * @returns {HttpPromise}
            */
            this.doFlowAction = function(flowId,memo,actionType){
                var url = "api/flow/flowaction?flowId=" + flowId + "&approvalMemo=" + memo +
                    "&actionType=" + actionType;
                return rtRestClient.get(url);
            };

            /*
             * 获取签核列表
             * @param {string} flowid 流程标识
             * @returns {HttpPromise}
             */
            this.getApproveList = function(formId){
                var url = "api/flow/approvelist?formId=" + formId;
                return rtRestClient.get(url);
            };
        }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rtLpms', ['$http', 'rtRestClient', 'rtUtils', function ($http, rtRestClient, rtUtils) {

            /**
             * 获取酒店ID
             * @returns {*}
             */
            this.getHotelId = function () {
                var hotelId;
                if (window.XrmDeviceData && window.XrmDeviceData.getHotelId) {
                    hotelId = window.XrmDeviceData.getHotelId().toUpperCase();
                }
                else if (!this.isNullOrEmptyString(localStorage.HotelId)) {
                    hotelId = localStorage.HotelId.toUpperCase();
                }
                else {
                    this.showErrorToast("没有获取到当前酒店的编号! ");
                }
                return hotelId;
            };

            /**
             * 通过post的方法调用WebAPI
             * @param {string} pmsApiUrl WebAPI的URL地址，如 "api/sys/modules?showIcon=&lastUpdateTime=";
             * @param {*} data 要post到服务器端的数据
             * @returns {HttpPromise}
             */
            this.postPms = function (pmsApiUrl, data, ignoreError) {
                var url = rtRestClient.getBaseApiUrl() + "api/pms/data?url=" + escape(pmsApiUrl);

                var token = rtRestClient.getXrmAuthToken();
                if (!rtUtils.isNullOrEmptyString(token)) {
                    $http.defaults.headers.common.Authorization = 'Basic ' + rtRestClient.getXrmAuthToken();
                }

                var resp = $http.post(url, data);
                return ignoreError === true ? resp : resp.error(function (msg) {
                    rtUtils.showErrorToast(msg);
                });
            };

            /**
             * 通过Get方法调用WebAPI
             * @param {string} pmsApiUrl WebAPI的URL地址
             * @returns {HttpPromise}
             */
            this.getPms = function (pmsApiUrl, ignoreError) {
                var url = rtRestClient.getBaseApiUrl() + "api/pms/data?url=" + escape(pmsApiUrl);

                var token = rtRestClient.getXrmAuthToken();
                if (!rtUtils.isNullOrEmptyString(token)) {
                    $http.defaults.headers.common.Authorization = 'Basic ' + rtRestClient.getXrmAuthToken();
                }

                var resp = $http.get(url);
                return ignoreError === true ? resp : resp.error(function (msg) {
                    rtUtils.showErrorToast(msg);
                });
            };

        }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rtPrivilege', ['$http', 'rtRestClient', function ($http, rtRestClient) {

            /**
             * 获取权限
             * @param {string} etn 实体名称
             * @param {string} id 单据名称
             * @param  type     权限类型
             * @returns {HttpPromise}
             */
            this.havePrivilege= function (etn, id, type) {
                var url = "api/privilege/GetPrivilegeByType?etn=" + etn + "&type=" + type;
                if(id !== null ){
                    url += "&id=" + id;
                }
                return rtRestClient.get(url);
            };
        }]);
})();
/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rtRestClient', ['$http', 'rtUtils', function ($http, rtUtils) {
            this.getBaseApiUrl = function(){
                if (window.XrmDeviceData && window.XrmDeviceData.getXrmBaseUrl) {
                    return window.XrmDeviceData.getXrmBaseUrl();
                }
                else if (!rtUtils.isNullOrEmptyString(localStorage.XrmBaseUrl)) {
                    return localStorage.XrmBaseUrl;
                }

                return null;
            };

            this.getXrmAuthToken = function () {
                if (window.XrmDeviceData && window.XrmDeviceData.getXrmAuthToken) {
                    return window.XrmDeviceData.getXrmAuthToken();
                }
                else if (!rtUtils.isNullOrEmptyString(localStorage.XrmAuthToken)) {
                    return localStorage.XrmAuthToken;
                }

                return null;
            };

            /**
             * 通过post的方法调用WebAPI
             * @param {string} apiUrl WebAPI的URL地址，如 "api/sys/modules?showIcon=&lastUpdateTime=";
             * @param {*} data 要post到服务器端的数据
             * @param {string} ignoreError 忽略错误处理
             * @returns {HttpPromise}
             */
            this.post = function (apiUrl, data, ignoreError) {
                var url = this.getBaseApiUrl() + apiUrl;

                var token = this.getXrmAuthToken();
                if (!rtUtils.isNullOrEmptyString(token)) {
                    $http.defaults.headers.common.Authorization = 'Basic ' + this.getXrmAuthToken();
                }

                var resp = $http.post(url, data);
                return ignoreError === true ? resp : resp.error(function (msg) {
                    rtUtils.showErrorToast(msg);
                });
            };

            /**
             * 通过Get方法调用WebAPI
             * @param {string} apiUrl WebAPI的URL地址
             * @param {string} ignoreError 忽略错误处理
             * @returns {HttpPromise}
             */
            this.get = function (apiUrl, ignoreError) {
                var url = this.getBaseApiUrl() + apiUrl;

                var token = this.getXrmAuthToken();
                if (!rtUtils.isNullOrEmptyString(token)) {
                    $http.defaults.headers.common.Authorization = 'Basic ' + this.getXrmAuthToken();
                }

                var resp = $http.get(url);
                return ignoreError === true ? resp : resp.error(function (msg) {
                    rtUtils.showErrorToast(msg);
                });
            };

            this.delete = function (apiUrl) {
                var url = this.getBaseApiUrl() + apiUrl;

                var token = this.getXrmAuthToken();
                if (!rtUtils.isNullOrEmptyString(token)) {
                    $http.defaults.headers.common.Authorization = 'Basic ' + this.getXrmAuthToken();
                }

                return $http.delete(url);
            };

            /**
             * 通过post的方法调用WebAPI
             * @param {string} apiUrl WebAPI的URL地址，如 "api/sys/modules?showIcon=&lastUpdateTime=";
             * @param {*} data 要post到服务器端的数据
             * @returns {HttpPromise}
             */
            this.put = function (apiUrl, data) {
                var url = this.getBaseApiUrl() + apiUrl;

                var token = this.getXrmAuthToken();
                if (!rtUtils.isNullOrEmptyString(token)) {
                    $http.defaults.headers.common.Authorization = 'Basic ' + this.getXrmAuthToken();
                }

                return $http.put(url, data);
            };

            /*
             * 删除实体记录
             * @param {string} etn 实体名称
             * @param {string} id 实体记录标识
             * @returns {HttpPromise}
             * */
            this.deleteFormData = function (etn, id) {
                var apiUrl = "api/form/deletedata?etn=" + etn + "&id=" + id;
                return this.post(apiUrl);
            };

            /*
             * 通过WebAPI获取表单记录
             * @param {string} etn 实体名称
             * @param {string} id 实体记录标识
             * @returns {HttpPromise}
             * */
            this.getFormData = function (etn, id) {
                var entity = "{" +
                    "\"Id\":\"" + id + "\"," +
                    "\"LogicalName\":\"" + etn + "\"," +
                    "\"LinkEntityList\":[{\"LinkFieldName\":\"" + "\",\"LinkedEntity\":{\"Id\":\"" + "\"}}]" +
                    "}";

                var apiUrl = "api/formeditex/get?entity=" + entity;
                return this.get(apiUrl);
            };

            /*
             * 通过WebAPI提交表单记录
             * */
            this.postFormData = function (etn, id, fields, linkField, linkFieldId, linkEntity) {
                var entity = "{" +
                    "\"Id\":\"" + id + "\"," +
                    "\"LogicalName\":\"" + etn + "\"," +
                    "\"LinkEntityList\":[{\"LinkFieldName\":\"" + "\",\"LinkedEntity\":{\"Id\":\"" + "\"}}]" +
                    "}";

                for (var i = 0; i < fields.length; i++) {
                    var field = fields[i];

                    if (field.IsReq === true && rtUtils.isNull(field.FieldValue) === true) {
                        var message = field.FieldName + "不能为空。";
                        rtUtils.showErrorToast(message);
                        return;
                    }

                    if (field.FieldType === "lookup" && rtUtils.isNull(field.FieldValue) === false) {
                        field.FieldValue = JSON.stringify(field.FieldValue[0]);
                    }
                }

                var data = {
                    Entity: JSON.parse(entity),
                    FieldList: fields
                };

                return this.post("api/formedit/post", data);
            };

            /**
             * 通过WebAPI获取只读表单
             * @param {string} etn 实体名称
             * @param {string} id 实体记录标识
             * @returns {HttpPromise}
             */
            this.getReadOnlyForm = function (etn, id) {
                var apiUrl = "api/form/data?etn=" + etn + "&id=" + id;
                return this.get(apiUrl);
            };

            this.getViewData = function (etn, fieldName, id, viewId) {
                var entity = "{" +
                    "\"LogicalName\":\"" + etn + "\"," +
                    "\"LinkEntityList\":[{\"LinkFieldName\":\"" + fieldName + "\",\"LinkedEntity\":{\"Id\":\"" + id + "\"}}]" +
                    "}";

                var view = "{" +
                    "\"ViewId\":\"" + _vm.selectedViewId + "\"" +
                    "}";

                var apiUrl = "api/ViewQuery/quicksearchdata?entity=" + entity +
                    "&view=" + view +
                    "&queryValue=" + _vm.queryValue +
                    "&pageIndex=" + _vm.pageIndex +
                    "&pageSize=" + _vm.pageSize;
                return rt.get(apiUrl);
            };

            /**
             * 通过WebAPI获取picklist的可选的options （包含额外的 “请选择” 的选项）
             * @param {string} etn 实体名字
             * @param {string} fieldName 字段名
             * @returns {HttpPromise}
             */
            this.getPicklistOptions = function (etn, fieldName) {
                var url = "api/Picklist/Get?etn=" + etn + "&fieldName=" + fieldName;
                return this.get(url);
            };

            /**
             * 通过WebAPI获取picklist的可选的options （不包含 “请选择”）
             * @param {string} etn 实体名字
             * @param {string} fieldName 字段名
             * @returns {HttpPromise}
             */
            this.getPicklistOptions2 = function (etn, fieldName) {
                var url = "api/Picklist/Get2?etn=" + etn + "&fieldName=" + fieldName;
                return this.get(url);
            };

            /**
             * 根据配置文件中的SQL语句查询相关数据，已JSON的形式返回
             * @param {string} configname 配置节点的名字
             * @param {string} paramslist 配置文件中的SQL语句参数
             * @returns {Object}
             */
            this.getData = function (configname, paramslist) {
                var url = "common/GetData.aspx?configname=" + configname + "&paramslist=" + paramslist;
                return this.get(url);
            };

            /**
             * 获取指定实体的字段的值
             * @param {string} id 实体数据库记录的ID
             * @param {string} typename 实体的名称
             * @param {string} returnField 多个字段用逗号隔开；如果要返回所有的值，则为空或者是 *
             * @returns {Object} 返回returnField中指定的字段的值
             */
            this.getFieldValue = function (id, typename, returnField) {
                if (rtUtils.isNull(returnField)) {
                    returnField = '*';
                }

                var url = "common/GetFieldValue.aspx?id=" + id + "&typename=" + typename + "&fieldlist=" + returnField;
                return this.get(url);
            };

            /**
             * 根据实体的某个字段查询实体的数据行
             * @param {string} id 字段的值
             * @param {string} typename 实体的名字
             * @param {string} refname 字段的名字
             * @param {string} returnField 返回的字段的列表，逗号分隔
             * @returns {Object}
             */
            this.getRefFieldValue = function (id, typename, refname, returnField) {
                if (rtUtils.isNull(returnField)) {
                    returnField = '*';
                }

                var url = "common/GetRefFieldValue.aspx?id=" + id + "&typename=" + typename + "&refname=" + refname + "&fieldlist=" + returnField;
                return this.get(url);
            };
        }]);
})();

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rtUtils', ['$http', '$ionicLoading', '$filter', '$ionicModal', '$ionicPopup', '$ionicViewService', '$timeout', '$ionicActionSheet', function ($http, $ionicLoading, $filter, $ionicModal, $ionicPopup, $ionicViewService, $timeout, $ionicActionSheet) {
            /**
             * 扩展日期对象，增加addDays的方法
             * @param days
             * @returns {Date}
             */
            Date.prototype.addDays = function (days) {
                var dat = new Date(this.valueOf());
                dat.setDate(dat.getDate() + days);
                return dat;
            };

            this.dateDiff = function (datepart, date1, date2) {
                var seconds = date2.getTime() - date1.getTime();

                if (datepart === "dd") {
                    return parseInt(seconds / (1000 * 60 * 60 * 24));
                }
            };

            /**
             * 判断字符是否有效的手机号码
             * @returns {boolean}
             */
            this.isCellphone = function (str) {
                var reg = /^0?1[3|4|5|8][0-9]\d{8}$/;
                return reg.test(str);
            };

            /**
             * 判断字符是否有效的身份证括号
             * @returns {boolean}
             */
            this.isIDCard = function (str) {
                var reg = /^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/;
                return reg.test(str);
            };

            /**
             * 判断一个变量是否是undefined或者null
             * @param o 需要进行判断的变量
             * @returns {boolean} 如果是undified或者null，则返回true，否则返回 false
             */
            this.isNull = function (o) {
                return o === undefined || o === null;
            };

            /**
             * 判断变量f是否是一个函数
             * @param f 变量f
             * @returns {boolean} 如果是函数则返回true，否则返回false
             */
            this.isFunction = function (f) {
                return f !== undefined && f !== null && typeof f === 'function';
            };

            /**
             * 判断一个字符串是否是undified、null、“”
             * @param s 字符串变量
             * @returns {boolean} 如果是，则返回true，否则返回false
             */
            this.isNullOrEmptyString = function (s) {
                if (this.isNull(s)) {
                    return true;
                }

                if (typeof(s) === "string" && this.trim(s) === "") {
                    return true;
                }

                return false;
            };

            /**
             * 不区分大小、{}，比较两个guid
             * @param guid1 参与比较的第一个Guid
             * @param guid2 参与比较的第二个Guid
             * @returns {boolean}
             */
            this.isSameGuid = function (guid1, guid2) {
                var isEqual = false;
                if (this.isNull(guid1) || this.isNull(guid2)) {
                    isEqual = false;
                } else {
                    isEqual = guid1.replace(/[{}]/g, "").toLowerCase() === guid2.replace(/[{}]/g, "").toLowerCase();
                }

                return isEqual;
            };

            /**
             * 移除字符串开通和结尾的空格等特殊字符
             * @param s 待trim的字符串
             * @param charsToTrim 要去掉的特殊字符串，如果为空，则代码移除空格
             * @returns {string} 返回移除后的字符串
             */
            this.trim = function (s, charsToTrim) {
                if (this.isNull(s) || typeof s !== "string" || s.length <= 0) {
                    return "";
                }

                var result = s;
                var ctrim = (this.isNull(s) || typeof charsToTrim !== "string" || charsToTrim.length <= 0) ? " " : charsToTrim;

                var index = 0, count = s.length;
                while (count > 0) { //trim the head position
                    if (ctrim.indexOf(result[0]) >= 0) {
                        result = result.substring(1);
                        count--;
                    } else {
                        break;
                    }
                }
                while (count > 0) { //trim the tail position
                    if (ctrim.indexOf(result[count - 1]) >= 0) {
                        result = result.substring(0, count - 1);
                        count--;
                    } else {
                        break;
                    }
                }
                return result;
            };

            /**
             * 判断是否是Date类型
             * @param d
             * @returns {boolean}
             */
            this.isDate = function (d) {
                if (this.isNull(d)) {
                    return false;
                }

                return d instanceof Date && !isNaN(d.valueOf());
            };

            /**
             * 弹出操作执行成功的Toast
             * @param {string} msg 提示信息
             */
            this.showSuccessToast = function (msg) {
                if (this.isNullOrEmptyString(msg)) {
                    return;
                }
                $ionicLoading.show({
                    template: '<div><i style="font-size:2em;" class="icon ion-checkmark-circled"></i></div><div>' + msg + '</div>',
                    duration: 3000
                });
            };

            /**
             * 弹出执行错误的Toast
             * @param {string} msg 错误消息
             */
            this.showErrorToast = function (msg) {
                if (this.isNullOrEmptyString(msg)) {
                    return;
                }
                $ionicLoading.show({
                    template: '<div><i style="font-size:2em;" class="icon ion-close-circled"></i></div><div>' + msg + '</div>',
                    duration: 3000
                });
            };

            /**
             * 弹出正在执行的Toast
             * @param {string} msg 提示信息
             */
            this.showLoadingToast = function (msg) {
                if (this.isNullOrEmptyString(msg)) {
                    return;
                }
                $ionicLoading.show({
                    template: '<div><i style="font-size:2em;" class="icon ion-loading-c"></i></div><div>' + msg + '</div>',
                    duration: 0,
                    noBackdrop: true
                });
            };

            /**
             * 隐藏正在执行的消息提示框
             */
            this.hideLoadingToast = function () {
                $ionicLoading.hide();
            };

            /**
             * 格式化日期类型
             * @param {Date} d  日期
             * @param {string} format 格式化字符串，如果不传值，在默认为 ‘yyyy-MM-dd’
             * @returns {string} 返回格式化后的日期字符串
             */
            this.formatDateTime = function (d, format) {
                if (!this.isDate(d)) {
                    return "";
                }
                if (this.isNullOrEmptyString(format)) {
                    format = 'yyyy-MM-dd';
                }
                return $filter('date')(d, format);
            };

            /**
             * 创建弹出的对话框页面，如lookup的选择也
             * @param {string} url 对话框模板html文件的地址
             * @param {$scope} $s angularjs的$scope对象
             * @param {function} cb 对话框的执行完成的回掉函数，如点击OK按钮或者选择一行数据
             * @returns {promise} 返回promise对象，需要在then方法中调用show方法显示对话框，如then(function(d){ d.show();});
             */
            this.createDialog = function (url, $s, cb) {
                return $ionicModal.fromTemplateUrl(url, function ($ionicModal) {
                    $s.dialog = $ionicModal;
                    $s.closeDialog = function () {
                        $s.dialog.hide();
                    };
                    if (cb !== undefined) {
                        $s.onDataSelected = cb;
                    }
                }, {
                    scope: $s, animation: 'slide-in-up'
                });
            };

            /**
             * 创建弹出的对话框页面，如lookup的选择也
             * @param {$scope} $s angularjs的$scope对象
             * @param {function} lookupParams 传递给Lookup对话框的参数
             * @returns {promise} 返回promise对象，需要在then方法中调用show方法显示对话框，如then(function(d){ d.show();});
             */
            this.sLookup = function ($s, lookupParams) {
                var url = 'module/lookup/singleLookupView.html';
                return $ionicModal.fromTemplateUrl(url, function ($ionicModal) {
                    $s.dialog = $ionicModal;
                    $s.closeDialog = function () {
                        $s.dialog.hide();
                    };
                    if (lookupParams !== undefined) {
                        $s.lookupParams = lookupParams;
                    }
                }, {
                    scope: $s, animation: 'slide-in-up'
                });
            };

            /**
             * 弹出确认选择的对话框，如：是否要删除，包含确认和取消两个按钮
             * @param {string} msgTitle 消息提示的标题
             * @param {string} msgBody 消息提示的正文
             * @param {Function} cb 点击确认按钮之后的回调函数
             */
            this.showPopupDialog = function (msgTitle, msgBody, cb) {
                var popup = $ionicPopup.show({
                    template: msgBody,
                    title: msgTitle,
                    buttons: [
                        {
                            text: '取消',
                            onTap: function () {
                                popup.close();
                            }
                        },
                        {
                            text: '确认',
                            type: 'button-positive',
                            onTap: function () {
                                popup.close();
                                if (cb !== undefined && cb !== null) {
                                    cb();
                                }
                            }
                        }
                    ]
                });
            };

            /**
             * 弹出确认选择的对话框，如：是否要删除，包含确认和取消两个按钮
             * @param {string} msgTitle 消息提示的标题
             * @param {string} msgBody 消息提示的正文
             */
            this.showConfirmDialog = function (msgTitle, msgBody) {
                return $ionicPopup.confirm({
                    title: msgTitle,
                    subTitle: msgBody,
                    cancelText: '取消',
                    okText: '确认'
                });
            };

            /**
             * 判断当前页面是新打开的页面还是从子页面导航返回的页面
             * @returns {boolean}
             */
            this.isNavigationBack = function () {
                return $ionicViewService.getNavDirection() === 'back';
            };

            this.getBackView = function () {
                return $ionicViewService.getBackView();
            };

            /**
             * 生成随机的Guid
             * @returns {string} Guid
             */
            this.newGuid = function () {
                function s4() {
                    return Math.floor((1 + Math.random()) * 0x10000)
                        .toString(16)
                        .substring(1);
                }

                return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                    s4() + '-' + s4() + s4() + s4();
            };

            this.getPaginationSize = function () {
                return 10;
            };

            this.convertGPS2BaiduLocation = function (l) {
                var apiUrl = "http://api.map.baidu.com/geoconv/v1/?coords=" + l.longitude +
                    "," + l.latitude + "&from=1&to=5&ak=ROninBdEIu93CBGDHc3fSPHE" + "&callback=JSON_CALLBACK";

                return $http.jsonp(apiUrl);
            };

            this.getGeolocation = function (success, error) {
                if (window.XrmDeviceGeo) {
                    var position = window.XrmDeviceGeo.getGeoLoaction();
                    if (position === null) {
                        if (error) {
                            error();
                        }
                    } else {
                        success(JSON.parse(position));
                    }
                } else {
                    var that = this;
                    navigator.geolocation.getCurrentPosition(function (position) {
                        if (position === null || position.coords === null) {
                            that.showErrorToast("定位失败，正在重新定位");
                            return;
                        }

                        //调用百度的API将GPS坐标转换成百度的火星坐标
                        var l = {latitude: position.coords.latitude, longitude: position.coords.longitude};
                        that.convertGPS2BaiduLocation(l)
                            .success(function (d) {
                                if (d === undefined || d === null || d.status !== 0 || d.result === null || d.result.length === 0) {
                                    that.showErrorToast("调用百度火星坐标转换的API错误,错误码：" + d.status);
                                    return;
                                }

                                l.longitude = d.result[0].x;
                                l.latitude = d.result[0].y;
                                // 创建地理编码实例
                                var myGeo = new BMap.Geocoder();
                                // 根据坐标得到地址描述
                                myGeo.getLocation(new BMap.Point(l.longitude, l.latitude), function (result) {
                                    if (result) {
                                        l.address = result.address;
                                    }
                                    success(l);
                                });

                            });
                    }, function (error) {
                        switch (error.code) {
                            case 0:
                                that.showErrorToast("获取位置是发生一个错误,错误信息：" + error.message);
                                break;
                            case 1:
                                that.showErrorToast("无法获取当前位置，请打开定位服务后重新尝试!");
                                break;
                            case 2:
                                that.showErrorToast("无法确定设备的位置，请返回后重新尝试! ");
                                break;
                            case 3:
                                that.showErrorToast("获取位置超时，请返回后重新尝试!");
                                break;
                            default :
                                that.showErrorToast("定位失败，发生未知错误");
                                break;
                        }
                        if (error) {
                            error();
                        }
                    }, {timeout: 10000});
                }
            };

            this.takePhoto = function (px, kb, callback) {
                location.href = "app:take-photo?px=" + px + "&kb=" + kb;

                function checkHasXrmDeviceImageData() {
                    if (window.XrmImageData && window.XrmImageData.getXrmImageData && window.XrmImageData.getXrmImageData() !== "") {

                        var image = window.XrmImageData.getXrmImageData();

                        if (callback !== null) {
                            callback(image);
                        }
                        window.XrmImageData.clearImageData();

                        return;
                    }
                    $timeout(checkHasXrmDeviceImageData, 1500);
                }

                checkHasXrmDeviceImageData();
            };

            this.choosePhoto = function (px, kb, callback) {
                location.href = "app:choose-photo?px=" + px + "&kb=" + kb;

                function checkHasXrmDeviceImageData() {
                    if (window.XrmImageData && window.XrmImageData.getXrmImageData && window.XrmImageData.getXrmImageData() !== "") {

                        var image = window.XrmImageData.getXrmImageData();

                        if (callback !== null) {
                            callback(image);
                        }
                        window.XrmImageData.clearImageData();

                        return;
                    }
                    $timeout(checkHasXrmDeviceImageData, 1500);
                }

                checkHasXrmDeviceImageData();
            };

            /**
             * 选择图片/照片
             * @param config
             *  {
             *      count：1，
             *      px:800,     //图片的像素
             *      kb:100,,    //
             *      sourceType：['album', 'camera']，
             *      success:function(image){
             *          //
             *      }
             *  }
             */
            this.chooseImage = function (config) {
                if (config === null || config === undefined) {
                    return;
                }

                //处理图片的大小
                if ((config.px === null || config.px === undefined) &&
                    (config.kb === null || config.kb === undefined)) {
                    config.px = 800;
                    config.kb = 100;
                }

                var ALBUM = "album";
                var CAMERA = "camera";

                var px = config.px;
                var kb = config.kb;
                //处理图片的来源
                var sourceTypes = [];
                if (config.sourceType === null) {
                    sourceTypes = [ALBUM, CAMERA];
                } else if (!(config.sourceType instanceof Array)) {
                    throw new Error("sourceType必须为数组!");
                } else {
                    for (var i = 0; i <= config.sourceType.length; i++) {
                        if (config.sourceType[i] === ALBUM || config.sourceType[i] === CAMERA) {
                            sourceTypes.push(config.sourceType[i]);
                        }
                    }
                }

                var that = this;

                //获取图片
                if (sourceTypes.length === 1) {
                    if (sourceTypes[0] === ALBUM) {
                        that.choosePhoto(px, kb, config.success);
                    } else if (sourceTypes[0] === CAMERA) {
                        that.takePhoto(px, kb, config.success);
                    }
                } else {
                    var selectedButtons = [
                        {text: "拍照"},
                        {text: "从手机相册选择"}
                    ];
                    $ionicActionSheet.show({
                        buttons: selectedButtons,
                        titleText: '选择视图',
                        cancelText: '取消',
                        cancel: function () {
                        },
                        buttonClicked: function (index) {
                            if (index === 0) {
                                that.takePhoto(px, kb, config.success);
                            } else if (index === 1) {
                                that.choosePhoto(px, kb, config.success);
                            }
                            return true;
                        }
                    });
                }
            };

            /**
             * 扫码
             * @param config
             * {
             *     success:function(){}
             * }
             */
            this.scanQRCode = function (config) {
                location.href = "app:scan";

                function getScanResult() {
                    var result;
                    if (window.XrmScanData && window.XrmScanData.getResult() !== "") {
                        result = window.XrmScanData.getResult();
                    }

                    if (result === null || result.length === 0) {
                        $timeout(getScanResult, 1500);
                    }
                    else {
                        $timeout.cancel();
                        if (config && config.success) {
                            config.success(result);
                        }
                    }
                }

                getScanResult();
            };

            this.getUserId = function () {
                var userId;
                if (window.XrmDeviceData && window.XrmDeviceData.getUserId) {
                    userId = window.XrmDeviceData.getUserId().toUpperCase();
                }
                else if (!this.isNullOrEmptyString(localStorage.UserId)) {
                    userId = localStorage.UserId.toUpperCase();
                }
                else {
                    this.showErrorToast("没有获取到当前用户的id! ");
                }
                return userId;
            };

            this.getUserName = function () {
                var userName;
                if (window.XrmDeviceData && window.XrmDeviceData.getUserName) {
                    userName = window.XrmDeviceData.getUserName();
                }
                else if (!this.isNullOrEmptyString(localStorage.UserName)) {
                    userName = localStorage.UserName;
                }
                else {
                    this.showErrorToast("没有获取到当前用户的账号! ");
                }
                return userName;
            };

            /**
             * 获取页面文档显示区域的宽度
             * @returns {boolean}
             */
            this.getDocumentWidth = function () {
                return document.documentElement.clientWidth || document.body.clientWidth;
            };

            /**
             * 获取页面文档显示区域的高度
             * @returns {boolean}
             */
            this.getDocumentHeight = function () {
                return document.documentElement.clientHeight || document.body.clientHeight;
            };
        }]);
})();

/**
 * Created by Joe on 2014-11-17.
 */

/*global angular:false */
/*global _:false */
/*global xrmApp:false */
(function () {
    'use strict';
    angular.module('xrmApp')
        .service('rt', ['rtUtils', 'rtRestClient', 'rtFlow', 'rtPrivilege', 'rtData', 'rtLpms', function (rtUtils, rtRestClient, rtFlow, rtPrivilege, rtData, rtLpms) {
            _.extend(this, rtUtils, rtRestClient, rtFlow, rtPrivilege, rtData, rtLpms);
        }]);
})();


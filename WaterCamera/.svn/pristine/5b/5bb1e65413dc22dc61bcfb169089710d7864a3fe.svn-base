/*global angular:false */
/*global xrmApp:false */
/*global _:false */
/*global Notice:false*/
/*global Attachment:false*/
/*global NoticeForm:false*/
xrmApp.factory('AttendanceService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取本周考勤记录
     * @param pageIndex 当前页
     * @param pageSize 每页显示的个数
     */
    function _getAttendancesWeekList(pageIndex, pageSize) {
        var apiUrl = "api/HmsAttendance/GetAttendanceWeekList?pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }
    /**
     * 获取本月考勤记录
     * @param pageIndex 当前页
     * @param pageSize 每页显示的个数
     */
    function _getAttendancesMonthList(pageIndex, pageSize) {
        var apiUrl = "api/HmsAttendance/GetAttendanceMonthList?pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }
    /*
    * 获取考勤详细
    * */
    function _getAttendanceDetail(id){
        var apiUrl = "api/HmsAttendance/GetAttendanceInfo?id=" + id;
        return rt.get(apiUrl);
    }
    /*
    * 提交考勤记录
    * */
    function _attendanceCheck(attendance){
        var apiUrl = "api/HmsAttendance/AttendanceCheck";
        return rt.post(apiUrl,attendance);
    }
    return {
        getAttendancesWeekList: _getAttendancesWeekList,
        getAttendancesMonthList: _getAttendancesMonthList,
        attendanceCheck:_attendanceCheck
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('CityVisitService', ['rt', function (rt) {
    'use strict';
    /**
     * 任务单跳转巡店单
     * @returns {*}
     * @private
     */
    function _getCityVisit(id) {
        var apiUrl = "api/BizCityVisit/GetCityVisit?id=" + id;
        return rt.get(apiUrl);
    }

    /**
     * 获取问题单表单界面
     * @param id
     * @param cityVisitId
     * @returns {*}
     * @private
     */
    function _getIssuesById(id, cityVisitId) {
        var apiUrl = "api/BizCityVisit/GetIssuesById?id=" + id + "&cityVisitId=" + cityVisitId;
        return rt.get(apiUrl);
    }

    /**
     * 保存城总巡问题
     * @param pro
     * @returns {*|HttpPromise}
     * @private
     */
    function _saveCityVisitIssue(pro) {
        var apiUrl = "api/BizCityVisit/SaveCityVisitIssue";
        return rt.post(apiUrl, pro);
    }

    /**
     * 结束巡店
     * @param visit
     * @returns {*|HttpPromise}
     * @private
     */
    function _endCityVisit(visit) {
        var apiUrl = "api/BizCityVisit/EndCityVisit";
        return rt.post(apiUrl, visit);
    }

    /**
     * 取消巡店
     * @param cityVisitId
     * @private
     */
    function _cancelCityVisit(cityVisitId) {
        var apiUrl = "api/BizCityVisit/CancelCityVisit?id=" + cityVisitId;
        return rt.get(apiUrl);
    }

    return {
        getCityVisit: _getCityVisit,
        getIssuesById: _getIssuesById,
        saveCityVisitIssue: _saveCityVisitIssue,
        endCityVisit: _endCityVisit,
        cancelCityVisit: _cancelCityVisit
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('ComplaintService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取投诉列表信息
     * @param ishandled 投诉信息状态
     * @param queryValue 查询信息
     * @param pageIndex 当前页数
     * @param pageSize  每页显示的条数
     * @returns {*}
     * @private
     */
    function _getComplaintList(ishandled,queryValue,pageIndex,pageSize) {
        var apiUrl = "api/HmsComplaint/list?ishandled=" + ishandled + "&queryValue=" + queryValue + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     * 获取投诉信息表单
     * @param complaintId 投诉信息Id
     * @returns {*}
     * @private
     */
    function _getComplaint(complaintId){
        var apiUrl = "api/HmsComplaint/form?complaintid="+complaintId;
        return rt.get(apiUrl);
    }

    /**
     * 保存投诉信息
     * @param complaint 投诉信息
     * @returns {*|HttpPromise}
     * @private
     */
    function _saveComplaint(complaint) {
        var apiUrl = "api/HmsComplaint/save";
        return rt.post(apiUrl,complaint);
    }

    /**
     * 根据酒店过滤投诉信息
     * @param hotelId 酒店id
     * @param pageIndex 当前页数
     * @param pageSize 每页显示的条数
     * @returns {*}
     * @private
     */
    function _getComplaintListByHotelId(hotelId,pageIndex,pageSize) {
        var apiUrl = "api/HmsComplaint/GetComplaintListByHotelId?hotelId=" + hotelId  + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     * 获取点评信息
     * @param pageIndex
     * @param pageSize
     * @returns {*}
     * @private
     */
    function _getAppraiseList(pageIndex,pageSize){
        var url = "api/Appraise/GetAppraiseList?pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(url);
    }

    /**
     * 获取详细点评信息
     * @param id
     * @private
     */
    function _getAppraiseDetail(id){
        var url = "api/Appraise/GetAppraiseDetailById?id=" + id;
        return rt.get(url);
    }

    /**
     * 回复点评信息
     * @param id
     * @param content
     * @private
     */
    function _replayAppraise(id, content){
        var url = "api/Appraise/ReplayTheAppraise?id=" + id + "&replayContent=" + content;
        return rt.post(url, content);
    }

    return {
        status : 0,
        getComplaintList: _getComplaintList,
        getComplaint:_getComplaint,
        saveComplaint: _saveComplaint,
        getComplaintListByHotelId:_getComplaintListByHotelId,
        getAppraiseList: _getAppraiseList,
        getAppraiseDetail: _getAppraiseDetail,
        replayAppraise: _replayAppraise
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('DailyService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取工作日志列表信息
     * @param timeSpace 时间区间
     * @param pageIndex 当前页
     * @param pageSize 每页显示的条数
     * @returns {*}
     * @private
     */
    function _getDailyList(timeSpace,pageIndex,pageSize){
        var api = "api/HmsDailyLog/list?type="+timeSpace+"&pageIndex="+pageIndex+"&pageSize="+pageSize;
        return rt.get(api);
    }

    /**
     * 获取工作日志信息
     * @param id 工作日志id
     * @private
     */
    function _getDailyLog(id){
        var api = "api/HmsDailyLog/form?id="+id;
        return rt.get(api);
    }

    /**
     * 获取工作日志标签
     * @private
     */
    function _getDailyLogLabel(){
        var api = "api/HmsDailyLog/label";
        return rt.get(api);
    }

    /**
     * 工作日志保存
     * @param daily 工作日志信息
     */
    function _save(daily){
        var api = "api/HmsDailyLog/save";
        return rt.post(api,daily);
    }


    return {
        timeSpace : 1,
        getDailyList:_getDailyList,
        save:_save,
        getDailyLogLabel:_getDailyLogLabel,
        getDailyLog:_getDailyLog
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('DialogService', ['rt', function (rt) {
    'use strict';
    /**
     * 获取当前能够巡视的门店
     * @param queryValue 查询条件
     * @param pageIndex 当前页数
     * @param pageSize 页面显示的条数
     * @private
     */
    function _getOwnerHotel(queryValue,pageIndex,pageSize){
        var apiUrl = "api/BizHotel/GetOwnerHotel?queryValue=" + queryValue + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    return {
        getOwnerHotel:_getOwnerHotel
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('HomeinnsService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取当前用户token信息
     * @returns {*}
     * @private
     */
    function _getK2Token(){
        var api = "api/ManageHomePage/GetK2Token";
        return rt.get(api);
    }


    return {
        getK2Token:_getK2Token
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('HotelService', ['rt', function (rt) {
    'use strict';

    //所有
    var All = 0;
    //脏房
    var Dirty = 1;
    //干净房
    var Clean = 2;
    //入住房
    var CheckIn = 3;
    //视图类型
    var viewType = {name: "所有", value: All};

    /**
     * 获取当前选中的视图
     * @returns {{name: string, value: number}}
     */
    function _getViewType() {
        return viewType;
    }

    /**
     * 指定范围内的酒店信息
     * @param queryValue
     * @param longitude
     * @param latitude
     * @returns {*}
     * @private
     */
    function _getHotelList(queryValue, longitude, latitude) {
        var apiUrl = "api/BizHotel/GetHotelList?queryValue=" + queryValue + "&longitude=" + longitude + "&latitude=" + latitude;
        return rt.get(apiUrl);
    }

    /**
     * 根据酒店名称模糊匹配出TOP N
     * @param queryValue
     * @returns {*}
     * @private
     */
    function _getTopNHotelList(queryValue){
        var apiUrl = "api/BizHotel/GetTopNHotelList?queryValue=" + escape(queryValue);
        return rt.get(apiUrl);
    }

    /**
     * 根据门店id获取门店详细信息
     * @param id
     * @returns {*}
     * @private
     */
    function _getHotelDetailById(id, pageIndex, pageSize) {
        var apiUrl = "api/BizHotel/GetHotelDetailById?id=" + id + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     * 根据门店id获取qc成绩
     * @param id
     * @returns {*}
     * @private
     */
    function _getQcScoreById(id) {
        var apiUrl = "api/BizHotel/GetBizScoreById?id=" + id;
        return rt.get(apiUrl);
    }

    /**
     * 获取pms房态信息
     * @param id
     * @param viewType
     * @returns {*}
     * @private
     */
    function _getRoomList(id, viewType) {
        var apiUrl = "api/BizHotel/GetRoomList?id=" + id + "&viewType=" + viewType;
        return rt.get(apiUrl);
    }

    /**
     * 开始巡店
     * @param id
     * @returns {*}
     * @private
     */
    function _startCityVisit(hotelId, visitId) {
        var apiUrl = "api/BizCityVisit/StartCityVisit?id=" + hotelId + "&visitId=" + visitId;
        return rt.get(apiUrl);
    }

    /**
     * 获取当前能够巡视的门店
     * @param queryValue 查询条件
     * @param pageIndex 当前页数
     * @param pageSize 页面显示的条数
     * @private
     */
    function _getOwnerHotel(queryValue,pageIndex,pageSize){
        var apiUrl = "api/BizHotel/GetOwnerHotel?queryValue=" + queryValue + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    function _canStartVisit(){
        var apiUrl = "api/BizHotel/CanStartVisit";

        return rt.get(apiUrl);
    }

    /**
     * 获得所有酒店的当日流量
     * @param hotelId
     * @returns {*}
     * @private
     */
    function _getHotelCurrentDayFlow(hotelId){
        var url = "api/BizHotel/GetHotelCurrentDayFlow?hotelId=" + hotelId;
        return rt.get(url);
    }

    /**
     * 获取店长巡店表单界面
     * @returns {*}
     * @private
     */
    function _getManageVisit(hotelId, pageIndex, pageSize) {
        var apiUrl = "api/BizHotel/GetManageVisitList?hotelId=" + hotelId + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     * 获取店长巡店表单界面
     * @returns {*}
     * @private
     */
    function _getManageVisitById(id, hotelId) {
        var apiUrl = "api/BizHotel/GetManageVisitById?id=" + id + "&hotelId=" + hotelId;
        return rt.get(apiUrl);
    }

    /**
     * 获取店长巡店问题
     * @param id
     * @returns {*}
     * @private
     */
    function _getManageIssueById(id) {
        var apiUrl = "api/BizManageVisit/GetManageIssueById?id=" + id;
        return rt.get(apiUrl);
    }

    return {
        getHotelList: _getHotelList,
        getTopNHotelList:_getTopNHotelList,
        getHotelDetailById: _getHotelDetailById,
        getQcScoreById: _getQcScoreById,
        getRoomList: _getRoomList,
        getViewType: _getViewType,
        startCityVisit: _startCityVisit,
        getOwnerHotel:_getOwnerHotel,
        canStartVisit:_canStartVisit,
        getHotelCurrentDayFlow: _getHotelCurrentDayFlow,
        getManageVisit: _getManageVisit,
        getManageVisitById: _getManageVisitById,
        getManageIssueById: _getManageIssueById
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('ManageVisitService', ['rt', function (rt) {
    'use strict';

    //本周
    var Week = 1;
    //本月
    var Month = 2;

    //视图类型
    var viewType = {name: "本周", value: Week};

    /**
     * 获取当前选中的视图
     * @returns {{name: string, value: number}}
     */
    function _getViewType() {
        return viewType;
    }

    /**
     * 获取店长巡店表单界面
     * @returns {*}
     * @private
     */
    function _getManageVisit(viewType, pageIndex, pageSize) {
        var apiUrl = "api/BizManageVisit/GetManageVisitList?viewType=" + viewType + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     * 获取店长巡店表单界面
     * @returns {*}
     * @private
     */
    function _getManageVisitById(id) {
        var apiUrl = "api/BizManageVisit/GetManageVisitById?id=" + id;
        return rt.get(apiUrl);
    }

    /**
     * 获取店长巡店问题
     * @param id
     * @returns {*}
     * @private
     */
    function _getManageIssueById(id) {
        var apiUrl = "api/BizManageVisit/GetManageIssueById?id=" + id;
        return rt.get(apiUrl);
    }

    /**
     * 保存店长巡店问题
     * @param issue
     * @returns {*|HttpPromise}
     * @private
     */
    function _saveManageVisitIssue(issue) {
        var apiUrl = "api/BizManageVisit/SaveManageVisitIssue";
        return rt.post(apiUrl, issue);
    }

    /**
     * 结束巡店
     * @param visit
     * @returns {*|HttpPromise}
     * @private
     */
    function _endManageVisit(visit) {
        var apiUrl = "api/BizManageVisit/EndManageVisit";
        return rt.post(apiUrl, visit);
    }

    /**
     * 取消巡店
     * @param manageVisitId
     * @returns {*|HttpPromise}
     * @private
     */
    function _cancelManageVisit(manageVisitId) {
        var apiUrl = "api/BizManageVisit/CancelManageVisit?id=" + manageVisitId;
        return rt.get(apiUrl);
    }

    return {
        getViewType: _getViewType,
        getManageVisit: _getManageVisit,
        getManageVisitById: _getManageVisitById,
        getManageIssueById: _getManageIssueById,
        saveManageVisitIssue: _saveManageVisitIssue,
        endManageVisit: _endManageVisit,
        cancelManageVisit: _cancelManageVisit
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('MyHotelService', ['rt', function (rt) {
    'use strict';

    //所有
    var All = 0;
    //脏房
    var Dirty = 1;
    //干净房
    var Clean = 2;
    //入住房
    var CheckIn = 3;
    //视图类型
    var viewType = {name: "所有", value: All};

    /**
     * 获取当前选中的视图
     * @returns {{name: string, value: number}}
     */
    function _getViewType() {
        return viewType;
    }



    /**
     * 根据门店id获取门店详细信息
     * @param id
     * @returns {*}
     * @private
     */
    function _getHotelDetailById(id, pageIndex, pageSize) {
        var apiUrl = "api/BizHotel/GetHotelDetailById?id=" + id + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     * 根据门店id获取qc成绩
     * @param id
     * @returns {*}
     * @private
     */
    function _getQcScoreById(id) {
        var apiUrl = "api/BizHotel/GetBizScoreById?id=" + id;
        return rt.get(apiUrl);
    }

    /**
     * 获取pms房态信息
     * @param id
     * @param viewType
     * @returns {*}
     * @private
     */
    function _getRoomList(id, viewType) {
        var apiUrl = "api/BizHotel/GetRoomList?id=" + id + "&viewType=" + viewType;
        return rt.get(apiUrl);
    }

    /**
     * 开始巡店
     * @param id
     * @returns {*}
     * @private
     */
    function _startCityVisit(hotelId, visitId) {
        var apiUrl = "api/BizCityVisit/StartCityVisit?id=" + hotelId + "&visitId=" + visitId;
        return rt.get(apiUrl);
    }

    /**
     * 获取当前能够巡视的门店
     * @param queryValue 查询条件
     * @param pageIndex 当前页数
     * @param pageSize 页面显示的条数
     * @private
     */
    function _getOwnerHotel(queryValue,pageIndex,pageSize){
        var apiUrl = "api/BizHotel/GetOwnerHotel?queryValue=" + queryValue + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    function _canStartVisit(){
        var apiUrl = "api/BizHotel/CanStartVisit";

        return rt.get(apiUrl);
    }

    /**
     * 获得所有酒店的当日流量
     * @param hotelId
     * @returns {*}
     * @private
     */
    function _getHotelCurrentDayFlow(hotelId){
        var url = "api/BizHotel/GetHotelCurrentDayFlow?hotelId=" + hotelId;
        return rt.get(url);
    }

    /**
     * 后台根据当前用户获取酒店Id,code,name--店长用
     * @param hotelId
     * @returns {*}
     * @private
     */
    function _getHotelNameByUserId(){
        var url = "api/BizHotel/GetHotelNameByUserId";
        return rt.get(url);
    }

    /**
     * 获取店长巡店表单界面
     * @returns {*}
     * @private
     */
    function _getManageVisit(hotelId, pageIndex, pageSize) {
        var apiUrl = "api/BizHotel/GetManageVisitList?hotelId=" + hotelId + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    return {
        getHotelDetailById: _getHotelDetailById,
        getQcScoreById: _getQcScoreById,
        getRoomList: _getRoomList,
        getViewType: _getViewType,
        startCityVisit: _startCityVisit,
        getOwnerHotel:_getOwnerHotel,
        canStartVisit:_canStartVisit,
        getHotelCurrentDayFlow: _getHotelCurrentDayFlow,
        getHotelNameByUserId: _getHotelNameByUserId,
        getManageVisit: _getManageVisit
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('NoticeService', ['rt', function (rt) {
    'use strict';

    /**
     * 公告列表
     * @param queryValue
     * @param pageIndex
     * @param pageSize
     * @returns {*}
     * @private
     */
    function _getNoticeList(queryValue, pageIndex, pageSize) {
        var apiUrl = "api/HmsNotice/GetNoticeList?queryValue=" + queryValue + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
        return rt.get(apiUrl);
    }

    /**
     *请求只读界面数据的接口
     * @param itemid
     * @returns {*}
     * @private
     */
    function _getFormData(itemid) {
        var apiUrl = "api/HmsNotice/GetReadNoticeList?readId=" + itemid;
        return rt.get(apiUrl);
    }

    return {
        getNoticeList: _getNoticeList,
        getFormData: _getFormData
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('PasswordService', ['rt', function (rt) {
    'use strict';

    function _getUserInfo(userCode,emailTypeId) {
        var url = "api/PasswordForget/GetUserInfo?userCode=" + userCode+"&emailTypeId="+emailTypeId;
        return rt.get(url, userCode);
    }

    function _getEmailTypeList(){
        var url = "api/PasswordForget/GetEmailTypeList";
        return rt.get(url);
    }

    /**
     * 重置密码
     * @returns {*|HttpPromise}
     * @private
     */
    function _resetPassword(userCode,emailTypeId) {
        var url = "api/PasswordForget/ResetPassword?userCode=" + userCode+"&emailTypeId="+emailTypeId;
        return rt.post(url, null);
    }

    return {
        getUserInfo: _getUserInfo,
        getEmailTypeList:_getEmailTypeList,
        resetPassword: _resetPassword
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('QuestionService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取问卷调查列表信息
     * @private
     */
    function _getQuestionList(pageIndex,pageSize){
        var api = "api/HmsQuestionnaire/list?pageIndex="+pageIndex+"&pageSize="+pageSize;
        return rt.get(api);
    }

    /**
     * 获取问卷题目
     * @param questionnaireid 问卷id
     * @returns {*}
     * @private
     */
    function _getQuestionSubject(questionnaireid){
        var api = "api/HmsQuestionnaire/subject?questionnaireid="+questionnaireid;
        return rt.get(api);
    }

    /**
     * 保存问卷信息
     * @param question
     * @private
     */
    function _save(question,hotelid){
        var api = "api/HmsQuestionnaire/save?hotelid="+hotelid;
        return rt.post(api,question);
    }

    return {
        getQuestionList:_getQuestionList,
        getQuestionSubject:_getQuestionSubject,
        save:_save
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('RevenueService', ['rt', function (rt) {
    'use strict';

    function _isFirstChange(){
        var url = "api/PriceChange/IsFirstChange";
        return rt.get(url);
    }

    /**
     * 获得所有酒店的当日流量
     * @param hotelId
     * @returns {*}
     * @private
     */
    function _getHotelCurrentDayFlow(){
        var url = "api/FlowToday/GetHotelCurrentDayFlow";
        return rt.get(url);
    }

    /**
     * 获取门店房型当日流量及房价
     * @param hotelId
     * @returns {*}
     * @private
     */
    function _getHotelRoomCurrentDayFlowAndPrice(hotelId){
        var url = "api/FlowToday/GetHotelRoomCurrentDayFlowAndPrice?hotelId=" + hotelId;
        return rt.get(url);
    }

    /**
     * 房价调整界面中的流量信息
     * @private
     */
    function _getPriceChangeHotelFlow(hotelId){
        var url = "api/PriceChange/GetHotelCurrentDayFlow?hotelId=" + hotelId;
        return rt.get(url);
    }

    /**
     * 房价调整界面中的房型门市价
     * @private
     */
    function _getPriceChangeRoomTypePrice(hotelId){
        var url = "api/PriceChange/GetHotelRoomCurrentDayFlowAndPrice?hotelId=" + hotelId;
        return rt.get(url);
    }

    /**
     * 获取流量阀值
     * @private
     */
    function _getTheFlowThreshold(){
        var url = "api/PriceChange/GetTheFlowThreshold";
        return rt.get(url);
    }

    /**
     * 修改流量阀值
     * @private
     */
    function _saveTheFlowThreshold(threshold){
        var url = "api/PriceChange/SaveTheFlowThreshold";
        return rt.post(url, threshold);
    }

    /**
     * 房价调整
     * @private
     */
    function _changeRoomPrice(body){
        var url = "api/PriceChange/SubmitRoomPrice";
        return rt.post(url, body);
    }

    /**
     * 房价调整
     * @private
     */
    function _changeRoomPriceTwo(body){
        var url = "api/PriceChange/SubmitRoomPriceTwo";
        return rt.post(url, body);
    }

    /**
     *检查所选房型是否已经申请
     * @private
     */
    function _checkApiIsUsed(body){
        var url = "api/PriceChange/CheckApiIsUsed";
        return rt.post(url, body);
    }

    return {
        isFirstChange:_isFirstChange,
        getHotelCurrentDayFlow: _getHotelCurrentDayFlow,
        getHotelRoomCurrentDayFlowAndPrice: _getHotelRoomCurrentDayFlowAndPrice,
        getPriceChangeHotelFlow: _getPriceChangeHotelFlow,
        getPriceChangeRoomTypePrice: _getPriceChangeRoomTypePrice,
        getTheFlowThreshold: _getTheFlowThreshold,
        saveTheFlowThreshold: _saveTheFlowThreshold,
        changeRoomPrice: _changeRoomPrice,
        changeRoomPriceTwo: _changeRoomPriceTwo,
        checkApiIsUsed: _checkApiIsUsed
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('RoomControlService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取当前用户需要打扫的房间
     * @private
     */
    function _getTheRoomList() {
        var url = "api/RoomClean/GetTheDividedRoomList";
        return rt.get(url);
    }

    function _startCleanRoom(roomData){
        var url = "api/RoomClean/StartClean";
        return rt.post(url, roomData);
    }

    /**
     * 打扫完成
     * @param roomData
     * @returns {*|HttpPromise}
     * @private
     */
    function _saveTheCleanedRoom(roomData) {
        var url = "api/RoomClean/FinishClean";
        return rt.post(url, roomData);
    }

    /**
     * 设置房间异常 阿姨设置异常
     * @param roomNo
     * @private
     */
    function _setRoomException(roomNo, exception) {
        var url = "api/RoomClean/SetRoomException?roomNo=" + roomNo + "&exception=" + exception;
        return rt.post(url);
    }

    /**
     * 设置房间异常 客房经理标注异常
     * @param roomNo
     * @private
     */
    function _setRoomExceptionNote(roomNo, exception) {
        var url = "api/RoomClean/SetRoomExceptionNote?roomNo=" + roomNo + "&exception=" + exception;
        return rt.post(url);
    }

    /**
     * 计算房间打扫的数量
     * @private
     */
    function _CountRoomCleaned(roomType) {
        var url = "api/RoomClean/CountRoomCleaned?roomType=" + roomType;
        return rt.get(url);
    }

    /**
     * 查房 评分 改房态
     * @private
     */
    function _checkRoom(roomNo, score, state, stateAfter) {
        var url = "api/RoomClean/CheckRoomAndGradeAndChangeState?roomNo=" + roomNo + "&score=";
        url += score + "&state=" + state + "&stateAfter=" + stateAfter;
        return rt.post(url);
    }

    /**
     * 获取阿姨列表
     * @returns {*}
     * @private
     */
    function _getAuntList(){
        var url = "api/RoomClean/GetAuntList";
        return rt.get(url);
    }

    /**
     * 对未打扫的房间重新分配
     * @private
     */
    function _reDivideRoomToAunt(roomNo, auntId){
        var url = "api/RoomClean/ReDivideRoomToAunt?roomNo=" + roomNo + "&auntId=" + auntId;
        return rt.post(url);
    }

    /**
     * 保存缓存
     * @param localStorage
     * @private
     */
    function _saveLocalStorage(localStorage){
        var url = "api/RoomClean/SaveLocalData";
        return rt.post(url, localStorage);
    }

    /**
     * 读取缓存
     * @private
     */
    function _getLocalStorage(){
        var url = "api/RoomClean/GetLocalStorage";
        return rt.get(url);
    }

    /**
     * 获取客用品统计数量
     * @returns {*}
     * @private
     */
    function _getGuestSupplies(){
        var url = "api/RoomClean/GetGuestSupplies";
        return rt.get(url);
    }

    /**
     * 保存客用品
     * @param guestSupplies
     * @returns {*|HttpPromise}
     * @private
     */
    function _saveGuestSupplies(guestSupplies){
        var url = "api/RoomClean/SaveGuestSupplies";
        return rt.post(url, guestSupplies);
    }

    /**
     * 获取二级菜单权限
     * @private
     */
    function _getPrivilege(){
        var url = "api/RoomClean/GetRoleName";
        return rt.get(url);
    }

    /**
     * 获取打扫完毕的房间
     * @private
     */
    function _getCleanedRooms(type){
        var url = "api/RoomClean/GetCleanedRooms?roomType=" + type;
        return rt.get(url);
    }

    /**
     * 获取客用品标配数量
     * @returns {*}
     * @private
     */
    function _getStandardNum(){
        var url = "api/GuestSupplies/GetGuestSupplies";
        return rt.get(url);
    }

    /**
     * 获取白中班时间
     * @returns {*}
     * @private
     */
    function _getTimePoint(){
        var url = "api/TimePoint/GetTimeList";
        return rt.get(url);
    }

    /**
     * 获取中班的房间
     * @returns {*}
     * @private
     */
    function _getRoomListMid(){
        var url = "api/RoomClean/GetRoomListMid";
        return rt.get(url);
    }

    /**
     * 更新房间信息
     * @param roomNo
     * @returns {*|HttpPromise}
     * @private
     */
    function _updateRoomInfo(roomNo){
        var url = "api/RoomClean/UpdateAuntSet";
        return rt.post(url, roomNo);
    }

    /**
     * 设置阿姨到房间
     * @param roomNo
     * @returns {*|HttpPromise}
     * @private
     */
    function _setAuntToRoom(roomNo){
        var url = "api/RoomClean/SetAuntToRoom";
        return rt.post(url, roomNo);
    }

    /**
     * 获取当前客房服务员是否为中班
     * @returns {*}
     * @private
     */
    function _getIsMid(){
        var url = "api/RoomClean/CheckIsMid";
        return rt.get(url);
    }

    /**
     * 计算房间已分房未打扫的房间
     * @private
     */
    function _countRoomUnCheckedUnCleaned(roomType) {
        var url = "api/RoomClean/CountRoomUnCheckedUnCleaned?roomType=" + roomType;
        return rt.get(url);
    }

    /**
     * 计算房间查房的数量和获取查房完毕的房间
     * @private
     */
    function _countRoomChecked(roomType) {
        var url = "api/RoomClean/CountRoomChecked?roomType=" + roomType;
        return rt.get(url);
    }

    /**
     * 计算房间已打扫未查房的房间
     * @private
     */
    function _countRoomUnCheckedCleaned(roomType) {
        var url = "api/RoomClean/CountRoomUnCheckedCleaned?roomType=" + roomType;
        return rt.get(url);
    }

    function _getSuppliesList(){
        var url = "api/GuestSupplies/GetSuppliesList";
        return rt.get(url);
    }

    function _saveSuppliesList(data){
        var url = "api/GuestSupplies/SaveTheSupplies";
        return rt.post(url, data);
    }

    return {
        getTheRoomList: _getTheRoomList,
        saveTheCleanedRoom: _saveTheCleanedRoom,
        setRoomException: _setRoomException,
        CountRoomCleaned: _CountRoomCleaned,
        setRoomExceptionNote: _setRoomExceptionNote,
        checkRoom: _checkRoom,
        getAuntList: _getAuntList,
        reDivideRoomToAunt: _reDivideRoomToAunt,
        saveLocalStorage: _saveLocalStorage,
        getLocalStorage: _getLocalStorage,
        getGuestSupplies: _getGuestSupplies,
        saveGuestSupplies: _saveGuestSupplies,
        startCleanRoom: _startCleanRoom,
        getPrivilege: _getPrivilege,
        getCleanedRooms: _getCleanedRooms,
        getStandardNum: _getStandardNum,
        getTimePoint: _getTimePoint,
        getRoomListMid: _getRoomListMid,
        updateRoomInfo: _updateRoomInfo,
        setAuntToRoom: _setAuntToRoom,
        getIsMid: _getIsMid,
        countRoomUnCheckedUnCleaned: _countRoomUnCheckedUnCleaned,
        countRoomChecked: _countRoomChecked,
        countRoomUnCheckedCleaned: _countRoomUnCheckedCleaned,
        getSuppliesList: _getSuppliesList,
        saveSuppliesList: _saveSuppliesList
    };
}]);

/*global angular:false */
/*global xrmApp:false */
xrmApp.factory('TaskService', ['rt', function (rt) {
    'use strict';

    /**
     * 获取任务信息
     * @param type 任务类型（0 我处理的 1我发起的
     * @param pageIndex 当前页表标
     * @param pageSize 每页显示的条数
     * @returns {*}
     * @private
     */
    function _getTaskList(type,pageIndex,pageSize){
        var api = "api/HmsTask/list?type="+type+"&pageIndex="+pageIndex+"&pageSize="+pageSize;
        return rt.get(api);
    }

    function _getSponsorTask(pageIndex,pageSize){
        var api = "api/HmsTask/sponsorTask?pageIndex="+pageIndex+"&pageSize="+pageSize;
        return rt.get(api);
    }

    function _getSponsorTaskList(id){
        var api = "api/HmsTask/sponsorTaskList?taskid="+id;
        return rt.get(api);
    }

    /**
     * 获取重要级别选项信息
     * @returns {*}
     * @private
     */
    function _getLevelOptions(){
        var api= "api/HmsTask/level";
        return rt.get(api);
    }

    /**
     * 获取用户列表信息
     * @param queryValue 查询条件
     * @param pageIndex 当前页数
     * @param pageSize 每页显示的条数
     * @private
     */
    function _getUserList(queryValue,hotelScreen, pageIndex, pageSize){
        var apiUrl = "api/HmsTask/GetUserList?queryValue="+queryValue+"&pageIndex="+pageIndex+"&pageSize="+pageSize+
            "&hotelProperty="+hotelScreen.HotelProperty.Value + "&hotelType="+hotelScreen.HotelType.Value+
            "&isManagedSelf="+hotelScreen.HotelIsManagedSelf.Value;
        return  rt.get(apiUrl);
    }

    /**
     * 获取待办任务的信息
     * @param id 待办任务的id
     * @returns {*}
     * @private
     */
    function _getTaskModel(id){
        var apiUrl = "api/HmsTask/get?id="+id;
        return rt.get(apiUrl);
    }

    /**
     * 保存任务信息
     * @param taskModel 任务信息
     * @private
     */
    function _save(taskModel){
        var apiUrl = "api/HmsTask/save";
        return rt.post(apiUrl,taskModel);
    }

    /**
     * 处理任务
     * @param taskModel 任务信息
     * @returns {*|HttpPromise}
     * @private
     */
    function _handlerTask(taskModel){
        var apiUrl = "api/HmsTask/handler";
        return rt.post(apiUrl,taskModel);
    }

    /**
     * 获取系统任务的类型
     * @param taskid 任务id
     * @returns {*}
     * @private
     */
    function _getTaskTypes(taskid){
        var apiUrl = "api/HmsTask/taskTypeList?taskid=" + taskid;
        return rt.get(apiUrl);
    }

    /**
     * 处理系统任务
     * @param taskTypes
     * @returns {*|HttpPromise}
     * @private
     */
    function _handlerTaskSystem(taskId,taskTypes){
        var apiUrl = "api/HmsTask/handlerTaskSystem?taskId="+taskId;
        return rt.post(apiUrl,taskTypes);
    }

    /**
     * 获取酒店筛选信息
     * @private
     */
    function _getHotelScreen(){
        var apiUrl = "api/HmsTask/hotelScreen";
        return rt.get(apiUrl);
    }

    function _getRoleList(queryValue){
        var apiUrl = "api/HmsTask/rolelist?queryValue="+queryValue;
        return rt.get(apiUrl);
    }

    function _getPrivilege(){
        var apiUrl = "api/HmsTask/GetPrivilege";
        return rt.get(apiUrl);
    }

    /**
     * 查询任务相关信息
     * @returns {*}
     * @private
     */
    function _getCityVisit(id) {
        var apiUrl = "api/HmsTask/GetTaskTaskRole?id=" + id;
        return rt.get(apiUrl);
    }

    /**
     * 获取问题单表单界面
     * @param id
     * @param cityVisitId
     * @returns {*}
     * @private
     */
    function _getIssuesById(id, cityVisitId) {
        var apiUrl = "api/HmsTask/GetIssuesById?id=" + id + "&cityVisitId=" + cityVisitId;
        return rt.get(apiUrl);
    }

    /**
     * 保存城总巡问题
     * @param pro
     * @returns {*|HttpPromise}
     * @private
     */
    function _saveCityVisitIssue(pro) {
        var apiUrl = "api/HmsTask/SaveCityVisitIssue";
        return rt.post(apiUrl, pro);
    }

    /**
     * 取消巡店
     * @param cityVisitId
     * @private
     */
    function _cancelCityVisit(cityVisitId) {
        var apiUrl = "api/BizCityVisit/CancelCityVisit?id=" + cityVisitId;
        return rt.get(apiUrl);
    }

    /**
     * 结束巡店
     * @param visit
     * @returns {*|HttpPromise}
     * @private
     */
    function _endCityVisit(taskroleId) {
        var apiUrl = "api/HmsTask/EndCityVisit?taskroleId=" + taskroleId;
        return rt.get(apiUrl);
    }

    return {
        getRoleList:_getRoleList,
        getSponsorTaskList:_getSponsorTaskList,
        getSponsorTask:_getSponsorTask,
        getTaskList:_getTaskList,
        getHotelScreen:_getHotelScreen,
        getUserList:_getUserList,
        getLevelOptions:_getLevelOptions,
        getTaskModel:_getTaskModel,
        save:_save,
        handlerTask:_handlerTask,
        getTaskTypes:_getTaskTypes,
        handlerTaskSystem:_handlerTaskSystem,
        getPrivilege:_getPrivilege,
        getCityVisit: _getCityVisit,
        getIssuesById: _getIssuesById,
        saveCityVisitIssue: _saveCityVisitIssue,
        cancelCityVisit: _cancelCityVisit,
        endCityVisit: _endCityVisit
    };
}]);

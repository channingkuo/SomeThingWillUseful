#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-09
说明 : 用于内嵌html5功能的webview页面
****************/
#endregion
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using CoreLocation;
using System.IO;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;

namespace RekTec.Corelib.Views
{
    /// <summary>
    /// 用于内嵌html5功能的webview页面
    /// </summary>
    public class WKWebViewController : BaseViewController
    {
        //菜单编码
        public string _menuUrl;
        //是否首次加载
        private bool _isFirstLoad = true;
        WKWebView _webView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (this.ParentViewController != null && this.ParentViewController.NavigationItem != null) {
                this.ParentViewController.NavigationItem.SetRightBarButtonItem(null, false);
            }
            //隐藏导航栏
            this.NavigationController.NavigationBarHidden = true;

            //清除缓存
            NSUrlCache.SharedCache.RemoveAllCachedResponses();

            //修改状态栏背景颜色
            UIView statusview = new UIView(new CGRect(0, 0, View.Frame.Width, UiStyleSetting.StatusBarHeight));
            statusview.BackgroundColor = UiStyleSetting.RektecBlueColor;
            View.AddSubview(statusview);

            //设置cookie的接受政策
            NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;

            var c = new WKWebViewConfiguration();
            _webView = new WKWebView(new CGRect(0, 
                UiStyleSetting.StatusBarHeight, 
                View.Bounds.Width, 
                View.Bounds.Height - UiStyleSetting.StatusBarHeight)
                , c);

            _webView.NavigationDelegate = new NavigationDelegate(this); 

            var address = string.Empty;
            if (GlobalAppSetting.XrmWebApiBaseUrl.EndsWith("/")) {
                address = GlobalAppSetting.XrmWebApiBaseUrl;
            } else {
                address = GlobalAppSetting.XrmWebApiBaseUrl + "/";
            }
            var url = Path.Combine(address, "debug/index.html");
      
            _webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));//跳转自定义url

            View.AddSubview(_webView);
        }

        class NavigationDelegate:WKNavigationDelegate
        {
            private WKWebViewController _c;

            public NavigationDelegate(WKWebViewController c)
            {
                _c = c;
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, System.Action<WKNavigationActionPolicy> decisionHandler)
            {
                var requesturl = webView.Url.ToString();
                if (requesturl.Contains("/app/close")) { //检测到URL里包含app/close,则关闭应用

                    if (_c != null && _c.NavigationController != null) {
                        _c.NavigationController.NavigationBarHidden = false;

                        if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                            _c.NavigationController.PopToRootViewController(false);
                        else
                            _c.NavigationController.PopToRootViewController(false);
                    }
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                    return;
                }

                if (requesturl.Contains("app:take-photo")) {//检测到URL里包含app:take-photo,则打开相机进行拍照
                    RekTec.Corelib.Utils.CameraUtil.TakePicture(_c, (img) => {
                        var imgBase64 = RekTec.Corelib.Utils.ImageUtil.ConvertImage2Base64String(img, 100, 100);
                        webView.EvaluateJavaScript(new NSString("window.XrmImageData={getXrmImageData:function(){return '" + imgBase64 + "';},clearImageData:function(){}}")
                            , (a, b) => {
                        });
                    }, true);

                    decisionHandler(WKNavigationActionPolicy.Cancel);
                    return;
                }

                if (requesturl.Contains("FileDownloadHandler.ashx")) {//检测到URL里包含FileDownloadHandler.ashx,则打开浏览器进行附件下载
                    UIApplication.SharedApplication.OpenUrl(webView.Url);

                    decisionHandler(WKNavigationActionPolicy.Cancel);
                    return;
                }

                decisionHandler(WKNavigationActionPolicy.Allow);
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                if (_c._isFirstLoad) {
                    var address = string.Empty;
                    if (GlobalAppSetting.XrmWebApiBaseUrl.EndsWith("/")) {
                        address = GlobalAppSetting.XrmWebApiBaseUrl;
                    } else {
                        address = GlobalAppSetting.XrmWebApiBaseUrl + "/";
                    }

                    webView.EvaluateJavaScript(new NSString("window.localStorage.setItem('XrmBaseUrl','" + address + "');"), (a, b) => {
                    });
                    webView.EvaluateJavaScript(new NSString("window.localStorage.setItem('XrmAuthToken','" + GlobalAppSetting.XrmAuthToken + "');"), (a, b) => {
                    });
                    webView.EvaluateJavaScript(new NSString("window.localStorage.setItem('UserId','" + GlobalAppSetting.UserId + "');"), (a, b) => {
                    });
                    webView.EvaluateJavaScript(new NSString(string.Format("window.location.href='index.html#/{0}?v=1';", _c._menuUrl)), (a, b) => {
                    });
                    _c._isFirstLoad = false;
                }   
            }
        }
    }
}


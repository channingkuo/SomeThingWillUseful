#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-13
说明 : 个人信息修改页面
****************/
#endregion

using System;
using System.Text.RegularExpressions;
using MonoTouch.Dialog;
using RekTec.Corelib.Configuration;
using RekTec.Corelib.Utils;
using RekTec.MyProfile.Services;
using RekTec.MyProfile.ViewModels;
using UIKit;

namespace RekTec.MyProfile.Views
{
    /// <summary>
    /// 个人信息修改页面
    /// </summary>
    public class MyProfileEditViewController : DialogViewController
    {
        private readonly string _title;
        private readonly EntryElement _element;
        private const string _regex = @"^1[34578]\d{9}$";

        public MyProfileEditViewController(RootElement r, string title, string value)
            : base(UITableViewStyle.Grouped, r, true)
        {
            _title = title;
            var root1 = r;
            _element = new EntryElement(string.Empty, "请输入" + root1.Caption, value)
            {
                AutocapitalizationType = UITextAutocapitalizationType.None,
                AutocorrectionType = UITextAutocorrectionType.No,
                KeyboardType = UIKeyboardType.PhonePad
            };
            root1.Add(new[]{ new Section { _element } });
            BindUIEvent();
        }

        /// <summary>
        /// 绑定控件的点击事件
        /// </summary>
        private void BindUIEvent()
        {
            _element.Changed += (sender, e) => {
                AuthenticationService.CurrentUserInfo.Phone = _element.Value;
            };
        }

        /// <summary>
        /// 错误处理
        /// </summary>
        private bool HandleError(string msg)
        {
            AlertUtil.Error(msg);
            return true;
        }

        /// <summary>
        /// 页面将要出现的时候
        /// </summary>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.TableView.BackgroundView = null;
            this.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            if (NavigationItem != null) {
                NavigationItem.Title = _title;
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Save), false);
                NavigationItem.RightBarButtonItem.Clicked += async(sender, e) => {
                    if (!Regex.IsMatch(_element.Value, _regex)) {
                        AlertUtil.Error("电话号码格式不正确！");
                        return;
                    }

                    AuthenticationService.CurrentUserInfo.Phone = _element.Value;
                    try
                    {
                        await UserService.UpdatePhoneNumber(new UserPhoneViewModel() {PhoneNumber = _element.Value});
                        this.NavigationController.PopViewController(false);
                    } catch (Exception error) {
                        ErrorHandlerUtil.ReportError(error.Message);
                    }
                };
            }

            ErrorHandlerUtil.Subscribe(HandleError);
        }

        /// <summary>
        /// 页面将要消失的时候
        /// </summary>
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ErrorHandlerUtil.UnSubscribe(HandleError);
        }
    }
}


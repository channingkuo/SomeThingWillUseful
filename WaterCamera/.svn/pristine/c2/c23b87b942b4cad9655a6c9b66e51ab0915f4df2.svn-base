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
using RekTec.Corelib.Views;
using RekTec.MyProfile.Services;
using RekTec.MyProfile.ViewModels;
using UIKit;

namespace RekTec.MyProfile.Views
{
	/// <summary>
	/// 个人信息修改页面
	/// </summary>
	public class SuggestEditViewController : DialogViewController
	{
		private readonly string _title;
		private readonly MultilineEntryElement _element;

		public SuggestEditViewController (RootElement r, string title)
			: base (UITableViewStyle.Grouped, r, true)
		{
			_title = title;
			_element = new MultilineEntryElement (string.Empty, "请输入意见反馈", string.Empty);
			r.Add (new[]{ new Section { _element } });
		}


		/// <summary>
		/// 错误处理
		/// </summary>
		private bool HandleError (string msg)
		{
			AlertUtil.Error (msg);
			return true;
		}

		/// <summary>
		/// 页面将要出现的时候
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.TableView.BackgroundView = null;
			this.TableView.BackgroundColor = UiStyleSetting.ViewControllerColor;
			this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			if (NavigationItem != null) {
				NavigationItem.Title = _title;
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem ("确定", UIBarButtonItemStyle.Plain, null), false);
				NavigationItem.RightBarButtonItem.Clicked += async(sender, e) => {
					if (string.IsNullOrEmpty (_element.Summary ()))
						return;

					try {
						await SystemSettingService.SaveSuggest (new SuggestionModel () {
							Id = Guid.NewGuid ().ToString (),
							Content = _element.Summary (),
							CreateBy = AuthenticationService.CurrentUserInfo.UserName,
							UserName = AuthenticationService.CurrentUserInfo.UserCode
						});
						this.NavigationController.PopViewController (false);
					} catch (Exception error) {
						ErrorHandlerUtil.ReportError (error.Message);
					}
				};
			}

			ErrorHandlerUtil.Subscribe (HandleError);
		}

		/// <summary>
		/// 页面将要消失的时候
		/// </summary>
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			ErrorHandlerUtil.UnSubscribe (HandleError);
		}
	}
}


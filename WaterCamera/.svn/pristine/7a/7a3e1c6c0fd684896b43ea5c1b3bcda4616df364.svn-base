#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-21
说明 : 关于我们页面
****************/
#endregion
using System;
using RekTec.Corelib.Views;
using UIKit;
using CoreGraphics;
using RekTec.Corelib.Configuration;
using Foundation;
using RekTec.Version.Services;

namespace RekTec.MyProfile.Views
{
	/// <summary>
	/// 关于我们页面
	/// </summary>
	public class AboutUsViewController:BaseViewController
	{
		UITableView _tableView;

		/// <summary>
		/// 页面加载时候执行
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			_tableView = new UITableView (new CGRect (View.Bounds.X, View.Bounds.Y, View.Bounds.Width, View.Bounds.Height)) {
				BackgroundView = null,
				BackgroundColor = UIColor.White,
				SeparatorStyle = UITableViewCellSeparatorStyle.None,
				ScrollEnabled = true,
				Source = new AboutUsSouce ()
			};
			Add (_tableView);
			this.NavigationItem.Title = "关于";
		}

		/// <summary>
		/// table绑定数据源
		/// </summary>
		public class AboutUsSouce:UITableViewSource
		{
			readonly string _title = "xCRM";
			readonly string _detailTitle = string.Format ("xCRM｀ IOS客户端\n{0}", VersionService.AppVersion);
			readonly string _companyInfo = @"瑞泰信息技术有限公司是一家专业从事企业管理软件咨询、开发与实施服务的科技型企业，是营销信息化领域的领跑者，致力于利用移动、社交、云、大数据等信息技术来推动企业的营销创新与变革。 
作为微软的金牌合作伙伴和微软CRM产品的最佳解决方案服务商，瑞泰信息提供包括CRM(客户关系管理)、BI（商业智能分析）、移动应用、社交平台整合、O2O 等全方位的营销与服务管理解决方案，并在消费品、高科技、医药、机械、汽车、零售、服务等行业拥有100多家成功客户，与徐工、宇通、松下、苏泊尔、广药、三金、传化、保利协鑫、汤臣倍健等行业龙头企业建立了广泛的合作。  ";
			readonly string _copyrightInfo = "瑞泰信息 版权所有\n Copyright © 2015 Rektec IT Co.，Ltd.\n All Rights Reserved";

			/// <summary>
			/// 设置一共有几个section
			/// </summary>
			public override nint NumberOfSections (UITableView tableView)
			{
				return 3;
			}

			/// <summary>
			/// 每个section有几行
			/// </summary>
			public override nint RowsInSection (UITableView tableview, nint section)
			{
				if ((int)section == 0)
					return 3;
				else if ((int)section == 1)
					return 1;
				else
					return 2;
			}

			/// <summary>
			/// 设置每行的高度
			/// </summary>
			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					if (indexPath.Row == 0)
						return 70;
					else if (indexPath.Row == 1)
						return 20;
					else
						return 50;
				} else if (indexPath.Section == 1) {
					return CalculateHeight (tableView);
				} else {
					if (indexPath.Row == 0)
						return tableView.Frame.Height - 260 - CalculateHeight (tableView);
					else
						return 60;
				}
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == 0 && indexPath.Section == 0)
					return CreateLogoCell (tableView);
				else if (indexPath.Row == 1 && indexPath.Section == 0)
					return CreateTitleCell (tableView);
				else if (indexPath.Row == 2 && indexPath.Section == 0)
					return CreateTitleDetailCell (tableView);
				else if (indexPath.Row == 0 && indexPath.Section == 1)
					return CreateContentCell (tableView);
				else if (indexPath.Row == 1 && indexPath.Section == 2)
					return CreateFooterCell (tableView);
				else
					return CreateSpaceCell (tableView); 
			}

			/// <summary>
			/// 创建显示logo的cell
			/// </summary>
			private UITableViewCell CreateLogoCell (UITableView tableView)
			{
				
				const string cellIdentifier = "TableViewCellLogo";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					UIImageView imageView = new UIImageView (new CGRect ((tableView.Bounds.Width - 60) / 2, 5, 60, 60));
					imageView.Image = UIImage.FromFile ("Icon-Small.png");
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					cell.ContentView.AddSubview (imageView);
				}

				return cell;
			}

			/// <summary>
			/// 创建显示title的cell
			/// </summary>
			private UITableViewCell CreateTitleCell (UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellTitle";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					cell.TextLabel.Text = _title;
					cell.TextLabel.Font = UIFont.BoldSystemFontOfSize (20);
					cell.TextLabel.TextColor = UiStyleSetting.ButtonTextBlue;
					cell.TextLabel.TextAlignment = UITextAlignment.Center;
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				}
				return cell;
			}

			/// <summary>
			/// 创建显示detail的cell
			/// </summary>
			private UITableViewCell CreateTitleDetailCell (UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellTitleDetail";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					cell.TextLabel.Text = _detailTitle;
					cell.TextLabel.Font = UIFont.BoldSystemFontOfSize (14);
					cell.TextLabel.TextColor = UIColor.LightGray;
					cell.TextLabel.TextAlignment = UITextAlignment.Center;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.SizeToFit ();
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				}
				return cell;
			}

			/// <summary>
			/// 创建显示content的cell
			/// </summary>
			private UITableViewCell CreateContentCell (UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellContent";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					cell.TextLabel.Font = UIFont.BoldSystemFontOfSize (14);
					cell.TextLabel.TextColor = UIColor.Gray;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.SizeToFit ();
					NSMutableParagraphStyle style = new NSMutableParagraphStyle () {
						FirstLineHeadIndent = 20,
						LineSpacing = 5
					};
					//首行缩进
					var str = new NSAttributedString (_companyInfo, null, null, null, null, style);
					cell.TextLabel.AttributedText = str;
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				}
				return cell;
			}

			/// <summary>
			/// 计算行的高度
			/// </summary>
			private nfloat CalculateHeight (UITableView tableView)
			{
				nfloat width = 0;
				if (tableView.Frame.Width > 320)
					width = tableView.Frame.Width - 150;
				else if (tableView.Frame.Height < 568)
					width = tableView.Frame.Width - 5;
				else
					width = tableView.Frame.Width - 100;
				CGSize size = _companyInfo.StringSize (UIFont.BoldSystemFontOfSize (14), new CGSize (width, 2000), UILineBreakMode.WordWrap);
				return size.Height;
			}

			/// <summary>
			/// 创建显示content的cell
			/// </summary>
			private UITableViewCell CreateFooterCell (UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellContent";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					cell.TextLabel.Font = UIFont.SystemFontOfSize (12);
					cell.TextLabel.TextColor = UIColor.LightGray;
					cell.TextLabel.Lines = 0;
					cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
					cell.TextLabel.SizeToFit ();
					cell.TextLabel.TextAlignment = UITextAlignment.Center;
					cell.TextLabel.Text = _copyrightInfo;
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				}
				return cell;
			}

			/// <summary>
			/// 创建空白的cell
			/// </summary>
			/// <returns>The space cell.</returns>
			/// <param name="tableView">Table view.</param>
			private UITableViewCell CreateSpaceCell (UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellsSpace";
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);

				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				}
				return cell;
			}
		}
	}
}


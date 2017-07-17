using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using BigTed;
using RekTec.Chat.Common;

namespace RekTec.Chat.UI
{
    public class LoginViewController : ChatBaseViewController
    {
        private UITableView _tableView;

        public LoginViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "登录";
            _tableView = new UITableView(new RectangleF(0, 44, View.Frame.Width, 250));
            _tableView.Source = new Souce(this);
            _tableView.BackgroundColor = UIColor.Clear;

            Add(_tableView);
        }

        public class Souce : UITableViewSource
        {
            private UITextField _txtUserName;
            private UITextField _txtPassword;

            private LoginViewController _c;

            public Souce(LoginViewController c)
            {
                _c = c;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Row == 2)
                {
                    if (string.IsNullOrWhiteSpace(_txtUserName.Text) || string.IsNullOrWhiteSpace(_txtPassword.Text))
                        return;
                        
                    try
                    {
                        AlertUtil.ShowWaiting("正在登录...");
                        ChatClient.Connect(_txtUserName.Text, _txtPassword.Text)
                        .ContinueWith((t) =>
                            {
                                _c.InvokeOnMainThread(() =>
                                    {
                                        AlertUtil.DismissWaiting();
                                    });
                                if (t.Result)
                                {
                                    _c.InvokeOnMainThread(() =>
                                        {
                                            try
                                            {
                                                ChatAppSetting.UserCode = _txtUserName.Text;
                                                ChatAppSetting.Password = _txtPassword.Text;
                                                _c.NavigationController.PopToRootViewController(false);
                                            }
                                            catch (System.Exception ex2)
                                            {
                                                LoggingUtil.Exception(ex2);
                                                AlertUtil.Error(ex2.Message);
                                            }
                                        });
                                }
                            });
                    }
                    catch (System.Exception ex)
                    {
                        LoggingUtil.Exception(ex);
                        AlertUtil.Error(ex.Message);
                    }
                }
            }

            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 44;
            }

            public override int RowsInSection(UITableView tableView, int section)
            {
                if (section == 0)
                    return 3;

                return 3;
            }

            public override int NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Row == 0)
                    return CreateUserNameCell(tableView);
                else if (indexPath.Row == 1)
                    return CreatePasswordCell(tableView);
                else if (indexPath.Row == 2)
                    return CreateLoginButtonCell(tableView);
                else
                    return null;
            }

            private UITableViewCell CreateUserNameCell(UITableView tableView)
            {
                var cellIdentifier = "TableViewCellUserName";
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                    var lbl = new UILabel(new RectangleF(15, 2, 80, 40))
                    {
                        Text = "用户：",
                        BackgroundColor = UIColor.White
                    };

                    _txtUserName = new UITextField(new RectangleF(60, 2, 320, 40))
                    {
                        Font = UIFont.SystemFontOfSize(16F),
                        Placeholder = "请输入用户账号"
                    };
                    _txtUserName.Text = ChatAppSetting.UserCode;

                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    cell.ContentView.AddSubview(lbl);
                    cell.ContentView.AddSubview(_txtUserName);
                }

                return cell;
            }

            private UITableViewCell CreatePasswordCell(UITableView tableView)
            {
                var cellIdentifier = "TableViewCellUserName";
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                    var lbl = new UILabel(new RectangleF(15, 2, 80, 40))
                    {
                        Text = "密码：",
                        BackgroundColor = UIColor.White
                    };

                    _txtPassword = new UITextField(new RectangleF(60, 2, 320, 40))
                    {
                        Font = UIFont.SystemFontOfSize(16F),
                        Placeholder = "请输入密码",
                        SecureTextEntry = true
                    };
                    _txtPassword.Text = ChatAppSetting.Password;

                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    cell.ContentView.AddSubview(lbl);
                    cell.ContentView.AddSubview(_txtPassword);
                }
                return cell;
            }


            private UITableViewCell CreateLoginButtonCell(UITableView tableView)
            {
                var cellIdentifier = "TableViewCellUserName";
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                    cell.TextLabel.Text = "登录";
                }
                return cell;
            }
        }
    }
}


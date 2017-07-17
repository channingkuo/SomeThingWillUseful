using System;
using System.Threading.Tasks;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using RekTec.Chat.DataRepository;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.Service
{
    internal class ChatConnectionService
    {
        private readonly XmppClientConnection _xmppConnection;

        private TaskCompletionSource<bool> _taskWaitor;

        internal ChatConnectionService(XmppClientConnection conn)
        {
            _xmppConnection = conn;
            _xmppConnection.Server = ChatAppSetting.ChatServerAddress;
            _xmppConnection.AutoAgents = false;
            _xmppConnection.Resource = "iPhone";

            _xmppConnection.OnSocketError += (sender, ex) => {
                TryCompleteTask(false);
                ErrorHandlerUtil.ReportException(ex);
            };

            _xmppConnection.OnStreamError += (sender, e) => {
                TryCompleteTask(false);
                ErrorHandlerUtil.ReportError(e.ToString());
            };

            _xmppConnection.OnError += (sender, ex) => {
                TryCompleteTask(false);
                ErrorHandlerUtil.ReportException(ex);
            };

            _xmppConnection.OnAuthError += (sender, e) => {
                TryCompleteTask(false);
                ErrorHandlerUtil.ReportError("用户账号或者密码错误！");
            };
                    
            _xmppConnection.OnSaslStart += (sender, args) => {
                args.Auto = false;
                args.Mechanism = Mechanism.GetMechanismName(MechanismType.PLAIN);
            };
                    
            _xmppConnection.OnLogin += sender => {
                try {
                    _xmppConnection.KeepAlive = true;
                    TryCompleteTask(true);
                } catch (Exception ex) {
                    TryCompleteTask(false);
                    ErrorHandlerUtil.ReportException(ex);
                }
            };
        }

        private void TryCompleteTask(bool isSuccess)
        {
            if (_taskWaitor != null) {
                _taskWaitor.TrySetResult(isSuccess);
                _taskWaitor = null;
            }
        }

        internal Task<bool> OpenConnection()
        {
            try {
                TryCompleteTask(false);
                _taskWaitor = new TaskCompletionSource<bool>();
                _xmppConnection.Open(ChatAppSetting.UserCode, ChatAppSetting.Password);

                if (_taskWaitor != null) {
                    return _taskWaitor.Task;
                } else {
                    return Task.Run(() => false);
                }
            } catch (Exception ex) {
                ErrorHandlerUtil.ReportException(ex);
                return Task.Run(() => false);
            }
        }

        internal async void CloseConnection()
        {
            _xmppConnection.KeepAlive = false;
            await _xmppConnection.CloseAsync();
        }
    }
}


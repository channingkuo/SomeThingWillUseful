using agsXMPP;
using agsXMPP.protocol.client;
using RekTec.Chat.DataRepository;
using RekTec.Chat.XmppElement;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.Service
{
    internal class ChatNotificationService
    {
        private readonly XmppClientConnection _xmppConnection;

        internal ChatNotificationService(XmppClientConnection conn)
        {
            _xmppConnection = conn;
        }

        internal void SetApnsDeviceToken()
        {
            if (string.IsNullOrWhiteSpace(ChatAppSetting.DeviceToken))
                return;

            var token = ChatAppSetting.DeviceToken.Replace(" ", "");
            if (token.Length < 66)
                return;
            token = token.Substring(1, 64);

            _xmppConnection.IqGrabber.SendIq(new ApnsIq(IqType.set, token), (sender, iq, data) => {
                if (iq.Type == IqType.error) {
                    ErrorHandlerUtil.ReportError(iq.Error.ToString());
                    return;
                }
            }, null, false);
        }

        internal void UnSetApnsDeviceToken()
        {
            var token = string.Empty;
            _xmppConnection.IqGrabber.SendIq(new ApnsIq(IqType.set, token), (sender, iq, data) => {
                if (iq.Type == IqType.error) {
                    ErrorHandlerUtil.ReportError(iq.Error.ToString());
                    return;
                }
            }, null, false);
        }
    }
}


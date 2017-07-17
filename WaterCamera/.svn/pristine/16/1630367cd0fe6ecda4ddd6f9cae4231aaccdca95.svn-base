using System;

namespace agsXMPP.net
{
    public abstract class ClientSocketException:ApplicationException
    {
        public  ClientSocketException(string msg, Exception ex)
            : base(msg, ex)
        {

        }
    }

    public class ClientSocketConnectionException:ClientSocketException
    {
        public ClientSocketConnectionException(string msg, Exception ex)
            : base(msg, ex)
        {

           
           
        }

        public static ClientSocketConnectionException Create(Exception ex)
        {
            var msg = "[网络]连接异常！";
            if (ex != null)
                msg = msg + ex.Message;

            return new ClientSocketConnectionException(msg, ex);
        }
    }

    public class ClientSocketConnectionTimeoutException:ClientSocketException
    {
        public ClientSocketConnectionTimeoutException(string msg, Exception ex)
            : base(msg, ex)
        {
           
        }

        public static ClientSocketConnectionTimeoutException Create(Exception ex)
        {
            var msg = "[网络]连接超时异常！";
            if (ex != null)
                msg = msg + ex.Message;

            return new ClientSocketConnectionTimeoutException(msg, ex);
        }
    }

    public class ClientSocketSendException:ClientSocketException
    {
        public ClientSocketSendException(string msg, Exception ex)
            : base(msg, ex)
        {
           
        }

        public static ClientSocketSendException Create(Exception ex)
        {
            var msg = "[网络]发送数据异常！";
            if (ex != null)
                msg = msg + ex.Message;

            return new ClientSocketSendException(msg, ex);
        }
    }


    public class ClientSocketReceiveException:ClientSocketException
    {
        public ClientSocketReceiveException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        public static ClientSocketReceiveException Create(Exception ex)
        {
            var msg = "[网络]接收数据异常！";
            if (ex != null)
                msg = msg + ex.Message;

            return new ClientSocketReceiveException(msg, ex);
        }
    }
}


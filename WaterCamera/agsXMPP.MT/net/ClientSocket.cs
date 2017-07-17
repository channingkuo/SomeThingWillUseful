/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2008 by AG-Software 											 *
 * All Rights Reserved.																 *
 * Contact information for AG-Software is available at http://www.ag-software.de	 *
 *																					 *
 * Licence:																			 *
 * The agsXMPP SDK is released under a dual licence									 *
 * agsXMPP can be used under either of two licences									 *
 * 																					 *
 * A commercial licence which is probably the most appropriate for commercial 		 *
 * corporate use and closed source projects. 										 *
 *																					 *
 * The GNU Public License (GPL) is probably most appropriate for inclusion in		 *
 * other open source projects.														 *
 *																					 *
 * See README.html for details.														 *
 *																					 *
 * For general enquiries visit our website at:										 *
 * http://www.ag-software.de														 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using agsXMPP.IO.Compression;
using System.Threading.Tasks;

namespace agsXMPP.net
{
    public class ClientSocket:IDisposable
    {

        public delegate void OnSocketDataHandler(object sender,byte[] data,int count);

        public delegate void OnSocketEndSendHandler(object id,bool isComplete);

        #region Event

        public event ObjectHandler OnConnect;

        protected void FireOnConnect()
        {
            if (OnConnect != null)
                OnConnect(this);
        }

        public event OnSocketDataHandler OnReceive;

        protected void FireOnReceive(byte[] b, int length)
        {
            if (OnReceive != null)
                OnReceive(this, b, length);
        }

        public event OnSocketDataHandler OnSend;

        protected void FireOnSend(byte[] b, int length)
        {
            if (OnSend != null)
                OnSend(this, b, length);
        }

        public event OnSocketEndSendHandler OnEndSend;

        protected void FireOnEndSend(object id, bool isComplete)
        {
            if (OnEndSend != null)
                OnEndSend(id, isComplete);
        }

        public event ErrorHandler OnSocketError;

        protected void FireOnSocketError(Exception ex)
        {
            if (OnSocketError != null)
                OnSocketError(this, ex);
        }

        #endregion

        #region 地址和端口号的属性

        private string m_Address = null;
        private int m_Port = 0;

        public string Address {
            get { return m_Address; }
            set { m_Address = value; }
        }

        public int Port {
            get { return m_Port; }
            set { m_Port = value; }
        }

        #endregion

        #region Timout的属性

        private long m_ConnectTimeout = 10000;

        public virtual long ConnectTimeout {
            get { return m_ConnectTimeout; }
            set { m_ConnectTimeout = value; }
        }

        private bool m_ConnectTimedOut = false;
        private Timer connectTimeoutTimer;

        #endregion

        #region Socket Stream 属性

        Socket _socket = null;
        NetworkStream _networkStream = null;
        const   int BUFFERSIZE = 1024;
        private byte[] _readBuffer = null;

        #endregion

        #region SSL 和 压缩相关 属性

        private bool m_SSL = false;

        public bool SSL {
            get { return m_SSL; }
        }

        public bool SupportsStartTls {
            get {
                return false;
            }
        }

        private bool m_Compressed = false;
        private Deflater deflater = null;
        private Inflater inflater = null;

        public bool Compressed {
            get { return m_Compressed; }
            set { m_Compressed = value; }
        }

        #endregion

        #region 连接相关的方法

        public bool Connected {
            get {
                if (_socket == null)
                    return false;
                
                return _socket.Connected;
            }
        }

        public void Connect(string address, int port)
        {
            Address = address;
            Port = port;

            Connect();
        }

        public void Connect()
        {
            _isClosing = false;

            m_Compressed = false;
			
            _readBuffer	= null;
            _readBuffer	= new byte[BUFFERSIZE];
            
            try {
                IPAddress ipAddress = null;
                if (!IPAddress.TryParse(Address, out ipAddress)) {
                    var ipHostInfo = Dns.GetHostEntry(Address);
                    ipAddress = ipHostInfo.AddressList[0];
                }
      
                IPEndPoint endPoint = new IPEndPoint(ipAddress, Port);
               
                m_ConnectTimedOut = false;
                TimerCallback timerDelegate = new TimerCallback(ConnectionTimeoutTimerCallback);
                connectTimeoutTimer = new Timer(timerDelegate, null, ConnectTimeout, ConnectTimeout);

                if (Socket.OSSupportsIPv6 && (endPoint.AddressFamily == AddressFamily.InterNetworkV6)) {
                    _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);                   
                } else {
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                    
                _socket.BeginConnect(endPoint, new AsyncCallback(EndConnectAsyncCallback), null);
            } catch (Exception ex) {
                FireOnSocketError(ClientSocketConnectionException.Create(ex));
            }			
        }

        private void EndConnectAsyncCallback(IAsyncResult ar)
        {
            if (m_ConnectTimedOut) {
                return;
            } 

            if (_socket == null)
                return;

            try { 
                // stop the timeout timer
                connectTimeoutTimer.Dispose();

                _socket.EndConnect(ar);

                _networkStream = new NetworkStream(_socket, false);                  

                FireOnConnect();

                this.Receive();
            } catch (Exception ex) {
                FireOnSocketError(ClientSocketConnectionException.Create(ex));
            }
        }

        bool _isClosing = false;

        public void Dispose()
        {
            _isClosing = true;
            #region stream Dispose

            if (_networkStream != null) {
                try {
                    _networkStream.Close();
                } catch {
                }

                try {
                    _networkStream.Dispose();
                } catch {
                }

                _networkStream = null;
            }
            #endregion

            #region Socket Dispose
            if (_socket != null) {

                try {
                    // first, shutdown the socket
                    _socket.Shutdown(SocketShutdown.Both);
                } catch {
                }

                try {
                    // next, close the socket which terminates any pending
                    // async operations
                    _socket.Close();
                } catch {
                }

                try {
                    _socket.Dispose();
                } catch {
                }

                _socket = null;
            }
            #endregion
        }

        private void ConnectionTimeoutTimerCallback(Object stateInfo)
        {
            connectTimeoutTimer.Dispose();
            m_ConnectTimedOut = true;
            Dispose();
            FireOnSocketError(ClientSocketConnectionTimeoutException.Create(null));
        }

        #endregion

        #region 压缩相关的方法

        /// <summary>
        /// Start Compression on the socket
        /// </summary>
        public void StartCompression()
        {
            InitCompression();
        }

        /// <summary>
        /// Initialize compression stuff (Inflater, Deflater)
        /// </summary>
        private void InitCompression()
        {
            StartCompression();

            inflater = new Inflater();
            deflater = new Deflater();
            
            // Set the compressed flag to true when we init compression
            m_Compressed = true;
        }


        /// <summary>
        /// Compress bytes
        /// </summary>
        /// <param name="bIn"></param>
        /// <returns></returns>
        private byte[] Compress(byte[] bIn)
        {                       
            int ret;

            // The Flush SHOULD be after each STANZA
            // The libds sends always one complete XML Element/stanza,
            // it doesn't cache stanza and send them in groups, and also doesnt send partial
            // stanzas. So everything should be ok here.
            deflater.SetInput(bIn);
            deflater.Flush();

            MemoryStream ms = new MemoryStream();
            do {
                byte[] buf = new byte[BUFFERSIZE];
                ret = deflater.Deflate(buf);                
                if (ret > 0)
                    ms.Write(buf, 0, ret);                

            } while (ret > 0);

            return ms.ToArray();

        }

        /// <summary>
        /// Decompress bytes
        /// </summary>
        /// <param name="bIn"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] Decompress(byte[] bIn, int length)
        {
            int ret;

            inflater.SetInput(bIn, 0, length);

            MemoryStream ms = new MemoryStream();    
            do {
                byte[] buf = new byte[BUFFERSIZE];
                ret = inflater.Inflate(buf);
                if (ret > 0)
                    ms.Write(buf, 0, ret);

            } while (ret > 0);           

            return ms.ToArray();
        }

        #endregion

        #region 数据发送相关方法

        public void Send(string data, object id)
        {			    
            Send(Encoding.UTF8.GetBytes(data), id);				
        }

        public void Send(byte[] bData, object id)
        {    
            if (_isClosing)
                return;

            if (bData == null) {
                return;
            }

            if (_networkStream == null) {
                FireOnEndSend(id, false);
                Dispose();
                FireOnSocketError(ClientSocketConnectionException.Create(null));
                return;
            }

            lock (this) {
                try {
                    FireOnSend(bData, bData.Length);

                    if (m_Compressed) {
                        byte[] tmpData = new byte[bData.Length];
                        bData.CopyTo(tmpData, 0);
                        bData = Compress(bData);
                    }

                    _networkStream.WriteAsync(bData, 0, bData.Length)
                                .ContinueWith((t) => {
                        if (t.IsFaulted) {
                            FireOnEndSend(id, false);
                            if (!_isClosing) {
                                Dispose();
                                FireOnSocketError(ClientSocketSendException.Create(t.Exception));
                            }
                        } else {
                            FireOnEndSend(id, true);
                        }
                    }); 
                } catch (Exception ex) {
                    FireOnEndSend(id, false);
                    if (!_isClosing) {
                        Dispose();
                        FireOnSocketError(ClientSocketSendException.Create(ex));
                    }
                }
            }
        }

        public async Task SendAsync(string data, object id)
        {               
            await SendAsync(Encoding.UTF8.GetBytes(data), id);             
        }

        public async Task SendAsync(byte[] bData, object id)
        {    
            if (_isClosing)
                return;

            if (bData == null) {
                return;
            }

            if (_networkStream == null) {
                FireOnEndSend(id, false);
                Dispose();
                FireOnSocketError(ClientSocketConnectionException.Create(null));
                return;
            }
                
            try {
                FireOnSend(bData, bData.Length);

                if (m_Compressed) {
                    byte[] tmpData = new byte[bData.Length];
                    bData.CopyTo(tmpData, 0);
                    bData = Compress(bData);
                }

                await _networkStream.WriteAsync(bData, 0, bData.Length);
                FireOnEndSend(id, true);
            } catch (Exception ex) {
                FireOnEndSend(id, false);
                if (!_isClosing) {
                    Dispose();
                    FireOnSocketError(ClientSocketSendException.Create(ex));
                }
            }
        }

        #endregion

        #region 数据接收相关的方法

        /// <summary>
        /// Read data from server.
        /// </summary>
        private void Receive()
        {		
            if (_isClosing)
                return;

            if (_socket == null)
                return;

            if (_networkStream == null) {
                Dispose();
            }

            try {
                _networkStream.BeginRead(_readBuffer, 0, BUFFERSIZE, new AsyncCallback(EndReceive), null);
            } catch (Exception ex) {
                Dispose();
                FireOnSocketError(ClientSocketReceiveException.Create(ex));
            }
        }

        private void EndReceive(IAsyncResult ar)
        {
            if (_isClosing)
                return;

            if (_socket == null)
                return;

            if (_networkStream == null) {
                Dispose();
            }

            try {
                int nBytes;
                nBytes = _networkStream.EndRead(ar);
                if (nBytes > 0) {
                    // uncompress Data if we are on a compressed socket
                    if (m_Compressed) {                        
                        byte[] buf = Decompress(_readBuffer, nBytes);
                        FireOnReceive(buf, buf.Length);
                     
                    } else {
                        FireOnReceive(_readBuffer, nBytes);
                    }
                    // Setup next Receive Callback
                    if (this.Connected)
                        this.Receive();
                  
                } else {
                    Dispose();
                    FireOnSocketError(ClientSocketReceiveException.Create(null));
                }
            } catch (System.Exception ex) {
                Dispose();
                FireOnSocketError(ClientSocketReceiveException.Create(ex));
            }
        }

        #endregion
    }
}
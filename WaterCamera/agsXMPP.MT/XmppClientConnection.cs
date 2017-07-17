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
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

using agsXMPP.protocol;
using agsXMPP.protocol.iq;
using agsXMPP.protocol.iq.auth;
using agsXMPP.protocol.iq.agent;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.register;
using agsXMPP.protocol.iq.version;
using agsXMPP.protocol.stream;
using agsXMPP.protocol.stream.feature.compression;
using agsXMPP.protocol.client;
using agsXMPP.protocol.tls;

using agsXMPP.protocol.extensions.caps;
using agsXMPP.protocol.extensions.compression;

using agsXMPP.exceptions;

using agsXMPP.sasl;
using agsXMPP.net;
using agsXMPP.net.dns;

using agsXMPP.Idn;
using System.Threading.Tasks;

namespace agsXMPP
{
    public delegate void XmlHandler(object sender,string xml);
    public delegate void ErrorHandler(object sender,Exception ex);
    public delegate void XmppConnectionStateHandler(object sender,XmppConnectionState state);
    public delegate void ObjectHandler(object sender);
    public delegate void XmppElementHandler(object sender,Element e);
    public delegate void EndSendHandler(string id,bool isComplete);

    /// <summary>
    /// Summary description for XmppClient.
    /// </summary>
    public class XmppClientConnection
    {
        private Timer m_KeepaliveTimer = null;

        #region << Events >>

        public event XmlHandler OnReadXml;
        public event XmlHandler OnWriteXml;

        public event ErrorHandler OnError;
   
        public event ErrorHandler OnSocketError;

        public event ObjectHandler OnLogin;
        public event SaslEventHandler OnSaslStart;
        public event ObjectHandler OnSaslEnd;
        public event XmppElementHandler OnAuthError;

        public event XmppElementHandler OnStreamError;
        public event IqHandler OnIq;

        public event MessageHandler OnMessage;
        public event EndSendHandler OnEndSend;

        #endregion

        #region Event never used by joesong

        /// <summary>
        /// This event just informs about the current state of the XmppConnection
        /// </summary>
        public event XmppConnectionStateHandler OnXmppConnectionStateChanged;
        /// <summary>
        /// We received a presence from a contact or chatroom.
        /// Also subscriptions is handles in this event.
        /// </summary>

        public event PresenceHandler OnPresence;
        /// <summary>
        /// This event is raised when a response to a roster query is received. The roster query contains the contact list.
        /// This lost could be very large and could contain hundreds of contacts. The are all send in a single XML element from 
        /// the server. Normally you show the contact list in a GUI control in you application (treeview, listview). 
        /// When this event occurs you couls Suspend the GUI for faster drawing and show change the mousepointer to the hourglass
        /// </summary>
        /// <remarks>see also OnRosterItem and OnRosterEnd</remarks>
        public event ObjectHandler OnRosterStart;

        /// <summary>
        /// This event is raised when a response to a roster query is received. It notifies you that all RosterItems (contacts) are
        /// received now.
        /// When this event occurs you could Resume the GUI and show the normal mousepointer again.
        /// </summary>
        /// <remarks>see also OnRosterStart and OnRosterItem</remarks>
        public event ObjectHandler OnRosterEnd;

        /// <summary>
        /// This event is raised when a response to a roster query is received. This event always contains a single RosterItem. 
        /// e.g. you have 150 friends on your contact list, then this event is called 150 times.
        /// </summary>
        /// <remarks>see also OnRosterItem and OnRosterEnd</remarks>
        public event RosterHandler OnRosterItem;

        /// <summary>
        /// This event is raised when a response to an agents query which could contain multiple agentitems.
        /// Normally you show the items in a GUI. This event could be used to suspend the UI for faster drawing.
        /// </summary>
        /// <remarks>see also OnAgentItem and OnAgentEnd</remarks>
        public event ObjectHandler OnAgentStart;

        /// <summary>
        /// This event is raised when a response to an agents query which could contain multiple agentitems.
        /// Normally you show the items in a GUI. This event could be used to resume the suspended userinterface.
        /// </summary>
        /// <remarks>see also OnAgentStart and OnAgentItem</remarks>
        public event ObjectHandler OnAgentEnd;

        /// <summary>
        /// This event returns always a single AgentItem from a agents query result.
        /// This is from the old jabber protocol. Instead of agents Disco (Service Discovery) should be used in modern
        /// application. But still lots of servers use Agents.
        /// <seealso cref=""/>
        /// </summary>
        /// <remarks>see also OnAgentStart and OnAgentEnd</remarks>
        public event AgentHandler OnAgentItem;
        public event ObjectHandler OnBinded;
        public event RegisterEventHandler OnRegisterInformation;
        public event ObjectHandler OnRegistered;

        public event ObjectHandler OnPasswordChanged;
        public event XmppElementHandler OnRegisterError;

        #endregion

        #region << Properties and Member Variables >>

        private     int m_Port = 5222;
        private     string m_Server = null;
        private     string m_ConnectServer = null;
        private     string m_StreamId = "";
        private     string m_StreamVersion = "1.0";
        private     XmppConnectionState m_ConnectionState = XmppConnectionState.Disconnected;
        private     ClientSocket m_ClientSocket = null;
        private     StreamParser m_StreamParser = null;
        private     SocketConnectionType m_SocketConnectionType = SocketConnectionType.Direct;
        private     bool m_AutoResolveConnectServer = false;
        private     int m_KeepAliveInterval = 120;
        private     bool m_KeepAlive = true;

        /// <summary>
        /// The Port of the remote server for the connection
        /// </summary>
        public int Port {
            get { return m_Port; }
            set { m_Port = value; }
        }

        /// <summary>
        /// domain or ip-address of the remote server for the connection
        /// </summary>
        public string Server {
            get { return m_Server; }
            set {
                #if !STRINGPREP
                if (value != null)
                    m_Server = value.ToLower();
                else
                    m_Server = null;
                #else
                if (value != null)
                m_Server = Stringprep.NamePrep(value);
                else
                m_Server = null;
                #endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectServer {
            get { return m_ConnectServer; }
            set { m_ConnectServer = value; }        
        }

        /// <summary>
        /// the id of the current xmpp xml-stream
        /// </summary>
        public string StreamId {
            get { return m_StreamId; }  
            set { m_StreamId = value; }
        }

        /// <summary>
        /// Set to null for old Jabber Protocol without SASL andstream features
        /// </summary>
        public string StreamVersion {
            get { return m_StreamVersion; }
            set { m_StreamVersion = value; }
        }

        public XmppConnectionState XmppConnectionState {
            get { return m_ConnectionState; }
        }

        /// <summary>
        /// Read Online Property ClientSocket
        /// returns the SOcket object used for this connection
        /// </summary>
        public ClientSocket Socket {
            get { return m_ClientSocket; }
            set { m_ClientSocket = value; }
        }

        /// <summary>
        /// the underlaying XMPP StreamParser. Normally you don't need it, but we make it accessible for
        /// low level access to the stream
        /// </summary>
        public StreamParser StreamParser {
            get { return m_StreamParser; }
        }

        public SocketConnectionType SocketConnectionType {
            get { return m_SocketConnectionType; }
            set {
                m_SocketConnectionType = value;
                InitSocket();
            }
        }

        public bool AutoResolveConnectServer {
            get { return m_AutoResolveConnectServer; }
            set { m_AutoResolveConnectServer = value; }
        }

        /// <summary>
        /// <para>
        /// the keep alive interval in seconds.
        /// Default value is 120
        /// </para>
        /// <para>
        /// Keep alive packets prevent disconenct on NAT and broadband connections which often
        /// disconnect if they are idle.
        /// </para>
        /// </summary>
        public int KeepAliveInterval {
            get {
                return m_KeepAliveInterval;
            }
            set {
                m_KeepAliveInterval = value;
            }
        }

        /// <summary>
        /// Send Keep Alives (for NAT)
        /// </summary>
        public bool KeepAlive {
            get {
                return m_KeepAlive;
            }
            set {
                m_KeepAlive = value;
            }
        }

        const string SRV_RECORD_PREFIX = "_xmpp-client._tcp.";

        public delegate void RosterHandler(object sender,RosterItem item);

        public delegate void AgentHandler(object sender,Agent agent);

        private SaslHandler m_SaslHandler = null;

        private bool m_CleanUpDone;

        private SRVRecord[] _SRVRecords;
        private SRVRecord _currentSRVRecord;


        private     string m_ClientLanguage = "zh-CN";
        private     string m_ServerLanguage = null;
        private     string m_Username = "";
        private     string m_Password = "";
        private     string m_Resource = "agsXMPP";
        private     string m_Status = "";
        private     int m_Priority = 5;
        private     ShowType m_Show = ShowType.NONE;
        private     bool m_AutoRoster = true;
        private     bool m_AutoAgents = true;
        private     bool m_AutoPresence = true;

        private     bool m_UseSSL = false;
        private     bool m_UseStartTLS = true;
        private     bool m_UseCompression = false;
        internal    bool m_Binded = false;
        private     bool m_Authenticated = false;

        private     IqGrabber m_IqGrabber = null;
        private     MessageGrabber m_MessageGrabber = null;
        private     PresenceGrabber m_PresenceGrabber = null;
        private     bool m_RegisterAccount = false;
        private     PresenceManager m_PresenceManager;
        private     RosterManager m_RosterManager;


        private     Capabilities m_Capabilities = new Capabilities();
        private     string m_ClientVersion = "1.0";
        private     bool m_EnableCapabilities = false;

        private     DiscoInfo m_DiscoInfo = new DiscoInfo();


        /// <summary>
        /// The prefered Client Language Attribute
        /// </summary>
        /// <seealso cref="agsXMPP.protocol.Base.XmppPacket.Language"/>
        public string ClientLanguage {
            get { return m_ClientLanguage; }
            set { m_ClientLanguage = value; }
        }

        /// <summary>
        /// The language which the server decided to use.
        /// </summary>
        /// <seealso cref="agsXMPP.protocol.Base.XmppPacket.Language"/>
        public string ServerLanguage {
            get { return m_ServerLanguage; }            
        }

        /// <summary>
        /// the username that is used to authenticate to the xmpp server
        /// </summary>
        public string Username {
            get { return m_Username; }
            set {
                // first Encode the user/node
                m_Username = value;

                string tmpUser = Jid.EscapeNode(value);

                if (value != null)
                    m_Username = tmpUser.ToLower();
                else
                    m_Username = null;


            }                
        }

        /// <summary>
        /// the password that is used to authenticate to the xmpp server
        /// </summary>
        public string Password {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// the resource for this connection each connection to the server with the same jid needs a unique resource.
        /// You can also set <code>Resource = null</code> and the server will assign a random Resource for you.
        /// </summary>
        public string Resource {
            get { return m_Resource; }
            set { m_Resource = value; }
        }

        /// <summary>
        /// our XMPP id build from Username, Server and Resource Property (user@server/resourcee)
        /// </summary>
        public Jid MyJID {
            get { 
                return BuildMyJid();               
            }
        }

        /// <summary>
        /// The status message of this connection which is sent with the presence packets.
        /// </summary>
        /// <remarks>
        /// you have to call the method <b>SendMyPresence</b> to send your updated presence to the server.        
        /// </remarks>
        public string Status {
            get {
                return m_Status;
            }
            set {
                m_Status = value;
            }
        }

        /// <summary>
        /// The priority of this connection send with the presence packets.
        /// The OPTIONAL priority element contains non-human-readable XML character data that specifies the priority level 
        /// of the resource. The value MUST be an integer between -128 and +127. If no priority is provided, a server 
        /// SHOULD consider the priority to be zero.        
        /// </summary>
        /// <remarks>you have to call the method <b>SendMyPresence</b> to send your updated presence to the server.</remarks>
        public int Priority {
            get { return m_Priority; }
            set {
                if (value > -128 && value < 128)
                    m_Priority = value;
                else
                    throw new ArgumentException("The value MUST be an integer between -128 and +127");
            }
        }

        /// <summary>
        /// change the showtype. 
        /// </summary>
        /// <remarks>you have to call the method <b>SendMyPresence</b> to send your updated presence to the server.</remarks>
        public ShowType Show {
            get { return m_Show; }
            set { m_Show = value; }
        }

        /// <summary>
        /// If set to true then the Roster (contact list) is requested automatically after sucessful login. 
        /// Set this property to false if you don't want to receive your contact list, or request it manual. 
        /// To save bandwidth is makes sense to cache the contact list and don't receive it on each login.
        /// </summary>
        /// <remarks>default value is <b>true</b></remarks>
        public bool AutoRoster {
            get { return m_AutoRoster; }
            set { m_AutoRoster = value; }
        }

        /// <summary>
        /// Sends the presence Automatically after successful login.
        /// This property works only in combination with AutoRoster (AutoRoster = true).
        /// </summary>
        public bool AutoPresence {
            get { return m_AutoPresence; }
            set { m_AutoPresence = value; }
        }



        /// <summary>
        /// If set to true then the Agents are requested automatically after sucessful login. 
        /// Set this property to false if you don't use agents at all, or if you request them manual.
        /// </summary>
        /// <remarks>default value is <b>true</b></remarks>
        public bool AutoAgents {
            get { return m_AutoAgents; }
            set { m_AutoAgents = value; }
        }

        /// <summary>
        /// use "old style" ssl for this connection (Port 5223).
        /// </summary>
        public bool UseSSL {
            get { return m_UseSSL; }
        }

        /// <summary>
        /// use Start-TLS on this connection when the server supports it. Make sure UseSSL is false when 
        /// you want to use this feature.
        /// </summary>
        public bool UseStartTLS {
            get { return m_UseStartTLS; }
        }

        /// <summary>
        /// Use Stream compression to save bandwidth?
        /// This should not be used in combination with StartTLS,
        /// because TLS has build in compression (see RFC 2246, http://www.ietf.org/rfc/rfc2246.txt)
        /// </summary>
        public bool UseCompression {
            get { return m_UseCompression; }
            set { m_UseCompression = value; }
        }

        /// <summary>
        /// Are we Authenticated to the server? This is readonly and set by the library
        /// </summary>
        public bool Authenticated {
            get { return m_Authenticated; }             
        }

        /// <summary>
        /// is the resource binded? This is readonly and set by the library
        /// </summary>
        public bool Binded {
            get { return m_Binded; }    
            set { m_Binded = value; }
        }

        /// <summary>
        /// Should the library register a new account on the server
        /// </summary>
        public bool RegisterAccount {
            get { return m_RegisterAccount; }
            set { m_RegisterAccount = value; }
        }

        public IqGrabber IqGrabber {
            get { return m_IqGrabber; }
        }

        public MessageGrabber MessageGrabber {
            get { return m_MessageGrabber; }
        }

        public PresenceGrabber PresenceGrabber {
            get { return m_PresenceGrabber; }
        }

        public RosterManager RosterManager {
            get { return m_RosterManager; }
        }

        public PresenceManager PresenceManager {
            get { return m_PresenceManager; }
        }

        public bool EnableCapabilities {
            get { return m_EnableCapabilities; }
            set { m_EnableCapabilities = value; }
        }

        public string ClientVersion {
            get { return m_ClientVersion; }
            set { m_ClientVersion = value; }
        }

        public Capabilities Capabilities {
            get { return m_Capabilities; }
            set { m_Capabilities = value; }
        }

        /// <summary>
        /// The DiscoInfo object is used to respond to DiscoInfo request if AutoAnswerDiscoInfoRequests == true in DisoManager objects,
        /// it's also used to build the Caps version when EnableCapabilities is set to true.
        /// <remarks>
        /// When EnableCapailities == true call UpdateCapsVersion after each update of the DiscoInfo object
        /// </remarks>
        /// </summary>
        public DiscoInfo DiscoInfo {
            get { return m_DiscoInfo; }
            set { m_DiscoInfo = value; }
        }

        #endregion

        #region Log && Error

        protected void FireOnReadXml(object sender, string xml)
        {
            if (OnReadXml != null)
                OnReadXml(sender, xml);
        }

        protected void FireOnWriteXml(object sender, string xml)
        {
            if (OnWriteXml != null)
                OnWriteXml(sender, xml);
        }

        protected void FireOnError(object sender, Exception ex)
        {
            if (OnError != null)
                OnError(sender, ex);
        }

        #endregion

        #region << Socket handers >>

        private void InitSocket()
        {
            m_ClientSocket = new ClientSocket();
            m_ClientSocket.OnConnect += new ObjectHandler(SocketOnConnect);
            m_ClientSocket.OnReceive += new ClientSocket.OnSocketDataHandler(SocketOnReceive);
            m_ClientSocket.OnSocketError += new ErrorHandler(SocketOnError);
            m_ClientSocket.OnEndSend += new ClientSocket.OnSocketEndSendHandler(SocketOnSendEnd);
        }

        public void SocketOnReceive(object sender, byte[] data, int count)
        {
            // put the received bytes to the parser
            lock (this) {
                StreamParser.Push(data, 0, count);
            }
        }

        /// <summary>
        /// Starts connecting of the socket
        /// </summary>
        public void SocketConnect()
        {
            DoChangeXmppConnectionState(XmppConnectionState.Connecting);
            Socket.Connect();
        }

        public void SocketConnect(string server, int port)
        {   
            Socket.Address = server;
            Socket.Port = port;
            SocketConnect();                
        }

        public void SocketOnConnect(object sender)
        {
            DoChangeXmppConnectionState(XmppConnectionState.Connected);

            SendStreamHeader(true);
        }

        public void SocketOnError(object sender, Exception ex)
        {
            if (OnSocketError != null)
                OnSocketError(this, ex);

            // Only cleaneUp Session and raise on close if the stream already has started
            // if teh stream gets closed because of a socket error we have to raise both errors fo course
            if (!m_CleanUpDone)
                CleanupSession();    
        }

        public void SocketOnSendEnd(object id, bool isComplete)
        {
            if (OnEndSend != null && id != null && id is string) {
                var msgId = id as string;
                OnEndSend(msgId, isComplete);
            }
        }

        internal void DoChangeXmppConnectionState(XmppConnectionState state)
        {
            m_ConnectionState = state;

            if (OnXmppConnectionStateChanged != null)
                OnXmppConnectionStateChanged(this, state);
        }

        private void OpenSocket()
        {
            if (ConnectServer == null)
                SocketConnect(Server, Port);
            else
                SocketConnect(this.ConnectServer, Port);
        }

        #endregion

        #region << Keepalive Timer functions >>

        protected void CreateKeepAliveTimer()
        {
            // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate = new TimerCallback(KeepAliveTick);
            int interval = m_KeepAliveInterval * 1000;
            // Create a timer that waits x seconds, then invokes every x seconds.
            m_KeepaliveTimer = new Timer(timerDelegate, null, interval, interval);
        }

        protected void DestroyKeepAliveTimer()
        {
            if (m_KeepaliveTimer == null)
                return;

            m_KeepaliveTimer.Dispose();
            m_KeepaliveTimer = null;
        }

        private void KeepAliveTick(Object state)
        {
            // Send a Space for Keep Alive
            Send(" ", string.Empty);
        }

        #endregion

        #region << Constructors >>

        public XmppClientConnection()
            : base()
        {			
            InitSocket();
            // Streamparser stuff
            m_StreamParser = new StreamParser();

            m_StreamParser.OnStreamStart += new StreamHandler(StreamParserOnStreamStart);
            m_StreamParser.OnStreamEnd += new StreamHandler(StreamParserOnStreamEnd);
            m_StreamParser.OnStreamElement += new StreamHandler(StreamParserOnStreamElement);
            m_StreamParser.OnStreamError += new StreamError(StreamParserOnStreamError);
            m_StreamParser.OnError += new ErrorHandler(StreamParserOnError);    

            m_IqGrabber = new IqGrabber(this);
            m_MessageGrabber	= new MessageGrabber(this);
            m_PresenceGrabber = new PresenceGrabber(this);
            m_PresenceManager	= new PresenceManager(this);
            m_RosterManager = new RosterManager(this);            
        }

        public XmppClientConnection(SocketConnectionType type)
            : this()
        {
            m_SocketConnectionType = agsXMPP.net.SocketConnectionType.Direct;
            SocketConnectionType = type;
        }

        public XmppClientConnection(string server)
            : this()
        {
            Server = server;
        }

        public XmppClientConnection(string server, int port)
            : this(server)
        {
            Port = port;
        }

        #endregion

        #region Open && Close

        /// <summary>
        /// This method open the connections to the xmpp server and authenticates you to ther server.
        /// This method is async, don't assume you are already connected when it returns. You have to wait for the OnLogin Event
        /// </summary>
        public void Open()
        {
            _Open();            
        }

        /// <summary>
        /// This method open the connections to the xmpp server and authenticates you to ther server.
        /// This method is async, don't assume you are already connected when it returns. You have to wait for the OnLogin Event
        /// </summary>
        /// <param name="username">your username</param>
        /// <param name="password">your password</param>
        public void Open(string username, string password)
        {            
            this.Username = username;
            this.Password = password;

            _Open();
        }

        /// <summary>
        /// This method open the connections to the xmpp server and authenticates you to ther server.
        /// This method is async, don't assume you are already connected when it returns. You have to wait for the OnLogin Event
        /// </summary>
        /// <param name="username">your username</param>
        /// <param name="password">your passowrd</param>
        /// <param name="resource">resource for this connection</param>
        public void Open(string username, string password, string resource)
        {
            this.m_Username = username;
            this.m_Password	= password;
            this.m_Resource	= resource;
            _Open();
        }

        /// <summary>
        /// This method open the connections to the xmpp server and authenticates you to ther server.
        /// This method is async, don't assume you are already connected when it returns. You have to wait for the OnLogin Event
        /// </summary>
        /// <param name="username">your username</param>
        /// <param name="password">your password</param>
        /// <param name="resource">resource for this connection</param>
        /// <param name="priority">priority which will be sent with presence packets</param>
        public void Open(string username, string password, string resource, int priority)
        {
            this.m_Username = username;
            this.m_Password	= password;
            this.m_Resource	= resource;
            this.m_Priority	= priority;
            _Open();
        }

        /// <summary>
        /// This method open the connections to the xmpp server and authenticates you to ther server.
        /// This method is async, don't assume you are already connected when it returns. You have to wait for the OnLogin Event
        /// </summary>
        /// <param name="username">your username</param>
        /// <param name="password">your password</param>
        /// <param name="priority">priority which will be sent with presence packets</param>
        public void Open(string username, string password, int priority)
        {
            this.m_Username = username;
            this.m_Password	= password;			
            this.m_Priority	= priority;
            _Open();
        }



        private void _Open()
        {
            m_CleanUpDone = false;
            //m_StreamStarted = false;

            StreamParser.Reset();

            if (SocketConnectionType == SocketConnectionType.Direct && AutoResolveConnectServer)
                ResolveSrv();

            OpenSocket();          
        }


        public void Open(string xml)
        {
            Send(xml, string.Empty);            
        }

        public async Task CloseAsync()
        {
            await SendAsync("</stream:stream>", string.Empty)
                .ConfigureAwait(continueOnCapturedContext: false);

            this.CleanupSession();
        }

        /// <summary>
        /// Does the Clieanup of the Session and sends the OnClose Event
        /// </summary>
        private void CleanupSession()
        {       
            if (this.Socket != null) {
                this.Socket.Dispose();
                this.Socket = null;
            }

            StreamParser.Reset();

            m_IqGrabber.Clear();
            m_MessageGrabber.Clear();

            if (m_SaslHandler != null) {
                m_SaslHandler.Dispose();
                m_SaslHandler = null;
            }

            m_Authenticated = false;
            m_Binded = false;

            DestroyKeepAliveTimer();

            m_CleanUpDone = true;

            DoChangeXmppConnectionState(XmppConnectionState.Disconnected);
        }

        #endregion

        #region Auth

        private void OnGetAuthInfo(object sender, IQ iq, object data)
        {   
            iq.GenerateId();
            iq.SwitchDirection();
            iq.Type = IqType.set;

            Auth auth = (Auth)iq.Query;

            auth.Resource = this.m_Resource;
            auth.SetAuth(this.m_Username, this.m_Password, this.StreamId);

            IqGrabber.SendIq(iq, new IqCB(OnAuthenticate), null);
        }

        /// <summary>
        /// Refreshes the myJid Member Variable
        /// </summary>
        private Jid BuildMyJid()
        {
            Jid jid = new Jid(null);

            jid.m_User = m_Username;
            jid.m_Server = Server;
            jid.m_Resource = m_Resource;

            jid.BuildJid();

            return jid;         
        }

        private void OnAuthenticate(object sender, IQ iq, object data)
        {           
            if (iq.Type == IqType.result) {
                m_Authenticated = true;
                RaiseOnLogin();                
            } else if (iq.Type == IqType.error) {
                if (OnAuthError != null)
                    OnAuthError(this, iq);
            }

        }

        internal void FireOnAuthError(Element e)
        {
            if (OnAuthError != null)
                OnAuthError(this, e);
        }

        private void m_SaslHandler_OnSaslStart(object sender, SaslEventArgs args)
        {
            // This acts only as a tunnel to the client
            if (OnSaslStart != null)
                OnSaslStart(this, args);
        }

        internal void RaiseOnLogin()
        {
            if (KeepAlive)
                CreateKeepAliveTimer();

            if (OnLogin != null)
                OnLogin(this);

            if (m_AutoAgents)
                RequestAgents();

            if (m_AutoRoster)
                RequestRoster();
        }

        private void m_SaslHandler_OnSaslEnd(object sender)
        {
            if (OnSaslEnd != null)
                OnSaslEnd(this);

            m_Authenticated = true;
        }

        internal void DoRaiseEventBinded()
        {
            if (OnBinded != null)
                OnBinded(this);
        }


        private void InitSaslHandler()
        {
            if (m_SaslHandler == null) {
                m_SaslHandler = new SaslHandler(this);
                m_SaslHandler.OnSaslStart += new SaslEventHandler(m_SaslHandler_OnSaslStart);
                m_SaslHandler.OnSaslEnd += new ObjectHandler(m_SaslHandler_OnSaslEnd);
            }
        }

        #endregion

        #region StreamParser

        private void SendStreamHeader(bool startParser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<stream:stream");
            sb.Append(" to='" + Server + "'");
            sb.Append(" xmlns='jabber:client'");
            sb.Append(" xmlns:stream='http://etherx.jabber.org/streams'");

            if (StreamVersion != null)
                sb.Append(" version='" + StreamVersion + "'");

            if (m_ClientLanguage != null)
                sb.Append(" xml:lang='" + m_ClientLanguage + "'");

            sb.Append(">");

            Open(sb.ToString());
        }

        public void StreamParserOnStreamStart(object sender, Node e)
        {
            string xml = e.ToString().Trim();
            xml = xml.Substring(0, xml.Length - 2) + ">";

            this.FireOnReadXml(this, xml);

            protocol.Stream st = e as protocol.Stream;
            if (st == null)
                return;

            if (st != null) {
                m_StreamId = st.StreamId;
                m_StreamVersion = st.Version;
            }  
                
            m_ServerLanguage = st.Language;               

            if (!RegisterAccount) {
                if (this.StreamVersion != null && this.StreamVersion.StartsWith("1.")) {
                    if (!Authenticated) {
                        InitSaslHandler();                      
                    }               
                } else {
                    // old auth stuff
                    RequestLoginInfo();
                }
            } else {
                if (this.StreamVersion == null)
                    GetRegistrationFields(null);
            }

        }


        public virtual void StreamParserOnError(object sender, Exception ex)
        {
            FireOnError(sender, ex);
        }

        public void StreamParserOnStreamEnd(object sender, Node e)
        {
            Element tag = e as Element;

            string qName;
            if (tag.Prefix == null)
                qName = tag.TagName;
            else
                qName = tag.Prefix + ":" + tag.TagName;

            string xml = "</" + qName + ">";

            this.FireOnReadXml(this, xml);            

            if (!m_CleanUpDone)
                CleanupSession();
        }

        public void StreamParserOnStreamElement(object sender, Node e)
        {
            this.FireOnReadXml(this, e.ToString());

            if (e.GetType() == typeof(IQ)) {
                if (OnIq != null)
                    OnIq(this, e as IQ);

                IQ iq = e as IQ;
                if (iq != null && iq.Query != null) {
                    // Roster
                    if (iq.Query is Roster)
                        OnRosterIQ(iq);                   
                }   
            } else if (e.GetType() == typeof(Message)) {
                if (OnMessage != null)
                    OnMessage(this, e as Message);
            } else if (e.GetType() == typeof(Presence)) {
                if (OnPresence != null)
                    OnPresence(this, e as Presence);
            } else if (e.GetType() == typeof(Features)) {
                // Stream Features
                // StartTLS stuff
                Features f = e as Features;
                if (m_UseCompression &&
                    f.SupportsCompression &&
                    f.Compression.SupportsMethod(CompressionMethod.zlib)) {
                    // Check for Stream Compression
                    // we support only ZLIB because its a free algorithm without patents
                    // yes ePatents suck                                       
                    DoChangeXmppConnectionState(XmppConnectionState.StartCompression);
                    this.Send(new Compress(CompressionMethod.zlib), string.Empty);                    
                } else if (f.SupportsRegistration && m_RegisterAccount) {
                    // Do registration after TLS when possible
                    if (f.SupportsRegistration)
                        GetRegistrationFields(e);
                    else {
                        // registration is not enabled on this server                        
                        FireOnError(this, new RegisterException("Registration is not allowed on this server"));
                    }
                }
            } else if (e.GetType() == typeof(Compressed)) {
                //DoChangeXmppConnectionState(XmppConnectionState.StartCompression);
                StreamParser.Reset();
                Socket.StartCompression();                
                // Start new Stream Header compressed.
                SendStreamHeader(false);

                DoChangeXmppConnectionState(XmppConnectionState.Compressed);
            } else if (e is agsXMPP.protocol.Error) {
                if (OnStreamError != null)
                    OnStreamError(this, e as Element);
            }

        }

        public void StreamParserOnStreamError(object sender, Exception ex)
        {
            FireOnError(this, ex);

            if (!m_CleanUpDone)
                CleanupSession();
        }

        internal void Reset()
        {
            StreamParser.Reset();
            SendStreamHeader(false);        
        }

        #endregion

        #region Send

        /// <summary>
        /// Send a xml string over the XmppConnection
        /// </summary>
        /// <param name="xml"></param>
        public void Send(string xml, string id)
        {
            if (m_ClientSocket == null)
                return;

            FireOnWriteXml(this, xml);
            m_ClientSocket.Send(xml, id);

            // reset keep alive timer if active to make sure the interval is always idle time from the last 
            // outgoing packet
            if (m_KeepAlive && m_KeepaliveTimer != null)
                m_KeepaliveTimer.Change(m_KeepAliveInterval * 1000, m_KeepAliveInterval * 1000);
        }

        public async Task SendAsync(string xml, string id)
        {
            if (m_ClientSocket == null)
                return;

            FireOnWriteXml(this, xml);
            await m_ClientSocket.SendAsync(xml, id);
            // reset keep alive timer if active to make sure the interval is always idle time from the last 
            // outgoing packet
            if (m_KeepAlive && m_KeepaliveTimer != null)
                m_KeepaliveTimer.Change(m_KeepAliveInterval * 1000, m_KeepAliveInterval * 1000);
        }

        public void Send(Element e, string id)
        {
            Element dummyEl = new Element("a");
            dummyEl.Namespace = Uri.CLIENT;

            dummyEl.AddChild(e);
            string toSend = dummyEl.ToString();

            Send(toSend.Substring(25, toSend.Length - 25 - 4), id);
        }

        public async Task SendAsync(Element e, string id)
        {
            Element dummyEl = new Element("a");
            dummyEl.Namespace = Uri.CLIENT;

            dummyEl.AddChild(e);
            string toSend = dummyEl.ToString();

            await SendAsync(toSend.Substring(25, toSend.Length - 25 - 4), id);
        }

        #endregion

        #region << RequestRoster >> Never Used By Joesong

        public void RequestRoster()
        {       
            RosterIq iq = new RosterIq(IqType.get);
            Send(iq, iq.Id);
        }

        private void OnRosterIQ(IQ iq)
        {   
            if (iq.Type == IqType.result && OnRosterStart != null)
                OnRosterStart(this);

            Roster r = iq.Query as Roster;
            if (r != null) {
                foreach (RosterItem i in r.GetRoster()) {
                    if (OnRosterItem != null)
                        OnRosterItem(this, i);
                }
            }

            if (iq.Type == IqType.result && OnRosterEnd != null)
                OnRosterEnd(this);

            if (m_AutoPresence && iq.Type == IqType.result)
                SendMyPresence();
        }

        #endregion

        #region << SRV functions >> Never used by joe song

        /// <summary>
        /// Resolves the connection host of a xmpp domain when SRV records are set
        /// </summary>
        private void ResolveSrv()
        {

        }

        private void SetConnectServerFromSRVRecords()
        {
            // check we have a response
            if (_SRVRecords != null && _SRVRecords.Length > 0) {
                //SRVRecord srv = _SRVRecords[0];
                _currentSRVRecord = PickSRVRecord();

                this.Port = _currentSRVRecord.Port;
                this.ConnectServer = _currentSRVRecord.Target;
            } else {
                // no SRV-Records set
                _currentSRVRecord = null;
                this.ConnectServer = null;
            }
        }

        private void RemoveSrvRecord(SRVRecord rec)
        {
            int i = 0;
            SRVRecord[] recs = new SRVRecord[_SRVRecords.Length - 1];
            foreach (SRVRecord srv in _SRVRecords) {
                if (!srv.Equals(rec)) {
                    recs[i] = srv;
                    i++;
                }
            }
            _SRVRecords = recs;
        }

        /// <summary>
        /// Picks one of the SRV records.
        /// priority and weight are evaluated by the following algorithm.
        /// </summary>
        /// <returns>SRVRecord</returns>
        private SRVRecord PickSRVRecord()
        {
            SRVRecord ret = null;

            // total weight of all servers with the same priority
            int totalWeight = 0;
            
            // ArrayList for the servers with the lowest priority
            ArrayList lowServers = new ArrayList();
            // check we have a response
            if (_SRVRecords != null && _SRVRecords.Length > 0) {
                // Find server(s) with the highest priority (could be multiple)
                foreach (SRVRecord srv in _SRVRecords) {
                    if (ret == null) {
                        ret = srv;
                        lowServers.Add(ret);
                        totalWeight = ret.Weight;
                    } else {
                        if (srv.Priority == ret.Priority) {
                            lowServers.Add(srv);
                            totalWeight += srv.Weight;
                        } else if (srv.Priority < ret.Priority) {
                            // found a servr with a lower priority
                            // clear the lowServers Array and start with this server
                            lowServers.Clear();
                            lowServers.Add(ret);
                            ret = srv;
                            totalWeight = ret.Weight;
                        } else if (srv.Priority > ret.Priority) {
                            // exit the loop, because servers are already sorted by priority
                            break;
                        }
                    }
                }
            }

            // if we have multiple lowServers then we have to pick a random one
            // BUT we have too involve the weight which can be used for "Load Balancing" here
            if (lowServers.Count > 1) {
                if (totalWeight > 0) {
                    // Create a random value between 1 - total Weight
                    int rnd = new Random().Next(1, totalWeight);
                    int i = 0;
                    foreach (SRVRecord sr in lowServers) {
                        if (rnd > i && rnd <= (i + sr.Weight)) {
                            ret = sr;
                            break;
                        } else {
                            i += sr.Weight;
                        }
                    }
                } else {
                    // Servers have no weight, they are all equal, pick a random server
                    int rnd = new Random().Next(lowServers.Count);                    
                    ret = (SRVRecord)lowServers[rnd];                    
                }
            }

            return ret;
        }

        #endregion

        #region Others Never used by joe song

        /// <summary>
        /// Sends our Presence, the packet is built of Status, Show and Priority
        /// </summary>
        public void SendMyPresence()
        {
            Presence pres = new Presence(m_Show, m_Status, m_Priority);

            // Add client caps when enabled
            if (m_EnableCapabilities) {
                if (m_Capabilities.Version == null)
                    UpdateCapsVersion();

                pres.AddChild(m_Capabilities);
            }

            this.Send(pres, string.Empty);
        }

        /// <summary>
        /// Sets the caps version automatically from the DiscoInfo object.
        /// Call this member after each change of the DiscoInfo object
        /// </summary>
        public void UpdateCapsVersion()
        {
            m_Capabilities.SetVersion(m_DiscoInfo);
        }

        internal void RequestLoginInfo()
        {			
            AuthIq iq = new AuthIq(IqType.get, new Jid(Server));
            iq.Query.Username = this.m_Username;

            IqGrabber.SendIq(iq, new IqCB(OnGetAuthInfo), null);
        }

        /// <summary>
        /// Changing the Password. You should use this function only when connected with SSL or TLS
        /// because the password is sent in plain text over the connection.		
        /// </summary>
        /// /// <remarks>
        ///		<para>
        ///			After this request was successful the new password is set automatically in the Username Property
        ///		</para>
        /// </remarks>		
        /// <param name="newPass">value of the new password</param>
        public void ChangePassword(string newPass)
        {
            RegisterIq regIq = new RegisterIq(IqType.set, new Jid(Server));			
            regIq.Query.Username = this.m_Username;
            regIq.Query.Password = newPass;
			
            IqGrabber.SendIq(regIq, new IqCB(OnChangePasswordResult), newPass);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data">contains the new password</param>
        private void OnChangePasswordResult(object sender, IQ iq, object data)
        {
            if (iq.Type == IqType.result) {
                if (OnPasswordChanged != null)
                    OnPasswordChanged(this);
				
                // Set the new password in the Password property on sucess
                m_Password = (string)data;
            } else if (iq.Type == IqType.error) {
                if (OnRegisterError != null)
                    OnRegisterError(this, iq);
            }
        }

        #endregion

        #region << Register new Account >> Neved user by joe song

        /// <summary>
        /// requests the registration fields
        /// </summary>
        /// <param name="obj">object which contains the features node which we need later for login again</param>
        private void GetRegistrationFields(object data)
        {
            RegisterIq regIq = new RegisterIq(IqType.get, new Jid(Server));
            IqGrabber.SendIq(regIq, new IqCB(OnRegistrationFieldsResult), data);
        }

        private void OnRegistrationFieldsResult(object sender, IQ iq, object data)
        {
            if (iq.Type != IqType.error) {
                if (iq.Query != null && iq.Query.GetType() == typeof(Register)) {
                    RegisterEventArgs args = new RegisterEventArgs(iq.Query as Register);
                    if (OnRegisterInformation != null)
                        OnRegisterInformation(this, args);

                    DoChangeXmppConnectionState(XmppConnectionState.Registering);

                    IQ regIq = new IQ(IqType.set);
                    regIq.GenerateId();
                    regIq.To = new Jid(Server);

                    //RegisterIq regIq = new RegisterIq(IqType.set, new Jid(base.Server));
                    if (args.Auto) {
                        Register reg = new Register(this.m_Username, this.m_Password);
                        regIq.Query = reg;
                    } else {
                        regIq.Query = args.Register;
                    }
                    IqGrabber.SendIq(regIq, new IqCB(OnRegisterResult), data);
                }
            } else {
                if (OnRegisterError != null)
                    OnRegisterError(this, iq);
            }
        }

        private void OnRegisterResult(object sender, IQ iq, object data)
        {
            if (iq.Type == IqType.result) {
                DoChangeXmppConnectionState(XmppConnectionState.Registered);
                if (OnRegistered != null)
                    OnRegistered(this);

                if (this.StreamVersion != null && this.StreamVersion.StartsWith("1.")) { 
                    // init sasl login
                    InitSaslHandler();
                    m_SaslHandler.OnStreamElement(this, data as Node);
                } else {
                    // old jabber style login
                    RequestLoginInfo();
                }
            } else if (iq.Type == IqType.error) {
                if (OnRegisterError != null)
                    OnRegisterError(this, iq);
            }
        }

        #endregion

        #region << RequestAgents >> Never used by joe song

        public void RequestAgents()
        {			
            AgentsIq iq = new AgentsIq(IqType.get, new Jid(Server));
            IqGrabber.SendIq(iq, new IqCB(OnAgents), null);
        }

        private void OnAgents(object sender, IQ iq, object data)
        {	
            if (OnAgentStart != null)
                OnAgentStart(this);
						
            Agents agents = iq.Query as Agents;
            if (agents != null) {
                foreach (Agent a in agents.GetAgents()) {
                    if (OnAgentItem != null)
                        OnAgentItem(this, a);				
                }
            }

            if (OnAgentEnd != null)
                OnAgentEnd(this);			
        }

        #endregion
    }
}
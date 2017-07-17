using System;
using agsXMPP.protocol.client;

namespace RekTec.Chat.XmppElement
{
    /// <summary>
    /// Summary description for BindIq.
    /// </summary>
    public class ApnsIq : IQ
    {
        private Apns m_apns = new Apns();

        public ApnsIq()
        {
            this.GenerateId();
            this.AddChild(m_apns);
        }

        public ApnsIq(IqType type, string token)
            : this()
        {           
            this.Type = type; 
            m_apns.Token = token;
        }

        public new Apns Query {
            get {
                return m_apns;
            }
        }
    }
}

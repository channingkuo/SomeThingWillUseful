using System;
using SQLite;

namespace RekTec.Mobile.Client.IOS
{
    public class LoggingViewModel
    {
        [PrimaryKey]
        public string LoggingId { get; set; }

        public string LoggingMessage { get; set; }

        public string LoggingMessageDetail { get; set; }

        public LoggingViewModelType LoggingType { get; set; }

        public DateTime LoggingDateTime { get; set; }

        public override string ToString()
        {
            return string.Format("[LoggingViewModel: LoggingId={0}, LoggingMessage={1}, LoggingMessageDetail={2}, LoggingType={3}, LoggingDateTime={4}]", LoggingId, LoggingMessage, LoggingMessageDetail, LoggingType, LoggingDateTime);
        }
    }

    public enum LoggingViewModelType
    {
        Error, Info
    }
}


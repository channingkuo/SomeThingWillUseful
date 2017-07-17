using System;
using SQLite;

namespace RekTec.Application.ViewModels
{
    public class SystemMenuSettingViewModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string SystemMenuSyncTime { get; set; }
    }
}


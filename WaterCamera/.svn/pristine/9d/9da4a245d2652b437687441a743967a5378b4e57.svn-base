#region 文件头部
/**********
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
****************
作者 :carziertong
日期 : 2015-04-09
说明 : 操纵数据的方法封装类
****************/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using RekTec.Chat.ViewModels;
using RekTec.Corelib.DataRepository;
using RekTec.Messages.Services;
using RekTec.Messages.ViewModels;

namespace RekTec.Chat.DataRepository
{
    /// <summary>
    /// 操纵数据的方法封装类
    /// </summary>
    public static class ChatDataRepository
    {
        static ChatDataRepository()
        {
            SqlDataRepository.CreateTable<ChatRoomViewModel>();

            SqlDataRepository.CreateTable<ChatRoomMemberViewModel>();

            SqlDataRepository.CreateTable<ChatFileOpenViewModel>();

            SqlDataRepository.CreateTable<ChatFileCloseViewModel>();

            SqlDataRepository.CreateTable<ChatFilePackageViewModel>();
        }



        #region Room

        private static readonly List<Action<ChatRoomViewModel, SqlDataChangeType>> _roomChangeEvents = new List<Action<ChatRoomViewModel, SqlDataChangeType>>();

        public static List<ChatRoomViewModel> GetAllRooms()
        {
            if (!SqlDataRepository.IsOpened)
                return new List<ChatRoomViewModel>(0);

            return SqlDataRepository.Table<ChatRoomViewModel>()
                .ToList();
        }

        public static List<ChatRoomViewModel> GetRoomsByLikeName(string name)
        {
            if (!SqlDataRepository.IsOpened)
                return new List<ChatRoomViewModel>(0);
            if (string.IsNullOrWhiteSpace(name))
                return new List<ChatRoomViewModel>(0);

            name = name.ToUpper();
            var rooms = SqlDataRepository.Table<ChatRoomViewModel>().ToList();
            return rooms.Where(c =>
                c.ChatRoomName.Contains(name)
            || c.ContactNamePinYinFirst.Contains(name)
            || c.ContactNamePinYin.Replace(" ", "").Contains(name)
            ).ToList();
        }

        public static ChatRoomViewModel GetRoomsById(string id)
        {
            if (!SqlDataRepository.IsOpened)
                return null;
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return SqlDataRepository.Table<ChatRoomViewModel>()
                .FirstOrDefault(r => r.ChatRoomId.ToLower() == id.ToLower());
        }

        public static void AddOrUpdate(ChatRoomViewModel room)
        {
            if (!SqlDataRepository.IsOpened)
                return;
            if (room == null)
                return;

            var existsRoom = SqlDataRepository.Table<ChatRoomViewModel>()
                .AsQueryable()
                .FirstOrDefault(c => c.ChatRoomId.ToLower() == room.ChatRoomId.ToLower());

            if (existsRoom == null)
            {
                SqlDataRepository.Insert(room);
            }
            else
            {
                SqlDataRepository.Update(room);
            }

            SqlDataRepository.Commit();
            NotifyChatRoomChange(room, existsRoom == null ? SqlDataChangeType.Add : SqlDataChangeType.Update);
        }

        public static void Remove(ChatRoomViewModel room)
        {
            if (!SqlDataRepository.IsOpened)
                return;
            if (room == null)
                return;
            if (string.IsNullOrWhiteSpace(room.ChatRoomId))
                return;

            var members = SqlDataRepository.Table<ChatRoomMemberViewModel>()
                .Where(m => m.ChatRoomId.ToLower() == room.ChatRoomId.ToLower())
                .ToList();

            members.ForEach(m => SqlDataRepository.Delete(m));

            var chatList = SqlDataRepository.Table<ChatListViewModel>().FirstOrDefault(l => l.ChatListId.ToLower() == room.ChatRoomId.ToLower());
            if (chatList != null)
                SqlDataRepository.Delete(chatList);

            SqlDataRepository.Delete(room);

            SqlDataRepository.Commit();
            NotifyChatRoomChange(room, SqlDataChangeType.Remove);

            if (chatList != null)
                MessagesDataRepository.NotifyChatListChange(null);
            if (members.Count > 0)
                NotifyChatRoomMemberChange(null, SqlDataChangeType.Remove);
        }

        public static void SubscribeChatRoomChange(Action<ChatRoomViewModel, SqlDataChangeType> cb)
        {
            _roomChangeEvents.Add(cb);
        }

        public static void UnSubscribeChatRoomChange(Action<ChatRoomViewModel, SqlDataChangeType> cb)
        {
            _roomChangeEvents.Remove(cb);
        }

        public static void NotifyChatRoomChange(ChatRoomViewModel c, SqlDataChangeType t)
        {
            _roomChangeEvents.ForEach(cb =>
            {
                if (cb != null)
                    cb(c, t);
            });
        }

        #endregion

        #region Room Member

        private static readonly List<Action<ChatRoomMemberViewModel, SqlDataChangeType>> _chatRoomMemberChangeEvents = new List<Action<ChatRoomMemberViewModel, SqlDataChangeType>>();

        public static ChatRoomMemberViewModel GetRoomMemberById(string roomId, string memberId)
        {
            if (!SqlDataRepository.IsOpened)
                return null;
            if (string.IsNullOrWhiteSpace(roomId) || string.IsNullOrWhiteSpace(memberId))
                return null;

            return SqlDataRepository.Table<ChatRoomMemberViewModel>()
                .FirstOrDefault(m => m.ChatRoomId.ToLower() == roomId.ToLower()
            && m.ContactId.ToLower() == memberId.ToLower());
        }

        public static List<ChatRoomMemberViewModel> GetRoomMembersByRoomId(string roomId)
        {
            if (!SqlDataRepository.IsOpened)
                return new List<ChatRoomMemberViewModel>(0);
            if (string.IsNullOrWhiteSpace(roomId))
                return new List<ChatRoomMemberViewModel>(0);

            return SqlDataRepository.Table<ChatRoomMemberViewModel>()
                .Where(m => m.ChatRoomId.ToLower() == roomId.ToLower())
                .ToList();
        }

        public static void AddOrUpdate(ChatRoomMemberViewModel member)
        {
            if (!SqlDataRepository.IsOpened)
                return;
            if (member == null)
                return;

            var exists = SqlDataRepository.Table<ChatRoomMemberViewModel>()
                .AsQueryable()
                .FirstOrDefault(c => c.ChatRoomId.ToLower() == member.ChatRoomId.ToLower()
                                     && c.ContactId.ToLower() == member.ContactId.ToLower());

            if (exists == null)
            {
                SqlDataRepository.Insert(member);
            }
            else
            {
                SqlDataRepository.Update(member);
            }

            SqlDataRepository.Commit();
            NotifyChatRoomMemberChange(member, exists == null ? SqlDataChangeType.Add : SqlDataChangeType.Update);
        }

        public static void Remove(ChatRoomMemberViewModel member)
        {
            if (!SqlDataRepository.IsOpened)
                return;
            if (member == null)
                return;
            if (string.IsNullOrWhiteSpace(member.ChatRoomId) || string.IsNullOrWhiteSpace(member.ContactId))
                return;

            var existsRoomMember = SqlDataRepository.Table<ChatRoomMemberViewModel>()
                .AsQueryable()
                .FirstOrDefault(c => c.ContactId.ToLower() == member.ContactId.ToLower()
                                     && c.ChatRoomId.ToLower() == member.ChatRoomId.ToLower());

            if (existsRoomMember != null)
            {
                SqlDataRepository.Delete(existsRoomMember);

                SqlDataRepository.Commit();
            }

            if (existsRoomMember != null)
                NotifyChatRoomMemberChange(existsRoomMember, SqlDataChangeType.Update);
        }

        public static void SubscribeChatRoomMemberChange(Action<ChatRoomMemberViewModel, SqlDataChangeType> cb)
        {
            _chatRoomMemberChangeEvents.Add(cb);
        }

        public static void UnSubscribeChatRoomMemberChange(Action<ChatRoomMemberViewModel, SqlDataChangeType> cb)
        {
            _chatRoomMemberChangeEvents.Remove(cb);
        }

        public static void NotifyChatRoomMemberChange(ChatRoomMemberViewModel c, SqlDataChangeType type)
        {
            _chatRoomMemberChangeEvents.ForEach(cb =>
            {
                if (cb != null)
                    cb(c, type);
            });
        }

        #endregion
    }
}

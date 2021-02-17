using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurkanKARAKASChatApp.Model;
using Newtonsoft.Json;
using FurkanKARAKASChatApp.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using FurkanKARAKASChatApp.SignalR;

namespace FurkanKARAKASChatApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public IConfiguration Configuration { get; }

        private readonly string mongoConnectionString;

        private readonly IHubContext<ChatHub> _chatHub;

        public HomeController(IDistributedCache distributedCache, IConfiguration configuration, IHubContext<ChatHub> chatHub)
        {
            _distributedCache = distributedCache;
            _chatHub = chatHub;

            Configuration = configuration;
            mongoConnectionString = this.Configuration.GetConnectionString("MongoConnectionString");

        }

         
        public async Task<IActionResult> Index()
        {
            await _distributedCache.SetStringAsync("name_surname", "furkan");
            string name = await _distributedCache.GetStringAsync("name_surname");

            ViewBag.Time = name;
            return View();
        }

        public async Task<IActionResult> JoinChatRoom(Guid RoomId, string UserNickName)
        {
            try
            {
                ChatRoomUsers chatRoomUsers = new ChatRoomUsers();
                string Json_ChatRoomUsers = await _distributedCache.GetStringAsync("ChatRoomUsers") ?? "";
                List<ChatRoomUsers> lst_ChatRoomUsers = JsonConvert.DeserializeObject<List<ChatRoomUsers>>(Json_ChatRoomUsers) ?? new List<ChatRoomUsers>();
                if (!lst_ChatRoomUsers.Any(x => x.UserNickName == UserNickName && x.ChatRoomId == RoomId))
                {

                    chatRoomUsers.ChatRoomId = RoomId;
                    chatRoomUsers.UserNickName = UserNickName;
                    chatRoomUsers.ChatRoomUsersId = Guid.NewGuid();

                    lst_ChatRoomUsers.Add(chatRoomUsers);

                    await _distributedCache.SetStringAsync("ChatRoomUsers", JsonConvert.SerializeObject(lst_ChatRoomUsers));

                    //MongoDB Loglama
                    //new BaseMongoRepository<ChatRoomUsers>(mongoConnectionString, "ChatApp", "ChatRoomLog").Create(chatRoomUsers);
                }
                return Json(new { message = "İşlem Başarılı", success = true, ChatRoomUsersId = chatRoomUsers.ChatRoomUsersId });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }

        public async Task<IActionResult> GetRoomMessage(Guid RoomId)
        {
            string Json_ChatMessage = await _distributedCache.GetStringAsync("ChatMessage") ?? "";
            List<ChatMessage> lst_ChatMessage = JsonConvert.DeserializeObject<List<ChatMessage>>(Json_ChatMessage).Where(x=> x.ChatRoomId == RoomId).ToList() ?? new List<ChatMessage>();


            return Json(lst_ChatMessage.OrderBy(x => x.SendDate));
        }

        public async Task<IActionResult> SendMessage(Guid RoomId, Guid ChatRoomUsersId, string Message)
        {
            string Json_ChatMessage = await _distributedCache.GetStringAsync("ChatMessage") ?? "";
            List<ChatMessage> lst_ChatMessage = JsonConvert.DeserializeObject<List<ChatMessage>>(Json_ChatMessage) ?? new List<ChatMessage>();

            ChatMessage chatMessage = new ChatMessage();
            chatMessage.ChatRoomId = RoomId;
            chatMessage.ChatRoomUsersId = ChatRoomUsersId;
            chatMessage.Message = Message;
            chatMessage.SendDate = DateTime.Now;

            string Json_ChatRoomUsers = await _distributedCache.GetStringAsync("ChatRoomUsers") ?? "";
            List<ChatRoomUsers> lst_ChatRoomUsers = JsonConvert.DeserializeObject<List<ChatRoomUsers>>(Json_ChatRoomUsers) ?? new List<ChatRoomUsers>();

            chatMessage.ChatRoomUsers = lst_ChatRoomUsers.FirstOrDefault(x => x.ChatRoomUsersId == ChatRoomUsersId);

            lst_ChatMessage.Add(chatMessage);

            await _distributedCache.SetStringAsync("ChatMessage", JsonConvert.SerializeObject(lst_ChatMessage));

            //MongoDB Loglama
            //new BaseMongoRepository<ChatMessage>(mongoConnectionString, "ChatApp", "ChatMessageLog").Create(chatMessage);


           await _chatHub.Clients.All.SendAsync("GetAllMessage", chatMessage);

            return Json(lst_ChatMessage);
        }
         
    }
}

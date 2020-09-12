using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Dotnet_Practice.Models;
using Microsoft.Data.Sqlite;

namespace Dotnet_Practice.Controllers
{
    public class ChatController: Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index(){
            var model = GetMessages();
            return View(model);
        }

        private List<WebChatModel> GetMessages(){
            var model = new List<WebChatModel>();
            using(var connection = new SqliteConnection(@"Data Source=C:\Users\StephonB\Desktop\DotNet Practice\ChatMessages.db")){
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT UserName, Message, TimeSent 
                    FROM MESSAGE
                ";
                using (var reader = command.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {
                        var message = new WebChatModel();
                        message.UserName = reader.GetString(0);
                        message.Message = reader.GetString(1);
                        message.Time = reader.GetDateTime(0);
                        model.Add(message);
                    }
                }

                return model;
            }
        }
        public async Task SendMessage(string user, string message, DateTime time){
            using(var connection = new SqliteConnection(@"C:\Users\StephonB\Desktop\DotNet Practice\ChatMessages.db")){
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    Insert INTO MESSAGE(UserName, Message, TimeSent) 
                    Values Message($user, $message, $time)
                ";

                command.Parameters.AddWithValue("$user", user);
                command.Parameters.AddWithValue("$message", message);
                command.Parameters.AddWithValue("$time", time);
                command.ExecuteNonQuery();
            }
            var model = new WebChatModel(){
                UserName = user,
                Message = message,
                Time = time
            };
            await _hubContext.Clients.All.SendAsync("TakeMessage", model);
        }
    }
}
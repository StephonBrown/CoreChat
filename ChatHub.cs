using System.ComponentModel.Design;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace Dotnet_Practice
{
    public class ChatHub: Hub
    {
        public override async Task OnConnectedAsync(){
            Console.WriteLine(Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}
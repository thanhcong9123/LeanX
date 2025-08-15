using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LearnX_App.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        
    }
}
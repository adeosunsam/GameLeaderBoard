using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MovieManiaSignalr
{
    public class ChatHub : Hub<IChatClient>
    {
        //private static readonly Dictionary<string, HashSet<string>> GroupUsers = new();

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(int playerScore, string userId)
        {
            /*Console.WriteLine("To: " + routeOb.To.ToString());
            Console.WriteLine("Message Recieved on: " + Context.ConnectionId);*/
            try
            {
                await Clients.User(userId: userId).RecieveScore(playerScore);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            /*if (routeOb.To.ToString() == string.Empty)
            {
                Console.WriteLine("Broadcast");
                var test = new string[1];
                test[0] = Context.ConnectionId;
                await Clients.Caller.ReceiveMessage(message);
                await Clients.AllExcept(test).ReceiveMessage(message);
            }
            else
            {
                string toClient = routeOb.To;
                Console.WriteLine("Targeted on: " + toClient);

                await Clients.Caller.ReceiveMessage(message);
                //await Clients.User(userId: Context.ConnectionId).ReceiveMessage(message);
                await Clients.Client(recieverEmail).ReceiveMessage(playerScore);
            }*/
        }

        /*public async Task SendMessageToGroup(string groupName, int playerScore)
        {
            await Clients.Group(groupName).ReceiveMessage(playerScore);
        }

        public async Task JoinGroup(string groupName)
        {
            if (!GroupUsers.ContainsKey(groupName))
            {
                GroupUsers[groupName] = new HashSet<string>();
            }

            var usersInGroup = GroupUsers[groupName];

            *//*if (usersInGroup.Count >= 2)
            {
                await Clients.Caller.SendAsync("Error", "Group is full.");
                return;
            }*//*

            if (usersInGroup.Add(Context.UserIdentifier))
            {
                await Groups.AddToGroupAsync(Context.UserIdentifier, groupName);
                await Clients.Group(groupName).ReceiveMessage(0);
            }
        }

        public async Task LeaveGroup(string groupName)
        {
            if (GroupUsers.ContainsKey(groupName))
            {
                GroupUsers[groupName].Remove(Context.ConnectionId);
                if (!GroupUsers[groupName].Any())
                {
                    GroupUsers.Remove(groupName);
                }

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).ReceiveMessage(0);
            }
        }
*/
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            /*foreach (var group in GroupUsers.Keys.ToList())
            {
                if (GroupUsers[group].Remove(Context.ConnectionId))
                {
                    if (!GroupUsers[group].Any())
                    {
                        GroupUsers.Remove(group);
                    }

                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                    await Clients.Group(group).ReceiveMessage(0);
                }
            }*/
            await base.OnDisconnectedAsync(exception);
        }
    }

    public class CurrentUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }
    }

    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task RecieveScore(int score);
        Task ReceiveConnID(string? userEmail);
    }
}

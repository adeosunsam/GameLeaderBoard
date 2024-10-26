using Domain.Entity.MovieMania;
using GameLeaderBoard.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MovieManiaSignalr
{
    public class ChatHub : Hub<IChatClient>
    {
        public ChatHub(LeaderBoardContext context)
        {
            _context = context;
        }

        private static readonly Dictionary<string, HashSet<string>> GroupUsers = new();
        private readonly LeaderBoardContext _context;

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(int playerScore, string groupName)
        {
            try
            {
                //await Clients.User(userId: userId).RecieveScore(playerScore);
                var otherInGroup = GroupUsers[groupName].Where(x => x != Context.UserIdentifier);

                await Clients.Users(otherInGroup).RecieveScore(playerScore);
                //await Clients.OthersInGroup(groupName).RecieveScore(playerScore);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*public async Task SendMessageToGroup(string groupName, int playerScore)
        {
            await Clients.Group(groupName).ReceiveMessage(playerScore);
        }*/

        public async Task CreateGroupAsync(string opponentId, string topicId)
        {
            var groupName = Guid.NewGuid().ToString();

            if (!GroupUsers.ContainsKey(groupName))
            {
                GroupUsers[groupName] = new HashSet<string>();
            }

            var usersInGroup = GroupUsers[groupName];

            usersInGroup.Add(Context.UserIdentifier);

            try
            {
                await _context.UserActivities.AddAsync(new UserActivity
                {
                    ChallengerId = Context.UserIdentifier,
                    UserId = opponentId,
                    TopicId = topicId,
                    ActivityAction = ActivityEnum.Challenge,
                    GroupId = groupName
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Clients.Caller.ReceiveMessage(ex.Message);
            }

            await Clients.Caller.ReceiveMessage("Group created successfully.");

            //await Clients.User(opponentId).RecieveNotification();
            await Clients.User(opponentId).RecieveNotification();
        }

        public async Task JoinGroupAsync(string activityId)
        {
            var activity = await (from a in _context.UserActivities
                                  where a.Id == activityId
                                  && a.ActivityAction == ActivityEnum.Challenge
                                  && !string.IsNullOrEmpty(a.GroupId)
                                  && !a.IsDeleted
                                  select a).FirstOrDefaultAsync();
            if (activity == null)
            {
                await Clients.All.ReceiveMessage("Unable to join at the moment.");
                return;
            }
            var usersInGroup = GroupUsers[activity.GroupId];

            if (usersInGroup.Count >= 2)
            {
                await Clients.Caller.ReceiveMessage("Group is full.");
                return;
            }

            usersInGroup.Add(Context.UserIdentifier);

            //await Groups.AddToGroupAsync(Context.UserIdentifier, groupName);

            //if (usersInGroup.Count > 1)
            await Clients.Users(usersInGroup).ReceiveConnection();

            /*if (usersInGroup.Add(Context.UserIdentifier))
            {
                try
                {
                    await Groups.AddToGroupAsync(Context.UserIdentifier, groupName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (usersInGroup.Count > 1)
            {
            await Clients.Users(usersInGroup).ReceiveConnection();
            }*/
        }

        public async Task LeaveGroup(string groupName)
        {
            if (GroupUsers.ContainsKey(groupName))
            {
                GroupUsers[groupName].Remove(Context.UserIdentifier);
                if (!GroupUsers[groupName].Any())
                {
                    GroupUsers.Remove(groupName);
                }

                await Groups.RemoveFromGroupAsync(Context.UserIdentifier, groupName);
                await Clients.Group(groupName).ReceiveMessage("");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var group in GroupUsers.Keys.ToList())
            {
                if (GroupUsers[group].Remove(Context.UserIdentifier))
                {
                    if (!GroupUsers[group].Any())
                    {
                        GroupUsers.Remove(group);
                    }

                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                    await Clients.Group(group).ReceiveMessage("");
                }
            }
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
        Task ReceiveConnection();
        Task RecieveNotification();
    }
}

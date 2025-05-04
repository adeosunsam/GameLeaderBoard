using System.Security.Claims;
using Domain.Entity.MovieMania;
using GameLeaderBoard.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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

        public async Task SendMessageAsync(int playerScore, string groupId)
        {
            try
            {
                //await Clients.User(userId: userId).RecieveScore(playerScore);
                var otherInGroup = GroupUsers[groupId].Where(x => x != Context.UserIdentifier);

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

        public async Task OnGameFinished(string groupId)
        {
            var otherInGroup = GroupUsers[groupId].Where(x => x != Context.UserIdentifier);

            await Clients.Users(otherInGroup).GameOverNotification();
        }

        public async Task CreateGroupAsync(string opponentId, string topicId, string groupId)
        {
            if (!GroupUsers.ContainsKey(groupId))
            {
                GroupUsers[groupId] = new HashSet<string>();
            }

            var usersInGroup = GroupUsers[groupId];

            usersInGroup.Add(Context.UserIdentifier);

            try
            {
                await _context.UserActivities.AddAsync(new UserActivity
                {
                    ChallengerId = Context.UserIdentifier,
                    UserId = opponentId,
                    TopicId = topicId,
                    ActivityAction = ActivityEnum.Challenge,
                    GroupId = groupId
                });
                await _context.SaveChangesAsync();

                await Clients.Caller.ReceiveMessage("Group created successfully.");
            }
            catch (Exception ex)
            {
                await Clients.Caller.ReceiveMessage(ex.Message);
            }

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

            activity.IsDeleted = true;

            try
            {
                var userGameNumber = await (from u in _context.UserGamingNumbers
                                            where (u.UserId == activity.ChallengerId || u.UserId == activity.UserId)
                                            && !u.IsDeleted
                                            select u).ToListAsync();

                if (userGameNumber != null && userGameNumber.Any())
                {
                    foreach (var user in userGameNumber)
                    {
                        user.TotalGamePlayed += 1;
                    }

                    if (userGameNumber.Count == 1)
                    {
                        if (userGameNumber.First().UserId == activity.ChallengerId)
                        {
                            await _context.UserGamingNumbers.AddAsync(new UserGamingNumber
                            {
                                UserId = activity.UserId,
                                TotalGamePlayed = 1
                            });
                        }
                        else
                        {
                            await _context.UserGamingNumbers.AddAsync(new UserGamingNumber
                            {
                                UserId = activity.ChallengerId,
                                TotalGamePlayed = 1
                            });
                        }
                    }
                }
                else
                {
                    await _context.UserGamingNumbers.AddRangeAsync(new List<UserGamingNumber>
                    {
                        new()
                        {
                            UserId = activity.ChallengerId,
                            TotalGamePlayed = 1
                        },
                        new()
                        {
                            UserId = activity.UserId,
                            TotalGamePlayed = 1
                        }
                    });
                }
            }
            finally
            {
                await _context.SaveChangesAsync();
            }

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
        Task GameOverNotification();
    }
}

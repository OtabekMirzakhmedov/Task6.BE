


using Microsoft.AspNetCore.SignalR;

namespace Task6.BE.Hubs
{
    public class MailHub: Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly MailDbContext _dbContext;

        public MailHub(IDictionary<string, UserConnection> connections, MailDbContext dbContext)
        {
            _connections = connections;
            _dbContext = dbContext;
        }

        public async Task SendMessage(string sender, string receiver, string subject, string message)
        {
            string receiverConnectionId = GetReceiverConnectionId(receiver);

            if (receiverConnectionId != null)
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", sender, subject, message);
            }
            var email = new Email
            {
                SenderName = sender,
                ReceiverName = receiver,
                Subject = subject,
                Body = message
            };

            _dbContext.Emails.Add(email);
            await _dbContext.SaveChangesAsync();
        }

        private string GetReceiverConnectionId(string receiverUsername)
        {
            if (_connections.TryGetValue(receiverUsername, out UserConnection userConnection))
            {
                return userConnection.ConnectionId;
            }

            return null;
        }

        private List<Email> GetUnsentEmails(string receiverUsername)
        {
            return _dbContext.Emails
                .Where(e => e.ReceiverName == receiverUsername)
                .ToList();
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"];

            string connectionId = Context.ConnectionId;

            var userConnection = new UserConnection
            {
                UserName = username,
                ConnectionId = connectionId
            };

            _connections[username] = userConnection;

            var unsentEmails = GetUnsentEmails(username);

            if (unsentEmails.Any())
            {
                foreach (var email in unsentEmails)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", email.SenderName, email.Subject, email.Body);
                }
            }

            await base.OnConnectedAsync();
        }
    }
}

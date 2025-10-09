using Microsoft.AspNetCore.SignalR;

namespace HACT.Hubs
{
    public class NotificationHub : Hub
    {
        // Όταν συνδέεται χρήστης
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
            {
                // Μόνο οι Admins μπαίνουν στο group "Admins"
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }

        // Προαιρετικά: όταν αποσυνδέεται
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Μέθοδος για αποστολή notification
        public async Task SendMessage(string message)
        {
            // Στέλνουμε ΜΟΝΟ στους Admins
            await Clients.Group("Admins").SendAsync("ReceiveMessage", message);
        }
    }
}

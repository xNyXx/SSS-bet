using System.Threading.Tasks;
using WebSocketManager;

namespace Infrastructure.Chat
{
    public class ChatHandler : WebSocketHandler
    {
        private readonly ChatManager _chatManager;
        public ChatHandler(WebSocketConnectionManager manager, ChatManager chatManager) : base(manager)
        {
            _chatManager = chatManager;
        }

        public async Task SendMessage(string socketId, string message) =>
            await InvokeClientMethodToAllAsync("SendMessage", socketId, message);

    }
}

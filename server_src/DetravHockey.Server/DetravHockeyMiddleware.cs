using DetravHockey.Server.Models;
using System.Net.WebSockets;

namespace DetravHockey.Server
{
    public class DetravHockeyMiddleware
    {
        private readonly RequestDelegate _next;

        public DetravHockeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/detravhockey")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await EchoAsync(webSocket);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task EchoAsync(WebSocket socket)
        {
            // 512 kb limit for message size
            var buffer = new byte[1024 * 512];
            int len = 0;

            var receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer, len, buffer.Length - len), CancellationToken.None);

            WSServerClient client = new WSServerClient(socket);


            while (!receiveResult.CloseStatus.HasValue)
            {
                len += receiveResult.Count;

                if (receiveResult.EndOfMessage)
                {
                    client.ProcessPacket(new ReadOnlySpan<byte>(buffer, 0, len));
                    len = 0;
                }

                receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer, len, buffer.Length - len), CancellationToken.None);
            }

            client.Dispose();

            await socket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }

}

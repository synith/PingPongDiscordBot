using Discord;
using Discord.WebSocket;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();

    private DiscordSocketClient _client;
    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        _client.Ready += Ready;
        _client.MessageReceived += MessageReceivedAsync;
        

        var token = File.ReadAllText(@"D:\PingPongBotResources\token.txt");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private Task Ready()
    {
        Console.WriteLine($"Connected as {_client.CurrentUser.Username}");
        return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author == _client.CurrentUser)
            return;
        if (message.Content.ToLower().Contains("ping"))
        {
            await message.Channel.SendMessageAsync($"Pong {_client.Latency}ms");
        }
    }
}
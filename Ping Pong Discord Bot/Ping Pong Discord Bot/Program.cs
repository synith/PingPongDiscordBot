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

        // token stuff
        var token = "OTY5NjA1MDk0MDIwNjI4NDkx.Ymv1HA.K0PKO108Phe46ofLLyghZL09Jek";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
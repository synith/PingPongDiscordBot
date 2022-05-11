using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketClient _client;
    private CommandService _commandService;
    public IServiceProvider _services;
    private CommandHandler _commandHandler;

    public async Task MainAsync()
    {
        // When working with events that have Cacheable<IMessage, ulong> parameters,
        // you must enable the message cache in your config settings if you plan to
        // use the cached message entity.

        var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
        _client = new DiscordSocketClient(_config);

        _commandService = new CommandService();

        var initialize = new Initialize(_commandService, _client);
        _services = initialize.BuildServiceProvider();


        _commandHandler = new CommandHandler(_services, _client, _commandService);

        var token = File.ReadAllText(@"D:\PingPongBotResources\token.txt");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await _commandHandler.InstallCommandsAsync();

        _client.Log += Log;
        
        _client.MessageReceived += MessageReceivedAsync;
        _client.MessageUpdated += MessageUpdated;

        _client.Ready += () =>
        {
            Console.WriteLine($"Connected as {_client.CurrentUser.Username}");
            return Task.CompletedTask;
        };




        await Task.Delay(-1);
    }

    private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        // If the message was not in the cache, downloading it will result in getting a copy of `after`.
        var message = await before.GetOrDownloadAsync();
        Console.WriteLine($"{message} -> {after}");
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
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

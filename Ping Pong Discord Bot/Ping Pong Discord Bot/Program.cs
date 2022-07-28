using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.IO;

public class Program
{
    static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketClient _client;
    private CommandService _commandService;
    private LoggingService _loggingService;
    public IServiceProvider _services;
    private CommandHandler _commandHandler;

    public async Task MainAsync()
    {
        ApiHelper.InitializeClient();

        var _config = new DiscordSocketConfig { MessageCacheSize = 100 };   
        
        _client = new DiscordSocketClient(_config);
        _commandService = new CommandService();
        _loggingService = new LoggingService(_client, _commandService);

        var initialize = new Initialize(_commandService, _client, _loggingService);

        _services = initialize.BuildServiceProvider();


        _commandHandler = new CommandHandler(_services, _client, _commandService);

        var token = File.ReadAllText(@"E:\PingPongBotResources\token.txt");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await _commandHandler.InstallCommandsAsync();
        
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
﻿using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public class Initialize
{
    private readonly CommandService _commands;
    private readonly DiscordSocketClient _client;
    private readonly LoggingService _logging;

    // Ask if there are existing CommandService and DiscordSocketClient
    // instance. If there are, we retrieve them and add them to the
    // DI container; if not, we create our own.
    public Initialize(CommandService commands = null, DiscordSocketClient client = null, LoggingService logging = null)
    {
        _commands = commands ?? new CommandService();
        _client = client ?? new DiscordSocketClient();
        _logging = logging ?? new LoggingService(_client, _commands);
        
    }

    public IServiceProvider BuildServiceProvider() => new ServiceCollection()
        .AddSingleton(_client)
        .AddSingleton(_commands)
        .AddSingleton(_logging)
        // You can pass in an instance of the desired type
        // ...or by using the generic method.
        //
        // The benefit of using the generic method is that 
        // ASP.NET DI will attempt to inject the required
        // dependencies that are specified under the constructor 
        // for us.
        .AddSingleton<CommandHandler>()
        .BuildServiceProvider();
}

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    // Retrieve client and CommandService instance via ctor
    public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands)
    {
        _services = services;
        _commands = commands;
        _client = client;
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived event into our command handler
        _client.MessageReceived += HandleCommandAsync;

        // Here we discover all of the command modules in the entry 
        // assembly and load them. Starting from Discord.NET 2.0, a
        // service provider is required to be passed into the
        // module registration method to inject the 
        // required dependencies.
        //
        // If you do not use Dependency Injection, pass null.
        // See Dependency Injection guide for more information.
        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                        services: _services);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        // Don't process the command if it was a system message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;

        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;

        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        if (!(message.HasCharPrefix('!', ref argPos) ||
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;

        // Create a WebSocket-based command context based on the message
        var context = new SocketCommandContext(_client, message);

        // Execute the command with the command context we just
        // created, along with the service provider for precondition checks.
        await _commands.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: _services);
    }
}
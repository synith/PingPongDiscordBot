using Discord.Commands;
public class IsThereAnyDealModule : ModuleBase<SocketCommandContext>
{
	[Command("itad")]
	[Summary("Check's if there is any deal on specified game")]
	public Task SayAsync([Remainder][Summary("The game to search for")] string gameTitle)
	{
		// do the things to determine the data in the message reply
		return ReplyAsync("Info about any deals on the game");
	}
}
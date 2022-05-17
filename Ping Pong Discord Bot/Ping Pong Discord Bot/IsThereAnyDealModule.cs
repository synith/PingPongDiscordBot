using Discord.Commands;
public class IsThereAnyDealModule : ModuleBase<SocketCommandContext>
{
    [Command("itad")]
    [Summary("Check's if there is any deal on specified game")]
    public async Task SayAsync([Remainder][Summary("The game to search for")] string gameTitle)
    {
        // do the ITAD things
        var key = File.ReadAllText(@"D:\PingPongBotResources\key.txt");
        GamePriceData gamePriceData = await GameSearchProcessor.LoadSearchResults(key, gameTitle);

        
        string responseString = $"Game is on sale for {gamePriceData.Price_New:C} at {gamePriceData.Price_Cut}% off.\n" + gamePriceData.Url;



        await ReplyAsync(responseString);
    }
}
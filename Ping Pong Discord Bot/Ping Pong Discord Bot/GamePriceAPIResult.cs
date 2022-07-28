using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class GameSearchResult
{
    public Dictionary<string, GameData> Data { get; set; }
}
public class GameData
{
    public List<GamePriceData> List { get; set; }
}
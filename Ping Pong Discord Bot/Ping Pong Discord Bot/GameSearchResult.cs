using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


public class GameSearchResult
{
    public GameSearchResultData Data { get; set; }

    public class GameSearchResultData
    {
        public GameData Stellaris { get; set; }

        public class GameData
        {
            public List<GamePriceData> List { get; set; }
        }
    }
}


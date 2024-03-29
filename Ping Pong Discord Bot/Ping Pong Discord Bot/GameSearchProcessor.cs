﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;

public class GameSearchProcessor
{
    public static async Task<GamePriceData> LoadSearchResults(string key, string plains)
    {
        string url = "https://api.isthereanydeal.com/v01/game/prices/?key=" + key + "&plains=" + plains;

        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
        {
            if (response.IsSuccessStatusCode)
            {
                GameSearchResult result = await response.Content.ReadAsAsync<GameSearchResult>();

                return result.Data.First().Value.List[0];
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public static async Task<GamePriceData> ConvertSearchQueryToPlain(string key, string title)
    {
        string encodedTitle = HttpUtility.UrlEncode(title);

        string url = "https://api.isthereanydeal.com/v01/game/plain/?key=" + key + "&title=" + encodedTitle;

        using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
        {
            if (response.IsSuccessStatusCode)
            {
                GameSearchResult result = await response.Content.ReadAsAsync<GameSearchResult>();

                return result.Data.First().Value.List[0];
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}


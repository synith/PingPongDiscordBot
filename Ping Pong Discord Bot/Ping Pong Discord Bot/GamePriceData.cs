using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GamePriceData
{
    public float Price_New { get; set; }
    public float Price_Old { get; set; }
    public int Price_Cut { get; set; }
    public string Url { get; set; }
    public Shop Shop { get; set; }
    public List<string> Drm { get; set; }
}

public class Shop
{
    public string Id { get; set; }
    public string Name { get; set; }
}
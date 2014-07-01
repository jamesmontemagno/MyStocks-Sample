using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Shared.Models.History
{
  [JsonObject("quote")]
  public class HistoryQuote
  {
    public string Symbol { get; set; }
    public string Date { get; set; }
    public string Open { get; set; }
    public string High { get; set; }
    public string Low { get; set; }
    public string Close { get; set; }
    public string Volume { get; set; }
    public string Adj_Close { get; set; }

    public float Value
    {
      get { return float.Parse(Close); }
    }
  }
}

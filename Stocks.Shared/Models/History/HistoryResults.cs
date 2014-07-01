using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Shared.Models.History
{
  [JsonObject("results")]
  public class HistoryResults
  {
    [JsonProperty("quote")]
    public List<HistoryQuote> Quotes { get; set; }
  }
}

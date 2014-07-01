using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Shared.Models.History
{
  [JsonObject("query")]
  public class HistoryQuery
  {
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("created")]
    public string Created { get; set; }

    [JsonProperty("lang")]
    public string Language { get; set; }

    [JsonProperty("results")]
    public HistoryResults Results { get; set; }
  }
}

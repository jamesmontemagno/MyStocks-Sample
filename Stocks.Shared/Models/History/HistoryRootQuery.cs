using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Shared.Models.History
{
  [JsonObject("RootObject")]
  public class HistoryRootQuery
  {
    [JsonProperty("query")]
    public HistoryQuery Query { get; set; }
  }
}

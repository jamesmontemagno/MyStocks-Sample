using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Shared.Models.Quote
{
  public class RootQuery
  {
    [JsonProperty("query")]
    public Query Query { get; set; }
  }
}

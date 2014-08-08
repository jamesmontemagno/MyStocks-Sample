using Newtonsoft.Json;
using Stocks.Shared.Models.History;
using Stocks.Shared.Models.Quote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Stocks.Shared.ViewModels
{
  public class StockViewModel : INotifyPropertyChanged
  {
    #region Properties
    private string quoteDisplay = "0.00";
    public string QuoteDisplay
    {
      get { return quoteDisplay; }
      set { quoteDisplay = value; OnPropertyChanged("QuoteDisplay"); }
    }

    private string symbol = "MSFT";
    public string Symbol
    {
      get { return symbol; }
      set { symbol = value; OnPropertyChanged("Symbol"); }
    }

    private bool isBusy;
    public bool IsBusy
    {
      get { return isBusy; }
      set { isBusy = value; OnPropertyChanged("IsBusy"); }
    }
    #endregion

    private const string QuoteUrl = @"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%3D%22{0}%22&format=json&diagnostics=true&env=http%3A%2F%2Fdatatables.org%2Falltables.env&callback=";

    public async Task<Quote> GetQuote(string symbol)
    {
      IsBusy = true;
      var client = new HttpClient();

      try
      {
        var json = await client.GetStringAsync(string.Format(QuoteUrl, symbol));

        var results = JsonConvert.DeserializeObject<RootQuery>(json);

        if (results.Query.Results.Quote.Error != null)
          return null;

        var quote = results.Query.Results.Quote;
        QuoteDisplay = quote.CurrentQuote + " | " + quote.Change;

        return quote;
      }
      catch (Exception ex)
      {
        QuoteDisplay = "Invalid";
        return null;
      }
      finally
      {
        IsBusy = false;
      }
    }


    private const string HistoryUrl = @"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.historicaldata%20where%20symbol=%22{0}%22%20and%20startDate%20%3D%20%22{1}%22%20and%20endDate%20%3D%20%22{2}%22&format=json&diagnostics=true&env=http%3A%2F%2Fdatatables.org%2Falltables.env&callback=";

    public Task<List<HistoryQuote>> GetHistory(string symbol)
    {
      return GetHistory(symbol, DateTime.Today.AddDays(-14), DateTime.Today);
    }
    public async Task<List<HistoryQuote>> GetHistory(string symbol, DateTime startDate, DateTime endDate)
    {
      IsBusy = true;
      var client = new HttpClient();

      if (endDate < startDate)
        endDate = startDate;

      try
      {
        var url = string.Format(HistoryUrl, symbol,
          startDate.ToString("yyyy-MM-dd"),
          endDate.ToString("yyyy-MM-dd"));

        var json = await client.GetStringAsync(url);

        var history = JsonConvert.DeserializeObject<HistoryRootQuery>(json);
        var items = history.Query.Results.Quotes;
        items.Reverse();
        return items;
      }
      catch (Exception ex)
      {
        return null;
      }
      finally
      {
        IsBusy = false;
      }

    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged == null)
        return;

      PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}

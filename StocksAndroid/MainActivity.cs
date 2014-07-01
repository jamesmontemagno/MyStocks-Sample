using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Stocks.Shared.ViewModels;
using Android.Graphics;
using System.Linq;
using BarChart;
using System.Collections.Generic;

namespace StocksAndroid
{
  [Activity(Label = "StocksAndroid", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : Activity
  {
    int count = 1;
    StockViewModel viewModel = new StockViewModel();
    BarChartView barChart;
    IEnumerable<BarChart.BarModel> data;
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      // Get our button from the layout resource,
      // and attach an event to it
      Button button = FindViewById<Button>(Resource.Id.MyButton);

      var quoteLable = FindViewById<TextView>(Resource.Id.textView1);
      var symbol = FindViewById<EditText>(Resource.Id.editText1);
      var progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
      barChart = FindViewById<BarChart.BarChartView>(Resource.Id.barChart);


      progressBar.Visibility = ViewStates.Invisible;
      quoteLable.Text = string.Empty;

      button.Click += async (sender, args) =>
        {

          progressBar.Visibility = ViewStates.Visible;

          if (string.IsNullOrWhiteSpace(symbol.Text))
          {
            quoteLable.Text = "Invalid";
            progressBar.Visibility = ViewStates.Invisible;
            return;
          }

          var quote = await viewModel.GetQuote(symbol.Text.Trim());

          if (quote != null)
          {
            quoteLable.Text = quote.CurrentQuote + " | " + quote.Change;

            quoteLable.SetTextColor(quote.StockIsUp ? Color.Green : Color.Red);
          }


          var items = await viewModel.GetHistory(
          symbol.Text.Trim(),
          DateTime.Today.AddDays(-14),
          DateTime.Today);

          if (items != null)
          {

            data = items.Select(
              s => new BarChart.BarModel
              {
                Value = s.Value,
                Legend = s.Date
              });
            RunOnUiThread(() =>
            {
              barChart.ItemsSource = data;
            });
          }


          progressBar.Visibility = ViewStates.Invisible;
        };
    }
  }
}


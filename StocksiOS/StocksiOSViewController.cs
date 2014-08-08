using System;
using System.Linq;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using Stocks.Shared.ViewModels;
using BarChart;
using System.Drawing;
using Stocks.Shared.Helpers;

namespace StocksiOS
{
  public partial class StocksiOSViewController : UIViewController
  {
    private readonly StockViewModel viewModel = new StockViewModel();

    public StocksiOSViewController(IntPtr handle)
      : base(handle)
    {
    }

    /// <summary>
    /// add bar chart in code behind until component is updated for compat with designer.
    /// </summary>
    BarChartView barChart;
    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      barChart = new BarChartView
      {
        Frame = new RectangleF(0, LabelQuote.Frame.Bottom, View.Frame.Width, View.Frame.Height - LabelQuote.Frame.Bottom),
        LegendHidden = true,
        BarColor = Color.Blue.ToUIColor()
      };

      View.AddSubview(barChart);
    }

    async partial void ButtonQuote_TouchUpInside(UIButton sender)
    {
      if (string.IsNullOrWhiteSpace(TextSymbol.Text))
      {
        var alert = new UIAlertView("No symbol", string.Empty, null, "OK");
        alert.Show();

        return;
      }

      TextSymbol.ResignFirstResponder();
      ProgressBar.StartAnimating();

      var quoteTask = viewModel.GetQuote(TextSymbol.Text);
      var historyTask = viewModel.GetHistory(
          TextSymbol.Text,
          DateTime.Today.AddDays(-14),
          DateTime.Today);

      await Task.WhenAll(quoteTask, historyTask);

      var quote = quoteTask.Result;
      if (quote == null)
      {
        LabelQuote.Text = "Invalid";
        ProgressBar.StopAnimating();

        return;
      }

      LabelQuote.Text = quote.CurrentQuote + " | " + quote.Change;
      LabelQuote.TextColor = quote.StockIsUp ? UIColor.Green : UIColor.Red;

      var items = historyTask.Result;
      if (items != null)
      {
        barChart.ItemsSource = items.Select(
            s => new BarChart.BarModel
            {
              Value = s.Value
            }).ToList();
      }

      ProgressBar.StopAnimating();
    }
  }
}
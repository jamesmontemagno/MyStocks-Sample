using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using StocksWindowsPhone.Resources;
using System.Windows.Media;
using Stocks.Shared.ViewModels;

namespace StocksWindowsPhone
{
  public partial class MainPage : PhoneApplicationPage
  {
    private StockViewModel viewModel;
    // Constructor
    public MainPage()
    {
      InitializeComponent();


      this.DataContext = viewModel = new StockViewModel();
      // Sample code to localize the ApplicationBar
      //BuildLocalizedApplicationBar();
    }

    SolidColorBrush red = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    SolidColorBrush green = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
    private async void ButtonGetQuote_Click(object sender, RoutedEventArgs e)
    {
      var quote = await viewModel.GetQuote(viewModel.Symbol);
      if (quote == null)
        return;

      TextQuote.Foreground = quote.StockIsUp ? green : red;
    }

    // Sample code for building a localized ApplicationBar
    //private void BuildLocalizedApplicationBar()
    //{
    //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
    //    ApplicationBar = new ApplicationBar();

    //    // Create a new button and set the text value to the localized string from AppResources.
    //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
    //    appBarButton.Text = AppResources.AppBarButtonText;
    //    ApplicationBar.Buttons.Add(appBarButton);

    //    // Create a new menu item with the localized string from AppResources.
    //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
    //    ApplicationBar.MenuItems.Add(appBarMenuItem);
    //}
  }
}
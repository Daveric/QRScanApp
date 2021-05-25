using Microsoft.Identity.Client;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScanApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LoginPage : ContentPage
  {
    public LoginPage()
    {
      InitializeComponent();
    }
    protected override async void OnAppearing()
    {
      try
      {
        // Look for existing account
        var accounts = await App.AuthenticationClient.GetAccountsAsync();

        var enumerable = accounts.ToList();
        if (enumerable.Any())
        {
          var result = await App.AuthenticationClient
            .AcquireTokenSilent(Constants.Scopes, enumerable.FirstOrDefault())
            .ExecuteAsync();

          await Navigation.PushAsync(new MainPage(result));
        }
      }
      catch
      {
        // Do nothing - the user isn't logged in
      }
      base.OnAppearing();
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
      try
      {
        var result = await App.AuthenticationClient
          .AcquireTokenInteractive(Constants.Scopes)
          .WithPrompt(Prompt.ForceLogin)
          .WithParentActivityOrWindow(App.UIParent)
          .ExecuteAsync();

        await Navigation.PushAsync(new MainPage(result));
      }
      catch (MsalClientException)
      {

      }
    }
  }
}
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using ScanApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScanApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LoginPage : ContentPage
  {
    private static string _userName;
    private static string _userRole;
    private readonly bool _logOut;
    public LoginPage(bool logOut)
    {
      _logOut = logOut;
      InitializeComponent();
    }

    protected override async void OnAppearing()
    {
      AuthenticationResult authenticationResult = null;

      // Look for existing account
      var accounts = await App.AuthenticationClient.GetAccountsAsync();
      var enumerable = accounts.ToList();
      if (enumerable.Any())
      {
        try
        {
          authenticationResult = await App.AuthenticationClient
            .AcquireTokenSilent(Constants.Scopes, enumerable.FirstOrDefault())
            .ExecuteAsync();
        }
        catch
        {
          await App.AuthenticationClient.RemoveAsync(enumerable.FirstOrDefault());
          authenticationResult = await LogIn();
        }

        if (_logOut && authenticationResult != null)
        {
          await App.AuthenticationClient.RemoveAsync(authenticationResult.Account);
          Application.Current.MainPage = new LoginPage(false);
        }
      }
      else
      {
        authenticationResult = await LogIn();
      }

      GetClaims(authenticationResult);

      var mainViewModel = MainViewModel.GetInstance();
      mainViewModel.UserName = _userName;
      Application.Current.MainPage = new MainPage(_userRole);
      base.OnAppearing();
    }

    private static async Task<AuthenticationResult> LogIn()
    {
      try
      {
        return await App.AuthenticationClient
          .AcquireTokenInteractive(Constants.Scopes)
          .WithPrompt(Prompt.ForceLogin)
          .WithParentActivityOrWindow(App.UIParent)
          .ExecuteAsync();
      }
      catch (MsalClientException ex)
      {
        if (ex.ErrorCode == MsalError.AuthenticationCanceledError)
        {
          Process.GetCurrentProcess().CloseMainWindow();
        }
        await Application.Current.MainPage.DisplayAlert(
          "Error",
          ex.Message,
          "Accept");
        return await LogIn();
      }
    }
    
    private static void GetClaims(AuthenticationResult result)
    {
      var token = result.IdToken;
      if (token == null) return;
      var handler = new JwtSecurityTokenHandler();
      var data = handler.ReadJwtToken(token);
      _userName = data.Claims.FirstOrDefault(x => x.Type.Equals("given_name"))?.Value;
      _userRole = data.Claims.FirstOrDefault(x => x.Type.Contains("Role"))?.Value;
    }
  }

  public interface ICloseApplication
  {
    void CloseApplication();
  }
}
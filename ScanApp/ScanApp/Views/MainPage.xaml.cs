using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ScanApp.Views;
using Xamarin.Forms;

namespace ScanApp
{
  public partial class MainPage : ContentPage
  {

    private readonly AuthenticationResult _authenticationResult;
    private string _userName = string.Empty;
    public MainPage(AuthenticationResult authResult)
    {
      _authenticationResult = authResult;
      InitializeComponent();
    }
    protected override void OnAppearing()
    {
      GetClaims();
      base.OnAppearing();
    }

    private void GetClaims()
    {
      var token = _authenticationResult.IdToken;
      if (token == null) return;
      var handler = new JwtSecurityTokenHandler();
      var data = handler.ReadJwtToken(_authenticationResult.IdToken);
      Label.Text = $"Welcome {data.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}\n" +
                   $" please hit the button to scan the QRCode";
      _userName = data.Claims.FirstOrDefault(x => x.Type.Equals("emails"))?.Value;
    }

    private async void OnScanClicked(object sender, EventArgs e)
    {
      try
      {
        var scanner = DependencyService.Get<IQrScanningService>();
        var result = await scanner.ScanAsync();
        if (result != null)
        {
          SetUserToApp(result);
        }
      }
      catch (Exception ex)
      {

        throw;
      }
    }

    private async void OnSignOutClicked(object sender, EventArgs e)
    {
      await App.AuthenticationClient.RemoveAsync(_authenticationResult.Account);
      await Navigation.PushAsync(new LoginPage());
    }

    private async void SetUserToApp(string appId)
    {
      var request = new UserRequest
      {
        AppId = appId,
        Email = _userName
      };
      var url = Application.Current.Resources["UrlAPI"].ToString();
      var response = await PostUserToAppAsync(
        url,
        "/api",
        "/Applications/SetUserToApp",
        request);

      if (!response.IsSuccess)
      {
        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
          "Error",
          response.Message,
          "Accept");
        return;
      }

      await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
        "Ok",
        response.Message,
        "Accept");
    }

    private async Task<Response> PostUserToAppAsync(string urlBase,
      string servicePrefix,
      string controller,
      UserRequest setUserToAppRequest)
    {
      try
      {
        var request = JsonConvert.SerializeObject(setUserToAppRequest);
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        var client = new HttpClient
        {
          BaseAddress = new Uri(urlBase)
        };

        var url = $"{servicePrefix}{controller}";
        var response = await client.PostAsync(url, content);
        var answer = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<Response>(answer);
        return obj;
      }
      catch (Exception ex)
      {
        return new Response
        {
          IsSuccess = false,
          Message = ex.Message,
        };
      }
    }
  }
}

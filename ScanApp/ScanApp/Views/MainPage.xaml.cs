using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ScanApp.Models;
using Xamarin.Forms;

namespace ScanApp.Views
{
  public partial class MainPage : ContentPage
  {
    private readonly AuthenticationResult _authenticationResult;
    private bool _userHasAccess = false;
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
                   "please hit the button to scan the QRCode";
      _userHasAccess = Convert.ToBoolean(data.Claims.FirstOrDefault(x => x.Type.Equals("ThirdLevelAuth"))?.Value);
    }

    private async void OnScanClicked(object sender, EventArgs e)
    {
      var scanner = DependencyService.Get<IQrScanningService>();
      var result = await scanner.ScanAsync();
      if (result != null)
      {
        SetAccessToMachine(result);
      }
    }

    private async void OnSignOutClicked(object sender, EventArgs e)
    {
      await App.AuthenticationClient.RemoveAsync(_authenticationResult.Account);
      await Navigation.PushAsync(new LoginPage());
    }

    private async void SetAccessToMachine(string appId)
    {
      var request = new MachineUpdateModel()
      {
        Id = appId,
        UserHasAccess = _userHasAccess
      };
      var url = Application.Current.Resources["UrlAPI"].ToString();
      var response = await Update(
        url,
        "/api",
        "/machine/",
        request);

      if (!response.IsSuccess)
      {
        await Application.Current.MainPage.DisplayAlert(
          "Error",
          response.Message,
          "Accept");
        return;
      }

      await Application.Current.MainPage.DisplayAlert(
        "Ok",
        response.Message,
        "Accept");
    }

    private static async Task<Response> Update(string urlBase,
      string servicePrefix,
      string controller,
      MachineUpdateModel machine)
    {
      try
      {
        var request = JsonConvert.SerializeObject(machine);
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        var client = new HttpClient
        {
          BaseAddress = new Uri(urlBase)
        };

        var url = $"{servicePrefix}{controller}";
        var response = await client.PutAsync(url, content);
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

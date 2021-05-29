using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScanApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScanApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ScanPage : ContentPage
  {
    public string UserRole { get; set; }

    public ScanPage()
    {
      InitializeComponent();
    }

    private async void OnScanClicked(object sender, EventArgs e)
    {
      var scanner = DependencyService.Get<IQrScanningService>();
      var result = await scanner.ScanAsync();
      if (result != null)
      {
        GetAccess(result);
      }
    }
    private async void GetAccess(string appId)
    {
      var request = new UnlockRequestSetAccessModel()
      {
        RequestId = appId,
        UserRole = UserRole
      };
      var url = Application.Current.Resources["UrlAPI"].ToString();
      var response = await UnlockAccess(
        url,
        "/api",
        "/unlock/access",
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

    private static async Task<Response> UnlockAccess(string urlBase,
      string servicePrefix,
      string controller,
      UnlockRequestSetAccessModel requestSetAccess)
    {
      try
      {
        var request = JsonConvert.SerializeObject(requestSetAccess);
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
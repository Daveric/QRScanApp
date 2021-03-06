using Microsoft.Identity.Client;
using ScanApp.Views;
using Xamarin.Forms;

namespace ScanApp
{
  public partial class App : Application
  {
    public static NavigationPage Navigator { get; internal set; }
    public static MainPage Main { get; internal set; }
    public static IPublicClientApplication AuthenticationClient { get; private set; }

    public static object UIParent { get; set; } = null;

    public App()
    {
      InitializeComponent();
      AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
        .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
        .WithB2CAuthority(Constants.AuthoritySignIn)
        .WithRedirectUri($"msal{Constants.ClientId}://auth")
        .Build();

      MainPage = new LoginPage(false);
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
  }
}

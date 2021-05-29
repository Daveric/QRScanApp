using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScanApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class MainPage : FlyoutPage
  {
    public MainPage(string userRole)
    {
      InitializeComponent();
      InitialPage.UserRole = userRole;
    }

    protected override void OnAppearing()
    {
      App.Navigator = Navigator;
      App.Main = this;
    }

  }
}

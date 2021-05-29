
using ScanApp.Views;

namespace ScanApp.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class MenuItemViewModel : Models.Menu
    {
        public ICommand SelectMenuCommand => new RelayCommand(SelectMenu);

        private async void SelectMenu()
        {
            App.Main.IsPresented = false;

            switch (PageName)
            {
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;
                default:
                    Application.Current.MainPage =new LoginPage(Title.Equals("Close session"));
                    break;
            }
        }
    }
}

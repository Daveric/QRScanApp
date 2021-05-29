
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ScanApp.Models;

namespace ScanApp.ViewModels
{
  public class MainViewModel
  {
    private static MainViewModel _instance;
    public string UserName { get; set; }
    public ObservableCollection<MenuItemViewModel> Menus { get; set; }

    public MainViewModel()
    {
      _instance = this;
      LoadMenus();
    }

    private void LoadMenus()
    {
      var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_info",
                    PageName = "AboutPage",
                    Title = "About"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = "Close session"
                }
            };

      Menus = new ObservableCollection<MenuItemViewModel>(
          menus.Select(m => new MenuItemViewModel
          {
            Icon = m.Icon,
            PageName = m.PageName,
            Title = m.Title
          }).ToList());
    }

    public static MainViewModel GetInstance()
    {
      return _instance ?? new MainViewModel();
    }
  }
}

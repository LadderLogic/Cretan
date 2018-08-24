using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cretan.ViewModels
{
    /// <summary>
    /// View Model for the main menu. 
    /// </summary>
    public class MainMenuViewModel:BaseViewModel
    {

        public DelegateCommand<string> Navigation { get; private set; }

        private INavigationService _navigationService;

        public MainMenuViewModel(INavigationService navigationService)
        {
            Navigation = new DelegateCommand<string>(DoNavigation);
            _navigationService = navigationService;
        }

        private void DoNavigation(string navigationStack)
        {
            _navigationService.NavigateAsync(navigationStack);
        }
    }
}

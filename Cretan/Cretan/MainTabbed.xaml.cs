using Cretan.Contracts;
using Cretan.ViewModels;
using Cretan.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cretan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbed : TabbedPage
    {
        public MainTabbed ()
        {
            InitializeComponent();
            Page definitionPage, aboutPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    definitionPage = new NavigationPage(new PaceDefinition())
                    {
                        Title = "Session"
                    };

                    aboutPage = new NavigationPage(new AboutPage())
                    {
                        Title = "About"
                    };
                    definitionPage.Icon = "tab_feed.png";
                    aboutPage.Icon = "tab_about.png";
                    break;
                default:
                    definitionPage = new PaceDefinition()
                    {
                        Title = "Session"
                    };

                    //aboutPage = new AboutPage()
                    //{
                    //    Title = "About"
                    //};
                    break;
            }

            Children.Add(definitionPage);
            //Children.Add(aboutPage);

            Title = "Cretan";

            MessagingCenter.Subscribe<PaceDefinitionViewModel, SessionSetting>(this, Messages.StartSession, async (obj, item) =>
            {
                var sessionSetting = item;

                await Navigation.PushAsync(new Session(sessionSetting));
            });



        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            //Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
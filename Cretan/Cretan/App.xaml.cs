using Cretan.Services;
using Cretan.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Cretan
{
	public partial class App : Prism.DryIoc.PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //await NavigationService.NavigateAsync("MainNavigation/MainMenu");
            await NavigationService.NavigateAsync("MainMenu");

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainMenu>();
            containerRegistry.RegisterForNavigation<FreeRunSetup>();
            containerRegistry.RegisterForNavigation<GoPage>();
            containerRegistry.RegisterForNavigation<SessionSummary>();
            containerRegistry.RegisterForNavigation<CretanPath>();

            // register services. move them to different module?
            containerRegistry.RegisterSingleton<IPaceKeeper, PaceKeeper>();

            //containerRegistry.RegisterForNavigation<MainNavigation>();
        }
    }
}

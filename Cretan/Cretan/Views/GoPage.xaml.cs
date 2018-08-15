using Cretan.Contracts;
using Cretan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cretan.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GoPage : ContentPage
	{
		public GoPage (SessionSetting sessionSettings)
		{
			InitializeComponent ();
            BindingContext = new GoViewModel(sessionSettings);
		}
	}
}
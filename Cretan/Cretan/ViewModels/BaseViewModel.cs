
using Cretan.Contracts;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace Cretan
{
    public class BaseViewModel : BindableBase
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string _debugString = string.Empty;

        public string DebugString
        {
            get { return _debugString; }
            set { SetProperty(ref _debugString, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }



    }
}

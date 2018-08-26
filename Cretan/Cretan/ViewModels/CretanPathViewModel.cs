using Cretan.Contracts;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cretan.ViewModels
{
	public class CretanPathViewModel : BindableBase
	{
        public CretanPathViewModel()
        {
            var dummyPack = new ProgramPack("dummy", "A dummy pack for the dummies");
            dummyPack.Programs.AddFirst(new ProgramSetting("Test1", "Test Description"));
            dummyPack.Programs.AddFirst(new ProgramSetting("Test2", "Test 2 Description"));
            dummyPack.Programs.AddFirst(new ProgramSetting("Test3", "Test 3 Description"));
            dummyPack.Programs.AddFirst(new ProgramSetting("Test4", "Test 4 Description"));
            SelectedPack = dummyPack;
        }

        private ProgramPack _selectedPack;
        public ProgramPack SelectedPack
        {
            get { return _selectedPack; }
            set { SetProperty(ref _selectedPack, value); }
        }
    }
}

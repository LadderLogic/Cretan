using Cretan.Contracts;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cretan.ViewModels
{


	public class CretanPathViewModel : BindableBase
	{
        public CretanPathViewModel(INavigationService navigationService)
        {
            Start = new DelegateCommand(StartProgram);
            _navigationService = navigationService;
            var programTest = new ProgramSetting("Working test", "Only working program");
            var firstSegment = new SegmentSetting() { Duration = TimeSpan.FromMinutes(5), TargetPaceInMph = 4 };
            var secondSegment = new SegmentSetting() { Duration = TimeSpan.FromMinutes(10), TargetPaceInMph = 6 };
            programTest.Segments.AddFirst(secondSegment);
            programTest.Segments.AddFirst(firstSegment);

            var dummyPack = new ProgramPack("dummy", "A dummy pack for the dummies");
            dummyPack.Programs.AddFirst(new ProgramSetting("Test1", "Test Description"));
            dummyPack.Programs.AddFirst(new ProgramSetting("Test2", "Test 2 Description"));
            dummyPack.Programs.AddFirst(new ProgramSetting("Test3", "Test 3 Description"));
            dummyPack.Programs.AddFirst(programTest);

            

            SelectedPack = dummyPack;
        }



        private void StartProgram()
        {
            var navParams = new NavigationParameters();
            navParams.Add(nameof(ProgramSetting), SelectedProgram);
            _navigationService.NavigateAsync("GoPage", navParams, true);
        }

        private ProgramPack _selectedPack;

        public ProgramPack SelectedPack
        {
            get { return _selectedPack; }
            set { SetProperty(ref _selectedPack, value); }
        }

        private ProgramSetting _selectedProgram;
        public ProgramSetting SelectedProgram
        {
            get { return _selectedProgram; }
            set { SetProperty(ref _selectedProgram, value); }

        }

        public DelegateCommand Start { get; }

        private INavigationService _navigationService;
    }
}

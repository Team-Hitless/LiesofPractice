using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using LiesOfPractice.Properties;
using LiesOfPractice.Viewmodels;
using System.Collections.ObjectModel;

namespace LiesOfPractice.Services;

public class DataService : OberservableObject, IDataService
{
    private readonly INavigationService _navigationService;
    private Page? _selectedPage;

    public DataService(INavigationService navigationService)
    {
        _navigationService = navigationService;
        LoadAppSettings();
        InitPages();
    }
    private Settings Settings { get; set; } = new();

    public AppSettings AppSettings { get; set; } = new();
    public ObservableCollection<Page> Pages { get; set; } = [];
    public Page? SelectedPage
    {
        get => _selectedPage;
        set
        {
            if (_selectedPage == value)
                return;

            _selectedPage = value;
            OnPropertyChanged(nameof(SelectedPage));
            _selectedPage?.Command.Execute(null);
        }
    }

    private void LoadAppSettings()
    {
        Settings.Reload();

        AppSettings.CheckforUpdates = Settings.CheckforUpdates;
        AppSettings.Gamepath = Settings.Gamepath;
        AppSettings.Steampath = Settings.Steampath;
    }

    public void SaveAppSettings()
    {
        Settings.CheckforUpdates = AppSettings.CheckforUpdates;
        Settings.Gamepath = AppSettings.Gamepath;
        Settings.Steampath = AppSettings.Steampath;

        Settings.Save();
    }

    private void InitPages()
    {
        Pages =
        [
            new Page {Name = "Player", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Utility", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Enemies", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Items", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Settings", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
        ];
        SelectedPage = Pages[0];
    }
}

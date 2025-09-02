using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using LiesOfPractice.Properties;
using LiesOfPractice.Viewmodels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace LiesOfPractice.Services;

public class DataService : OberservableObject, IDataService
{
    static readonly string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\LiesOfPractice";
    static readonly string _filePath = $@"{_path}\Settings.json";
    

    private readonly INavigationService _navigationService;
    private readonly IJsonService _jsonService;
    private Page? _selectedPage;
    private bool _isLoaded;

    public DataService(INavigationService navigationService, IJsonService jsonService)
    {
        _navigationService = navigationService;
        _jsonService = jsonService;
        LoadAppSettings();
        InitPages();
    }
    public bool IsLoaded
    {
        get => _isLoaded;
        set
        {
            _isLoaded = value;
            OnPropertyChanged(nameof(IsLoaded));
        }
    }
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
    private async void LoadAppSettings() => AppSettings = await _jsonService.DeserializeAsync<AppSettings>(_filePath);
    public void SaveAppSettings() => _jsonService.SerializeAsync(AppSettings, _path, _filePath);

    private void InitPages()
    {
        Pages =
        [
            new Page {Name = "Player", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Utility", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Enemies", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Items", Command = new DelegateCommand(obj => _navigationService.NavigateTo<PlayerViewModel>())},
            new Page {Name = "Settings", Command = new DelegateCommand(obj => _navigationService.NavigateTo<SettingsViewModel>())},
        ];
        SelectedPage = Pages[0];
    }
}

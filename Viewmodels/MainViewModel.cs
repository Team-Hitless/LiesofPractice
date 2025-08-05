using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using LiesOfPractice.Properties;
using LiesOfPractice.Services;
using System.Windows.Input;

namespace LiesOfPractice.Viewmodels;
public class MainViewModel: ViewModelBase
{
    private readonly IGameLaunchService _gameLaunchService;
    private readonly IDataService _dataService;

    public MainViewModel(IGameLaunchService gameLaunchService, IDataService dataService)
    {
        _gameLaunchService = gameLaunchService;
        _dataService = dataService;
        LaunchGameCommand = new DelegateCommand(LaunchGame);
        SelectGamepathCommand = new DelegateCommand(SelectGamepath);
    }
    #region Commands
    public ICommand LaunchGameCommand { get; set; }
    public ICommand SelectGamepathCommand { get; set; }
    #endregion

    #region Public Properies
    public AppSettings AppSettings
    {
        get => _dataService.AppSettings;
        set => _dataService.AppSettings = value;
    }
    #endregion

    #region Private Properies

    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void LaunchGame(object? obj) => _gameLaunchService.LaunchGame();
    private void SelectGamepath(object? obj) => _gameLaunchService.SelectGamepath();
    #endregion
}

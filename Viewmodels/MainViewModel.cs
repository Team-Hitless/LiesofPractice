using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using System.Windows.Input;

namespace LiesOfPractice.Viewmodels;
public class MainViewModel: ViewModelBase
{
    private readonly IGameLaunchService _gameLaunchService;

    public MainViewModel(IGameLaunchService gameLaunchService)
    {
        _gameLaunchService = gameLaunchService;
        LaunchGameCommand = new DelegateCommand(LaunchGame);
    }
    #region Commands
    public ICommand LaunchGameCommand { get; set; }
    #endregion

    #region Public Properies
    #endregion

    #region Private Properies

    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void LaunchGame(object? obj) => _gameLaunchService.LaunchGame();
    #endregion
}

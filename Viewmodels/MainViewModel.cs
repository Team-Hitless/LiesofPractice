using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using LiesOfPractice.Services;
using System.Windows.Input;
using System.Windows.Threading;
using LiesOfPractice.Memory;

namespace LiesOfPractice.Viewmodels;
public class MainViewModel : ViewModelBase
{
    private readonly IGameLaunchService _gameLaunchService;
    private readonly IDataService _dataService;
    private readonly IMemoryIoService _memoryIo;
    private readonly TempService _tempService;
    private readonly AoBScanner _aoBScanner;
    private readonly DispatcherTimer _gameTimer;

    private bool _hasScanned = false;

    public MainViewModel(IGameLaunchService gameLaunchService, IMemoryIoService memoryIo, AoBScanner aoBScanner, TempService tempService, IDataService dataService)
    {
        _gameLaunchService = gameLaunchService;
        _memoryIo = memoryIo;
        _aoBScanner = aoBScanner;
        _tempService = tempService;

        _dataService = dataService;
        LaunchGameCommand = new DelegateCommand(LaunchGame);
        SelectGamepathCommand = new DelegateCommand(SelectGamepath);

        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(25)
        };
        _gameTimer.Tick += Timer_Tick;
        _gameTimer.Start();

    }
    #region Commands
    public ICommand LaunchGameCommand { get; set; }
    public ICommand SelectGamepathCommand { get; set; }
    #endregion

    #region Public Properies
    private bool _isAttached;
    public bool IsAttached
    {
        get => _isAttached;
        set
        {
            if (_isAttached == value) return;
            _isAttached = value;

            OnPropertyChanged(nameof(IsAttached));
        }
    }
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

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_memoryIo.IsAttached)
        {
            IsAttached = true;
            if (!_hasScanned)
            {
                _aoBScanner.Scan();
                _hasScanned = true;
                Console.WriteLine($"Base: 0x{_memoryIo.BaseAddress.ToInt64():X}");
            }
        }
        else
            IsAttached = false;
    }
    private void SelectGamepath(object? obj) => _gameLaunchService.SelectGamepath();
    #endregion
}

using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using System.Windows.Input;
using System.Windows.Threading;
using LiesOfPractice.Memory;
using LiesOfPractice.Services;

namespace LiesOfPractice.Viewmodels;
public class MainViewModel: ViewModelBase
{
    private readonly IGameLaunchService _gameLaunchService;
    private readonly MemoryIo _memoryIo;
    private readonly TempService _tempService;
    private readonly AoBScanner _aoBScanner;
    private readonly DispatcherTimer _gameTimer;
    
    private bool _hasScanned;
    private bool _hasAllocatedMemory;

    public MainViewModel(IGameLaunchService gameLaunchService, MemoryIo memoryIo, AoBScanner aoBScanner, TempService tempService)
    {
        _gameLaunchService = gameLaunchService;
        _memoryIo = memoryIo;
        _memoryIo.StartAutoAttach();
        _aoBScanner = aoBScanner;
        _tempService = tempService;
        
        LaunchGameCommand = new DelegateCommand(LaunchGame);
        
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(25)
        };
        _gameTimer.Tick += Timer_Tick;
        _gameTimer.Start();
    }
    #region Commands
    public ICommand LaunchGameCommand { get; set; }
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
        {
            IsAttached = false;
            
        }
    }
    #endregion
}

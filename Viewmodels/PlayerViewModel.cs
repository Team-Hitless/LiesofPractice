using System.Windows.Input;
using LiesOfPractice.Core;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;

namespace LiesOfPractice.Viewmodels;

public class PlayerViewModel : ViewModelBase
{
    private readonly IPlayerService _playerService;

    public PlayerViewModel(IPlayerService playerService)
    {
        _playerService = playerService;
        SavePositionCommand = new DelegateCommand(SavePosition);
        RestorePositionCommand = new DelegateCommand(RestorePosition);
        RestCommand = new DelegateCommand(Rest);

        AreOptionsEnabled = true; // True for now, need to find a way to detect if player is in game
    }
    
    #region Commands
    
    public ICommand SavePositionCommand { get; set; }
    public ICommand RestorePositionCommand { get; set; }
    public ICommand RestCommand { get; set; }

    #endregion
    
    #region Public Properies
    
    
    private bool _areOptionsEnabled;
    public bool AreOptionsEnabled 
    { 
        get => _areOptionsEnabled; 
        set { _areOptionsEnabled = value; OnPropertyChanged(nameof(AreOptionsEnabled)); } 
    }
    
    private bool _isPos1Saved;
    public bool IsPos1Saved 
    { 
        get => _isPos1Saved; 
        set { _isPos1Saved = value; OnPropertyChanged(nameof(IsPos1Saved)); } 
    }

    private bool _isPos2Saved;
    public bool IsPos2Saved 
    { 
        get => _isPos2Saved; 
        set { _isPos2Saved = value; OnPropertyChanged(nameof(IsPos2Saved)); } 
    }
    
    private bool _isNoDamageEnabled;
    public bool IsNoDamageEnabled 
    { 
        get => _isNoDamageEnabled; 
        set
        {
            _isNoDamageEnabled = value;
            OnPropertyChanged(nameof(_isNoDamageEnabled));
            _playerService.ToggleChrDebugFlagA(_isNoDamageEnabled, (int)Offsets.DebugFlagsBaseA.Flags.NoDamage);
        } 
    }

    private bool _isNoDeathEnabled;
    public bool IsNoDeathEnabled 
    { 
        get => _isNoDeathEnabled; 
        set
        {
            _isNoDeathEnabled = value;
            OnPropertyChanged(nameof(_isNoDeathEnabled));
            _playerService.ToggleChrDebugFlagA(_isNoDeathEnabled, (int)Offsets.DebugFlagsBaseA.Flags.ChrNoDeath);
        } 
    }

    private bool _isInfiniteFableEnabled;
    public bool IsInfiniteFableEnabled 
    { 
        get => _isInfiniteFableEnabled; 
        set
        {
            _isInfiniteFableEnabled = value;
            OnPropertyChanged(nameof(_isInfiniteFableEnabled));
            _playerService.ToggleChrDebugFlagA(_isInfiniteFableEnabled, (int)Offsets.DebugFlagsBaseA.Flags.InfiniteFable);
        } 
    }

    private bool _isOneShotEnabled;
    public bool IsOneShotEnabled 
    { 
        get => _isOneShotEnabled;
        set
        {
            _isOneShotEnabled = value;
            OnPropertyChanged(nameof(IsOneShotEnabled));
            _playerService.ToggleChrDebugFlagA(_isOneShotEnabled, (int)Offsets.DebugFlagsBaseA.Flags.OneShot);
        } 
    }
    
    private bool _isInfiniteConsumablesEnabled;
    public bool IsInfiniteConsumablesEnabled 
    { 
        get => _isInfiniteConsumablesEnabled;
        set
        {
            _isInfiniteConsumablesEnabled = value;
            OnPropertyChanged(nameof(IsInfiniteConsumablesEnabled));
            _playerService.ToggleInfiniteConsumables(_isInfiniteConsumablesEnabled);
        } 
    }
    
    private bool _isNoErgoLossEnabled;
    public bool IsNoErgoLossEnabled 
    { 
        get => _isNoErgoLossEnabled;
        set
        {
            _isNoErgoLossEnabled = value;
            OnPropertyChanged(nameof(IsNoErgoLossEnabled));
            _playerService.ToggleNoErgoLossOnDeath(_isNoErgoLossEnabled);
        } 
    }
    
    #endregion
    
    #region Private Methods
    
    private void SavePosition(object parameter)
    {
        int index = Convert.ToInt32(parameter);
        _playerService.SavePos(index);
        if (index == 0) IsPos1Saved = true;
        else IsPos2Saved = true;
    }
    private void RestorePosition(object parameter) => _playerService.RestorePos(Convert.ToInt32(parameter));

    private void Rest(object? obj) => _playerService.Rest();



    #endregion
}
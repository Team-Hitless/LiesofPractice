using LiesOfPractice.Core;
using LiesOfPractice.Enums;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;
using System.Windows.Input;

namespace LiesOfPractice.Viewmodels;

public class PlayerViewModel : ViewModelBase
{
    private readonly IPlayerService _playerService;
    private readonly IHotkeyService _hotkeyService;
    private readonly IEventService _eventService;

    private bool _areOptionsEnabled;
    private bool _isPos1Saved;
    private bool _isPos2Saved;
    private bool _isNoDamageEnabled;
    private bool _isNoDeathEnabled;
    private bool _isOneShotEnabled;
    private bool _isInfiniteConsumablesEnabled;
    private bool _isNoErgoLossEnabled;
    private bool _isInfiniteFableEnabled;

    public PlayerViewModel(IPlayerService playerService, IHotkeyService hotkeyService, IEventService eventService)
    {
        _playerService = playerService;
        _hotkeyService = hotkeyService;
        _eventService = eventService;
        SavePositionCommand = new DelegateCommand(SavePosition);
        RestorePositionCommand = new DelegateCommand(RestorePosition);
        RestCommand = new DelegateCommand(Rest);
        
        RegisterHotkeys();
        _eventService.Subscribe(GameEvent.Loaded, OnGameLoaded);
        _eventService.Subscribe(GameEvent.NotLoaded, OnGameNotLoaded);
    }


    #region Commands

    public ICommand SavePositionCommand { get; set; }

    public ICommand RestorePositionCommand { get; set; }

    public ICommand RestCommand { get; set; }

    #endregion

    #region Public Properies
    
    public bool AreOptionsEnabled 
    { 
        get => _areOptionsEnabled; 
        set { _areOptionsEnabled = value; OnPropertyChanged(nameof(AreOptionsEnabled)); } 
    }
    
    public bool IsPos1Saved 
    { 
        get => _isPos1Saved; 
        set { _isPos1Saved = value; OnPropertyChanged(nameof(IsPos1Saved)); } 
    }
    
    public bool IsPos2Saved 
    { 
        get => _isPos2Saved; 
        set { _isPos2Saved = value; OnPropertyChanged(nameof(IsPos2Saved)); } 
    }  
    
    public bool IsNoDamageEnabled 
    { 
        get => _isNoDamageEnabled; 
        set
        {
            _isNoDamageEnabled = value;
            OnPropertyChanged(nameof(IsNoDamageEnabled));
            _playerService.ToggleChrDebugFlagA(_isNoDamageEnabled, (int)Offsets.DebugFlagsBaseA.Flags.NoDamage);
        } 
    }
    
    public bool IsNoDeathEnabled 
    { 
        get => _isNoDeathEnabled; 
        set
        {
            _isNoDeathEnabled = value;
            OnPropertyChanged(nameof(IsNoDeathEnabled));
            _playerService.ToggleChrDebugFlagA(_isNoDeathEnabled, (int)Offsets.DebugFlagsBaseA.Flags.ChrNoDeath);
        } 
    }

    public bool IsInfiniteFableEnabled 
    { 
        get => _isInfiniteFableEnabled; 
        set
        {
            _isInfiniteFableEnabled = value;
            OnPropertyChanged(nameof(IsInfiniteFableEnabled));
            _playerService.ToggleChrDebugFlagA(_isInfiniteFableEnabled, (int)Offsets.DebugFlagsBaseA.Flags.InfiniteFable);
        } 
    }

    
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

    private void RegisterHotkeys()
    {
        _hotkeyService.RegisterAction(ActionTag.InfiniteConsumables, () => { IsInfiniteConsumablesEnabled = !IsInfiniteConsumablesEnabled; });
        _hotkeyService.RegisterAction(ActionTag.NoErgoLossOnDeath, () => { IsNoErgoLossEnabled = !IsNoErgoLossEnabled; });
        _hotkeyService.RegisterAction(ActionTag.OneShot, () => { IsOneShotEnabled = !IsOneShotEnabled; });
        _hotkeyService.RegisterAction(ActionTag.InfiniteFable, () => { IsInfiniteFableEnabled = !IsInfiniteFableEnabled; });
        _hotkeyService.RegisterAction(ActionTag.NoDeath, () => { IsNoDeathEnabled = !IsNoDeathEnabled; });
        _hotkeyService.RegisterAction(ActionTag.NoDamage, () => { IsNoDamageEnabled = !IsNoDamageEnabled; });
        _hotkeyService.RegisterAction(ActionTag.SavePos1, () => SavePosition(0));
        _hotkeyService.RegisterAction(ActionTag.SavePos2, () => SavePosition(1));
        _hotkeyService.RegisterAction(ActionTag.RestorePos1, () =>
        {
            if (!IsPos1Saved) return;
            RestorePosition(0);
        });
        _hotkeyService.RegisterAction(ActionTag.RestorePos2, () =>
        {
            if (!IsPos2Saved) return;
            RestorePosition(1);
        });
    }
    
    private void OnGameLoaded()
    {
        AreOptionsEnabled = true;
        if (IsNoDamageEnabled)
            _playerService.ToggleChrDebugFlagA(true, (int)Offsets.DebugFlagsBaseA.Flags.NoDamage);
        if (IsNoDeathEnabled)
            _playerService.ToggleChrDebugFlagA(true, (int)Offsets.DebugFlagsBaseA.Flags.ChrNoDeath);
        if (IsInfiniteFableEnabled)
            _playerService.ToggleChrDebugFlagA(true, (int)Offsets.DebugFlagsBaseA.Flags.InfiniteFable);
        if (IsOneShotEnabled)
            _playerService.ToggleChrDebugFlagA(true, (int)Offsets.DebugFlagsBaseA.Flags.OneShot);
        if (IsInfiniteConsumablesEnabled)
            _playerService.ToggleInfiniteConsumables(true);
        if (IsNoErgoLossEnabled)
            _playerService.ToggleNoErgoLossOnDeath(true);
            
    }
    
    
    private void OnGameNotLoaded()
    {
        AreOptionsEnabled = false;
    }


    #endregion
}
using H.Hooks;
using LiesOfPractice.Core;
using LiesOfPractice.Enums;
using LiesOfPractice.Interfaces;
using System.Reflection;

namespace LiesOfPractice.Viewmodels;

public class SettingsViewModel : ViewModelBase
{
    // Player
    private string _duelistSetup = "";
    private string _oneShot = "";
    private string _noDeath = "";
    private string _noDamage = "";
    private string _savePos1 = "";
    private string _savePos2 = "";
    private string _restorePos1 = "";
    private string _restorePos2 = "";
    private string _infiniteConsumables = "";
    private string _infiniteFable = "";
    private string _noErgoLossOnDeath = "";
    private string _noPulsCellCharge = "";

    // Utility
    private string _noClip = "";
    private string _quitout = "";
    private string _increaseGameSpeed = "";
    private string _decreaseGameSpeed = "";
    private string _increaseNoClipSpeed = "";
    private string _decreaseNoClipSpeed = "";
    private string _enableFreeCam = "";
    private string _camToPlayer = "";
    private string _deathCam = "";

    // Enemie
    private string _allNoDamage = "";
    private string _allNoDeath = "";
    private string _disableAllAi = "";
    private string _allRepeatAction = "";
    private string _enableTargetOptions = "";
    private string _freezeHP = "";
    private string _targetRepeatAction = "";
    private string _disableTargetAi = "";
    private string _increaseTargetSpeed = "";
    private string _decreaseTargetSpeed = "";
    private string _showTargetResist = "";
    private string _killTarget = "";
    private string _showTargetsView = "";
    private string _setTargetCustomHp = "";

    private readonly IHotkeyService _hotkeyService;
    private readonly IDataService _dataService;
    private LowLevelKeyboardHook? _tempHook;

    public SettingsViewModel(IHotkeyService hotkeyService, IDataService dataService)
    {
        _hotkeyService = hotkeyService;
        _dataService = dataService;
        GotFocusCommand = new DelegateCommand(StartSettingHotkey);
        LostFocusCommand = new DelegateCommand(StopSettingHotkey);
        LoadHotkeyDisplays();

        if (GlobalHotkeys)
            _hotkeyService.Start();
    }

    #region Commands
    public System.Windows.Input.ICommand GotFocusCommand { get; set; }
    public System.Windows.Input.ICommand LostFocusCommand { get; set; }
    #endregion

    #region Public Properties
    public bool GlobalHotkeys
    {
        get => _dataService.AppSettings.GlobalHotkeys;
        set
        {
            _dataService.AppSettings.GlobalHotkeys = value;
            if (value) 
                _hotkeyService.Start();
            else 
                _hotkeyService.Stop();
        }
    }

    public bool CheckforUpdates
    {
        get => _dataService.AppSettings.CheckforUpdates;
        set => _dataService.AppSettings.CheckforUpdates = value;
    }

    public bool StaysOnTop
    {
        get => _dataService.AppSettings.StaysOnTop;
        set => _dataService.AppSettings.StaysOnTop = value;
    }

    // Player Tag
    [ActionTag(ActionTag.DuelistSetup)]
    public string DuelistSetup
    {
        get => _duelistSetup;
        set
        {
            _duelistSetup = value;
            OnPropertyChanged(nameof(DuelistSetup));
        }
    }

    [ActionTag(ActionTag.OneShot)]
    public string OneShot
    {
        get => _oneShot;
        set { _oneShot = value; OnPropertyChanged(nameof(OneShot)); }
    }

    [ActionTag(ActionTag.NoDeath)]
    public string NoDeath
    {
        get => _noDeath;
        set { _noDeath = value; OnPropertyChanged(nameof(NoDeath)); }
    }

    [ActionTag(ActionTag.NoDamage)]
    public string NoDamage
    {
        get => _noDamage;
        set { _noDamage = value; OnPropertyChanged(nameof(NoDamage)); }
    }

    [ActionTag(ActionTag.SavePos1)]
    public string SavePos1
    {
        get => _savePos1;
        set { _savePos1 = value; OnPropertyChanged(nameof(SavePos1)); }
    }

    [ActionTag(ActionTag.SavePos2)]
    public string SavePos2
    {
        get => _savePos2;
        set { _savePos2 = value; OnPropertyChanged(nameof(SavePos2)); }
    }

    [ActionTag(ActionTag.RestorePos1)]
    public string RestorePos1
    {
        get => _restorePos1;
        set { _restorePos1 = value; OnPropertyChanged(nameof(RestorePos1)); }
    }

    [ActionTag(ActionTag.RestorePos2)]
    public string RestorePos2
    {
        get => _restorePos2;
        set { _restorePos2 = value; OnPropertyChanged(nameof(RestorePos2)); }
    }

    [ActionTag(ActionTag.InfiniteConsumables)]
    public string InfiniteConsumables
    {
        get => _infiniteConsumables;
        set { _infiniteConsumables = value; OnPropertyChanged(nameof(InfiniteConsumables)); }
    }

    [ActionTag(ActionTag.InfiniteFable)]
    public string InfiniteFable
    {
        get => _infiniteFable;
        set { _infiniteFable = value; OnPropertyChanged(nameof(InfiniteFable)); }
    }

    [ActionTag(ActionTag.NoErgoLossOnDeath)]
    public string NoErgoLossOnDeath
    {
        get => _noErgoLossOnDeath;
        set { _noErgoLossOnDeath = value; OnPropertyChanged(nameof(NoErgoLossOnDeath)); }
    }

    [ActionTag(ActionTag.NoPulsCellCharge)]
    public string NoPulsCellCharge
    {
        get => _noPulsCellCharge;
        set { _noPulsCellCharge = value; OnPropertyChanged(nameof(NoPulsCellCharge)); }
    }

    // Utility Tags
    [ActionTag(ActionTag.NoClip)]
    public string NoClip
    {
        get => _noClip;
        set { _noClip = value; OnPropertyChanged(nameof(NoClip)); }
    }

    [ActionTag(ActionTag.Quitout)]
    public string Quitout
    {
        get => _quitout;
        set { _quitout = value; OnPropertyChanged(nameof(Quitout)); }
    }

    [ActionTag(ActionTag.IncreaseGameSpeed)]
    public string IncreaseGameSpeed
    {
        get => _increaseGameSpeed;
        set { _increaseGameSpeed = value; OnPropertyChanged(nameof(IncreaseGameSpeed)); }
    }

    [ActionTag(ActionTag.DecreaseGameSpeed)]
    public string DecreaseGameSpeed
    {
        get => _decreaseGameSpeed;
        set { _decreaseGameSpeed = value; OnPropertyChanged(nameof(DecreaseGameSpeed)); }
    }

    [ActionTag(ActionTag.IncreaseNoClipSpeed)]
    public string IncreaseNoClipSpeed
    {
        get => _increaseNoClipSpeed;
        set { _increaseNoClipSpeed = value; OnPropertyChanged(nameof(IncreaseNoClipSpeed)); }
    }

    [ActionTag(ActionTag.DecreaseNoClipSpeed)]
    public string DecreaseNoClipSpeed
    {
        get => _decreaseNoClipSpeed;
        set { _decreaseNoClipSpeed = value; OnPropertyChanged(nameof(DecreaseNoClipSpeed)); }
    }

    [ActionTag(ActionTag.EnableFreeCam)]
    public string EnableFreeCam
    {
        get => _enableFreeCam;
        set { _enableFreeCam = value; OnPropertyChanged(nameof(EnableFreeCam)); }
    }

    [ActionTag(ActionTag.CamToPlayer)]
    public string CamToPlayer
    {
        get => _camToPlayer;
        set { _camToPlayer = value; OnPropertyChanged(nameof(CamToPlayer)); }
    }

    [ActionTag(ActionTag.DeathCam)]
    public string DeathCam
    {
        get => _deathCam;
        set { _deathCam = value; OnPropertyChanged(nameof(DeathCam)); }
    }

    // Enemy Tags
    [ActionTag(ActionTag.AllNoDamage)]
    public string AllNoDamage
    {
        get => _allNoDamage;
        set { _allNoDamage = value; OnPropertyChanged(nameof(AllNoDamage)); }
    }

    [ActionTag(ActionTag.AllNoDeath)]
    public string AllNoDeath
    {
        get => _allNoDeath;
        set { _allNoDeath = value; OnPropertyChanged(nameof(AllNoDeath)); }
    }

    [ActionTag(ActionTag.DisableAllAi)]
    public string DisableAllAi
    {
        get => _disableAllAi;
        set { _disableAllAi = value; OnPropertyChanged(nameof(DisableAllAi)); }
    }

    [ActionTag(ActionTag.AllRepeatAction)]
    public string AllRepeatAction
    {
        get => _allRepeatAction;
        set { _allRepeatAction = value; OnPropertyChanged(nameof(AllRepeatAction)); }
    }

    [ActionTag(ActionTag.EnableTargetOptions)]
    public string EnableTargetOptions
    {
        get => _enableTargetOptions;
        set { _enableTargetOptions = value; OnPropertyChanged(nameof(EnableTargetOptions)); }
    }

    [ActionTag(ActionTag.FreezeHP)]
    public string FreezeHP
    {
        get => _freezeHP;
        set { _freezeHP = value; OnPropertyChanged(nameof(FreezeHP)); }
    }

    [ActionTag(ActionTag.TargetRepeatAction)]
    public string TargetRepeatAction
    {
        get => _targetRepeatAction;
        set { _targetRepeatAction = value; OnPropertyChanged(nameof(TargetRepeatAction)); }
    }

    [ActionTag(ActionTag.DisableTargetAi)]
    public string DisableTargetAi
    {
        get => _disableTargetAi;
        set { _disableTargetAi = value; OnPropertyChanged(nameof(DisableTargetAi)); }
    }

    [ActionTag(ActionTag.IncreaseTargetSpeed)]
    public string IncreaseTargetSpeed
    {
        get => _increaseTargetSpeed;
        set { _increaseTargetSpeed = value; OnPropertyChanged(nameof(IncreaseTargetSpeed)); }
    }

    [ActionTag(ActionTag.DecreaseTargetSpeed)]
    public string DecreaseTargetSpeed
    {
        get => _decreaseTargetSpeed;
        set { _decreaseTargetSpeed = value; OnPropertyChanged(nameof(DecreaseTargetSpeed)); }
    }

    [ActionTag(ActionTag.ShowTargetResist)]
    public string ShowTargetResist
    {
        get => _showTargetResist;
        set { _showTargetResist = value; OnPropertyChanged(nameof(ShowTargetResist)); }
    }

    [ActionTag(ActionTag.KillTarget)]
    public string KillTarget
    {
        get => _killTarget;
        set { _killTarget = value; OnPropertyChanged(nameof(KillTarget)); }
    }

    [ActionTag(ActionTag.ShowTargetsView)]
    public string ShowTargetsView
    {
        get => _showTargetsView;
        set { _showTargetsView = value; OnPropertyChanged(nameof(ShowTargetsView)); }
    }

    [ActionTag(ActionTag.SetTargetCustomHp)]
    public string SetTargetCustomHp
    {
        get => _setTargetCustomHp;
        set { _setTargetCustomHp = value; OnPropertyChanged(nameof(SetTargetCustomHp)); }
    }
    #endregion

    #region hotkeyStuff
    private void LoadHotkeyDisplays()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<ActionTagAttribute>();
            if (attribute == null)
                continue;

            property.SetValue(this, _hotkeyService.GetHotkey(attribute.Tag)?.ToString());
        }
    }

    private void SetPropertyValueByTag(ActionTag actionTag, Keys? keys)
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var property = properties.Where(x => x.GetCustomAttribute<ActionTagAttribute>()?.Tag == actionTag).FirstOrDefault();
        property?.SetValue(this, keys?.ToString());
    }

    public void StartSettingHotkey(object? obj)
    {
        if (obj is ActionTag actionTag)
        {
            SetPropertyValueByTag(actionTag, null);

            _tempHook = new LowLevelKeyboardHook
            {
                IsExtendedMode = true
            };

            _tempHook.Down += (obj, e) => TempHook_Down(obj, e, actionTag);
            _tempHook.Start();
        }
    }

    private void TempHook_Down(object? sender, KeyboardEventArgs e, ActionTag actionTag)
    {
        if (e.Keys.IsEmpty)
            return;

        var hotkeys = e.Keys.Values;
        if (hotkeys.Intersect([Key.Tab, Key.OemBackTab]).Any())
        {
            StopSettingHotkey(null);
        }
        else if (hotkeys.Intersect([Key.Enter, Key.Return, Key.Escape]).Any())
        {
            StopSettingHotkey(null);
            e.IsHandled = true;
        }
        else
        {
            HandleExistingHotkey(e.Keys);
            SetHotkey(actionTag, e.Keys);
            e.IsHandled = true;
        }
    }

    private void StopSettingHotkey(object? obj)
    {
        if (_tempHook is not null)
        {
            _tempHook.Down -= (obj, e) => TempHook_Down(obj, e, ActionTag.None);
            _tempHook.Dispose();
            _tempHook = null;
        }
    }

    private void HandleExistingHotkey(Keys currentKeys)
    {
        var actionTag = _hotkeyService.GetActionTagByKeys(currentKeys);
        if (actionTag is null) return;

        _hotkeyService.ClearHotkey(actionTag ?? ActionTag.None);
        SetPropertyValueByTag(actionTag ?? ActionTag.None, null);
    }

    private void SetHotkey(ActionTag actionTag, Keys keys)
    {
        _hotkeyService.SetHotkey(actionTag, keys);
        SetPropertyValueByTag(actionTag, keys);
    }
    #endregion
}
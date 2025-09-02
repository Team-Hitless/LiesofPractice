using H.Hooks;
using LiesOfPractice.Enums;
using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using System.Runtime.InteropServices;

namespace LiesOfPractice.Services;

#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public class HotkeyService : IHotkeyService
{
    [DllImport("user32.dll")]
    private static extern nint GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern nint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

    private readonly IMemoryIoService _memoryIo;
    private readonly IDataService _dataService;
    private readonly LowLevelKeyboardHook? _keyboardHook;


    public HotkeyService(IMemoryIoService memoryIo, IDataService dataService)
    {
        _memoryIo = memoryIo;
        _dataService = dataService;
        _keyboardHook = new()
        {
            HandleModifierKeys = true
        };

        _keyboardHook.Down += KeyboardHook_Down;

        if (AppSettings.GlobalHotkeys) _keyboardHook.Start();
    }

    #region Private Properties
    private AppSettings AppSettings
    {
        get => _dataService.AppSettings;
        set => _dataService.AppSettings = value;
    }

    private List<HotKeyActions> HotKeyActions
    {
        get => AppSettings.KeyActions;
        set => AppSettings.KeyActions = value;
    }
    #endregion

    #region Private Functions
    private void KeyboardHook_Down(object? sender, KeyboardEventArgs e)
    {
        if (!IsGameFocused())
            return;

        var hotKeyAction = HotKeyActions.First(x => x.Keys == e.Keys);

        if (hotKeyAction.Action is not null)
            hotKeyAction?.Action?.Invoke();
    }

    private bool IsGameFocused()
    {
        if (_memoryIo.TargetProcess is null || _memoryIo.TargetProcess.Id == 0)
            return false;

        nint foregroundWindow = GetForegroundWindow();
        GetWindowThreadProcessId(foregroundWindow, out uint foregroundProcessId);
        return foregroundProcessId == (uint)_memoryIo.TargetProcess.Id;
    }
    #endregion

    #region Public Functions
    public void Start() => _keyboardHook?.Start();
    public void Stop() => _keyboardHook?.Stop();
    public Keys? GetHotkey(ActionTag actionTag) => HotKeyActions.Find(x => x.ActionTag == actionTag)?.Keys;
    public ActionTag? GetActionTagByKeys(Keys keys) => HotKeyActions.Find(x => x.Keys == keys)?.ActionTag;

    public void RegisterAction(ActionTag actionTag, Action action)
    {
        var keyAction = HotKeyActions.Find(x => x.ActionTag == actionTag);
        if (keyAction != null)
            keyAction.Action = action;
    }

    public void SetHotkey(ActionTag actionTag, Keys keys)
    {
        var keyAction = HotKeyActions.Find(x => x.ActionTag == actionTag);
        if (keyAction != null)
            keyAction.Keys = keys;
        else
            HotKeyActions.Add(new() { ActionTag = actionTag, Keys = keys });

    }

    public void ClearHotkey(ActionTag actionTag)
    {
        var keyAction = HotKeyActions.Find(x => x.ActionTag == actionTag);
        if (keyAction != null)
            HotKeyActions.Remove(keyAction);
    }
    #endregion
}
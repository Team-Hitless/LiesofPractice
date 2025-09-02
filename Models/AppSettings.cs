using H.Hooks;
using LiesOfPractice.Core;
using System.Globalization;

namespace LiesOfPractice.Models;

public class AppSettings : OberservableObject
{
    private string _gamepath = "";
    private string _steampath = "";
    private bool _checkForUpdates = true;
    private bool _staysOnTop;
    private bool _globalHotkeys;

    public string Gamepath
    {
        get => _gamepath;
        set
        {
            _gamepath = value;
            OnPropertyChanged(nameof(Gamepath));
        }
    }
    public string Steampath
    {
        get => _steampath;
        set
        {
            _steampath = value;
            OnPropertyChanged(nameof(Steampath));
        }
    }
    public bool CheckforUpdates
    {
        get => _checkForUpdates;
        set
        {
            _checkForUpdates = value;
            OnPropertyChanged(nameof(CheckforUpdates));
        }
    }
    public bool StaysOnTop
    {
        get => _staysOnTop;
        set
        {
            _staysOnTop = value;
            OnPropertyChanged(nameof(StaysOnTop));
        }
    }
    public bool GlobalHotkeys
    {
        get => _globalHotkeys;
        set
        {
            _globalHotkeys = value;
            OnPropertyChanged(nameof(GlobalHotkeys));
        }
    }
    public List<HotKeyActions> KeyActions { get; set; } = [];
}

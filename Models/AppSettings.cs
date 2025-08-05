using LiesOfPractice.Core;

namespace LiesOfPractice.Models;

public class AppSettings : OberservableObject
{
    private string _gamepath = "";
    private string _steampath = "";
    private bool _checkForUpdates;

    public string Gamepath
    {
        get => _gamepath;
        set
        {
            if (_gamepath == value)
                return;

            _gamepath = value;
            OnPropertyChanged(nameof(Gamepath));
        }
    }
    public string Steampath
    {
        get => _steampath;
        set
        {
            if (_steampath == value)
                return;

            _steampath = value;
            OnPropertyChanged(nameof(Steampath));
        }
    }
    public bool CheckforUpdates
    {
        get => _checkForUpdates;
        set
        {
            if (_checkForUpdates == value)
                return;

            _checkForUpdates = value;
            OnPropertyChanged(nameof(CheckforUpdates));
        }
    }
}

using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using LiesOfPractice.Properties;

namespace LiesOfPractice.Services;

public class DataService : IDataService
{
    public DataService()
    {
        LoadAppSettings();
    }

    public AppSettings AppSettings { get; set; } = new();
    private Settings Settings { get; set; } = new();

    private void LoadAppSettings()
    {
        Settings.Reload();

        AppSettings.CheckforUpdates = Settings.CheckforUpdates;
        AppSettings.Gamepath = Settings.Gamepath;
        AppSettings.Steampath = Settings.Steampath;
    }

    public void SaveAppSettings()
    {
        Settings.CheckforUpdates = AppSettings.CheckforUpdates;
        Settings.Gamepath = AppSettings.Gamepath;
        Settings.Steampath = AppSettings.Steampath;

        Settings.Save();
    }
}

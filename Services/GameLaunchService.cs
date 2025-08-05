using LiesOfPractice.Interfaces;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace LiesOfPractice.Services;

public class GameLaunchService(IDataService dataService, IWindowService windowService) : IGameLaunchService
{
    public void LaunchGame()
    {
        try
        {
            if (string.IsNullOrEmpty(dataService.AppSettings.Gamepath))
                return;

            var process = new Process { StartInfo = new(dataService.AppSettings.Gamepath) };
            process.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to launch: {ex.Message}",
                "Launch Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    //public static void SetVersionOffsets()
    //{
    //    try
    //    {
    //        string exePath = GetGameExePath();
    //        if (exePath == null)
    //        {
    //            return;
    //        }

    //        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(exePath);

    //        int major = versionInfo.FileMajorPart;
    //        int minor = versionInfo.FileMinorPart;

    //        if (major == 1 && minor <= 8)
    //        {
    //            Offsets.WorldChrMan.ChrBehaviorModule.AnimSpeed = 0xA38;
    //            IsDlcAvailable = false;
    //        }

    //        if (major == 1 && minor < 12)
    //        {
    //            Offsets.WorldChrMan.PlayerInsOffsets.CharFlags1 = 0x1ED8;
    //            Offsets.WorldChrMan.PlayerInsOffsets.Modules = 0x1F80;
    //            Offsets.WorldChrMan.DeathCam = 0x88;
    //        }
    //        else if (major == 1 && minor == 12)
    //        {
    //            Offsets.WorldChrMan.PlayerInsOffsets.CharFlags1 = 0x1EE0;
    //            Offsets.WorldChrMan.PlayerInsOffsets.Modules = 0x1F88;
    //            Offsets.WorldChrMan.DeathCam = 0x90;
    //        }
    //        else
    //        {
    //            Offsets.WorldChrMan.PlayerInsOffsets.CharFlags1 = 0x1EE8;
    //            Offsets.WorldChrMan.PlayerInsOffsets.Modules = 0x1F90;
    //            Offsets.WorldChrMan.DeathCam = 0x90;
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        Offsets.WorldChrMan.PlayerInsOffsets.CharFlags1 = 0x1EE8;
    //        Offsets.WorldChrMan.PlayerInsOffsets.Modules = 0x1F90;
    //    }
    //}

    public void InitGameExePath()
    {
        if (!string.IsNullOrEmpty(dataService.AppSettings.Gamepath) && 
            File.Exists(dataService.AppSettings.Gamepath))
            return;

        dataService.AppSettings.Gamepath = string.Empty;

        try
        {
            var steamPath = GetSteampath();
            if (string.IsNullOrEmpty(steamPath))
                throw new FileNotFoundException("Steam installation path not found in registry.");

            var configPath = Path.Combine(steamPath, @"steamapps\libraryfolders.vdf");
            if (!File.Exists(configPath))
                throw new FileNotFoundException($"Steam library configuration not found at {configPath}");

            var paths = new List<string> { steamPath };
            var regex = new Regex(@"""path""\s*""(.+?)""");

            foreach (var line in File.ReadLines(configPath))
            {
                var match = regex.Match(line);
                if (match.Success) paths.Add(match.Groups[1].Value.Replace(@"\\", @"\"));
            }

            foreach (var path in paths)
            {
                var fullPath = Path.Combine(path, @"steamapps\common\Lies of P\LOP.exe");

                if (File.Exists(fullPath))
                {
                    dataService.AppSettings.Gamepath = fullPath;
                    return;
                }

            }
            return;

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error finding Lies of P: {ex.Message}", "Game Not Found", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            return;
        }
        finally
        {
            dataService.SaveAppSettings();
        }
    }

    private string GetSteampath()
    {
        if (string.IsNullOrEmpty(dataService.AppSettings.Steampath))
            return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null) as string ?? "";
        else
            return dataService.AppSettings.Steampath;
    }

    public void SelectGamepath()
    {
        try
        {
            var result = MessageBoxResult.OK;

            while (result == MessageBoxResult.OK)
            {
                var filename = windowService.OpenFileDialog("Select Lies of P Executable", "Executable Files (*.exe)|*.exe", GetSteampath());

                if (Path.GetFileName(filename).Equals("LOP.exe", StringComparison.OrdinalIgnoreCase))
                {
                    dataService.AppSettings.Gamepath = filename;
                    break;
                }

                result = windowService.ShowNotifyBox("Invalid File", "Please select LOP.exe.");
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error finding Lies of P: {ex.Message}", "Game Not Found", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            return;
        }
        finally
        {
            dataService.SaveAppSettings();
        }
    }
}
using LiesOfPractice.Interfaces;
using LiesOfPractice.Properties;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace LiesOfPractice.Services;

public class GameLaunchService(Settings settings) : IGameLaunchService
{
    public void LaunchGame()
    {
        try
        {
            InitGameExePath();
            if (settings.Gamepath == null)
                return;

            var process = new Process { StartInfo = new(settings.Gamepath) };
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
        var gameExePath = settings.Gamepath;

        if (!string.IsNullOrEmpty(settings.Gamepath) && File.Exists(gameExePath))
            return;

        settings.Gamepath = null;

        try
        {
            var steamPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null) as string;
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
                    settings.Gamepath = fullPath;
                    return;
                }

            }

            var msg = "Lies of P executable could not be found automatically.\r\n" +
                      "Please select DarkSoulsIII.exe manually.\n\n" +
                      "Note: Certain features will not work unless the correct executable is selected.";
            MessageBoxResult result = MessageBox.Show(msg, "Executable Not Found", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            while (result == MessageBoxResult.OK)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Select Lies of P Executable",
                    Filter = "Executable Files (*.exe)|*.exe",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                if (openFileDialog.ShowDialog() != true)
                    return;

                if (Path.GetFileName(openFileDialog.FileName).Equals("LOP.exe", StringComparison.OrdinalIgnoreCase))
                {
                    settings.Gamepath = openFileDialog.FileName;
                    break;
                }

                result = MessageBox.Show("Please select LOP.exe.", "Invalid File", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
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
            settings.Save();
        }
    }
}
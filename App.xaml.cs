using LiesOfPractice.Interfaces;
using LiesOfPractice.Properties;
using LiesOfPractice.Services;
using LiesOfPractice.Viewmodels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using LiesOfPractice.Memory;

namespace LiesOfPractice;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<MemoryIo>();
        services.AddSingleton<AoBScanner>();
        services.AddSingleton<TempService>();
        services.AddSingleton<IDataService, DataService>();

        services.AddScoped<IWindowService, WindowService>();
        services.AddScoped<IGitHubService, GitHubService>();
        services.AddScoped<IGameLaunchService, GameLaunchService>();
        services.AddScoped<IJsonService, JsonService>();
        

        services.AddSingleton<MainViewModel>();
        services.AddScoped<GitHubViewModel>();

        services.AddSingleton(sp => new MainWindow()
        {
            DataContext = sp.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var bgWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        bgWorker.DoWork += (s, arg) => _serviceProvider.GetService<IGitHubService>()?.CheckGitHubNewerVersion();
        bgWorker.DoWork += (s, arg) => _serviceProvider.GetService<IGameLaunchService>()?.InitGameExePath();
        bgWorker.RunWorkerAsync();

        var startForm = _serviceProvider.GetRequiredService<MainWindow>();
        startForm.Show();        
        base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider.GetRequiredService<IDataService>().SaveAppSettings();
        base.OnExit(e);
    }
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var windowService = _serviceProvider.GetService<IWindowService>();
        var viewModel = _serviceProvider.GetService<NotifyBoxViewModel>();

        if (windowService != null && viewModel != null)
        {
            viewModel.Message = e.Exception.Message;
            viewModel.Title = "Error";
            windowService.OpenWindow(viewModel);
        }
        else
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

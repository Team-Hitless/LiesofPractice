﻿using LiesOfPractice.Interfaces;
using LiesOfPractice.Properties;
using LiesOfPractice.Services;
using LiesOfPractice.Viewmodels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

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

        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IGitHubService, GitHubService>();
        services.AddSingleton<IGameLaunchService, GameLaunchService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<GitHubViewModel>();
        services.AddSingleton<Settings>(x => new());

        services.AddSingleton(sp => new MainWindow()
        {
            DataContext = sp.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _serviceProvider.GetService<IGitHubService>()?.CheckGitHubNewerVersion();
        _serviceProvider.GetService<IGameLaunchService>()?.InitGameExePath();
        _serviceProvider.GetService<Settings>()?.Reload();
        
        var startForm = _serviceProvider.GetRequiredService<MainWindow>();
        startForm.Show();
        base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e)
    {
        //var startForm = _serviceProvider.GetRequiredService<IDataService>();
        //startForm.SaveConfigAsync();
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

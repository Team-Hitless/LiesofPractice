using LiesOfPractice.Viewmodels;
using System.Windows;

namespace LiesOfPractice.Interfaces;

public interface IWindowService
{
    public void NotifierInformation(string message);
    public void NotifierSuccess(string message);
    public void NotifierWarning(string message);
    public void NotifierError(string message);

    public void OpenWindowDialog<T>(T viewModel, ViewModelBase? parent = null);
    public void OpenWindow<T>(T viewModel);
    public void CloseWindow<T>(T viewModel);
    public string OpenFolderWindow(string path);
    public MessageBoxResult ShowNotifyBox(string title, string message);
    public string OpenFileDialog(string title, string filter, string path = "");
}

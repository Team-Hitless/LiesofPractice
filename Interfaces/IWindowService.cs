using LiesOfPractice.Viewmodels;

namespace LiesOfPractice.Interfaces;

public interface IWindowService
{
    public void NotifierInformation(string message);
    public void NotifierSuccess(string message);
    public void NotifierWarning(string message);
    public void NotifierError(string message);

    public void OpenWindowDialog<T>(T viewModel, ViewModelBase parent);
    public void OpenWindow<T>(T viewModel);
    public void CloseWindow<T>(T viewModel);
    public string OpenFolderWindow(string path);
}

using LiesOfPractice.Core;
using System.Windows;
using System.Windows.Input;

namespace LiesOfPractice.Viewmodels;

public class NotifyBoxOkCancelViewModel : ViewModelBase
{
    private string _message = string.Empty;
    private string _title = string.Empty;

    public NotifyBoxOkCancelViewModel()
    {
        OkResultCommand = new DelegateCommand(OkResult);
        LoadedCommand = new DelegateCommand(ResetResult);
    }
    public ICommand OkResultCommand { get; set; }
    public ICommand LoadedCommand { get; set; }

    public MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;
    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
                return;
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
    public string Message
    {
        get => _message;
        set
        {
            if (_message == value)
                return;
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }
    private void ResetResult(object obj) => Result = MessageBoxResult.Cancel;
    private void OkResult(object obj) => Result = MessageBoxResult.OK;
}

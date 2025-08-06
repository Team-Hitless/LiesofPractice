using System.ComponentModel;
using System.Diagnostics;

namespace LiesOfPractice.Core;

public class OberservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        Debug.WriteLine($"OnPropertyChanged: {propertyName}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

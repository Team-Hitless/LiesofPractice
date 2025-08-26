using LiesOfPractice.Viewmodels;
using LiesOfPractice.Interfaces;

namespace LiesOfPractice.Services;

public class NavigationService(Func<Type, ViewModelBase> viewModelFactory) : ViewModelBase, INavigationService
{
    private ViewModelBase _currentView = new();

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView == value)
                return;

            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }

    public void NavigateTo<T>() where T : ViewModelBase => CurrentView = viewModelFactory.Invoke(typeof(T));
}

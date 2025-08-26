using LiesOfPractice.Viewmodels;

namespace LiesOfPractice.Interfaces;

public interface INavigationService
{
    ViewModelBase CurrentView { get; set; }

    void NavigateTo<T>() where T : ViewModelBase;
}
using LiesOfPractice.Models;
using LiesOfPractice.Properties;
using System.Collections.ObjectModel;

namespace LiesOfPractice.Interfaces;

public interface IDataService
{
    public bool IsLoaded { get; set; }
    public AppSettings AppSettings { get; set; }
    public ObservableCollection<Page> Pages { get; set; }
    public Page? SelectedPage { get; set; }
    public void SaveAppSettings();
}
using LiesOfPractice.Models;
using LiesOfPractice.Properties;

namespace LiesOfPractice.Interfaces;

public interface IDataService
{
    public AppSettings AppSettings { get; set; }
    public void SaveAppSettings();
}
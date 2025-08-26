using System.Windows.Input;

namespace LiesOfPractice.Models;

public class Page
{
    public required string Name { get; set; }
    public required ICommand Command { get; set; }
}

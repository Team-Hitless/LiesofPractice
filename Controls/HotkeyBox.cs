using System.Windows;
using System.Windows.Controls;

namespace LiesOfPractice.Controls;

public class HotkeyBox : TextBox
{
    private const string PlaceholderTextBlockName = "PART_Placeholder";
    private TextBlock _placeholderTextbox = new();

    static HotkeyBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HotkeyBox), new FrameworkPropertyMetadata(typeof(HotkeyBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _placeholderTextbox = (TextBlock)Template.FindName(PlaceholderTextBlockName, this);
        _placeholderTextbox.Text = "None";

        GotFocus += (s, e) => { _placeholderTextbox.Text = "Press keys..."; };
        LostFocus += (s, e) => { _placeholderTextbox.Text = "None"; };
        TextChanged += (s, e) => HandleTextBlockAppearance();
        KeyDown += (s, e) => HandleTextBlockAppearance();
    }

    private void HandleTextBlockAppearance()
    {
        if (string.IsNullOrEmpty(Text))
        {
            _placeholderTextbox.Visibility = Visibility.Visible;
            _placeholderTextbox.Text = "None";
        }
        else
            _placeholderTextbox.Visibility = Visibility.Collapsed;
    }
}

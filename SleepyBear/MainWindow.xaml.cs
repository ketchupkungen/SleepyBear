using System.Windows;
using System.Windows.Controls.Primitives;

namespace SleepyBear;


// Main window for the application.
// Handles user interaction and the switch between awake/asleep modes via SleepController.cs file.
public partial class MainWindow : Window
{
    // Responsible for controlling the system's sleep/awake state.
    // Encapsulates OS-specific logic (currently Windows).
    private readonly SleepController _sleepController = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    // Triggered when the toggle is switched ON.
    // Enables keep-awake mode and updates the UI accordingly.
    private void KeepAwakeToggle_Checked(object sender, RoutedEventArgs e)
    {
        _sleepController.EnableKeepAwake(); // Tell OS to stay awake
        UpdateUi(isAwakeMode: true);        // Updates UI to reflect state
    }

    // Triggered when toggle is switched OFF.
    // Restores "normal" sleep behavior and updates the UI.
    private void KeepAwakeToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        _sleepController.DisableKeepAwake(); // Allow normal sleep behavior
        UpdateUi(isAwakeMode: false);        // Updates UI to reflect state
    }

    // Updates UI elements based on whether the app is in keep-awake mode or not.
    private void UpdateUi(bool isAwakeMode)
    {
        if (KeepAwakeToggle is ToggleButton toggle)
        {
            toggle.Content = isAwakeMode ? "On" : "Off";
        }

        StatusTextBlock.Text = isAwakeMode
            ? "Will stay awake."
            : "Will fall asleep";
    }
}
# WinUtilities.Keyboard

This is the WinUtilities module designed for interactions with the keyboard.

### Usage

#### WPF
On WPF applications, it's very straight forward, here's an example.

```csharp
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        KeyboardManager.Instance.OnKeyPressEvent += OnKeyPressEvent;
            
        Application.Current.Exit += (sender, args) =>
        {
            if (KeyboardManager.Instance.UnHook())
            {
                Debug.WriteLine("Unhooked keyboard events.");
            }
        };

    }

    private void OnKeyPressEvent(object sender, KeyEventArgs eventArgs)
    {
        Debug.WriteLine(eventArgs.KeyCode);

        if (eventArgs.KeyCode == Key.H)
        {
            eventArgs.Handled = true;
            Debug.WriteLine("Keypress has been handled, so it won't be sent to other applications!");
        }
    }
}
```

This example will print the name of any key that is pressed, and it will block any presses to the H key.  
One thing to note is that it's very important to call `UnHook()` prior to shutting down the application, as Windows doesn't automatically handle the cleanup of hooks.
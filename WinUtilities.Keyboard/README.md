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
        
        // Subscribe to the key press event.
        KeyboardManager.Instance.OnKeyPressEvent += OnKeyPressEvent;
        // Subscribe to the key release event.
        KeyboardManager.Instance.OnKeyReleaseEvent += OnKeyReleaseEvent;
    }
        
    private void OnKeyPressEvent(object sender, KeyEventArgs eventArgs)
    {
        Debug.WriteLine($"{eventArgs.KeyCode} was pressed!");

        if (eventArgs.KeyCode == Key.H)
        {
            // Setting Handled to true consumes the event.
            eventArgs.Handled = true;
            Debug.WriteLine("Key press has been handled, so it won't be sent to other applications!");
        }
    }

    private void OnKeyReleaseEvent(object sender, KeyEventArgs eventArgs)
    {
        Debug.WriteLine($"{eventArgs.KeyCode} was released!");

        if (eventArgs.KeyCode == Key.H)
        {
            // Setting Handled to true consumes the event.
            eventArgs.Handled = true;
            Debug.WriteLine("Key release has been handled, so it won't be sent to other applications!");
        }
    }
}
```

This example will print the name of any key that is pressed, and it will block any presses to the H key.
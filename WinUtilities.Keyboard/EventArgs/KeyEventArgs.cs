using WinUtilities.Keyboard.Enums;

namespace WinUtilities.Keyboard.EventArgs
{
    public class KeyEventArgs
    {
        /// <summary>
        /// Enum representing the key used to trigger the event.
        /// </summary>
        public Key KeyCode { get; }
        
        /// <summary>
        /// Set this to true to prevent default behaviour.
        /// </summary>
        public bool Handled { get; set; } = false;

        public KeyEventArgs(Key keyCode)
        {
            KeyCode = keyCode;
        }
    }
}
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WinUtilities.Core;
using WinUtilities.Core.Enums;
using WinUtilities.Keyboard.Enums;
using WinUtilities.Keyboard.EventArgs;

namespace WinUtilities.Keyboard
{
    public class KeyboardManager
    {
        public static KeyboardManager Instance { get; } = new KeyboardManager();

        private WindowsHookExDelegate _keyboardHookProcedure;
        private IntPtr _keyboardHookId;
        private bool _currentlyHooked = false;

        private KeyboardManager()
        {
            _keyboardHookProcedure = KeyboardHookCallback;
            Hook();
        }

        /// <summary>
        /// Run this to apply hooks. This is run by the constructor, so it is only needed if you unhooked at some point.
        /// </summary>
        /// <returns>True if hooks were applied. False if the hooks were already applied.</returns>
        public bool Hook()
        {
            if (_currentlyHooked)
                return false;
            Console.WriteLine("Hooking up...");
            _keyboardHookId = SetKeyboardHook(_keyboardHookProcedure);
            _currentlyHooked = true;
            return true;
        }

        /// <summary>
        /// IMPORTANT: Make sure to unhook before exiting the application, because Windows will not handle this.
        /// Unhook the windows hooks. This will cause windows to stop sending events.
        /// </summary>
        public bool UnHook()
        {
            if (!_currentlyHooked)
                return false;
            HookUtils.UnhookWindowsHookEx(_keyboardHookId);
            _currentlyHooked = false;
            return true;
        }
        
        private static IntPtr SetKeyboardHook(WindowsHookExDelegate proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                
                return HookUtils.SetWindowsHookEx((int) IdHook.WH_KEYBOARD_LL, proc,
                    HookUtils.GetModuleHandle(curModule?.ModuleName), 0);
            }
        }

        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var keyDown = (IntPtr) KeyEvent.KEY_DOWN;
            var keyUp = (IntPtr) KeyEvent.KEY_UP;

            if (nCode < 0 || (wParam != keyDown && wParam != keyUp))
                return HookUtils.CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
            
            var key = (Key) Marshal.ReadInt32(lParam);
            var eventArgs = new KeyEventArgs(key);

            switch ((KeyEvent) wParam)
            {
                case KeyEvent.KEY_DOWN:
                    OnKeyPressEvent(this, eventArgs);
                    break;
                case KeyEvent.KEY_UP:
                    OnKeyReleaseEvent(this, eventArgs);
                    break;
                case KeyEvent.SYS_KEY_DOWN:
                case KeyEvent.SYS_KEY_UP:
                default:
                    throw new ArgumentOutOfRangeException(nameof(wParam), wParam, null);
            }

            if (eventArgs.Handled)
            {
                return (IntPtr) 1;
            }

            return HookUtils.CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
        }
        
        public delegate void KeyEventHandler(object sender, KeyEventArgs eventArgs);

        public event KeyEventHandler OnKeyPressEvent = delegate { };
        public event KeyEventHandler OnKeyReleaseEvent = delegate { };
    }
}
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WinUtilities.Core
{
    public static class MessageUtils
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }

            public override string ToString()
            {
                return $"X: {X}, Y: {Y}";
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            IntPtr hwnd;
            uint message;
            UIntPtr wParam;
            IntPtr lParam;
            int time;
            POINT pt;
            int lPrivate;
        }

        [DllImport("user32.dll")]
        public static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
            uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        /// <summary>
        /// This starts a blocking loop that receives windows messages. This can be used for programs that don't subscribe to the windows message loop by default, like console applications.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public static void StartMessageLoop(CancellationToken? cancellationToken = null)
        {
            Console.WriteLine("Start");
            // Console.WriteLine(GetMessage(out var msg, HookUtils.GetModuleHandle(curModule?.ModuleName), 0, 0));
            MSG msg;
            int ret;
            Console.WriteLine(GetMessage(out msg, IntPtr.Zero, 0, 0));

            // while ((ret = GetMessage(out var msg, IntPtr.Zero, 0, 0)) != 0)
            // {
            //     return;
            //     if (ret == -1)
            //     {
            //         Console.WriteLine("Something is wrong!");
            //         return;
            //     }
            //     
            //     
            //     TranslateMessage(ref msg);
            //     DispatchMessage(ref msg);
            // }
            Console.WriteLine("Stop");
        }
    }
}
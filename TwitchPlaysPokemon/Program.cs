using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TwitchBot
{
    class Program
    {
        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, Keys wParam, ulong lParam);

        [DllImport("user32.dll")]
        static extern byte VkKeyScan(Keys key);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x0101;
        const int WM_ACTIVATE = 0x0006;
        const int WM_SETCURSOR = 0x0020;

        public static IntPtr getProcess(string title)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(title))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

        static void Main(string[] args)
        {
            // Password from www.twitchapps.com/tmi/
            // include the "oauth:" portion
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "Mcubed333", "oauth:yswvejwvu4cf7dpjewc29jz3hp783l");
            string channel = "mcubed333";
            irc.joinRoom(channel);

            /*
            if (windowHandle == IntPtr.Zero)
            {
                Console.WriteLine("Window is not running.");
                return;
            }
            */

            string message, user;
            int index;

            while (true)
            {
                message = irc.readMessage();
                //message ":mcubed333!mcubed333@mcubed333.tmi.twitch.tv PRIVMSG #mcubed333 :!mcubed"  string
                //message ":tpark565!tpark565@tpark565.tmi.twitch.tv PRIVMSG #mcubed333 :aleiraoejr"	string

                index = message.IndexOf("@");
                if (index >= 0)
                {
                    message = message.Substring(index + 1);

                    user = GetUsername(message);
                    index = message.IndexOf("#" + channel + " :");
                    if (index >= 0)
                    {
                        message = message.Substring(index + channel.Length + 3);
                    }

                    // presses up arrow
                    if (message.Contains("up"))
                    {
                        SendExtendedKey(Keys.Up);
                    }
                    // presses down arrow key
                    else if (message.Contains("down"))
                    {
                        SendExtendedKey(Keys.Down);
                    }
                    // presses left arrow key
                    else if (message.Contains("left"))
                    {
                        SendExtendedKey(Keys.Left);
                    }
                    // presses right arrow key
                    else if (message.Contains("right"))
                    {
                        SendExtendedKey(Keys.Right);
                    }
                    // press A button (X on keyboard)
                    else if (message.Contains("A"))
                    {
                        SendKey(Keys.X);
                    }
                    // presses B button (Z on keyboard)
                    else if (message.Contains("B"))
                    {
                        SendKey(Keys.Z);
                    }
                    // presses L button (A on keyboard)
                    else if (message.Contains("L"))
                    {
                        SendKey(Keys.A);
                    }
                    // presses R button (S on keyboard)
                    else if (message.Contains("R"))
                    {
                        SendKey(Keys.S);
                    }
                    // presses Select button (backspace on keyboard)
                    else if (message.Contains("select"))
                    {
                        SendKey(Keys.X);
                    }
                    // presses Start button (Enter on keyboard)
                    else if (message.Contains("start"))
                    {
                        SendKey(Keys.Enter);
                    }
                    else if(message.Contains("brandon"))
                    {
                        irc.sendChatMessage("brandon is gay");
                    }
                    else if (message.Contains("!terik"))
                    {
                        irc.sendChatMessage("MrDestructoid SELF MrDestructoid DESTRUCT MrDestructoid");
                    }
                }
            }
        }
        public static string GetUsername(string message)
        {
            char[] userArray = new char[30];

            for (int x = 0; x < message.Length; x++)
            {
                if (message[x] == '.')
                {
                    return new string(userArray, 0, x);
                }
                else
                {
                    userArray[x] = message[x];
                }
            }

            return null;
        }

        public static void SendKey(Keys key)
        {
            IntPtr handler = getProcess("mGBA - 0.5.0");

            if (!handler.Equals(IntPtr.Zero))
            {
                IntPtr editWnd = FindWindowEx(handler, IntPtr.Zero, null, null);
                if (!editWnd.Equals(IntPtr.Zero))
                {
                    //PostMessage(handler, WM_SETCURSOR, null, 0x02000001);
                    //PostMessage(handler, WM_ACTIVATE, 0, 0);
                    //SetForegroundWindow(handler);
                    uint scanCode = MapVirtualKey((uint)key, 0);
                    ulong lParam = (0x00000001 | (scanCode << 16));
                    PostMessage(handler, WM_KEYDOWN, key, lParam);
                    uint repeat = 1;
                    uint up = 1;
                    lParam = (0xC0000000 | (scanCode << 16) | (repeat << 30) | 0x00000001);

                    PostMessage(handler, WM_KEYUP, key, lParam);
                }
            }
        }
        public static void SendExtendedKey(Keys key)
        {
            IntPtr handler = getProcess("mGBA - 0.5.0");

            if (!handler.Equals(IntPtr.Zero))
            {
                IntPtr editWnd = FindWindowEx(handler, IntPtr.Zero, null, null);
                if (!editWnd.Equals(IntPtr.Zero))
                {
                    //SetForegroundWindow(handler);
                    //PostMessage(handler, WM_ACTIVATE, 0, 0);
                    uint scanCode = MapVirtualKey((uint)key, 0);
                    ulong lParam = (0x01000001 | (scanCode << 16));
                    PostMessage(handler, WM_KEYDOWN, key, lParam);
                    uint repeat = 1;
                    uint up = 1;
                    lParam = (0xC1000000 | (scanCode << 16) | (repeat << 30) | 0x00000001);

                    PostMessage(handler, WM_KEYUP, key, lParam);
                }
            }
        }
    }
}

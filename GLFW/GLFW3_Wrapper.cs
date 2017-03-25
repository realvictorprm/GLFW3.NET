using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace glfw3
{
    public partial class Glfw
    {
        static Glfw() {
            Init();

        }

        public static KeyModifier[] keyModifiers = new KeyModifier[] { KeyModifier.ModAlt, KeyModifier.ModControl, KeyModifier.ModShift, KeyModifier.ModSuper };

        public static List<KeyModifier> GetKeyModifiers(int mods)
        {
            var modifiers = new List<KeyModifier>();
            foreach (var key in keyModifiers)
                if ((mods & (int)key) == (int)key) modifiers.Add(key);
            return modifiers;
        }

    }

    #region NonGeneratableCodeExtensions

    public partial class GLFWwindow : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                Glfw.DestroyWindow(this);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    public partial class GLFWcursor : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                Glfw.DestroyCursor(this);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    #endregion

    #region ObjectOrientedWrapper

    public partial class Monitor
    {
        public GLFWmonitor Handle
        {
            get;
            protected set;
        }

        public Monitor(Window window)
        {
            Handle = Glfw.GetWindowMonitor(window.Handle);
        }

        public Monitor(GLFWmonitor monitor)
        {
            Handle = monitor;
        }

        public unsafe Monitor[] getMonitors()
        {
            int count = 0;
            var ptr = new System.IntPtr((int*)count);
            IntPtr raw = Glfw.__Internal.GetWindowMonitor_0(ptr);
            Monitor[] monitors = new Monitor[count];
            for (int i = 0; i < count; i++)
            {
                monitors[i] = new Monitor(GLFWmonitor.__CreateInstance(raw));
                raw = IntPtr.Add(raw, IntPtr.Size);
            }
            return monitors;
        }

    }

    public partial class Window
    {

        protected GLFWwindowsizefun SizeChangedCallback = null;
        protected GLFWkeyfun KeyPressedCallback = null;


        protected string title = String.Empty;

        public GLFWwindow Handle
        {
            get;
            protected set;
        }


        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                Glfw.SetWindowTitle(Handle, value);
            }
        }

        /// <summary>
        /// Event args for the size changed event. 
        /// </summary>
        public struct SizeChangedEventArgs { public Window source; public int width; public int height; };

        /// <summary>  This is the event args for the keyboard key event.</summary>
        public struct KeyEventArgs {

            /// <summary>
            /// The window that received the event.
            /// </summary>
            public Window source;

            /// <summary>
            /// The keyboard key
            /// </summary>
            public Key key;

            /// <summary>
            /// The system-specific scancode of the key
            /// </summary>
            public int scancode;

            /// <summary>
            /// `GLFW_PRESS`, `GLFW_RELEASE` or `GLFW_REPEAT`.
            /// </summary>
            public State action;

            /// <summary>
            /// Bit field describing which modifier keys. Use KeyModifiers for extracting the bits.
            /// </summary>
            public int mods;
        };

        /// <summary>
        /// Will be called if the Size of the window has been changed
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> SizeChanged;

        /// <summary>
        /// Will be called if a key has been pressed
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyChanged;

        #region Constructors
        protected Window()
        {
            Init();
        }

        public Window(int width, int height, string title)
        {
            Handle = Glfw.CreateWindow(width, height, title, null, null);
            this.title = title;
            Init();
        }

        public Window(int width, int height, string title, Monitor m)
        {
            Handle = Glfw.CreateWindow(width, height, title, m.Handle, null);
            this.title = title;
            Init();
        }

        public Window(int width, int height, string title, Monitor m, Window w)
        {
            Handle = Glfw.CreateWindow(width, height, title, m.Handle, w.Handle);
            this.title = title;
            Init();
        }

        public Window(GLFWwindow w)
        {
            Handle = w;
            Init();
        }
#endregion

        public bool ShouldClose()
        {
            return System.Convert.ToBoolean(Glfw.WindowShouldClose(Handle));
        }

        public void Show()
        {
            Glfw.ShowWindow(Handle);
        }

        public void Hide()
        {
            Glfw.HideWindow(Handle);
        }

        public void SwapBuffers()
        {
            Glfw.SwapBuffers(Handle);
        }

        private void Init()
        {
            SizeChangedCallback = (IntPtr _handle, int width, int height) => {
                SizeChanged.Invoke(this, new SizeChangedEventArgs { source = this, width = width, height = height });
            };
            Glfw.SetWindowSizeCallback(Handle, SizeChangedCallback);
            KeyPressedCallback = (IntPtr _handle, int key, int scancode, int action, int mods) =>
            {
                var args = new KeyEventArgs {
                    source = this,
                    key = (Key)System.Enum.Parse(typeof(Key), key.ToString()),
                    action = (State)System.Enum.Parse(typeof(State), action.ToString()),
                    scancode = scancode,
                    mods = mods};            
                KeyChanged.Invoke(this, args);
            };
            Glfw.SetKeyCallback(Handle, KeyPressedCallback);
        }
    } 
    #endregion
}

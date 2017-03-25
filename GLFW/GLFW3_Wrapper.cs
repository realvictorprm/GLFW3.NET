using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace glfw3
{
    public partial class glfw3
    {
        static glfw3() {
            Init();

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
                glfw3.DestroyWindow(this);
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
                glfw3.DestroyCursor(this);
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
            Handle = glfw3.GetWindowMonitor(window.Handle);
        }

        public Monitor(GLFWmonitor monitor)
        {
            Handle = monitor;
        }

        public unsafe Monitor[] getMonitors()
        {
            int count = 0;
            var ptr = new System.IntPtr((int*)count);
            IntPtr raw = glfw3.__Internal.GetWindowMonitor_0(ptr);
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
                glfw3.SetWindowTitle(Handle, value);
            }
        }

        /// <summary>
        /// Event args for the size changed event. 
        /// </summary>
        public struct SizeChangedEventArgs { public Window source; public int width; public int height; };

        /// <summary>  This is the event args for the keyboard key event.</summary>
        public struct KeyPressedEventArgs {

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
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region Constructors
        protected Window()
        {
            Init();
        }

        public Window(int width, int height, string title)
        {
            Handle = glfw3.CreateWindow(width, height, title, null, null);
            this.title = title;
            Init();
        }

        public Window(int width, int height, string title, Monitor m)
        {
            Handle = glfw3.CreateWindow(width, height, title, m.Handle, null);
            this.title = title;
            Init();
        }

        public Window(int width, int height, string title, Monitor m, Window w)
        {
            Handle = glfw3.CreateWindow(width, height, title, m.Handle, w.Handle);
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
            return System.Convert.ToBoolean(glfw3.WindowShouldClose(Handle));
        }

        public void Show()
        {
            glfw3.ShowWindow(Handle);
        }

        public void Hide()
        {
            glfw3.HideWindow(Handle);
        }

        public void SwapBuffers()
        {
            glfw3.SwapBuffers(Handle);
        }

        private void Init()
        {
            SizeChangedCallback = (IntPtr _handle, int width, int height) => {
                SizeChanged.Invoke(this, new SizeChangedEventArgs { source = this, width = width, height = height });
            };
            glfw3.SetWindowSizeCallback(Handle, SizeChangedCallback);
            KeyPressedCallback = (IntPtr _handle, int key, int scancode, int action, int mods) =>
            {
                var args = new KeyPressedEventArgs {
                    source = this,
                    key = (Key)System.Enum.Parse(typeof(Key), key.ToString()),
                    action = (State)System.Enum.Parse(typeof(State), action.ToString()),
                    mods = mods};            
                KeyPressed.Invoke(this, args);
            };
            glfw3.SetKeyCallback(Handle, KeyPressedCallback);
        }
    } 
    #endregion
}

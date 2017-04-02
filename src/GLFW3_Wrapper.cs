using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace glfw3
{
	
    #region ObjectOrientedWrapper

    public partial class GLFWmonitor
    {

        public GLFWmonitor(GLFWwindow window)
        {
            this.__Instance = Glfw.__Internal.GetWindowMonitor_0(window.__Instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe GLFWmonitor[] getMonitors()
        {
            int count = 0;
            var ptr = new System.IntPtr((int*)count);
            IntPtr raw = Glfw.__Internal.GetWindowMonitor_0(ptr);
            GLFWmonitor[] monitors = new GLFWmonitor[count];
            for (int i = 0; i < count; i++)
            {
                monitors[i] = __CreateInstance(raw);
                raw = IntPtr.Add(raw, IntPtr.Size);
            }
            return monitors;
        }

    }

    public partial class GLFWwindow
    {
        #region Internal

        protected GLFWwindowsizefun SizeChangedCallback = null;
        protected GLFWkeyfun KeyPressedCallback = null;



        protected string title = String.Empty;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                Glfw.SetWindowTitle(this, value);
            }
        }

		/// <summary>
		/// Init this instance.
		/// Callbacks are here initialized for the events.
		/// </summary>
        private void Init()
        {
            SizeChangedCallback = (IntPtr _handle, int width, int height) => {
                SizeChanged.Invoke(this, new SizeChangedEventArgs { source = this, width = width, height = height });
            };
            Glfw.SetWindowSizeCallback(this, SizeChangedCallback);
            KeyPressedCallback = (IntPtr _handle, int key, int scancode, int action, int mods) =>
            {
                var args = new KeyEventArgs
                {
                    source = this,
                    key = (Key)System.Enum.Parse(typeof(Key), key.ToString()),
                    action = (State)System.Enum.Parse(typeof(State), action.ToString()),
                    scancode = scancode,
                    mods = mods
                };
                KeyChanged.Invoke(this, args);
            };
            Glfw.SetKeyCallback(this, KeyPressedCallback);
        }

        #endregion

        /// <summary>
        /// Event args for the size changed event. 
        /// </summary>
        public struct SizeChangedEventArgs { 
			/// <summary>
			/// The event source.
			/// </summary>
			public GLFWwindow source; 
			/// <summary>
			/// The new width.
			/// </summary>
			public int width;
			/// <summary>
			/// The new height.
			/// </summary>
			public int height; 
		};

        /// <summary>  This is the event args for the keyboard key event.</summary>
        public struct KeyEventArgs {

            /// <summary>
            /// The window that received the event.
            /// </summary>
            public GLFWwindow source;

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
        protected GLFWwindow()
        {
            Init();
        }

        public GLFWwindow(int width, int height, string title)
        {
            __Instance = Glfw.__Internal.CreateWindow_0(width, height, title, IntPtr.Zero, IntPtr.Zero);
            this.title = title;
            Init();
        }

        public GLFWwindow(int width, int height, string title, GLFWmonitor m)
        {
            __Instance = Glfw.__Internal.CreateWindow_0(width, height, title, m.__Instance, IntPtr.Zero);
            this.title = title;
            Init();
        }

        public GLFWwindow(int width, int height, string title, GLFWmonitor m, GLFWwindow w)
        {
            __Instance = Glfw.__Internal.CreateWindow_0(width, height, title, m.__Instance, w.__Instance);
            this.title = title;
            Init();
        }

        public GLFWwindow(GLFWwindow w)
        {
            __Instance = w.__Instance;
            Init();
        }
        #endregion

        #region Object Oriented Methods

		/// <summary>
		/// Returns whether the window should close.
		/// </summary>
		/// <returns><c>true</c>, if close was requested, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ShouldClose()
        {
            return System.Convert.ToBoolean(Glfw.WindowShouldClose(this));
        }

		/// <summary>
		/// Show this window.
		/// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show()
        {
            Glfw.ShowWindow(this);
        }

		/// <summary>
		/// Hide this window.
		/// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hide()
        {
            Glfw.HideWindow(this);
        }

		/// <summary>
		/// Swaps the buffers.
		/// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapBuffers()
        {
            Glfw.__Internal.SwapBuffers_0(__Instance);
        }

		/// <summary>
		/// Returns the current window size as tuple
		/// </summary>
		/// <returns>The size.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Tuple<int, int> GetSize()
        {
            int width = 0, height = 0;
            Glfw.GetWindowSize(this, ref width, ref height);
            return new Tuple<int, int>(width, height);
        }

		/// <summary>
		/// Sets the referenced values to the width and height values of the window
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSize(ref int width, ref int height)
        {
            Glfw.GetWindowSize(this, ref width, ref height);
        }
        #endregion


    }

	/// <summary>
	/// Analias for the type GLFWwindow. Can be used exactly the same expect the typename is <i>Window</i>
	/// </summary>
    public partial class Window : GLFWwindow
    {
        public Window(int width, int height, string title) : base(width, height, title) { }
        public Window(int width, int height, string title, Monitor m) : base(width, height, title, m) { }
        public Window(int width, int height, string title, Monitor m, Window w) : base(width, height, title, m, w) { }
        public Window(Window w) : base(w) { }

    }
	/// <summary>
	/// An alias for the type GLFWmonitor. Can be used exactly the same expect the typename is <i>Monitor</i>
	/// </summary>
    public partial class Monitor : GLFWmonitor {
        public Monitor(Window window) : base(window) { }
    }
    #endregion
}

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
    public partial class Native
    {
        /// <summary>
        /// Unsafe cast an object to a different type. 
        /// </summary>
        /// <remarks>Use it only if you know what you're doing.</remarks>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDest"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// 
        [Obsolete("Cast in a different, safe way.")]
        public static unsafe TDest ReinterpretCast<TDest, TSource>(TSource source)
        {
            System.Diagnostics.Debug.Print($"WARNING: REINTERPRET CAST USED FROM TYPE {typeof(TSource)} TO {typeof(TDest)}");
            var sourceRef = __makeref(source);
            var dest = default(TDest);
            var destRef = __makeref(dest);
            *(IntPtr*)&destRef = *(IntPtr*)&sourceRef;
            return __refvalue(destRef, TDest);
        }

    }

    //[Obsolete("Use your handles as IntPtrs")]
    //protected partial class VkAllocationCallbacks { }
    //[Obsolete("Use your handles as IntPtrs")]
    //protected partial class VkInstance { }
    //[Obsolete("Use your handles as IntPtrs")]
    //protected partial class VkPhysicalDevice { }
    //[Obsolete("Use your handles as IntPtrs")]
    //protected partial class VkSurfaceKHR { }

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

        public unsafe static string[] GetRequiredInstanceExtensions()
        {
            uint count = 0u;
            var s = __Internal.GetRequiredInstanceExtensions_0(&count);
            var res = new string[count];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new String(s[i]);
            }
            return res;
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
        public struct SizeChangedEventArgs { public GLFWwindow source; public int width; public int height; };

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
            __Instance = Glfw.__Internal.CreateWindow_0(width, height, title, IntPtr.Zero, __Instance);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ShouldClose()
        {
            return System.Convert.ToBoolean(Glfw.WindowShouldClose(this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Show()
        {
            Glfw.ShowWindow(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Hide()
        {
            Glfw.HideWindow(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapBuffers()
        {
            Glfw.__Internal.SwapBuffers_0(__Instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Tuple<int, int> GetSize()
        {
            int width = 0, height = 0;
            Glfw.GetWindowSize(this, ref width, ref height);
            return new Tuple<int, int>(width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSize(ref int width, ref int height)
        {
            Glfw.GetWindowSize(this, ref width, ref height);
        }
        #endregion


    }

    public partial class Window : GLFWwindow
    {
        public Window(int width, int height, string title) : base(width, height, title) { }
        public Window(int width, int height, string title, Monitor m) : base(width, height, title, m) { }
        public Window(int width, int height, string title, Monitor m, Window w) : base(width, height, title, m, w) { }
        public Window(Window w) : base(w) { }

    }
    public partial class Monitor : GLFWmonitor {
        public Monitor(Window window) : base(window) { }
    }
    #endregion
}

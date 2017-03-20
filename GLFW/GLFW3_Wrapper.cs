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
        static glfw3() { GlfwInit(); }

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
                glfw3.GlfwDestroyWindow(this);
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
                glfw3.GlfwDestroyCursor(this);
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
            Handle = glfw3.GlfwGetWindowMonitor(window.Handle);
        }

        public Monitor(GLFWmonitor monitor)
        {
            Handle = monitor;
        }

        public unsafe Monitor[] getMonitors()
        {
            int count = 0;
            IntPtr raw = glfw3.__Internal.GlfwGetMonitors_0((int*)count);
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
        public struct SizeChangedEventArgs { public Window source; public int width; public int height; };
        public event EventHandler<SizeChangedEventArgs> SizeChanged;

        protected GLFWwindowsizefun SizeChangedCallback = null;

        public GLFWwindow Handle
        {
            get;
            protected set;
        }

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
                glfw3.GlfwSetWindowTitle(Handle, value);
            }
        }

        protected Window()
        {
            Init();
        }

        public Window(int width, int height, string title)
        {
            Handle = glfw3.GlfwCreateWindow(width, height, title, null, null);
            this.title = title;
            Init();
        }

        public Window(int width, int height, string title, Monitor m)
        {
            Handle = glfw3.GlfwCreateWindow(width, height, title, m.Handle, null);
            this.title = title;
            Init();
        }

        public Window(int width, int height, string title, Monitor m, Window w)
        {
            Handle = glfw3.GlfwCreateWindow(width, height, title, m.Handle, w.Handle);
            this.title = title;
            Init();
        }

        public Window(GLFWwindow w)
        {
            Handle = w;
            Init();
        }

        public bool ShouldClose()
        {
            return System.Convert.ToBoolean(glfw3.GlfwWindowShouldClose(Handle));
        }

        public void Show()
        {
            glfw3.GlfwShowWindow(Handle);
        }

        public void Hide()
        {
            glfw3.GlfwHideWindow(Handle);
        }

        public void SwapBuffers()
        {
            glfw3.GlfwSwapBuffers(Handle);
        }

        private void Init()
        {
            SizeChangedCallback = (IntPtr _handle, int width, int height) => {
                SizeChanged.Invoke(this, new SizeChangedEventArgs { source = this, width = width, height = height });
            };
            glfw3.GlfwSetWindowSizeCallback(Handle, SizeChangedCallback);
        }
    } 
    #endregion
}

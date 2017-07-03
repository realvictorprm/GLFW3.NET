using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace glfw3{

#region NonGeneratableCode
	public partial class Glfw
    {
        static Glfw() {
            Init();

        }
#region VulkanRelatedCode
        [SuppressUnmanagedCodeSecurity]
        [DllImport("glfw3", CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "glfwGetInstanceProcAddress")]
        public static extern System.IntPtr GetInstanceProcAddress(IntPtr instance, [MarshalAs(UnmanagedType.LPStr)] string procname);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("glfw3", CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "glfwGetPhysicalDevicePresentationSupport")]
        public static extern int GetPhysicalDevicePresentationSupport(IntPtr instance, IntPtr device, uint queuefamily);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("glfw3", CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "glfwCreateWindowSurface")]
        /// Creates a window surface with the specified handles
        public static extern VkResult CreateWindowSurface(IntPtr instance, IntPtr window, IntPtr allocator, Int64 surface);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("glfw3", CallingConvention = CallingConvention.Cdecl,
        EntryPoint = "glfwCreateWindowSurface")]
        /// Creates a window surface with the specified handles
        public static extern VkResult CreateWindowSurface(UIntPtr instance, UIntPtr window, UIntPtr allocator, UInt64 surface);

#endregion // VulkanRelatedCode

        public static KeyModifier[] keyModifiers = new KeyModifier[] { KeyModifier.ModAlt, KeyModifier.ModControl, KeyModifier.ModShift, KeyModifier.ModSuper };

        public static List<KeyModifier> GetKeyModifiers(int mods)
        {
            var modifiers = new List<KeyModifier>();
            foreach (var key in keyModifiers)
                if ((mods & (int)key) == (int)key) modifiers.Add(key);
            return modifiers;
        }

#region ManualInterop
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
#endregion // ManualInterop
    }


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
#endregion // Disposable

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
#endregion //Disposable

    }

#endregion // NonGeneratableCode


}
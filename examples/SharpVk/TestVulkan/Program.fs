// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
// Try also http://fsharpforfunandprofit.com 
open glfw3
open SharpVk
open System

open System.Runtime.InteropServices;
open System.Security;

let nullptr = IntPtr.Zero

// Unfortunately GLFW3.NET provides only IntPtrs and not UIntPtrs. Therefore some conversions have to be made.
let createSurface (instance:byref<SharpVk.Instance>) (window:glfw3.Window) =
    let mutable surface = 0L;
    // A reinterpret cast is needed to acquire the IntPtr from the structure
    let instancePtr = IntPtr(int64(instance.RawHandle.ToUInt64()))
    let res = Glfw.CreateWindowSurface(instancePtr, window.__Instance, nullptr, &surface)
    SharpVk.Surface.CreateFromHandle(instance, System.Convert.ToUInt64(surface))


let TestSharpVkWithGLFW3dotNet () =
    //Disable OpenGL
    do Glfw.WindowHint(int State.ClientApi, int State.NoApi)
    // Create the glfw window
    let window = new glfw3.Window(100, 100, "SharpVk & GLFW test");
    // Test the size callback
    do window.SizeChanged.Add((fun (args) -> printfn "title: %A, width: %A, height: %A" args.source.Title args.width args.height)) 
    //Test the key callback
    do window.KeyChanged.Add(fun args -> printfn "key: %A, modifiers_ %A" (Glfw.GetKeyName((int)Key.Unknown, args.scancode)) (Glfw.GetKeyModifiers(args.mods));) 
    printfn "Created GLFW-window."
    
    let version = SharpVk.Version(1, 0, 30)

    //Create an appinfo
    let mutable appInfo = ApplicationInfo()
    do appInfo.ApiVersion <- version
    do appInfo.ApplicationName <- "SharpVk & GLFW test"
    do appInfo.EngineName <- "SharpVk & GLFW test"
    do appInfo.EngineVersion <- version
    do appInfo.ApiVersion <- version

    // Get the required extensions for surface creation
    let extensions = Glfw.GetRequiredInstanceExtensions(); 
    printfn "extensions: %A, count: %A" extensions (System.Convert.ToUInt32(extensions.Length))

    //Create an instance
    let mutable instanceCreateInfo = new InstanceCreateInfo()
    do instanceCreateInfo.ApplicationInfo <-  Nullable appInfo 
    do instanceCreateInfo.EnabledExtensionNames <- extensions
    do instanceCreateInfo.EnabledLayerNames <- [| "VK_LAYER_LUNARG_api_dump" |]
    let mutable instance = Instance.Create(instanceCreateInfo)
    
    //Create a rendering surface => VkSurfaceKHR
    let surface = createSurface &instance window

    //Display the window
    do window.Show()

    while not (window.ShouldClose()) do
        do window.SwapBuffers()
        //printfn "swaping buffers"
        do Glfw.PollEvents()

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    TestSharpVkWithGLFW3dotNet ()
    0 // return an integer exit code

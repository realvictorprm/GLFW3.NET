// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open glfw3

let TestCallbacks () =
    let window = new glfw3.Window(100, 100, "test");
    do window.SizeChanged.Add((fun (args) -> printfn "title: %A, width: %A, height: %A" args.source.Title args.width args.height))
    do window.KeyChanged.Add(fun args -> printfn "key: %A, modifiers_ %A" (Glfw.GetKeyName((int)Key.Unknown, args.scancode)) (Glfw.GetKeyModifiers(args.mods));)
    printfn "created window"
    do window.Show()

    while not (window.ShouldClose()) do
        do window.SwapBuffers()
        //printfn "swaping buffers"
        do Glfw.PollEvents()

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    TestCallbacks ()
    0 // return an integer exit code

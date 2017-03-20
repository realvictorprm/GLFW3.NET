// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let TestCallbacks () =
    let window = new glfw3.Window(100, 100, "test");
    do window.SizeChanged.Add((fun (args) -> printfn "title: %A, width: %A, height: %A" args.source.Title args.width args.height))
    printfn "created window"
    do window.Show()

    while not (window.ShouldClose()) do
        do window.SwapBuffers()
        printfn "swaping buffers"
        do glfw3.glfw3.GlfwPollEvents()

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    TestCallbacks ()
    0 // return an integer exit code

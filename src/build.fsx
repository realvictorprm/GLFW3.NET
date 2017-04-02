#r "packages/FAKE/tools/FakeLib.dll"
open Fake

let buildDir = "./build/"
let deployDir = "./deploy/"


// version info
let version = "0.9a"  // or retrieve from CI server

Target "Clean" (fun _ ->
        CleanDir buildDir
)

Target "Build" (fun _ ->
        !! @"*.csproj"
        ++ @"*.fsproj"
            |> MSBuildRelease buildDir "Build"
            |> Log "Build-Output: "
)

Target "Build WrapperBinding" (fun _ ->
        !! @"WrapperGLFW\*.csproj"
            |> MSBuildRelease buildDir "Build" 
            |> Log "Build-Output: "
)        

Target "Build RawBinding" (fun _ ->
        !! (@"BindingGLFW\*.csproj")
            |> MSBuildRelease buildDir "Build"
            |> Log "Build-Output: "
)

Target "Build Generator" (fun _ ->
        !! @"BindingGenerator\*.fsproj"
            |> MSBuildRelease buildDir "Build"
            |> Log "Build-Output: "
)

Target "Build all bindings" (fun _ ->
        !! @"WrapperGLFW\*.csproj"
        ++ @"WrapperGLFW.csproj"
            |> MSBuildRelease buildDir "Build"
            |> Log "Build-Output: "
)

RunTargetOrDefault "Build WrapperBinding"

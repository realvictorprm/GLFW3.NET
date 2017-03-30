#r "packages/FAKE/tools/FakeLib.dll"
open Fake

let buildDir = "./build/"
let deployDir = "./deploy/"

let appReferences = !! @"*.csproj"    

// version info
let version = "0.9a"  // or retrieve from CI server

Target "Build" (fun _ ->
        MSBuildRelease buildDir "Build" appReferences
            |> Log "Build-Output: "
)

RunTargetOrDefault "Build"

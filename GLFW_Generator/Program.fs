// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open CppSharp
open CppSharp.Parser
open CppSharp.Generators
open System 
open CppSharp.Types

[<TypeMap("GLFWwindow*")>]
type GlfwWindowPointer() =
    inherit CppSharp.Types.TypeMap()


    override t.CSharpSignature ctx =
        "Window"
    
    override t.CSharpMarshalToNative ctx =
        ctx.Return.Write(ctx.Parameter.Name)
    
    override t.CSharpMarshalToManaged ctx =
        ctx.Return.Write(ctx.ReturnVarName)

[<TypeMap("GLFWwindow")>]
type CustomTyper() =
    inherit CppSharp.Types.TypeMap()
    
    override t.CSharpConstruct () =
        "struct Window {
            IntPtr handle;
         }"

    override t.CSharpSignature ctx =
        "Window"
    
    override t.CSharpMarshalToNative ctx =
        ctx.Return.Write(ctx.Parameter.Name)
    
    override t.CSharpMarshalToManaged ctx =
        ctx.Return.Write(ctx.ReturnVarName)

type PassGLFW() as this =
    inherit Passes.TranslationUnitPass()
    

    override t.VisitDeclaration dec =
        if t.AlreadyVisited(dec) then false
        else 
            let entities = dec.PreprocessedEntities
            let excludes = dec.ExcludeFromPasses
            entities |> Seq.iter(fun i -> printfn "%A" i)
            excludes |> Seq.iter(fun i -> printfn "Exclude: %A" i)
            printfn "%A" dec.Name
            
            true

type Generator() =
    interface ILibrary with
        member t.Setup driver =
            let options = driver.Options;
            options.OutputDir <- "../../generated"
            options.GeneratorKind <- GeneratorKind.CSharp;
            let glfw = options.AddModule("Sample");
            let path = Environment.CurrentDirectory;
            glfw.Headers.Add(path + "\glfw3.h");
            //glfw.Libraries.Add("Sample.lib");
            ()
        member t.SetupPasses driver =
            do driver.AddTranslationUnitPass(new PassGLFW())

            ()
        member t.Preprocess (driver, ctx) =
            ctx.SetClassAsValueType("GLFWwindow")
            let s = ctx.FindTypedef("GLFWwindow")
            printfn "%A" s
            s
            |> Seq.iter (fun o -> printfn "is gen: %A" o.IsGenerated)
            ()
        member t.Postprocess (driver, ctx) =
            
            ()
        


[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    ConsoleDriver.Run(new Generator());
    Console.ReadKey() |> ignore
    0 // return an integer exit code

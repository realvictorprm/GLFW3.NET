// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open CppSharp
open CppSharp.Generators
open System 
open CppSharp.Types    


let root_directory = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

let tempFilePath = System.IO.Path.GetTempPath() + "\Glfw"

/// <summary>
/// Converts the input data to enum names. The text file contains the complete define section of the original glfw header.
/// </summary>
let GetEnumNames path =
    let input = System.IO.File.ReadAllLines(path)
    [| for line in input do
                let splitted = line.Split(' ')
                for s in splitted do
                    if s.StartsWith("GLFW_") then
                        yield s |]

let ToTitleCase s = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s)

let macrosTxtPath = root_directory + @"\misc\"
let keyMacros = GetEnumNames (macrosTxtPath + "KeyMacros.txt")
let mouseMacros = GetEnumNames (macrosTxtPath + "MouseMacros.txt")
let joystickMacros = GetEnumNames (macrosTxtPath + "JoystickMacros.txt")
let keyActionMacros = [| "GLFW_MOD_SHIFT"; "GLFW_MOD_CONTROL"; "GLFW_MOD_ALT"; "GLFW_MOD_SUPER" |]
let errorCodeMacros = [| "GLFW_NOT_INITIALIZED";  "GLFW_NO_CURRENT_CONTEXT"; "GLFW_INVALID_ENUM"; "GLFW_INVALID_VALUE"; "GLFW_OUT_OF_MEMORY"; 
                        "GLFW_API_UNAVAILABLE"; "GLFW_VERSION_UNAVAILABLE"; "GLFW_PLATFORM_ERROR"; "GLFW_FORMAT_UNAVAILABLE"; "GLFW_NO_WINDOW_CONTEXT"|]
let stateMacros = GetEnumNames (macrosTxtPath + "State.txt")
let versionMacros = GetEnumNames (macrosTxtPath + "Version.txt")

type GLFWTranslationPass() as this =
    inherit Passes.TranslationUnitPass()
    let mutable once = false

    override t.VisitDeclaration dec =
        if t.AlreadyVisited(dec) then false
        else 
            //dec.PreprocessedEntities |> Seq.iter (fun i -> if i :? CppSharp.AST.MacroDefinition then t.processMacro dec (i :?> AST.MacroDefinition) else ())
            if dec.Name.Contains("glfw") then
                dec.Name <- dec.Name.Replace("glfw", "");
            true 

type Generator() =
    let AdjustMouseEnums (mouse:CppSharp.AST.Enumeration) =
        let mouseAdjustfun (e:CppSharp.AST.Enumeration.Item) =
            let n = e.Name
            let pattern = [| "Last"; "Left"; "Right"; "Middle"|]
            let values = [| 7uL; 0uL; 1uL; 2uL; |]
            pattern |> Array.iteri(fun i s -> e.Name <- e.Name.Replace("Button", "_"); if n.Contains(s) then e.Value <- values.[i]; )

        mouse.Items |> Seq.iter mouseAdjustfun
    
    ///<summary> Adds all missing enums from the defines in the glfw header</summary>
    let AddMissingEnums (ctx:CppSharp.AST.ASTContext) =
        let beautify (o:CppSharp.AST.Enumeration.Item) s =
                o.Name <- o.Name.ToLower().Replace(s, "")
                o.Name <- ToTitleCase(o.Name)
                o.Name <- o.Name.Replace("_", "")
        let BeautifyEnum (e:CppSharp.AST.Enumeration) = 
            
            e.Items |> Seq.iter (fun o -> beautify o "glfw" ) 
            
        let BeautifyBigEnum (e:CppSharp.AST.Enumeration) (s:string) =
            let pattern = ("glfw_" + s.ToLower())
            e.Items |> Seq.iter (fun o -> beautify o pattern; if Char.IsDigit(o.Name, 0) then o.Name <- "_" + o.Name) 

        let keyEnum = ctx.GenerateEnumFromMacros("Key", keyMacros);
        BeautifyBigEnum keyEnum  "key"
        keyEnum.Items.Find(fun i -> i.Name = "Last").Value <- 348uL

        let mouseEnum = ctx.GenerateEnumFromMacros("Mouse", mouseMacros);
        BeautifyBigEnum mouseEnum "mouse"
        AdjustMouseEnums mouseEnum

        let joystickEnum = ctx.GenerateEnumFromMacros("Joystick", joystickMacros);
        BeautifyBigEnum joystickEnum "joystick"

        joystickEnum.Items.Find(fun i -> i.Name.Contains("Last")).Value <- 15uL

        let keyActionEnum = ctx.GenerateEnumFromMacros("KeyModifier", keyActionMacros);
        BeautifyEnum keyActionEnum
        let errorCodes = ctx.GenerateEnumFromMacros("Error", errorCodeMacros);
        BeautifyEnum errorCodes
        let stateEnum = ctx.GenerateEnumFromMacros("State", stateMacros);
        BeautifyEnum stateEnum
        let versionEnum = ctx.GenerateEnumFromMacros("Version", versionMacros);
        BeautifyEnum versionEnum
    
    let BeautifyMethods (ctx:CppSharp.AST.ASTContext) =
        ctx.FindDecl<CppSharp.AST.Method>("Glfw") |> Seq.iter(fun i -> printfn "func: %A" i)

        ()

    interface ILibrary with
        member t.Setup driver =
            let options = driver.Options;
            options.OutputDir <- "../../../generated"
            options.GeneratorKind <- GeneratorKind.CSharp
            let glfw = options.AddModule("glfw3")
            glfw.Defines.Add("VK_VERSION_1_0")
            glfw.Headers.Add(tempFilePath)
            Diagnostics.Level <- DiagnosticKind.Debug
            ()
        member t.SetupPasses driver =
            let pass = new GLFWTranslationPass()
            do driver.AddTranslationUnitPass(pass)
            ()

        member t.Preprocess (driver, ctx) =
            ()

        member t.Postprocess (driver, ctx) =
            
            AddMissingEnums ctx
            BeautifyMethods ctx

            ()

            
        
let FillTempFile (file:System.IO.StreamWriter) =
    let glfw = (root_directory + "/headers/glfw3.h")
    let dummies = (root_directory + "/headers/dummies.h")
    let dummiesLines = System.IO.File.ReadLines(dummies)
    dummiesLines |> Seq.iter (fun s -> file.WriteLine(s))
    let glfwLines = System.IO.File.ReadLines(glfw)
    glfwLines |> Seq.iter(fun s -> file.WriteLine(s))
    file.Flush()

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    let file = System.IO.File.CreateText(tempFilePath)
    do FillTempFile file
    ConsoleDriver.Run(new Generator())
    printfn "Done. Press any key to exit"
    Console.ReadKey() |> ignore
    file.Close()
    System.IO.File.Delete(tempFilePath)
    0 // return an integer exit code

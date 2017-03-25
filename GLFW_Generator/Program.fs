// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open CppSharp
open CppSharp.Generators
open System 
open CppSharp.Types    


let root_directory = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

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

    //let createEnum (dec:AST.Declaration) (macro_name:string) (enum_type:string) (macro_value:string)= 
    //    let enum = new AST.Enumeration()
    //    do enum.Name <- ToTitleCase (enum_type.ToLower())
    //    do enum.Namespace <- dec.TranslationUnit
    //    let name = ToTitleCase(macro_name.Replace(enum_type + "_", "").ToLower()).Replace("_", "")
    //    printfn "enum name: %A" enum.Name
    //    let value = try 
    //                    System.Convert.ToUInt64(macro_value)
    //                with
    //                | _ -> 0x0uL
    //    let item = AST.Enumeration.Item(Name = name, Value = value)
    //    do item.Namespace <- enum 
                
    //    do enum.Items.Add(item)
                
    //    if dec.TranslationUnit.Declarations.Exists((fun i -> i.Name = enum.Name )) then 
    //        let correct_enum = dec.TranslationUnit.Declarations.Find((fun i-> i.Name = enum.Name)) :?> AST.Enumeration
    //        do item.Namespace <- correct_enum
    //        do correct_enum.Items.Add(item)

    //    else dec.TranslationUnit.Declarations.Add(enum)
        
    //    ()

    //member t.processMacro (dec:AST.Declaration) (macro:AST.MacroDefinition)=
    //    let allowed_enums = [ "JOYSTICK"; "KEY"; "MOUSE"]
          
    //    if macro.Name.StartsWith("GLFW_") then
    //        let macro_name = macro.Name.Substring(5)
    //        let isEnum = allowed_enums |> List.tryFind (fun s -> macro.Name.StartsWith("GLFW_" + s))
    //        match isEnum with
    //        | Some enum_type -> createEnum dec macro_name enum_type macro.Expression
    //        | None ->
    //                 ()

        
    //    ()

    override t.VisitDeclaration dec =
        if t.AlreadyVisited(dec) then false
        else 
            //dec.PreprocessedEntities |> Seq.iter (fun i -> if i :? CppSharp.AST.MacroDefinition then t.processMacro dec (i :?> AST.MacroDefinition) else ())
            if dec.Name.Contains("glfw") then
                dec.Name <- dec.Name.Replace("glfw", "");
            true 

    override t.VisitFieldDecl dec =
        if t.AlreadyVisited(dec) then false
        else printfn "Field: %A" dec.Name; true 
    
    override t.VisitMacroDefinition macro =
        printfn "Macro definition: %A" macro.Name
        (macro.Name.Contains("GLFW_"))        

type Generator() =
    let AdjustMouseEnums (mouse:CppSharp.AST.Enumeration) =
        let mouseAdjustfun (e:CppSharp.AST.Enumeration.Item) =
            let n = e.Name
            let pattern = [| "Last"; "Left"; "Right"; "Middle"|]
            let values = [| 7uL; 0uL; 1uL; 2uL; |]
            pattern |> Array.iteri(fun i s -> e.Name <- e.Name.Replace("Button", "_"); if n.Contains(s) then e.Value <- values.[i]; )

        mouse.Items |> Seq.iter mouseAdjustfun
    
    ///<summary> Adds all missing enums from the defines in the glfw header</summary>
    ///
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
          // glfw.Defines.Add("GLFW_INCLUDE_VULKAN")
          //  glfw.IncludeDirs.Add(root_directory.ToString() + "/headers")
            glfw.Headers.Add(root_directory.ToString() + "/headers/glfw3.h")
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

            
        


[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    ConsoleDriver.Run(new Generator());
    printfn "Done. Press any key to exit"
    Console.ReadKey() |> ignore
    0 // return an integer exit code

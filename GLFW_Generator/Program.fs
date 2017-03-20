// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open CppSharp
open CppSharp.Generators
open System 
open CppSharp.Types    


let root_directory = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
//[<TypeMap("GLFWcursor")>]
//type GLFW_Cursor_Type() =
//    inherit CppSharp.Types.TypeMap()
//    let name = "NativeCursor"

//    override t.CSharpSignature ctx =
//        name
    
//    override t.CSharpMarshalToNative ctx =
//        ctx.Return.Write(ctx.Parameter.Name + "._handle")
    
//    override t.CSharpMarshalToManaged ctx =
//        ctx.Return.Write("new "+name+"(" + ctx.ReturnVarName + ")")

//[<TypeMap("GLFWmonitor")>]
//type GLFW_Monitor_Type() =
//    inherit CppSharp.Types.TypeMap()
//    let name = "NativeMonitor"

//    override t.CSharpSignature ctx =
//        name
    
//    override t.CSharpMarshalToNative ctx =
//        ctx.Return.Write(ctx.Parameter.Name + "._handle")
    
//    override t.CSharpMarshalToManaged ctx =
//        ctx.Return.Write("new "+name+"(" + ctx.ReturnVarName + ")")

//[<TypeMap("GLFWwindow")>]
//type GLFW_Window_Type() =
//    inherit CppSharp.Types.TypeMap()
//    let name = "NativeWindow"

//    override t.CSharpSignature ctx =
//        name
    
//    override t.CSharpMarshalToNative ctx =
//        ctx.Return.Write(ctx.Parameter.Name + "._handle")

    
//    override t.CSharpMarshalToManaged ctx =
//        ctx.Return.Write("new "+name+"(" + ctx.ReturnVarName + ")")


type GLFWTranslationPass() as this =
    inherit Passes.TranslationUnitPass()
    let mutable once = false

    let createEnum (dec:AST.Declaration) (macro_name:string) (enum_type:string) (macro_value:string)= 
        let enum = new AST.Enumeration()
        do enum.Name <- enum_type
        do enum.Namespace <- dec.TranslationUnit
        let name = macro_name.Remove(0, enum_type.Length + 1)
        let value = try 
                        System.Convert.ToUInt64(macro_value)
                    with
                    | _ -> 0x0uL
        let item = AST.Enumeration.Item(Name = name, Value = value)
        do item.Namespace <- enum 
                
        do enum.Items.Add(item)
                
        if dec.TranslationUnit.Declarations.Exists((fun i -> i.Name = enum_type)) then 
            let correct_enum = dec.TranslationUnit.Declarations.Find((fun i-> i.Name = enum_type)) :?> AST.Enumeration
            do item.Namespace <- correct_enum
            do correct_enum.Items.Add(item)

        else dec.TranslationUnit.Declarations.Add(enum)
        
        ()

    member t.processMacro (dec:AST.Declaration) (macro:AST.MacroDefinition)=
        let allowed_enums = [ "JOYSTICK"; "KEY"; "MOUSE"]
          
        if macro.Name.StartsWith("GLFW_") then
            let macro_name = macro.Name.Substring(5)
            let isEnum = allowed_enums |> List.tryFind (fun s -> macro_name.Contains(s))
            match isEnum with
            | Some enum_type -> createEnum dec macro_name enum_type macro.Expression
            | None -> let mutable res = 0uL
                      let isHexValue = UInt64.TryParse(macro.Expression.Replace("0x", ""), &res)
                      if isHexValue then
                        printfn "Hex value: %A" res 
                        let field = AST.Property()
                        do field.Namespace <- dec.TranslationUnit
                        do field.Name <- macro_name
                        
                        //
                        ()  

                      else
                        ()

        
        ()

    override t.VisitDeclaration dec =
        if t.AlreadyVisited(dec) then false
        else 
            dec.PreprocessedEntities |> Seq.iter (fun i -> if i :? CppSharp.AST.MacroDefinition then t.processMacro dec (i :?> AST.MacroDefinition) else ())

            true 
    
    override t.VisitFieldDecl dec =
        if t.AlreadyVisited(dec) then false
        else printfn "Field: %A" dec.Name; true 
    
    override t.VisitMacroDefinition macro =
        printfn "Macro definition: %A" macro.Name
        (macro.Name.Contains("GLFW_"))        

type Generator() =
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
            //let res = ctx.GenerateEnumFromMacros("Key", "GLFW_KEY");
            //res.Items |> Seq.iter( fun i -> printfn "%A" i)

            ()
        member t.Postprocess (driver, ctx) =
            
            ()
        


[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    ConsoleDriver.Run(new Generator());
    printfn "Done. Press any key to exit"
    Console.ReadKey() |> ignore
    0 // return an integer exit code

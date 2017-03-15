// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open CppSharp
open CppSharp.Generators
open System 
open CppSharp.Types

[<TypeMap("GLFWwindow")>]
type CustomTyper() =
    inherit CppSharp.Types.TypeMap()

    override t.CSharpSignature ctx =
        "Window"
    
    override t.CSharpMarshalToNative ctx =
        ctx.Return.Write(ctx.Parameter.Name)
    
    override t.CSharpMarshalToManaged ctx =
        ctx.Return.Write(ctx.ReturnVarName)

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
            printfn "decleration name: %A" dec.Name; 
            //printfn "preprocessed entities: %A" dec.PreprocessedEntities
            dec.PreprocessedEntities |> Seq.iter (fun i -> printfn "%A" i; if i :? CppSharp.AST.MacroDefinition then t.processMacro dec (i :?> AST.MacroDefinition) else ()) 
            
            //let translationunit = dec.TranslationUnit
            //if not (isNull(translationunit))then dec.TranslationUnit.Macros |> Seq.iter (fun o -> if o <> null then dec.PreprocessedEntities.Remove(o) |> ignore )

            true 
    
    override t.VisitFieldDecl dec =
        if t.AlreadyVisited(dec) then false
        else printfn "Field: %A" dec.Name; true 
    
    override t.VisitMacroDefinition macro =
        printfn "Macro definition: %A" macro.Name;
        (macro.Name.Contains("GLFW_")) 


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

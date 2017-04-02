# GLFW3.NET
[![Join us on Gitter!](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/glfw3dotnet/Lobby)

Automatic generated bindings of GLFW3 for .NET

### Build Status
| .NET | Mono |
|---------------------------|---------------------------|
| **Windows** | **Linux & Mac**
| [![Build status](https://ci.appveyor.com/api/projects/status/30o9i6m2vvwvnar7?svg=true)](https://ci.appveyor.com/project/EveResearchFoundation/glfw3-net) | [![Linux and Mac build](https://travis-ci.org/realvictorprm/GLFW3.NET.svg?branch=master)](https://travis-ci.org/realvictorprm/GLFW3.NET)

### Nuget package
[![NuGet version](https://badge.fury.io/nu/glfw.net.svg)](https://badge.fury.io/nu/glfw.net)

Please keep in mind to add the file WrapperGLFW.dll.config to your project and change the option ```CopyToOutputPath``` of the file WrapperGLFW.dll.config to **Always**.
The file can be found under ```/yoursolutionpath/packages/GLFW.NET.x.x.x/Content/WrapperGLFW.dll.config```.
This is **required** for Linux or Mac and if you don't do it, the library _won't_ work. 

## Introduction
This repository provides bindings for the latest GLFW3 version. It covers almost the complete API except Vulkan.

The binding is generated automatically but for a small part.

To get support for the Vulkan specific functions it's recommeneded to use the additional file _GLFW3_Wrapper.cs_ which includes the missing bindings (for Vulkan) and an object oriented wrapper. 

**The projects under _src_ are the generator, raw GLFW binding and an oop wrapper with binding for GLFW.**

To get the binding as dll please read further.

## How to get a working library file
### Windows
1. Clone 
2. Go into src/
2. Build the project with the command ```Build.cmd```
3. Link the output dll to your project and copy the native glfw.dll into your output directory

The x64 _glfw3.dll_ can be found in the _dependencies_ folder. If you need a custom binary for windows go to the glfw website
Pre-build binaries for all other architectures on windows e.g. x86 can be found there.

### Linux/Mac
1. Install glfw with your package-manager (version glfw 3.2.1)
2. Clone 
3. Go into src/
4. Build the library file with the command ```sh build.sh```
5. Link the dll to your project and link/copy WrapperGLFW.dll.config to your build-output directory (needed!)

### Other build targets
There are also custom targets available for FAKE:
- ```Build WrapperBinding``` to get the binding *with oop wrapper* (default target).
- ```Build RawBinding``` to get the binding *without oop wrapper*.
- ```Build Generator``` to build the *generator*.
- ```Build all bindings``` to build the *raw and wrapper binding*.
- ```Build`` to build *all projects* available in the src folder.
- ```Clean``` to *clean* the build folder.

To use them simply add them with quotation marks after the build command.

## Important remarks
### Compatibility:
- The project is compatible with Mono. It can be opened and built by MonoDevelop too but using the script file is recommended.
- The current GLFW.dll is for windows. For Linux/Mac please keep in mind to install glfw with your package-manager  
- Again, the wrapper _which is highly recommended to prevent seg faults_ can be found under _src/GLFW_Wrapper.cs_

### Examples
Few usage examples are under the _examples_ folder:
- How to use GLFW3.NET with SharpVk to acquire a VkSurfaceKHR from GLFW

Other examples can be seen in the wiki page of this repository.

### Dependencies for generation:
- The library uses _CppSharp_ for binding generation.

## Contributing

Contributions to make this binding better and staying up to date are readily appreciated, feel free to send PR's and open issues if there are any.

*To build the generator under Linux or Mac you have to compile the latest CppSharp on your own.*

Windows developers can easily use the existing dlls in the _dependencies_ folder.

## Questions, Suggestions

We're having a gitter chat for discussing binding generation and or additional features!
Feel free to join us!


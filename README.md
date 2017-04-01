# GLFW3.NET
[![Join us on Gitter!](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/glfw3dotnet/Lobby)

Automatic generated bindings of GLFW3 for .NET

### Build Status
| Windows .NET | Linux & Mac Mono
|---------------------------|---------------------------|
| [![Build status](https://ci.appveyor.com/api/projects/status/30o9i6m2vvwvnar7?svg=true)](https://ci.appveyor.com/project/EveResearchFoundation/glfw3-net) | [![Linux and Mac build](https://travis-ci.org/realvictorprm/GLFW3.NET.svg?branch=master)](https://travis-ci.org/realvictorprm/GLFW3.NET)

## Introduction
This repository provides bindings for the latest GLFW3 version. It covers almost the complete API except Vulkan.

The binding is generated automatically but for a small part.

To get support for the Vulkan specific functions it's recommeneded to use the additional file _GLFW3_Wrapper.cs_ which includes the missing bindings (for Vulkan) and an object oriented wrapper. 

The projects under _src_ are the generator and the wrapper for GLFW itself. 

To get the binding as dll please read further.

### Examples
Few usage examples are under the _examples_ folder:
- How to use GLFW3.NET with SharpVk to acquire a VkSurfaceKHR from GLFW

## How to get a working library file
### Windows
1. Clone 
2. Go into src/WrapperGLFW
2. Build the project with the Build.cmd file
3. Link the WrapperGLFW.dll to your project and copy the native glfw.dll into your output directory

The x64 _glfw3.dll_ can be found in the _dependencies_ folder. The x86 binary can be downloaded from the GLFW website.

All additional pre-build binaries for Windows can be found there.

### Linux/Mac
1. Install glfw with your package-manager (version glfw 3.2.1)
2. Clone 
3. Go into src/WrapperGLFW
4. Execute the sh file to build the wrapper-binding
5. Link the dll to your project and link/copy WrapperGLFW.dll.config into your output directory (needed!)

## Important remarks
### Compatibility:
- The project is compatible with Mono. It can be opened and built by MonoDevelop too but using the script file is recommended.
- The current GLFW.dll is for windows. For Linux/Mac please keep in mind to install glfw with your package-manager  
- The wrapper, which is highly recommended, can be found under _generated_ or _src/WrapperGLFW/GLFW_Wrapper.cs_

### Dependencies for generation:
- The library uses _CppSharp_ for binding generation.

## Contributing

Contributions to make this binding better and staying up to date are readily appreciated, feel free to send PR's and open issues if there are any.

*To build the generator under Linux or Mac you have to compile the latest CppSharp on your own.*

Windows developers can easily use the existing dlls in the _dependencies_ folder.

## Questions, Suggestions

We're having a gitter chat for discussing binding generation and or additional features!
Feel free to join us!


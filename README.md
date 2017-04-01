# GLFW3.NET
[![Join us on Gitter!](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/glfw3dotnet/Lobby)

Automatic generated bindings of GLFW3 for .NET

### Build Status
| Windows .NET | Linux & Mac Mono
|---------------------------|---------------------------|
| [![Build status](https://ci.appveyor.com/api/projects/status/30o9i6m2vvwvnar7?svg=true)](https://ci.appveyor.com/project/EveResearchFoundation/glfw3-net) | [![Linux and Mac build](https://travis-ci.org/realvictorprm/GLFW3.NET.svg?branch=master)](https://travis-ci.org/realvictorprm/GLFW3.NET)

## Introduction
This repository provides bindings for the latest GLFW3 version. Nearly the complete binding is generated automatically but a small amount needed manual generation. Most important is that the automatic generated files provide support for nearly all GLFW functions expect Vulkan. To get support for the Vulkan specific functions it's recommeneded to use the additional file _GLFW3_Wrapper.cs_ which includes the missing bindings (for Vulkan) and an object oriented wrapper. 

The projects under _src_ are the generator and the glfw wrapper project. 
To get the binding as dll it's recommended to read further.

### Examples
How to use the binding can be seen in the examples folder.
Currently there can be found:
- How to use GLFW3.NET with SharpVk to acquire a VkSurfaceKHR from GLFW

## How to get a working library file
### Windows
1. Clone 
2. Go into src/WrapperGLFW
2. Build the project with the Build.cmd file
3. Link the WrapperGLFW.dll to your project and copy the native glfw.dll into your output directory

The _glfw3.dll_ can be found in the folder dependencies. It's only for x64 and not for x86 therefor if you need one for x86 download it from the glfw website. Additional pre-build binaries for Windows can be found there.

### Linux/Mac
1. Install glfw with your paket-manager (version glfw 3.2.1)
2. Clone 
3. Go into src/WrapperGLFW
4. Execute the sh file to build the wrapper-binding
5. Link the dll to your project and link/copy WrapperGLFW.dll.config into your output directory (needed!)

## Important remarks
### Compatibility:
- The project is compatible with Mono. It can be opened and build by MonoDevelop too but the using the script file is recommended.
- The current GLFW.dll is for windows. For Linux/Mac please keep in mind to install glfw with your paket-manager  
- The wrapper which is highly recommended can be found under _generated_ or _src/WrapperGLFW/GLFW_Wrapper.cs_

### Dependencies for build:
- Mono is a must have

### Dependencies for generation:
- The library uses _CppSharp_ for binding generation.

## Contributing

It's appreciated if people try to help us making this binding better and staying up to date.
Feel free to send PR's and open issues if there are any!

*To build the generator under Linux or Mac you have to compile the latest CppSharp on your own.*
Windows developers can easily use the existing dlls in the folder dependencies.

## Questions, Suggestions
We're having a gitter chat for discussing binding generation and or additional features!
Feel free to join us!


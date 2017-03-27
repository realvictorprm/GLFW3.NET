# GLFW3.NET
[![Join us on Gitter!](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/glfw3dotnet/Lobby)
Automatic generated bindings of GLFW3 for .NET

## Introduction
The projects generates the bindings which are only half usable.
Reason for that is that the C-API uses headers from other API's which are not included in GLFW like OpenGL and Vulkan.
Therefore the bindings provide only names for the various unknown types, which can easily be changed to the type names from the binding you're using for OpenGL or Vulkan.

## Usage
1. Change the type mappings for the extension bindings if it's needed. 
2. Build the project
3. Run the project
4. Use the generated bindings with your bindings of OpenGL or Vulkan

## Important remarks
### Compatibility:
- The project is compatible with Mono. It can be opened by MonoDevelop and should be build there too
- The current GLFW.dll is for windows. Please keep in mind to use the correct GLFW.dll!
- There are generated bindings available in the _generated_ folder and may be compatible with every platform

### What we're using:
- The library uses _CppSharp_ for binding generation.

## Contributing

It's appreciated if people try to help us making this binding better and staying up to date.
Feel free to send PR's and open issues if there are any!

We're having a gitter chat for discussing binding generation and or additional features!
Feel free to join us!


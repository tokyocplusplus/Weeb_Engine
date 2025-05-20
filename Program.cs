//IN DEVELOPMENT STILL
using Silk.NET.Core;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
//using Silk.NET.Vulkan;
using System;
using Silk.NET.GLFW;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Silk.NET.Windowing;
using System.ComponentModel.Design;
//made with blood, sweat, and tears :3
//weeb engine
/*
About
[--Silk.NET--]
The high-speed OpenGL, 
OpenCL, OpenAL, OpenXR,
GLFW, SDL, Vulkan, Assimp,
WebGPU, and DirectX bindings 
library your mother warned you about.
*/
public class WeebEngine
{
    private static float idk;
    private static IWindow window;
    private static GL gl;
    private static uint vao;
    private static uint vbo;
    private static uint ebo;
    private static uint sProgram;
    public static void Main()
    {
        var res = new Vector2D<int>(1280, 720);
        var options = WindowOptions.Default;
        options.Size = res;
        options.Title = "HAIII:33!!";
        window = Window.Create(options);
        window.Render += RenderFrame;
        window.Load += LoadOnce;
        window.Update += UpdateOnFrame;
        window.FramebufferResize += OnFrameBufferResize;
        window.Run();
        window.Dispose();
    }

    private static unsafe void LoadOnce()
    {
        Console.WriteLine("RATIO = '16:9'\n");
        Console.WriteLine("RES = '1280x720'");
        //they'll never suspect it's hardcoded lmao
    }
    private static unsafe void RenderFrame(double deltaTime)
    {

    }
    private static void UpdateOnFrame(double deltaTime)
    {

    }
    private static void OnFrameBufferResize(Vector2D<int> D)
    {
        
    }
    private static void OnClose()
    {
        Console.WriteLine("exiting now");
    }
}

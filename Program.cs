using Silk.NET.Core;
using Silk.NET.OpenGL;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using System.Drawing;
using System.Runtime.CompilerServices;
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
public class Program
{
    const string vss = @"
    #version 330 core
    layout (location = 0) in vec3 aPos;
    void main()
    {
        gl_Position = vec4(aPos.xyz,1); // aPos.x, aPos.y, aPos.z, 1
    }
    ";
    const string fss = @"
    #version 330 core
    out vec4 fragColor;
    void main()
    {
        fragColor = vec4(1,0,1,1); // pink color
    }
    ";
    public static float[] vertices =
    {
        0.5f,0.5f,0.0f,  //0
        -0.5f,0.5f,0.0f, //1
        -0.5f,-0.5f,0.0f,//2
        0.5f,-0.5f,0.5f  //3
    };
    public static uint[] indices =
    {
        0,1,2,
        2,3,0
    };
    private static IWindow window;
    private static GL gl;
    private static uint vao;
    private static uint vbo;
    private static uint ebo;
    private static uint s;
    public static void Main(string[] args)
    {
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "haii:3"
        };
        window = Window.Create(options);
        window.Load += OnLoad;
        window.Update += OnUpdate;
        window.Render += OnRender;
        window.Run();
    }
    private static unsafe void OnLoad()
    {
        gl = window.CreateOpenGL();
        Console.WriteLine("Load!");
        IInputContext input = window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
        {
            input.Keyboards[i].KeyDown += KeyDown;
        }
        //gl.ClearColor(Color.DeepPink);
        vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);
        vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        fixed (float* buf = vertices)
        {
            gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }
        ebo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        fixed (uint* ibuf = indices)
        {
            gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), ibuf, BufferUsageARB.StaticDraw);
        }
        uint vS = gl.CreateShader(ShaderType.VertexShader);
        gl.ShaderSource(vS, vss);
        gl.CompileShader(vS);
        uint fS = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(fS, fss);
        gl.CompileShader(fS);
        s = gl.CreateProgram();
        gl.AttachShader(s, vS);
        gl.AttachShader(s, fS);
        gl.LinkProgram(s);
        gl.DetachShader(s, vS);
        gl.DetachShader(s, fS);
        gl.DeleteShader(vS);
        gl.DeleteShader(fS);
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), null);
        gl.EnableVertexAttribArray(0);

        gl.BindVertexArray(0);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

    }
    private static void OnUpdate(double deltaTime)
    {
        //Thread.Sleep(250);
    }
    private static unsafe void OnRender(double deltaTime)
    {
        gl.ClearColor(Color.Crimson);
        gl.Clear(ClearBufferMask.ColorBufferBit);
        gl.BindVertexArray(vao);
        gl.UseProgram(s);
        gl.DrawElements(PrimitiveType.Triangles, (uint)indices.Length, DrawElementsType.UnsignedInt, null); // haha nullptr
    }
    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
        {
            window.Close();
        }
    }
}

using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
//made with blood, sweat, and tears :3
//weeb engine
namespace mainFile
{
    public class Game : GameWindow
    {
        private Timer timer;
        private int vao;
        private int vbo;
        public Shader shader;

        private int ebo;
        //commenting out the ebo shit so then it can be used for indices later idk
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }
        float[] vertices = {
             0.5f,  0.5f, 0.0f,  1.0f,0.0f,0.0f,// top right
             0.5f, -0.5f, 0.0f,  0.0f,1.0f,0.0f,// bottom right
            -0.5f, -0.5f, 0.0f,  0.0f,0.0f,1.0f,// bottom left
            -0.5f,  0.5f, 0.0f,  1.0f,1.0f,0.0f// top left
        };
        uint[] indices = {
            0,1,3,
            1,2,3
        };
        public static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "haiii:3"))
            {
                game.Run();
            }
        }
        public double gettime()
        {
            return GLFW.GetTime();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffer(vbo);
            ///vertex shit
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            shader = new Shader("shaders/vert.glsl", "shaders/frag.glsl");
            shader.Use();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0f, 1f, 1f, 1f);
            shader.Use();
            //matrices handling for teh future but for now we're doing this color shit
            double timeValue = gettime();
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "uniColor");
            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            //rendering code goes here (bind shader, bind vao, render with drawarrays or drawelements)
            SwapBuffers();
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

//using System.Numerics;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
//made with blood, sweat, and tears :3
//weeb engine
//so far ive:
//1. created a square
//2. colored the square
//3. setup uniforms
//4. im still trying to get it into 3d
namespace mainFile
{
    public class Game : GameWindow
    {
        private int vao;
        private int vbo;
        public Shader shader;
        public Meow _meow;

        private int ebo;
        private float spinnyvalue = 1.0f;
        //commenting out the ebo shit so then it can be used for indices later idk
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }
        float[] vertices = {
            -0.5f, -0.5f, -0.5f,  1.0f,0.0f,1.0f,
            0.5f, -0.5f, -0.5f,  1.0f,0.0f,1.0f,
            0.5f,  0.5f, -0.5f,  1.0f,0.0f,1.0f,
            0.5f,  0.5f, -0.5f,  1.0f,0.0f,1.0f,
            -0.5f,  0.5f, -0.5f,  1.0f,0.0f,1.0f,
            -0.5f, -0.5f, -0.5f, 1.0f,0.0f,1.0f,

            -0.5f, -0.5f,  0.5f,  1.0f,0.0f,1.0f,
            0.5f, -0.5f,  0.5f,  1.0f,0.0f,1.0f,
            0.5f,  0.5f,  0.5f,  1.0f,0.0f,1.0f,
            0.5f,  0.5f,  0.5f,  1.0f,0.0f,1.0f,
            -0.5f,  0.5f,  0.5f,  1.0f,0.0f,1.0f,
            -0.5f, -0.5f,  0.5f,  1.0f,0.0f,1.0f,

            -0.5f,  0.5f,  0.5f,  1.0f,0.0f,1.0f,
            -0.5f,  0.5f, -0.5f,  1.0f,0.0f,1.0f,
            -0.5f, -0.5f, -0.5f, 1.0f,0.0f,1.0f,
            -0.5f, -0.5f, -0.5f, 1.0f,0.0f,1.0f,
            -0.5f, -0.5f,  0.5f,  1.0f,0.0f,1.0f,
            -0.5f,  0.5f,  0.5f,  1.0f,0.0f,1.0f,

            0.5f,  0.5f,  0.5f, 1.0f,0.0f,1.0f,
            0.5f,  0.5f, -0.5f, 1.0f,0.0f,1.0f,
            0.5f, -0.5f, -0.5f,  1.0f,0.0f,1.0f,
            0.5f, -0.5f, -0.5f,  1.0f,0.0f,1.0f,
            0.5f, -0.5f,  0.5f, 1.0f,0.0f,1.0f,
            0.5f,  0.5f,  0.5f, 1.0f,0.0f,1.0f,

            -0.5f, -0.5f, -0.5f, 1.0f,0.0f,1.0f,
            0.5f, -0.5f, -0.5f, 1.0f,0.0f,1.0f,
            0.5f, -0.5f,  0.5f,  1.0f,0.0f,1.0f,
            0.5f, -0.5f,  0.5f, 1.0f,0.0f,1.0f,
            -0.5f, -0.5f,  0.5f,  1.0f,0.0f,1.0f,
            -0.5f, -0.5f, -0.5f,  1.0f,0.0f,1.0f,

            -0.5f,  0.5f, -0.5f, 1.0f,0.0f,1.0f,
            0.5f,  0.5f, -0.5f,  1.0f,0.0f,1.0f,
            0.5f,  0.5f,  0.5f, 1.0f,0.0f,1.0f,
            0.5f,  0.5f,  0.5f,  1.0f,0.0f,1.0f,
            -0.5f,  0.5f,  0.5f, 1.0f,0.0f,1.0f,
            -0.5f,  0.5f, -0.5f,  1.0f,0.0f,1.0f,
        };
        /*uint[] indices = {
            0,1,3,
            1,2,3
        };*/
        public static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "haiii:3"))
            {
                game.Run();
            }
        }
        public float gettime()
        {
            return (float)GLFW.GetTime();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            spinnyvalue += 0.3f;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            //ebo = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffer(vbo);
            ///vertex shit
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            //this is lowkey a shitty way to do this
            shader = new Shader("shaders/vert.glsl", "shaders/frag.glsl");
            shader.Use();
            GL.Enable(EnableCap.DepthTest);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);
            shader.Use();
            //matrices handling for teh future but for now we're doing this color shit
            float time = (float)gettime();
            
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800f / 600f, 0.1f, 100.0f);
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spinnyvalue));
            int modelLoc = GL.GetUniformLocation(shader.Handle, "model");
            int viewLoc = GL.GetUniformLocation(shader.Handle, "view");
            int projectionLoc = GL.GetUniformLocation(shader.Handle, "proj");
            GL.UniformMatrix4(modelLoc, false, ref model);
            GL.UniformMatrix4(viewLoc, false, ref view);
            GL.UniformMatrix4(projectionLoc, false, ref projection);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
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

//using System.Numerics;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using camClass;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using System;

//made with blood, sweat, and tears :3
//weeb engine
//so far ive:
//1. created a square
//2. colored the square
//3. setup uniforms
//4. GOT IT INTO 3D!!!
//5. nearly implemented a camera with camera movement, WASD SHIFT CONTROL SPACE all works

namespace mainFile
{
    public class Game : GameWindow
    {
        public int width = 1280;
        public int height = 720;
        private int vao;
        private int vbo;
        private int floorVAO;
        private int floorVBO;
        public Vector3 camPos = new Vector3(0.0f, 0.0f, 3.0f); // -3.0f is forward, 3.0f is backwards
        public Vector3 camTarget;
        public Vector3 camDir;
        bool _firstMove = true;
        public Vector3 camRight;
        public Vector2 _lastPos;
        public int modelLoc;
        public int viewLoc;
        private int NUM_OF_FACES = 6;
        private int TRIANGLES_PER_FACE = 2;
        private int VERTICES_PER_TRIANGLE = 3;
        public int projectionLoc;
        public Camera _camera;
        public Vector3 camUp;
        public Vector2 lastPos;
        public float Pitch;
        public float mouseX;
        public float mouseY;
        public Vector3 camFront;
        private float speed = 1.5f;
        private float sprintSpeed = 2.5f;
        public Shader shader;
        public bool sprinting = false;
        public Meow _meow;
        private int ebo;
        private float spinnyvalue = 1.0f;
        //commenting out the ebo shit so then it can be used for indices later idk
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }
        float[] vertices = {
            // Back face - Red
            -0.5f, -0.5f, -0.5f,  1f, 0f, 0f, // Red
            0.5f, -0.5f, -0.5f,  1f, 0f, 0f,
            0.5f,  0.5f, -0.5f,  1f, 0f, 0f,
            0.5f,  0.5f, -0.5f,  1f, 0f, 0f,
            -0.5f,  0.5f, -0.5f,  1f, 0f, 0f,
            -0.5f, -0.5f, -0.5f,  1f, 0f, 0f, 

            // Front face - Orange
            -0.5f, -0.5f,  0.5f,  1f, 0.5f, 0f, // Orange
            0.5f, -0.5f,  0.5f,  1f, 0.5f, 0f,
            0.5f,  0.5f,  0.5f,  1f, 0.5f, 0f,
            0.5f,  0.5f,  0.5f,  1f, 0.5f, 0f,
            -0.5f,  0.5f,  0.5f,  1f, 0.5f, 0f,
            -0.5f, -0.5f,  0.5f,  1f, 0.5f, 0f, 

            // Left face - Yellow
            -0.5f,  0.5f,  0.5f,  1f, 1f, 0f, // Yellow
            -0.5f,  0.5f, -0.5f,  1f, 1f, 0f,
            -0.5f, -0.5f, -0.5f,  1f, 1f, 0f,
            -0.5f, -0.5f, -0.5f,  1f, 1f, 0f,
            -0.5f, -0.5f,  0.5f,  1f, 1f, 0f,
            -0.5f,  0.5f,  0.5f,  1f, 1f, 0f, 

            // Right face - Green
            0.5f,  0.5f,  0.5f,  0f, 1f, 0f, // Green
            0.5f,  0.5f, -0.5f,  0f, 1f, 0f,
            0.5f, -0.5f, -0.5f,  0f, 1f, 0f,
            0.5f, -0.5f, -0.5f,  0f, 1f, 0f,
            0.5f, -0.5f,  0.5f,  0f, 1f, 0f,
            0.5f,  0.5f,  0.5f,  0f, 1f, 0f, 

            // Bottom face - Blue
            -0.5f, -0.5f, -0.5f,  0f, 0f, 1f, // Blue
            0.5f, -0.5f, -0.5f,  0f, 0f, 1f,
            0.5f, -0.5f,  0.5f,  0f, 0f, 1f,
            0.5f, -0.5f,  0.5f,  0f, 0f, 1f,
            -0.5f, -0.5f,  0.5f,  0f, 0f, 1f,
            -0.5f, -0.5f, -0.5f,  0f, 0f, 1f, 

            // Top face - Violet
            -0.5f,  0.5f, -0.5f,  0.5f, 0f, 0.5f, // Violet
            0.5f,  0.5f, -0.5f,  0.5f, 0f, 0.5f,
            0.5f,  0.5f,  0.5f,  0.5f, 0f, 0.5f,
            0.5f,  0.5f,  0.5f,  0.5f, 0f, 0.5f,
            -0.5f,  0.5f,  0.5f,  0.5f, 0f, 0.5f,
            -0.5f,  0.5f, -0.5f,  0.5f, 0f, 0.5f
        };
        float[] floorVertices =
        {
            -1,-1,-1, 1,0,1, // forward left
            1,-1,-1,  1,0,1, // forward right
            -1,-1,1,  0,1,1, // back left
            1,-1,1,   1,1,0  // back right
            /*
            Triangles strips are rendered in a zigzag order, it's hard to explain...
            */
        };
        /*uint[] indices = {
            0,1,3,
            1,2,3
        };*/
        public static void Main(string[] args)
        {
            using (Game game = new Game(1280, 720, "rubber duck when"))
            {
                game.Run();
                //aiosndcosa
            }
        }
        public float gettime()
        {
            return (float)GLFW.GetTime();
        }
        public void updateCamPos(FrameEventArgs args)
        {
            KeyboardState Input = KeyboardState;
            const float sensitivity = 0.2f;
            if (Input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.LeftControl))
            {
                _camera.Position -= _camera.Up * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.LeftShift))
            {
                sprinting = true;
            }
            else
            {
                sprinting = false;
            }
            var mouse = MouseState;
            if (_firstMove) // this is like so if its the first time clicking on the window and stuff
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // apply the yaw and pitch, i could explain this more in depth but ill just give a simplified explanation
                /*
                * so as everybody is generally semi familiar with calculus and projection math, i'll explain it using trigonometry
                imagine a right angle triangle with the 90 degree angle facing a wall in front of you, your eyes is where the angle of elevation and depression begins.
                the Hypotenuse is always set to 1;
                S = O/H; C = A/H; T = O/A
                Sine controls pitch, sin(y/h)  (y is the resolution, h is always 1);
                Cosine controls yaw, cos(x/h) (x is the resolution, h is always 1);
                as you know about finding angles and side lengths, we have some specific formulas around it.
                This math singlehandedly controls the projection matrix and camera controls.
                You'll see this math quite often in raycasting, in which not many of you are familiar with; In order to understand this, you should have some background in building raycasters.
                */
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            KeyboardState input = KeyboardState;
            //updatecam();
            if (!IsFocused)
            {
                return; // checking if the window isn't focused lol, just what it looks like, nothing special happening over here xD
            }
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            updateCamPos(args);
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            //_meow.Haiiiiiii();
            //initcamstuff();
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            Console.WriteLine("Vertex Array Binded");
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            Console.WriteLine("Vertex Buffer Object Binded");
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);
            Console.WriteLine("Vertices Attrib Pointer Binded");
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            Console.WriteLine("Colours Attrib Pointer Binded");
            floorVAO = GL.GenVertexArray();
            GL.BindVertexArray(floorVAO);
            floorVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, floorVertices.Length * sizeof(float), floorVertices, BufferUsageHint.StaticDraw);
            //ebo = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffer(vbo);
            ///vertex shit
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            Console.WriteLine("Vertices Attrib Pointer Binded");
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            Console.WriteLine("Colours Attrib Pointer Binded");

            //this is lowkey a shitty way to do this
            shader = new Shader("shaders/vert.glsl", "shaders/frag.glsl");
            shader.Use();
            Console.WriteLine("Shader Binded");
            //_meow.Haiiiiiii();
            GL.Enable(EnableCap.DepthTest);
            _camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f), (float)ClientSize.X / (float)ClientSize.Y);
            CursorState = CursorState.Grabbed;
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);
            shader.Use();
            modelLoc = GL.GetUniformLocation(shader.Handle, "model");
            viewLoc = GL.GetUniformLocation(shader.Handle, "view");
            projectionLoc = GL.GetUniformLocation(shader.Handle, "proj");
            //matrices handling for teh future but for now we're doing this color shit
            //float time = (float)gettime();
            float dt = (float)args.Time; // delta time in seconds and stuff as well as converting it from a double to a float
            spinnyvalue += 90.0f * dt; // it rotates 90 degrees every second
            float spinnyX = spinnyvalue * -1.0f; // make it negative rotation speed
            float spinnyY = spinnyvalue * -1.0f; // same thing as above me
            Matrix4 view = _camera.GetViewMatrix();//Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = _camera.GetProjectionMatrix();//Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 1280f / 720f, 0.1f, 100.0f);
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spinnyY)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(spinnyX));
            Matrix4 model2 = Matrix4.Identity;
            //Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spinnyvalue));
            //model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(spinnybutx));
            GL.UniformMatrix4(modelLoc, false, ref model); // for some reason it doesn't like direct values, it prefers a reference to a memory address of the value in the stack memory
            GL.UniformMatrix4(viewLoc, false, ref view); // for some reason it doesn't like direct values, it prefers a reference to a memory address of the value in the stack memory
            GL.UniformMatrix4(projectionLoc, false, ref projection); // for some reason it doesn't like direct values, it prefers a reference to a memory address of the value in the stack memory
            GL.BindVertexArray(vao); // you have you bind the vertex array before you do anything that is even remotely related to using the vertices or indices like drawing and shit
            GL.DrawArrays(PrimitiveType.Triangles, 0, NUM_OF_FACES * TRIANGLES_PER_FACE * VERTICES_PER_TRIANGLE); // actually draw the cube, i wonder if i could get away with 8 vertices instead of this ass handling.. 36 elements is a lot

            // Set the model matrix for the floor separaely
            GL.UniformMatrix4(modelLoc, false, ref model2);
            GL.BindVertexArray(floorVAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4); // theres only 4 vertices
            //GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();

        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _camera.AspectRatio = (float)ClientSize.X / (float)ClientSize.Y;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            //Console.WriteLine("Exiting program now..");
            //Thread.Sleep(500);
            new Thread(() =>
            {
                Console.WriteLine("Unbinding VAOs");
                GL.BindVertexArray(0);
                GL.DeleteVertexArray(vao);
                GL.DeleteVertexArray(floorVAO);
                Console.WriteLine("VAOs unbinded successfully");
                Thread.Sleep(200);
            }).Start();
            new Thread(() =>
            {
                Console.WriteLine("Unbinding VBOs");
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(vbo);
                GL.DeleteBuffer(floorVBO);
                Console.WriteLine("VBOs unbinded successfully");
                Thread.Sleep(200);
            }).Start();
        }
    }
}

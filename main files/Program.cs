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
using translationFuncs;
using ImGuiNET;
using Dear_ImGui_Sample;
using Json.Net;
//using System.Numerics;
//using OpenTK.Graphics.OpenGL;
#pragma warning disable CS8618 //  this warning is ASS i hate it i hate it
#pragma warning disable CS0809 // i hate this warning lmao
//i need to organize this code a little..
//using OpenTK.Graphics.OpenGL;

/*
update this section with how many lines of code there are in the project so far:
a couple days ago: 
- 656 lines of code
around the 3rd of june (HAPPY PRIDE MONTH!!!): 
- 1430 lines of code (holy shit)
6th of june now :3 (HAPPY PRIDE MONTH AGAIN!!!):
- 1541 lines of code (damn)
*/

// Add a flag to track UI navigation mode

//made with "blood, sweat, and tears™" :3
//weeb engine
//656 lines of code, i think im getting somewhere
//so far:
//1. created a square :3
//2. colored the square :3
//3. setup uniforms :3
//4. GOT IT INTO 3D!!! :3
//5. implemented a camera with camera movement, WASD SHIFT CONTROL SPACE all works :3
//6. implemented basic UI :3
//7. implementing basic light :3
//7.5 IMPLEMENT A LEVEL EDITOR?? as this is opengl, i might actually just make a python script or something idk, im too slack to code a level editor full on in C and shit, or maybe i can do what raylib did? with the drawcube functiosn and stuff, but that wouldn't work out too well for long and big projects??? i dont know dude.. im thinking of the python script instead
//8. add obj loading when?? 
//9. it seems i've got a lot to learn, especially about making apps, or i just need some monster to drink :p
//10. i wish i was getting somewhere, it feels like im just nowhere. its all working but how do i make a level editor, shouldn't i just use blender? i wonder if i could make it in SDL or something.. maybe?? idk..

namespace mainFile
{
    public class Game : GameWindow
    {
        /*----------VARIABLES FOR GAME----------*/
        //creating an object to use the functions that make my code simpler.
        ImGuiController _controller;
        TranslationFuncs funcs = new TranslationFuncs();
        //window width
        public int width = 1280;
        //window height
        public int height = 720;
        //vertex array object
        private int vao;
        //vertex buffer object
        private int vbo;
        private bool _navigationMode = false;
        private int modelVAO;
        public float skillissue;
        private int modelVBO;
        public static ZenConfigParser parser;
        private int modelEBO;
        private bool pressed;
        private int howmuchilovethisproject; // how much i love this project??
        //vao but for the floor object/walls/etc
        private int floorVAO;
        //vbo but once again for the walls/floor/etc
        private int floorVBO;
        // this is used to tell whether it's the first time
        // that their house has moved.
        bool _firstMove = true;
        // this is for calculating the offset of the mouse to make sure it runs smoothly.
        public Vector2 _lastPos;
        //this is used for the location of matrices
        public int modelLoc;
        //this is used for the location of matrices
        public int viewLoc;
        public int lampvao;
        public int lampvbo;
        public int lampebo;
        public byte[]? stringinput = new byte[512];
        //this is used for the location of matrices
        public int projectionLoc;
        //creating a camera object, initialize it later in the main file
        public Camera _camera;
        //lastPos is used for the mouse stuff;
        public Vector2 lastPos;
        //used for some of the calculations for mouse movement and cam rotation math
        public float Pitch;
        //private Vector3 _back; // i dont know what i tried to do with this one
        //mouse position.x
        public float mouseX;
        //mouse position.y
        public float mouseY;
        //public Vector3 camUp;
        //maybe i'll use this maybe not but it is kinda just the cross product of v(0,0,1) and v(1,0,0) so it's not hard to calculate
        public Vector3 camFront;
        //basic speed (not running)
        private float speed = 1.5f;
        //for speed calc
        private float sprintSpeed = 2.5f;
        //creating shader object but initialize it later
        public Shader shader;
        //sprinting bool for speed calc
        public bool sprinting = false;
        //meow
        public Meow _meow;
        //ebo shit is no longer commented out
        private int ebo;
        //how many degrees (this gets converted from deg to rad);
        private float spinnyvalue = 1.0f;
        //this is an object for the game

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }
        //#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        //vertex data and stuff
        float[] vertices = {
            // Positions         // Colors
            // Back face
            -0.5f, -0.5f, -0.5f,  1f, 0f, 0f, // Bottom-left
             0.5f, -0.5f, -0.5f,  1f, 0f, 0f, // Bottom-right
             0.5f,  0.5f, -0.5f,  1f, 0f, 0f, // Top-right
            -0.5f,  0.5f, -0.5f,  1f, 0f, 0f, // Top-left

            // Front face
            -0.5f, -0.5f,  0.5f,  1f, 0.5f, 0f, // Bottom-left
             0.5f, -0.5f,  0.5f,  1f, 0.5f, 0f, // Bottom-right
             0.5f,  0.5f,  0.5f,  1f, 0.5f, 0f, // Top-right
            -0.5f,  0.5f,  0.5f,  1f, 0.5f, 0f, // Top-left

            // Left face
            -0.5f,  0.5f,  0.5f,  1f, 1f, 0f, // Top-front
            -0.5f,  0.5f, -0.5f,  1f, 1f, 0f, // Top-back
            -0.5f, -0.5f, -0.5f,  1f, 1f, 0f, // Bottom-back
            -0.5f, -0.5f,  0.5f,  1f, 1f, 0f, // Bottom-front

            // Right face
             0.5f,  0.5f,  0.5f,  0f, 1f, 0f, // Top-front
             0.5f,  0.5f, -0.5f,  0f, 1f, 0f, // Top-back
             0.5f, -0.5f, -0.5f,  0f, 1f, 0f, // Bottom-back
             0.5f, -0.5f,  0.5f,  0f, 1f, 0f, // Bottom-front

            // Bottom face
            -0.5f, -0.5f, -0.5f,  0f, 0f, 1f, // Back-left
             0.5f, -0.5f, -0.5f,  0f, 0f, 1f, // Back-right
             0.5f, -0.5f,  0.5f,  0f, 0f, 1f, // Front-right
            -0.5f, -0.5f,  0.5f,  0f, 0f, 1f, // Front-left

            // Top face
            -0.5f,  0.5f, -0.5f,  0.5f, 0f, 0.5f, // Back-left
             0.5f,  0.5f, -0.5f,  0.5f, 0f, 0.5f, // Back-right
             0.5f,  0.5f,  0.5f,  0.5f, 0f, 0.5f, // Front-right
            -0.5f,  0.5f,  0.5f,  0.5f, 0f, 0.5f  // Front-left
        };
        //index data and stuff
        uint[] indices = {
            // Back face
            0, 1, 2, 2, 3, 0,
            // Front face
            4, 5, 6, 6, 7, 4,
            // Left face
            8, 9, 10, 10, 11, 8,
            // Right face
            12, 13, 14, 14, 15, 12,
            // Bottom face
            16, 17, 18, 18, 19, 16,
            // Top face
            20, 21, 22, 22, 23, 20
        };
        //more vertex data
        float[] floorVertices =
        {
            -1,-1,-1, 1,0,1, // forward left
            1,-1,-1,  1,0,1, // forward right
            -1,-1,1,  0,1,1, // back left
            1,-1,1,   1,1,0  // back right
            /*
            Triangles strips are rendered in a zigzag order, it's hard to explain...
            //generally its forward left to back right but in a zig zag, hard to imagine so ill give a zig zag example
            the quad is like this
            1,1,1,1,
            1,1,1,1,
            1,1,1,1,
            1,1,1,1,
            but it's rendered using vertices like this
            V1,  -->, -->,  V2,
            0,   0,  <--,   0,
            0,   <--,  0,   0,
            V3,  -->,  -->, V4
            then it turns out in straight lines in the assembly shader
            1,1,1,1,
            0,0,1,0,
            0,1,0,0,
            1,1,1,1
            then it's rendered into triangles, causing two seperate triangles to merge into a cube
            1,1,1,1,
            1,1,0,0
            and
            0,0,1,1,
            1,1,1,1
            that then forms
            1,1,1,1,
            1,1,1,1,
            1,1,1,1,
            1,1,1,1
            */
        };
        //no index data needed, im using triangle strips for this
        /*uint[] indices = {
            0,1,3,
            1,2,3
        };*/
        public static void Main(string[] args)
        {
            parser = new ZenConfigParser("Config/Config.zenconf");
            using (Game game = new Game(parser.Width, parser.Height, parser.Title))
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
            Vector3 camfrontfixed = new Vector3(_camera.Front.X, 0, _camera.Front.Z);
            //_back = -Vector3.Cross(_camera.Up, _camera.Right);
            if (Input.IsKeyDown(Keys.W))
            {
                _camera.Position += camfrontfixed * speed * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                _camera.Position -= camfrontfixed * speed * (float)args.Time;
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
            // Calculate x  the offset of the mouse position
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y); 
            _camera.Yaw += deltaX * sensitivity;
            _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            
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
                If you do understand this code, congrats
                */
            
        }
        // Track if the mouse is captured or not
        private bool _mouseCaptured = true;

        protected override void OnLoad()
        {
            base.OnLoad();
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            Console.WriteLine("Vertex Array Binded");
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            Console.WriteLine("Vertex Buffer Object Binded");
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            Console.WriteLine("Element Array Buffer Object Binded");
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            Console.WriteLine("Vertices Attrib Pointer Binded");
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            Console.WriteLine("Colours Attrib Pointer Binded");
            //floor stuff and floor data init
            floorVAO = GL.GenVertexArray();
            GL.BindVertexArray(floorVAO);
            Console.WriteLine("Floor VAO Binded");
            floorVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, floorVertices.Length * sizeof(float), floorVertices, BufferUsageHint.StaticDraw);
            Console.WriteLine("Floor Vertex Buffer Object Binded");
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            Console.WriteLine("Vertices Attrib Pointer2 Binded");
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            Console.WriteLine("Colours Attrib Pointer2 Binded");

            lampvao = GL.GenVertexArray();
            GL.BindVertexArray(lampvao);
            lampvbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, lampvbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            lampebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, lampebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            Console.WriteLine("Vertices Attrib Pointer2 Binded");
            //color enabling
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            Console.WriteLine("Colours Attrib Pointer2 Binded");

            //shader handling
            shader = new Shader("shaders/vert.glsl", "shaders/frag.glsl");
            shader.Use();
            Console.WriteLine("Shader Binded");
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            _camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f), (float)ClientSize.X / (float)ClientSize.Y);
            // Start with mouse captured
            CursorState = CursorState.Grabbed;
            _mouseCaptured = true;
            
            _meow = new Meow("haii");
            Console.WriteLine(parser.whatdoesthisstringdo);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (!IsFocused)
            {
                return;
            }

            // Prevent camera/game input when ImGui wants keyboard input
            bool imguiWantsKeyboard = ImGui.GetIO().WantCaptureKeyboard || ImGui.GetIO().WantTextInput;
            bool imguiWantsMouse = ImGui.GetIO().WantCaptureMouse;

            if (!_navigationMode && !imguiWantsKeyboard && !imguiWantsMouse)
            {
                updateCamPos(args);
                _mouseCaptured = true;
                CursorState = CursorState.Hidden;
                //CursorState = CursorState.Grabbed;
                //CursorState = CursorState.Confined;
            }
            else
            {
                _mouseCaptured = false;
                CursorState = CursorState.Normal;
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            //if (e.Key == Keys.Tab && _mouseCaptured)
            //{
            //CursorState = CursorState.Normal;
            //_mouseCaptured = false;
            //}
            if (e.Key == Keys.Tab)
            {
                _navigationMode = !_navigationMode;
                //this works lmao
            }
            if (e.Key == Keys.Escape)
            {
                CursorState = CursorState.Normal;
            }
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            //??? the fuck do i do??
        }
        [Obsolete]
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _controller.Update(this, (float)args.Time);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);
            ImGui.Begin("haii this is my GUI");
            ImGui.StyleColorsDark();
            ImGui.Text("hai!!");
            if (ImGui.Button("press me to open popup"))
            {
                ImGui.OpenPopup("popup!");
            }
            if (ImGui.BeginPopup("popup!"))
            {
                ImGui.SliderInt("How much i love this project", ref howmuchilovethisproject, 0, 100);
                ImGui.Text("this is a popup!");
                ImGui.Button("this button does nothing");
                ImGui.EndPopup();
            }
            ImGui.End();
            ImGui.Begin("another window haha");
            ImGui.Text("this is a test text thing");
            ImGui.InputText("idk type this in", stringinput, (UInt32)512);
            ImGui.End();
            shader.Use();
            modelLoc = GL.GetUniformLocation(shader.Handle, "model");
            viewLoc = GL.GetUniformLocation(shader.Handle, "view");
            projectionLoc = GL.GetUniformLocation(shader.Handle, "proj");
            //matrices handling for teh future but for now we're doing this color shit
            //float time = (float)gettime();
            float dt = (float)args.Time; // delta time in seconds and stuff as well as converting it from a double to a float
            spinnyvalue += 90.0f * dt; // it rotates 90 degrees every second
            float spinnyX = spinnyvalue * -1.0f; // make it negative rotation speed
            float spinnyY = spinnyvalue * -1.0f; // same thing as above this line
            Matrix4 view = _camera.GetViewMatrix();//Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = _camera.GetProjectionMatrix();//Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 1280f / 720f, 0.1f, 100.0f);
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spinnyY)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(spinnyX));
            Matrix4 floor = Matrix4.CreateScale(new Vector3(3.0f, 1.0f, 3.0f));
            Matrix4 forwardwall = Matrix4.CreateScale(new Vector3(3f, 1f, 3f)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateTranslation(new Vector3(0.0f, 2.0f, -2f));
            Matrix4 rightwall = Matrix4.CreateScale(new Vector3(3.0f, 1.0f, 3.0f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateTranslation(new Vector3(2.0f, 2.0f, 0.0f));
            Matrix4 leftwall = Matrix4.CreateScale(new Vector3(3.0f, 1.0f, 3.0f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateTranslation(new Vector3(-4.0f, 2.0f, 0.0f));
            Matrix4 backwall = Matrix4.CreateScale(new Vector3(3.0f, 1.0f, 3.0f)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateTranslation(new Vector3(0.0f, 2.0f, 4.0f));
            Matrix4 roof = Matrix4.CreateScale(new Vector3(3.0f, 1.0f, 3.0f)) * Matrix4.CreateTranslation(new Vector3(0.0f, 6.0f, 0f));
            Matrix4 lamp = Matrix4.CreateTranslation(new Vector3(1.0f, 1.0f, 1.0f));
            //Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(spinnyvalue));
            //model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(spinnybutx));
            GL.UniformMatrix4(viewLoc, false, ref view); // for some reason it doesn't like direct values, it prefers a reference to a memory address of the value in the stack memory
            GL.UniformMatrix4(projectionLoc, false, ref projection); // for some reason it doesn't like direct values, it prefers a reference to a memory address of the value in the stack memory
            GL.BindVertexArray(vao); // you have you bind the vertex array before you do anything that is even remotely related to using the vertices or indices like drawing and shit
            //GL.DrawArrays(PrimitiveType.Triangles, 0, NUM_OF_FACES * TRIANGLES_PER_FACE * VERTICES_PER_TRIANGLE); // actually draw the cube, i wonder if i could get away with 8 vertices instead of this ass handling.. 36 elements is a lot
            //GL.DrawElements(PrimitiveType.Triangles, NUM_OF_FACES * TRIANGLES_PER_FACE * VERTICES_PER_TRIANGLE, DrawElementsType.UnsignedInt, 0);
            GL.UniformMatrix4(modelLoc, false, ref model); // for some reason it doesn't like direct values, it prefers a reference to a memory address of the value in the stack memory
            funcs.DrawButIndices(vao, 6, 2, 3, false);
            funcs.PlaceWall(floorVAO, false, modelLoc, ref floor);          // floor
            funcs.PlaceWall(floorVAO, false, modelLoc, ref forwardwall);    // front wall
            funcs.PlaceWall(floorVAO, false, modelLoc, ref backwall);       // back wall
            funcs.PlaceWall(floorVAO, false, modelLoc, ref roof);           // roof
            funcs.PlaceWall(floorVAO, false, modelLoc, ref rightwall);      // right wall
            funcs.PlaceWall(floorVAO, false, modelLoc, ref leftwall);       // left wall
            GL.UniformMatrix4(modelLoc, false, ref lamp);
            GL.BindVertexArray(lampvao);
            GL.DrawElements(BeginMode.Triangles, 12 * 3, DrawElementsType.UnsignedInt, 0);
            _controller.Render();
            SwapBuffers();
        }
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _camera.AspectRatio = (float)ClientSize.X / (float)ClientSize.Y;
            _controller.WindowResized(ClientSize.X, ClientSize.Y);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            //Console.WriteLine("Exiting program now..");
            //Thread.Sleep(500);
            //this is probably the most unsafe way to do this lmao
            new Thread(
            () =>
            {
                Console.WriteLine("Unbinding VAOs");
                GL.BindVertexArray(0);
                GL.DeleteVertexArray(vao);
                GL.DeleteVertexArray(floorVAO);
                Console.WriteLine("VAOs unbinded successfully");
                Thread.Sleep(200);
                //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(vbo);
                Console.WriteLine("VBO Successfully Deleted");
                Thread.Sleep(200);
                //GL.DeleteBuffer(ebo);

            }
            ).Start();
            new Thread(
            () =>
            {
                Console.WriteLine("Unbinding VBOs");
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(vbo);
                GL.DeleteBuffer(floorVBO);
                Console.WriteLine("VBOs unbinded successfully");
                Thread.Sleep(200);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.DeleteBuffer(ebo);
                Console.WriteLine("Ebo successfully deleted");
                Thread.Sleep(200);
                //funcs.PlaceWall();
            }
            ).Start();
        }
    }
}

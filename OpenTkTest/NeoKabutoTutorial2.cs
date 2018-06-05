using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.IO;

namespace OpenTkTest
{
    public class NeoKabutoTutorial2 : GameWindow
    {
        private int programId;
        private int vertexShaderId;
        private int fragmentShaderId;

        private int attribute_vcol;
        private int attribute_vpos;
        private int uniform_mview;

        private int vertexBufferObject_position;
        private int vertexBufferObject_color;
        private int vertexBufferObject_modelView;

        private Vector3[] vertexData;
        private Vector3[] colorData;
        private Matrix4[] modelViewData;

        private const string vPosition = "vPosition";
        private const string vColor = "vColor";
        private const string modelView = "modelView";

        public NeoKabutoTutorial2() { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeGPUProgram();

            vertexData = new Vector3[]
            {
                new Vector3(-0.8f, -0.8f, 0.0f),
                new Vector3(0.8f, -0.8f, 0.0f),
                new Vector3(0.0f, 0.8f, 0.0f),
            };

            colorData = new Vector3[]
            {
                new Vector3(1f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 0f),
            };

            modelViewData = new Matrix4[]
            {
                Matrix4.Identity
            };

            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            //GL.Begin(PrimitiveType.Triangles);
            //GL.Vertex3(vertexData[0]);
            //GL.Vertex3(vertexData[1]);
            //GL.Vertex3(vertexData[2]);
            //GL.End();

            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_position);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * Vector3.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject_color);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, false, 0, 0);


            GL.UseProgram(programId);
            GL.UniformMatrix4(uniform_mview, false, ref modelViewData[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        }

        private void InitializeGPUProgram()
        {
            // Initialize Graphics Program
            programId = GL.CreateProgram();

            // Load the Vertex and Fragment Shaders from the resources resx
            LoadShader(new MemoryStream(Properties.Resources.vs), ShaderType.VertexShader, programId, out vertexShaderId);
            LoadShader(new MemoryStream(Properties.Resources.fs), ShaderType.FragmentShader, programId, out fragmentShaderId);

            // Link the Graphics Program
            GL.LinkProgram(programId);
            Console.WriteLine(GL.GetProgramInfoLog(programId));

            // Get the Attribute IDs for the position, color, and modelView variables from the graphics program
            attribute_vpos = GL.GetAttribLocation(programId, vPosition);
            attribute_vcol = GL.GetAttribLocation(programId, vColor);
            uniform_mview = GL.GetUniformLocation(programId, modelView);

            // Check that all of hte attributes bound correctly
            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
                Console.WriteLine("Error binding attribute...");

            // Generate the buffers for the graphics attributes
            GL.GenBuffers(1, out vertexBufferObject_position);
            GL.GenBuffers(1, out vertexBufferObject_color);
            GL.GenBuffers(1, out vertexBufferObject_modelView);
        }

        private void LoadShader(Stream shaderSource, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);

            using (StreamReader sr = new StreamReader(shaderSource))
            {
                string source = sr.ReadToEnd();
                GL.ShaderSource(address, source);
            }

            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
    }
}

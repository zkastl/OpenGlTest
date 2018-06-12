using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace OpenTkTest
{
    class SuperBibleTutorial : GameWindow
    {
        private double elapsedTime;
        private int programId;
        private int vertexArrayObject;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CompileShaders();
            GL.CreateVertexArrays(1, out vertexArrayObject);
            GL.BindVertexArray(vertexArrayObject);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            elapsedTime += e.Time;

            GL.ClearColor(new Color4((float)(Math.Sin(elapsedTime)) * 0.5f + 0.5f, (float)(Math.Cos(elapsedTime)) * 0.5f + 0.5f, 0.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(programId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            GL.DeleteProgram(programId);
            GL.DeleteVertexArray(vertexArrayObject);
        }

        private void CompileShaders()
        {
            int vertexShaderId, fragmentShaderId;

            // Create and compile the vertex shader
            vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderId, new StreamReader(new MemoryStream(Properties.Resources.vs)).ReadToEnd());
            GL.CompileShader(vertexShaderId);

            // Create and compile the fragment shader
            fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderId, new StreamReader(new MemoryStream(Properties.Resources.fs)).ReadToEnd());
            GL.CompileShader(fragmentShaderId);

            // Create the shader programe, attach the compiled shaders and link it
            programId = GL.CreateProgram();
            GL.AttachShader(programId, vertexShaderId);
            GL.AttachShader(programId, fragmentShaderId);
            GL.LinkProgram(programId);

            // Delete the shaders as the program has them
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);
        }
    }
}

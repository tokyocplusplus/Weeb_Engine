using System.Security.Cryptography.X509Certificates;
using mainFile;
using OpenTK.Graphics.OpenGL;

public class Shader
{
    public int Handle;
    int VertexShader;
    int FragmentShader;
    public Shader(string vertexPath, string fragmentPath)
    {
        string VertexShaderSource = File.ReadAllText(vertexPath);
        string FragmentShaderSource = File.ReadAllText(fragmentPath);
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);
        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);
        GL.CompileShader(VertexShader);
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }
        GL.CompileShader(FragmentShader);
        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success2);
        if (success2 == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);
        GL.LinkProgram(Handle);
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }
    public void Use()
    {
        GL.UseProgram(Handle);
    }
}
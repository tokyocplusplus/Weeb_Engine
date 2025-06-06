using mainFile;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace translationFuncs
{
    public class TranslationFuncs
    {
        public void PlaceWall(int VAO, bool transposematrix, int location, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(location, false, ref matrix);
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }
        public void DrawButIndices(int VAO,  int NUM_OF_FACES, int TRIANGLES_PER_FACE, int VERTICES_PER_TRIANGLE, bool transposematrix)
        {
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, NUM_OF_FACES * TRIANGLES_PER_FACE * VERTICES_PER_TRIANGLE, DrawElementsType.UnsignedInt, 0);
        }
    }
}
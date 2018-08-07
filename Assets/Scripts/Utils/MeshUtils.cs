using Unity.Mathematics;
using UnityEngine;

public static class MeshUtils
{
    /// <summary>
    /// Generates a simple quad of any size
    /// </summary>
    /// <param name="size">The size of the quad</param>
    /// <param name="pivot">Where the mesh pivots</param>
    /// <returns>The quad mesh</returns>
    public static Mesh GenerateQuad(float2 size, Vector2 pivot)
    {
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        Vector3[] _vertices =
        {
            new Vector3(pivot.x + halfWidth, 0, pivot.y + halfHeight),
            new Vector3(pivot.x + halfWidth, 0, pivot.y - halfHeight),
            new Vector3(pivot.x - halfWidth, 0, pivot.y - halfHeight),
            new Vector3(pivot.x - halfWidth, 0, pivot.y + halfHeight)
        };

        Vector2[] _uv =
        {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(0, 1)
        };

        int[] triangles =
        {
            0, 1, 2,
            2, 3, 0
        };

        return new Mesh
        {
            vertices = _vertices,
            uv = _uv,
            triangles = triangles
        };
    }
}
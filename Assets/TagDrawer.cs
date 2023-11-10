using System.Linq;
using UnityEngine;

sealed class TagDrawer 
{
    private Material _tagMaterial;

    public TagDrawer(Material tagMaterial)
    {
        _tagMaterial = tagMaterial;
    }

    public void Draw(int id, Vector3 position, Quaternion rotation, float tagSize)
    {
        // Draw a square around the cube
        Vector3[] corners = CalculateBoundingBoxCorners(position, rotation, tagSize);
        DrawBoundingBox(corners, _tagMaterial);

        // Include the code to draw the AprilTag marker itself here if needed.
    }

    private Vector3[] CalculateBoundingBoxCorners(Vector3 position, Quaternion rotation, float tagSize)
    {
        Vector3 halfExtents = new Vector3(tagSize / 2f, tagSize / 2f, tagSize / 2f);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        Vector3[] localCorners = new Vector3[]
        {
        new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z),
        new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z),
        new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z),
        new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z),
        };

        Vector3[] worldCorners = new Vector3[localCorners.Length];
        for (int i = 0; i < localCorners.Length; i++)
        {
            // Rotate and translate the local corners to the world space
            worldCorners[i] = position + rotationMatrix.MultiplyPoint(localCorners[i]);
        }

        return worldCorners;
    }


    private void DrawBoundingBox(Vector3[] corners, Material material)
    {
        // Draw lines between corners using Gizmos
        for (int i = 0; i < corners.Length; i++)
        {
            int nextIndex = (i + 1) % corners.Length;
            Debug.DrawLine(corners[i], corners[nextIndex], material.color);
        }
        // Connect the last and first corners
        Debug.DrawLine(corners[corners.Length - 1], corners[0], material.color);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject FloorBlockPrefab, WallBlockPrefab;

    private int FloorBlockSize, WallBlockSize;

    private List<GameObject> Bounds = new List<GameObject>();

    public void GenerateBoundsAtPosition(Vector3 Position, int WidthInUnits, int HeightInUnits, int LengthInUnits)
    {
        Bounds = new List<GameObject>();

        FloorBlockSize = (int)FloorBlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        WallBlockSize = (int)WallBlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y;

        int Width = WidthInUnits * FloorBlockSize;
        int Length = LengthInUnits * FloorBlockSize;
        int Height = HeightInUnits * WallBlockSize;

        // Generar suelo
        for (int x = 0; x < Width; x += FloorBlockSize)
        {
            for (int z = 0; z < Length; z += FloorBlockSize)
            {
                Vector3 FloorPosition = Position + new Vector3(x, 0, z);
                Bounds.Add(Instantiate(FloorBlockPrefab, FloorPosition, Quaternion.identity, transform));
            }
        }

        // Generar techo
        for (int x = 0; x < Width; x += FloorBlockSize)
        {
            for (int z = 0; z < Length; z += FloorBlockSize)
            {
                Vector3 CeilingPosition = Position + new Vector3(x, Height, z);
                Bounds.Add(Instantiate(FloorBlockPrefab, CeilingPosition, Quaternion.identity, transform));
            }
        }

        // Generar paredes
        for (int y = 0; y < Height; y += WallBlockSize)
        {
            for (int x = 0; x < Width; x += FloorBlockSize)
            {
                Vector3 FrontWallPosition = Position + new Vector3(x, y + FloorBlockSize / 2, -FloorBlockSize / 2);
                Vector3 BackWallPosition = Position + new Vector3(x, y + FloorBlockSize / 2, Length - FloorBlockSize / 2);
                Quaternion FronWallRotation = Quaternion.Euler(0, 90, 0);
                Quaternion BackWallRotation = Quaternion.Euler(0, -90, 0);
                Bounds.Add(Instantiate(WallBlockPrefab, FrontWallPosition, FronWallRotation, transform));
                Bounds.Add(Instantiate(WallBlockPrefab, BackWallPosition, BackWallRotation, transform));
            }

            for (int z = 0; z < Length; z += FloorBlockSize)
            {
                Vector3 LeftWallPosition = Position + new Vector3(-FloorBlockSize / 2, y + FloorBlockSize / 2, z);
                Vector3 RightWallPosition = Position + new Vector3(Width - FloorBlockSize / 2, y + FloorBlockSize / 2, z);
                Quaternion LeftWallRotation = Quaternion.Euler(0, 180, 0);
                Bounds.Add(Instantiate(WallBlockPrefab, LeftWallPosition, LeftWallRotation, transform));
                Bounds.Add(Instantiate(WallBlockPrefab, RightWallPosition, Quaternion.identity, transform));
            }
        }
    }

    public void DeleteBounds()
    {
        foreach (GameObject Block in Bounds)
        {
            DestroyImmediate(Block);
        }
    }
}

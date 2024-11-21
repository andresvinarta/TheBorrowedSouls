using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class LayerGenerator : MonoBehaviour
{

    [SerializeField]
    PlatformGenerator PlatformGenerator;

    [SerializeField]
    private GameObject BlockPrefab;

    private int BlockSize;

    private List<List<(GameObject, int, int)>> Layers;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateLayersAtPostition(new Vector3(0, (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x, 0), 8, 1, 10, 10, 4, 2, 4, 0.5f);
    }

    public void GenerateLayersAtPostition(Vector3 Position, int LayerAmount, int SeparationBetweenLayersInUnits, int SpaceWidthInUnits, int SpaceHeightInUnits, int PartitionMinSizeInUnits, int PlatformMinSizeInUnits, int MaxPosibleDivisions, float BoundariesProbability)
    {
        BlockSize = (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        Layers = new List<List<(GameObject, int, int)>>();

        PlatformGenerator.InitiateValuesInUnits(SpaceWidthInUnits, SpaceHeightInUnits, PartitionMinSizeInUnits, PlatformMinSizeInUnits, MaxPosibleDivisions, BoundariesProbability);

        PlatformGenerator.gameObject.transform.position = Position;

        for (int CurrentLayer = 1; CurrentLayer <= LayerAmount; CurrentLayer++)
        {
            PlatformGenerator.GeneratePlatformsAtPosition(new Vector3(Position.x, Position.y + CurrentLayer * SeparationBetweenLayersInUnits * BlockSize, Position.z));
            Layers.Add(PlatformGenerator.GetGeneratedPlatforms());
        }

        PlatformGenerator.gameObject.GetComponent<NavMeshSurface>().RemoveData();
        PlatformGenerator.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public List<List<(GameObject, int, int)>> GetGeneratedLayers()
    {
        return Layers;
    }

    public void DeleteLayers()
    {
        foreach (List<(GameObject, int, int)> Layer in Layers)
        {
            foreach ((GameObject, int, int) Platform in Layer)
            {
                DestroyImmediate(Platform.Item1);
            }
        }
    }
}

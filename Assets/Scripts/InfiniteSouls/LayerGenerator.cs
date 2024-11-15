using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class LayerGenerator : MonoBehaviour
{

    [SerializeField]
    PlatformGenerator PlatformGenerator;

    [SerializeField]
    private GameObject BlockPrefab, Enemies;

    private int BlockSize;

    private List<List<(GameObject, int, int)>> Layers;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLayersAtPostition(new Vector3(0, 0, 0), 20, 2, 10, 10, 4, 2, 4);
    }

    public void GenerateLayersAtPostition(Vector3 Position, int LayerAmount, int SeparationBetweenLayersInUnits, int SpaceWidthInUnits, int SpaceHeightInUnits, int PartitionMinSizeInUnits, int PlatformMinSizeInUnits, int MaxPosibleDivisions)
    {
        BlockSize = (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        Layers = new List<List<(GameObject, int, int)>>();

        PlatformGenerator.InitiateValuesInUnits(SpaceWidthInUnits, SpaceHeightInUnits, PartitionMinSizeInUnits, PlatformMinSizeInUnits, MaxPosibleDivisions);

        for (int CurrentLayer = 0; CurrentLayer < LayerAmount; CurrentLayer++)
        {
            PlatformGenerator.GeneratePlatformsAtPosition(new Vector3(Position.x, Position.y + CurrentLayer * SeparationBetweenLayersInUnits * BlockSize, Position.z));
            Layers.Add(PlatformGenerator.GetGeneratedPlatforms());
        }

        PlatformGenerator.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        Enemies.GetComponent<EnemyGenerator>().GenerateEnemies(Layers, 2, 0.65f);
    }

    public List<List<(GameObject, int, int)>> GetGeneratedLayers()
    {
        return Layers;
    }
}

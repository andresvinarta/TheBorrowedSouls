using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("Generators")]
    [SerializeField]
    private BoundsGenerator BoundsGenerator;
    [SerializeField]
    private LayerGenerator LayerGenerator;
    [SerializeField]
    private EnemyGenerator EnemyGenerator;


    //Parametros de tama�o de la sala en unidades
    [SerializeField]
    private int RoomWidth, RoomHeight, RoomLength;

    //Parametros para la generacion de plataformas en unidades
    [SerializeField]
    private int MinPartitionSize, MinPlatformSize, MaxAllowedDivisions;
    [SerializeField]
    private float PlatformBoundariesProbability;

    //Parametros para la generacion de capas en unidades
    [SerializeField]
    private int LayerAmount, SeparationBetweenLayers;

    //Parametros para EnemyGenerator
    [SerializeField]
    private int MinEnemiesPerPlatform;
    [SerializeField]
    float PlatformEnemyDensity, t800SpawnProbability;

    //Asignar parametros del tama�o de la sala
    public void SetRoomSize(int WidthInUnits, int HeightInUnits, int LengthInUnits)
    {
        RoomWidth = WidthInUnits;
        RoomHeight = HeightInUnits;
        RoomLength = LengthInUnits;
    }

    public void SetPlatfomGenerationParameters(int MinimunPartitionSizeInUnits, int MinimunPlatfomSizeInUnits, int MaxDivisions, float BoundariesProbability)
    {
        MinPartitionSize = MinimunPartitionSizeInUnits;
        MinPlatformSize = MinimunPlatfomSizeInUnits;
        MaxAllowedDivisions = MaxDivisions;
        PlatformBoundariesProbability = BoundariesProbability;
    }

    public void SetLayerGenerationParameters(int AmountOfLayers, int SeparationBetweenLayersInUnits)
    {
        LayerAmount = AmountOfLayers;
        SeparationBetweenLayers = SeparationBetweenLayersInUnits;
    }

    //Asignar parametros para la generacion de enemigos
    public void SetEnemyGenerationParameters(int MinEnemies, float EnemyDensity, float t800Probability)
    {
        MinEnemiesPerPlatform = MinEnemies;
        PlatformEnemyDensity = EnemyDensity;
        t800SpawnProbability = t800Probability;
    }

    public void GenerateRoomAtPosition(Vector3 Position)
    {
        if (BoundsGenerator == null || LayerGenerator == null || EnemyGenerator == null) return;

        BoundsGenerator.GenerateBoundsAtPosition(Position, RoomWidth, RoomHeight, RoomLength);
        LayerGenerator.GenerateLayersAtPostition(Position, LayerAmount, SeparationBetweenLayers, RoomWidth, RoomLength, MinPartitionSize, MinPlatformSize, MaxAllowedDivisions, PlatformBoundariesProbability);
        EnemyGenerator.GenerateEnemies(LayerGenerator.GetGeneratedLayers(), MinEnemiesPerPlatform, PlatformEnemyDensity, t800SpawnProbability);
    }

    public void DeletePreviousRoom()
    {
        if (BoundsGenerator == null || LayerGenerator == null || EnemyGenerator == null) return;

        EnemyGenerator.DeleteEnemies();
        LayerGenerator.DeleteLayers();
        BoundsGenerator.DeleteBounds();
    }

    public int GetEnemyAmout()
    {
        return EnemyGenerator.GetGeneratedEnemies().Count;
    }

    public void ActivateEnemies()
    {
        foreach (GameObject Enemy in EnemyGenerator.GetGeneratedEnemies())
        {
            Enemy.SetActive(true);
        }
    }
}

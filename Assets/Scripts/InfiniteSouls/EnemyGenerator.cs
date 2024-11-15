using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;

    private List<GameObject> Enemies;

    public void GenerateEnemies(List<List<(GameObject, int, int)>> Layers, int MinEnemiesPerPlatform, float PlatformEnemyDensity)
    {
        Enemies = new List<GameObject>();

        foreach (List<(GameObject, int, int)> Layer in Layers)
        {
            foreach ((GameObject, int, int) Platform in Layer)
            {
                int MaxEnemies = Mathf.CeilToInt((Platform.Item2 + Platform.Item3) / 2); //Mathf.Max(Platform.Item2, Platform.Item3);
                int NumEnemies = Mathf.Max(MinEnemiesPerPlatform, Mathf.FloorToInt(MaxEnemies * PlatformEnemyDensity));

                List<Transform> BlocksTransforms = new List<Transform>(Platform.Item1.GetComponentsInChildren<Transform>());
                BlocksTransforms.RemoveAt(0);
                int Interval = Mathf.FloorToInt(BlocksTransforms.Count / NumEnemies);
                for (int i = 0; i < NumEnemies; i++)
                {
                    Enemies.Add(Instantiate(EnemyPrefab, new Vector3(BlocksTransforms[i * Interval].position.x, BlocksTransforms[i * Interval].position.y + 1, BlocksTransforms[i * Interval].position.z), Quaternion.identity));
                }
            }
        }
    }
}

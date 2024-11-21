using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject t800Prefab, t200Prefab;

    private List<GameObject> Enemies;

    public void GenerateEnemies(List<List<(GameObject, int, int)>> Layers, int MinEnemiesPerPlatform, float PlatformEnemyDensity, float t800Probability)
    {
        Enemies = new List<GameObject>();
        int CurrentLayer = -1, CurrentPlatform = -1;
        foreach (List<(GameObject, int, int)> Layer in Layers)
        {
            CurrentLayer++;
            CurrentPlatform = -1;
            foreach ((GameObject, int, int) Platform in Layer)
            {
                CurrentPlatform++;
                Platform.Item1.name = "Layer " + CurrentLayer + " Platfrom " + CurrentPlatform;

                int MaxEnemies = Mathf.CeilToInt((Platform.Item2 + Platform.Item3) / 2); //Mathf.Max(Platform.Item2, Platform.Item3);
                int NumEnemies = Mathf.Max(MinEnemiesPerPlatform, Mathf.FloorToInt(MaxEnemies * PlatformEnemyDensity));

                List<Transform> BlocksTransforms = new List<Transform>(Platform.Item1.GetComponentsInChildren<Transform>());
                BlocksTransforms.RemoveAt(0);
                int Interval = Mathf.FloorToInt(BlocksTransforms.Count / NumEnemies);
                for (int i = 0; i < NumEnemies; i++)
                {
                    GameObject EnemySpawned;
                    if (Random.value < t800Probability)
                    {
                        EnemySpawned = Instantiate(t800Prefab, new Vector3(BlocksTransforms[i * Interval].position.x, BlocksTransforms[i * Interval].position.y + 1, BlocksTransforms[i * Interval].position.z), Quaternion.identity);
                        EnemySpawned.name = "Layer " + CurrentLayer + " Platfrom " + CurrentPlatform + "t800 " + i;
                    }
                    else
                    {
                        EnemySpawned = Instantiate(t200Prefab, new Vector3(BlocksTransforms[i * Interval].position.x, BlocksTransforms[i * Interval].position.y + 1, BlocksTransforms[i * Interval].position.z), Quaternion.identity);
                        EnemySpawned.name = "Layer " + CurrentLayer + " Platfrom " + CurrentPlatform + "t200 " + i;
                    }
                    EnemySpawned.SetActive(false);
                    Enemies.Add(EnemySpawned);
                }
            }
        }
    }

    public List<GameObject> GetGeneratedEnemies()
    {
        return Enemies;
    }

    public void DeleteEnemies()
    {
        foreach (GameObject Enemy in Enemies)
        {
            DestroyImmediate(Enemy);
        }
    }
}

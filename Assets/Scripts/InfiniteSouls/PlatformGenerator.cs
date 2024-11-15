using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    //[SerializeField, Header("Tamaño de Configuración")]
    //private int WidthInUnits = 10;
    //[SerializeField]
    //private int HeightInUnits = 10;
    //[SerializeField]
    //private int MinPartitionSizeInUnits = 4;
    //[SerializeField]
    //private int MinPlatformSizeInUnits = 1;
    //[SerializeField]
    //private int MaxDivisionsToDo = 6;

    private int Width, Height, BlockSize, MinPartitionSize, MinPlatformSizeInUnits; // Anchura, altura, tamaño del bloque, tamaño mínimo de partición y tamaño mínimo de plataforma

    private int MaxDivisions, CurrentDivisions;

    [SerializeField]
    private float ApplyBoundariesProbability = 0.55f;

    [Header("Prefab de Bloque")]
    [SerializeField]
    private GameObject BlockPrefab; // Prefab del bloque unitario

    private List<RectInt> Partitions; // Lista de particiones generadas por BSP
    private List<(GameObject, int, int)> Platforms;

    void Start()
    {
        //Partitions = new List<RectInt>();
        //InitiateValuesInUnits(WidthInUnits, HeightInUnits, MinPartitionSizeInUnits, MaxDivisionsToDo);
        //RectInt initialSpace = new RectInt(0, 0, Width, Height);
        //DivideSpace(initialSpace);

        //// Generar bloques en cada partición creada
        //GenerateBlocks();

        //transform.position = new Vector3(-Width/2, 0, -Height/2);
    }

    public void InitiateValuesInUnits(int SpaceWidthInUnits, int SpaceHeightInUnits, int PartitionMinSizeInUnits, int PlatformMinSizeInUnits, int MaxPosibleDivisions)
    {
        BlockSize = (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        MaxDivisions = MaxPosibleDivisions;
        Width = SpaceWidthInUnits * BlockSize;
        Height = SpaceHeightInUnits * BlockSize;
        MinPartitionSize = PartitionMinSizeInUnits * BlockSize;
        MinPlatformSizeInUnits = PlatformMinSizeInUnits;
    }


    public void GeneratePlatformsAtPosition(Vector3 Position)
    {
        this.transform.position = Position;
        Partitions = new List<RectInt>();
        Platforms = new List<(GameObject, int, int)>();
        DivideSpace(new RectInt(0, 0, Width, Height));
        GenerateBlocks();
    }

    public List<(GameObject, int, int)> GetGeneratedPlatforms()
    {
        return Platforms;
    }

    // Método para dividir el espacio usando BSP
    private void DivideSpace(RectInt space)
    {
        // Limitar las divisiones si se alcanzan las condiciones de parada
        if (CurrentDivisions >= MaxDivisions || (space.width <= MinPartitionSize && space.height <= MinPartitionSize))
        {
            Partitions.Add(space);
            return;
        }

        // Decidir aleatoriamente si dividir verticalmente u horizontalmente
        bool divideVertically = Random.value > 0.5f;

        // Si no se puede dividir en la dirección seleccionada, intentar la otra dirección
        if (divideVertically && space.width > MinPartitionSize * 2)
        {
            // Dividir verticalmente en una posición válida
            int splitX = Random.Range(MinPartitionSize, space.width - MinPartitionSize);
            splitX = (splitX / BlockSize);
            splitX *= BlockSize; // Alinear a múltiplo de BlockSize

            RectInt leftSpace = new RectInt(space.x, space.y, splitX, space.height);
            RectInt rightSpace = new RectInt(space.x + splitX, space.y, space.width - splitX, space.height);

            CurrentDivisions++;
            
            // Llamada recursiva en las nuevas particiones
            DivideSpace(leftSpace);
            DivideSpace(rightSpace);
        }
        else if (!divideVertically && space.height > MinPartitionSize * 2)
        {
            // Dividir horizontalmente en una posición válida
            int splitY = Random.Range(MinPartitionSize, space.height - MinPartitionSize);
            splitY = (splitY / BlockSize);
            splitY *= BlockSize;// Alinear a múltiplo de BlockSize

            RectInt topSpace = new RectInt(space.x, space.y, space.width, splitY);
            RectInt bottomSpace = new RectInt(space.x, space.y + splitY, space.width, space.height - splitY);

            CurrentDivisions++;
            
            // Llamada recursiva en las nuevas particiones
            DivideSpace(topSpace);
            DivideSpace(bottomSpace);
        }
        else
        {
            // Si no se puede dividir en ninguna dirección, agregar el espacio a las particiones finales
            Partitions.Add(space);
        }
    }

    // Generar bloques dentro de cada partición
    private void GenerateBlocks()
    {
        int CurrentPartitionNumber = 0;
        foreach (RectInt Partition in Partitions)
        {
            // Solo generar bloques si la partición tiene un tamaño adecuado
            if (Partition.width >= BlockSize && Partition.height >= BlockSize)
            {
                int PartitionXUnits = Partition.width / BlockSize;
                int PartitionYUnits = Partition.height / BlockSize;

                int PlatformXUnits = 0;
                int PlatformYUnits = 0;

                int PlatformXStartUnit = 0;
                int PlatformYStartUnit = 0;

                int PlatformXEndUnit = 0;
                int PlatformYEndUnit = 0;

                if (Random.value <= ApplyBoundariesProbability)
                {
                    if (PartitionXUnits >= MinPlatformSizeInUnits + 2)
                    {
                        PlatformXUnits = Random.Range(MinPlatformSizeInUnits, PartitionXUnits - 2);
                        PlatformXStartUnit = Random.Range(1, PartitionXUnits - PlatformXUnits - 1);
                        PlatformXEndUnit = PlatformXStartUnit + PlatformXUnits;
                    }
                    else
                    {
                        PlatformXUnits = Random.Range(MinPlatformSizeInUnits, PartitionXUnits);
                        PlatformXStartUnit = Random.Range(0, PartitionXUnits - PlatformXUnits);
                        PlatformXEndUnit = PlatformXStartUnit + PlatformXUnits;
                    }
                    if (PartitionYUnits >= MinPlatformSizeInUnits + 2)
                    {
                        PlatformYUnits = Random.Range(MinPlatformSizeInUnits, PartitionYUnits - 2);
                        PlatformYStartUnit = Random.Range(1, PartitionYUnits - PlatformYUnits - 1);
                        PlatformYEndUnit = PlatformYStartUnit + PlatformYUnits;
                    }
                    else
                    {
                        PlatformYUnits = Random.Range(MinPlatformSizeInUnits, PartitionYUnits);
                        PlatformYStartUnit = Random.Range(0, PartitionYUnits - PlatformYUnits);
                        PlatformYEndUnit = PlatformYStartUnit + PlatformYUnits;
                    }
                }
                else
                {
                    PlatformXUnits = Random.Range(MinPlatformSizeInUnits, PartitionXUnits);
                    PlatformYUnits = Random.Range(MinPlatformSizeInUnits, PartitionYUnits);

                    PlatformXStartUnit = Random.Range(0, PartitionXUnits - PlatformXUnits);
                    PlatformYStartUnit = Random.Range(0, PartitionYUnits - PlatformYUnits);

                    PlatformXEndUnit = PlatformXStartUnit + PlatformXUnits;
                    PlatformYEndUnit = PlatformYStartUnit + PlatformYUnits;
                }

                GameObject NewPlatform = new GameObject();
                NewPlatform.transform.parent = transform;
                NewPlatform.transform.position = Vector3.zero;
                NewPlatform.name = "Platform " + CurrentPartitionNumber.ToString();
                Platforms.Add((NewPlatform, PlatformXUnits, PlatformYUnits));
                CurrentPartitionNumber++;

                for (int x = Partition.x + PlatformXStartUnit * BlockSize; x < Partition.x + PlatformXEndUnit * BlockSize; x += BlockSize)
                {
                    for (int y = Partition.y + PlatformYStartUnit * BlockSize; y < Partition.y + PlatformYEndUnit * BlockSize; y += BlockSize)
                    {
                        Vector3 position = new Vector3(x, 0, y);
                        Instantiate(BlockPrefab, position, Quaternion.identity, NewPlatform.transform);
                    }
                }
            }
        }
    }
}
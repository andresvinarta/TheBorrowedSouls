using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class PlatformGenerator : MonoBehaviour
{
    private int Width, Height, BlockSize, MinPartitionSize, MinPlatformSizeInUnits; // Anchura, altura, tamaño del bloque, tamaño mínimo de partición y tamaño mínimo de plataforma

    private int MaxDivisions, CurrentDivisions; // Máximas divisiones posibles y las divisiones que se han realizado hasta el momento

    private float ApplyBoundariesProbability = 0.55f; // Probabilidad de aplicar los bordes a la hora de generar plataformas

    [SerializeField]
    private GameObject BlockPrefab; // Prefab del bloque unitario para la construcción de plataformas

    [SerializeField]
    private GameObject PillarPrefab; // Prefab del pilar que sujeta las plataformas

    private List<RectInt> Partitions; // Lista de particiones generadas por BSP
    private List<(GameObject, int, int)> Platforms; // Lista de los GameObjects de las plataformas junto con sus dimensiones X e Y
    private List<GameObject> Blocks; // Lista de los GameObjects de los bloques instanciados

    private float PlatformsHeigth; // La altura a la que se han de generar las plataformas

    public void InitiateValuesInUnits(int SpaceWidthInUnits, int SpaceHeightInUnits, int PartitionMinSizeInUnits, int PlatformMinSizeInUnits, int MaxPosibleDivisions, float PlatformBoundariesPropability)
    {
        BlockSize = (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        MaxDivisions = MaxPosibleDivisions;
        Width = SpaceWidthInUnits * BlockSize;
        Height = SpaceHeightInUnits * BlockSize;
        MinPartitionSize = PartitionMinSizeInUnits * BlockSize;
        MinPlatformSizeInUnits = PlatformMinSizeInUnits;
        ApplyBoundariesProbability = PlatformBoundariesPropability;
    }

    public void GeneratePlatformsAtPosition(Vector3 Position)
    {
        PlatformsHeigth = Position.y;
        Partitions = new List<RectInt>();
        Platforms = new List<(GameObject, int, int)>();
        Blocks = new List<GameObject>();
        CurrentDivisions = 0;
        DivideSpace(new RectInt((int)Position.x, (int)Position.z, Width, Height));
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
                NewPlatform.transform.localPosition = new Vector3(0, PlatformsHeigth, 0);
                NewPlatform.name = "Platform " + CurrentPartitionNumber.ToString();
                Platforms.Add((NewPlatform, PlatformXUnits, PlatformYUnits));
                CurrentPartitionNumber++;

                GameObject[] CornerBlocks = new GameObject[4];

                for (int x = Partition.x + PlatformXStartUnit * BlockSize; x < Partition.x + PlatformXEndUnit * BlockSize; x += BlockSize)
                {
                    for (int y = Partition.y + PlatformYStartUnit * BlockSize; y < Partition.y + PlatformYEndUnit * BlockSize; y += BlockSize)
                    {
                        Vector3 position = new Vector3(x, PlatformsHeigth, y);
                        //Asignación a los bloques esquina para que sean padres de los pilares
                        GameObject BlockInstance = Instantiate(BlockPrefab, position, Quaternion.identity, NewPlatform.transform);
                        Blocks.Add(BlockInstance);
                        if (x == Partition.x + PlatformXStartUnit * BlockSize && y == Partition.y + PlatformYStartUnit * BlockSize)
                        {
                            CornerBlocks[0] = BlockInstance;
                        }
                        if ((x + BlockSize) == Partition.x + PlatformXEndUnit * BlockSize && y == Partition.y + PlatformYStartUnit * BlockSize)
                        {
                            CornerBlocks[1] = BlockInstance;
                        }
                        if (x == Partition.x + PlatformXStartUnit * BlockSize && (y + BlockSize) == Partition.y + PlatformYEndUnit * BlockSize)
                        {
                            CornerBlocks[2] = BlockInstance;
                        }
                        if ((x + BlockSize) == Partition.x + PlatformXEndUnit * BlockSize && (y + BlockSize) == Partition.y + PlatformYEndUnit * BlockSize)
                        {
                            CornerBlocks[3] = BlockInstance;
                        }

                    }
                }

                // Calcular las variables necesarias para decidir las esquinas de los pilares
                float HalfBlockSize = BlockSize / 2f;
                float HalfPillarSize = PillarPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x / 2f * PillarPrefab.transform.localScale.x; //97.82669f;
                float PillarHeight = PillarPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * PillarPrefab.transform.localScale.y; //220.75055f;

                //Calcular las esquinas de cada pilar
                Vector3[] PillarCorners = new Vector3[]
                {
                    new Vector3(Partition.x + PlatformXStartUnit * BlockSize - HalfBlockSize + HalfPillarSize, PlatformsHeigth - PillarHeight + 0.95f, Partition.y + PlatformYStartUnit * BlockSize - HalfBlockSize + HalfPillarSize),
                    new Vector3(Partition.x + PlatformXEndUnit * BlockSize - HalfBlockSize - HalfPillarSize, PlatformsHeigth - PillarHeight + 0.95f, Partition.y + PlatformYStartUnit * BlockSize - HalfBlockSize + HalfPillarSize),
                    new Vector3(Partition.x + PlatformXStartUnit * BlockSize - HalfBlockSize + HalfPillarSize, PlatformsHeigth - PillarHeight + 0.95f, Partition.y + PlatformYEndUnit * BlockSize - HalfBlockSize - HalfPillarSize),
                    new Vector3(Partition.x + PlatformXEndUnit * BlockSize - HalfBlockSize - HalfPillarSize, PlatformsHeigth - PillarHeight + 0.95f, Partition.y + PlatformYEndUnit * BlockSize - HalfBlockSize - HalfPillarSize)
                };

                // Generar pilares en las esquinas de la plataforma
                int PillarNum = -1;
                foreach (Vector3 PillarCorner in PillarCorners)
                {
                    PillarNum += 1;
                    if (Physics.Raycast(new Vector3(PillarCorner.x, PillarCorner.y + PillarHeight - 1.5f, PillarCorner.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
                    {
                        int PillarAmount = Mathf.CeilToInt(hit.distance / PillarHeight);
                        //Debug.Log(PillarAmount);
                        for (int i = 0; i < PillarAmount; i++)
                        {
                            GameObject PillarInstance = Instantiate(PillarPrefab, new Vector3(PillarCorner.x, PillarCorner.y - i * PillarHeight, PillarCorner.z), Quaternion.identity, NewPlatform.transform);
                            PillarInstance.transform.SetParent(CornerBlocks[PillarNum].transform);
                        }
                    }
                    //else
                    //{
                    //    Debug.Log("NO PILLAR " + PillarNum + " IN " + NewPlatform.name);
                    //}
                }
            }
        }

        // Comnprobar que la capa no ha sido completamente rellenada con bloques. Si ocurre, eliminar un bloque aleatorio para dejar paso al jugador
        int SpaceWidthInUnits = Width * BlockSize;
        int SpaceHeightInUnits = Height * BlockSize;
        int BlockAmount = Blocks.Count;
        if (BlockAmount == SpaceWidthInUnits * SpaceHeightInUnits)
        {
            DestroyImmediate(Blocks[UnityEngine.Random.Range(0, BlockAmount)]);
        }
    }
}
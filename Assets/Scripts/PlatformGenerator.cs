using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Tama�o de Configuraci�n")]
    [SerializeField]
    private int Width, Height, BlockSize, MinPartitionSize; // Anchura, altura, tama�o del bloque, y tama�o m�nimo de partici�n

    private int MaxDivisions, CurrentDivisions;

    [Header("Prefab de Bloque")]
    [SerializeField]
    private GameObject BlockPrefab; // Prefab del bloque unitario

    private List<RectInt> Partitions; // Lista de particiones generadas por BSP

    public GameObject Plane;

    void Start()
    {
        Partitions = new List<RectInt>();
        InitiateValues(20, 20, 4, 6);
        Plane.transform.localScale = new Vector3 (10, 1, 10);
        RectInt initialSpace = new RectInt(0, 0, Width, Height);
        DivideSpace(initialSpace);

        // Generar bloques en cada partici�n creada
        GenerateBlocks();

        transform.position = new Vector3(-Width/2, 0, -Height/2);
    }

    public void InitiateValues(int SpaceWidthInUnits, int SpaceHeightInUnits, int MinSizeInUnits, int MaxPosibleDivisions)
    {
        BlockSize = (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        MaxDivisions = MaxPosibleDivisions;
        Width = SpaceWidthInUnits * BlockSize;
        Height = SpaceHeightInUnits * BlockSize;
        MinPartitionSize = MinSizeInUnits * BlockSize;
    }

    // M�todo para dividir el espacio usando BSP
    void DivideSpace(RectInt space)
    {
        // Limitar las divisiones si se alcanzan las condiciones de parada
        if (CurrentDivisions >= MaxDivisions || (space.width <= MinPartitionSize && space.height <= MinPartitionSize))
        {
            Partitions.Add(space);
            return;
        }

        // Determinar si es mejor dividir vertical u horizontalmente basado en las proporciones
        bool divideVertically;
        float aspectRatio = (float)space.width / space.height;

        // Usar un umbral m�s estricto para forzar particiones m�s equilibradas
        if (aspectRatio > 1.25f) // Prefiere divisi�n vertical si la partici�n es significativamente m�s ancha que alta
        {
            divideVertically = true;
        }
        else if (aspectRatio < 0.8f) // Prefiere divisi�n horizontal si la partici�n es significativamente m�s alta que ancha
        {
            divideVertically = false;
        }
        else
        {
            // Si la proporci�n es razonable, dividir al azar
            divideVertically = Random.value > 0.5f;
        }

        if (divideVertically && space.width > MinPartitionSize * 2)
        {
            int minSplit = MinPartitionSize;
            int maxSplit = space.width - MinPartitionSize;

            // Elegir un punto de divisi�n cerca del centro para evitar particiones alargadas
            int splitX = Random.Range(minSplit, maxSplit);
            splitX = Mathf.Clamp(splitX, minSplit + (space.width - minSplit) / 3, maxSplit - (space.width - minSplit) / 3);
            splitX = (splitX / BlockSize) * BlockSize; // Alinear a m�ltiplo de BlockSize

            RectInt leftSpace = new RectInt(space.x, space.y, splitX, space.height);
            RectInt rightSpace = new RectInt(space.x + splitX, space.y, space.width - splitX, space.height);

            CurrentDivisions++;
            DivideSpace(leftSpace);
            DivideSpace(rightSpace);
        }
        else if (!divideVertically && space.height > MinPartitionSize * 2)
        {
            int minSplit = MinPartitionSize;
            int maxSplit = space.height - MinPartitionSize;

            // Elegir un punto de divisi�n cerca del centro para evitar particiones alargadas
            int splitY = Random.Range(minSplit, maxSplit);
            splitY = Mathf.Clamp(splitY, minSplit + (space.height - minSplit) / 3, maxSplit - (space.height - minSplit) / 3);
            splitY = (splitY / BlockSize) * BlockSize; // Alinear a m�ltiplo de BlockSize

            RectInt topSpace = new RectInt(space.x, space.y, space.width, splitY);
            RectInt bottomSpace = new RectInt(space.x, space.y + splitY, space.width, space.height - splitY);

            CurrentDivisions++;
            DivideSpace(topSpace);
            DivideSpace(bottomSpace);
        }
        else
        {
            // Si no se puede dividir en ninguna direcci�n, agregar el espacio a las particiones finales
            Partitions.Add(space);
        }
    }

    // Generar bloques dentro de cada partici�n
    void GenerateBlocks()
    {
        foreach (RectInt Partition in Partitions)
        {
            // Solo generar bloques si la partici�n tiene un tama�o adecuado
            if (Partition.width >= BlockSize && Partition.height >= BlockSize)
            {
                int PlatformXUnits = Partition.width / BlockSize;
                int PlatformYUnits = Partition.height / BlockSize;

                // Definir un m�nimo de unidades de separaci�n (puedes ajustar este valor para variar m�s)
                int minSeparation = Random.Range(0, Mathf.Max(1, Mathf.Min(PlatformXUnits, PlatformYUnits) / 3));

                // Elegir un punto de inicio y asegurar que el punto final est� al menos a `minSeparation` de distancia
                int PlatformXStartUnit = Random.Range(0, PlatformXUnits - minSeparation);
                int PlatformXEndUnit = Random.Range(PlatformXStartUnit + minSeparation, PlatformXUnits);

                int PlatformYStartUnit = Random.Range(0, PlatformYUnits - minSeparation);
                int PlatformYEndUnit = Random.Range(PlatformYStartUnit + minSeparation, PlatformYUnits);

                Debug.Log("Plataforma con: " + (PlatformXEndUnit - PlatformXStartUnit + 1).ToString() + " en X, " + (PlatformYEndUnit - PlatformYStartUnit + 1).ToString() + " en Y");

                for (int x = Partition.x + PlatformXStartUnit * BlockSize; x <= Partition.x + PlatformXEndUnit * BlockSize; x += BlockSize)
                {
                    for (int y = Partition.y + PlatformYStartUnit * BlockSize; y <= Partition.y + PlatformYEndUnit * BlockSize; y += BlockSize)
                    {
                        Vector3 position = new Vector3(x, 0, y);
                        Instantiate(BlockPrefab, position, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
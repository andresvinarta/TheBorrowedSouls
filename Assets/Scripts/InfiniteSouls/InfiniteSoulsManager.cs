using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSoulsManager : MonoBehaviour
{
    [Header("Referencias a objetos")]
    [SerializeField]
    private RoomGenerator RoomGenerator;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameObject BlockPrefab;

    [SerializeField]
    private GameObject Altar, Portal, Music, InfiniteSoulsStatsMenu;

    [Header("Pesos de la heurística")]
    [SerializeField]
    private float CombatDurationWeight;
    [SerializeField]
    private float AirTimeWeight, DamageAndHealWeight, AccuracyWeight, GravityChangesWeight, DoubleJumpsWeight, DashesWeight;

    [Header("Rangos de normalización para Easy")]
    [SerializeField]
    private float EasyMinCombatDuration;
    [SerializeField]
    private float EasyMaxCombatDuration, EasyMinAirTimePercentage, EasyMaxAirTimePercentage, EasyMinDamageAndHeal, EasyMaxDamageAndHeal, EasyMinAccuracy, EasyMaxAccuracy, 
                  EasyMinGravityChanges, EasyMaxGravityChanges, EasyMinDoubleJumps, EasyMaxDoubleJumps, EasyMinDashes, EasyMaxDashes;

    [Header("Rangos de normalización para Medium")]
    [SerializeField]
    private float MediumMinCombatDuration;
    [SerializeField]
    private float MediumMaxCombatDuration, MediumMinAirTimePercentage, MediumMaxAirTimePercentage, MediumMinDamageAndHeal, MediumMaxDamageAndHeal, MediumMinAccuracy, MediumMaxAccuracy,
                  MediumMinGravityChanges, MediumMaxGravityChanges, MediumMinDoubleJumps, MediumMaxDoubleJumps, MediumMinDashes, MediumMaxDashes;

    [Header("Rangos de normalización para Hard")]
    [SerializeField]
    private float HardMinCombatDuration;
    [SerializeField]
    private float HardMaxCombatDuration, HardMinAirTimePercentage, HardMaxAirTimePercentage, HardMinDamageAndHeal, HardMaxDamageAndHeal, HardMinAccuracy, HardMaxAccuracy,
                  HardMinGravityChanges, HardMaxGravityChanges, HardMinDoubleJumps, HardMaxDoubleJumps, HardMinDashes, HardMaxDashes;

    [Header("Rangos de normalización para Gods' Scar")]
    [SerializeField]
    private float GodsScarMinCombatDuration;
    [SerializeField]
    private float GodsScarMaxCombatDuration, GodsScarMinAirTimePercentage, GodsScarMaxAirTimePercentage, GodsScarMinDamageAndHeal, GodsScarMaxDamageAndHeal, GodsScarMinAccuracy, GodsScarMaxAccuracy,
                  GodsScarMinGravityChanges, GodsScarMaxGravityChanges, GodsScarMinDoubleJumps, GodsScarMaxDoubleJumps, GodsScarMinDashes, GodsScarMaxDashes;


    [Header("Rangos de generación para Easy")]
    [SerializeField]
    private int MinRoomHeightEasy;
    [SerializeField]
    private int MaxRoomHeightEasy, MinRoomWidthEasy, MaxRoomWidthEasy, MinRoomLengthEasy, MaxRoomLengthEasy, MinMinPartitionSizeEasy, MaxMinPartitionSizeEasy, MinMinPlatformSizeEasy, MaxMinPlatformSizeEasy,
                MinMaxDivisionsEasy, MaxMaxDivisonsEasy, MinLayersEasy, MinMinEnemiesEasy, MaxMinEnemiesEasy;
    [SerializeField]
    private float MinApplyBoundariesEasy, MaxApplyBoundariesEasy, MinEnemyDensityEasy, MaxEnemyDensityEasy, MinT800ProbabilityEasy, MaxT800ProbabilityEasy;

    [Header("Rangos de generación para Medium")]
    [SerializeField]
    private int MinRoomHeightMedium;
    [SerializeField]
    private int MaxRoomHeightMedium, MinRoomWidthMedium, MaxRoomWidthMedium, MinRoomLengthMedium, MaxRoomLengthMedium, MinMinPartitionSizeMedium, MaxMinPartitionSizeMedium, MinMinPlatformSizeMedium, MaxMinPlatformSizeMedium,
                MinMaxDivisionsMedium, MaxMaxDivisonsMedium, MinLayersMedium, MinMinEnemiesMedium, MaxMinEnemiesMedium;
    [SerializeField]
    private float MinApplyBoundariesMedium, MaxApplyBoundariesMedium, MinEnemyDensityMedium, MaxEnemyDensityMedium, MinT800ProbabilityMedium, MaxT800ProbabilityMedium;

    [Header("Rangos de generación para Hard")]
    [SerializeField]
    private int MinRoomHeightHard;
    [SerializeField]
    private int MaxRoomHeightHard, MinRoomWidthHard, MaxRoomWidthHard, MinRoomLengthHard, MaxRoomLengthHard, MinMinPartitionSizeHard, MaxMinPartitionSizeHard, MinMinPlatformSizeHard, MaxMinPlatformSizeHard,
                MinMaxDivisionsHard, MaxMaxDivisonsHard, MinLayersHard, MinMinEnemiesHard, MaxMinEnemiesHard;
    [SerializeField]
    private float MinApplyBoundariesHard, MaxApplyBoundariesHard, MinEnemyDensityHard, MaxEnemyDensityHard, MinT800ProbabilityHard, MaxT800ProbabilityHard;

    [Header("Rangos de generación para Gods' Scar")]
    [SerializeField]
    private int MinRoomHeightGodsScar;
    [SerializeField]
    private int MaxRoomHeightGodsScar, MinRoomWidthGodsScar, MaxRoomWidthGodsScar, MinRoomLengthGodsScar, MaxRoomLengthGodsScar, MinMinPartitionSizeGodsScar, MaxMinPartitionSizeGodsScar, MinMinPlatformSizeGodsScar, MaxMinPlatformSizeGodsScar,
                MinMaxDivisionsGodsScar, MaxMaxDivisonsGodsScar, MinLayersGodsScar, MinMinEnemiesGodsScar, MaxMinEnemiesGodsScar;
    [SerializeField]
    private float MinApplyBoundariesGodsScar, MaxApplyBoundariesGodsScar, MinEnemyDensityGodsScar, MaxEnemyDensityGodsScar, MinT800ProbabilityGodsScar, MaxT800ProbabilityGodsScar;



    //Enum para la representar las diferentes dificultades
    public enum RoomCR
    {
        Easy,
        Medium,
        Hard,
        GodsScar
    }
    private RoomCR CurrentRoomCR = RoomCR.Easy, PreviousRoomCR = RoomCR.Easy;

    //Datos de comportamiento del jugador
    private float CombatDuration = 0, AirTime = 0; //Datos de tiempo
    private int DamageTaken = 0, HealthHealed = 0; //Datos de vida
    private int ShotsFired = 0, ShotsHit = 0; //Datos de precisión
    private int GravityChanges = 0, DoubleJumps = 0, Dashes = 0; //Datos de acciones de movimiento

    //Variables extra para funcionamiento
    private int EnemyAmount = 0;
    private bool InCombat = false;
    private int BlockSize;

    void Start()
    {
        BlockSize = (int)BlockPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        //RoomGenerator.SetRoomSize(10, 10, 10);
        //RoomGenerator.SetPlatfomGenerationParameters(4, 2, 4, 0.5f);
        //RoomGenerator.SetLayerGenerationParameters(2, 4);
        //RoomGenerator.SetEnemyGenerationParameters(2, 0.65f, 0.5f);
        //RoomGenerator.GenerateRoomAtPosition(Vector3.zero);
        //EnemyAmount = RoomGenerator.GetEnemyAmout();
        GenerateNextRoom(false);
    }

    private void Update()
    {
        if (InCombat)
        {
            CombatDuration += Time.deltaTime;
            if (!Player.GetComponent<player_movement>().IsPlayerGrounded())
            {
                AirTime += Time.deltaTime;
            }
        }
    }

    public void GenerateNextRoom(bool DeletePrevious)
    {
        //Eliminar la sala anterior
        if (DeletePrevious) RoomGenerator.DeletePreviousRoom();

        int RoomHeight = 0;
        int RoomWidth = 0;
        int RoomLength = 0;
        int MinPartitionSize = 0;
        int MinPlatformSize = 0;
        int MaxAllowedDivisions = 0;
        float ApplyBoundaries = 0;
        int LayerAmount = 0;
        int BetweenLayersSpaceInUnits = 0;
        int MinimunEnemies = 0;
        float PlatformEnemyDensity = 0, t800SpawnPropability = 0;

        switch (CurrentRoomCR)
        {
            case RoomCR.Easy:
                //Generar los tamaños de la sala en Easy
                RoomHeight = UnityEngine.Random.Range(MinRoomHeightEasy, MaxRoomHeightEasy);
                RoomWidth = UnityEngine.Random.Range(MinRoomWidthEasy, MaxRoomWidthEasy);
                RoomLength = UnityEngine.Random.Range(MinRoomLengthEasy, MaxRoomLengthEasy);

                //Generar los parámetros de las plataformas en Easy
                MinPartitionSize = UnityEngine.Random.Range(MinMinPartitionSizeEasy, MaxMinPartitionSizeEasy);
                MinPlatformSize = UnityEngine.Random.Range(MinMinPlatformSizeEasy, MaxMinPlatformSizeEasy);
                MaxAllowedDivisions = UnityEngine.Random.Range(MinMaxDivisionsEasy, MaxMaxDivisonsEasy);
                ApplyBoundaries = UnityEngine.Random.Range(MinApplyBoundariesEasy, MaxApplyBoundariesEasy);

                //Generar los parámetros de las capas en Easy
                LayerAmount = UnityEngine.Random.Range(Mathf.Min(MinLayersEasy, RoomHeight - 1), RoomHeight);
                BetweenLayersSpaceInUnits = Mathf.Max(1, Mathf.FloorToInt((RoomHeight - 2) / Mathf.Max(LayerAmount, 2)));

                //Generar los parámetros para los enemigos en Easy
                MinimunEnemies = UnityEngine.Random.Range(MinMinEnemiesEasy, MaxMinEnemiesEasy);
                PlatformEnemyDensity = UnityEngine.Random.Range(MinEnemyDensityEasy, MaxEnemyDensityEasy);
                t800SpawnPropability = UnityEngine.Random.Range(MinT800ProbabilityEasy, MaxT800ProbabilityEasy);

                break;

            case RoomCR.Medium:
                //Generar los tamaños de la sala en Medium
                RoomHeight = UnityEngine.Random.Range(MinRoomHeightMedium, MaxRoomHeightMedium);
                RoomWidth = UnityEngine.Random.Range(MinRoomWidthMedium, MaxRoomWidthMedium);
                RoomLength = UnityEngine.Random.Range(MinRoomLengthMedium, MaxRoomLengthMedium);

                //Generar los parámetros de las plataformas en Medium
                MinPartitionSize = UnityEngine.Random.Range(MinMinPartitionSizeMedium, MaxMinPartitionSizeMedium);
                MinPlatformSize = UnityEngine.Random.Range(MinMinPlatformSizeMedium, MaxMinPlatformSizeMedium);
                MaxAllowedDivisions = UnityEngine.Random.Range(MinMaxDivisionsMedium, MaxMaxDivisonsMedium);
                ApplyBoundaries = UnityEngine.Random.Range(MinApplyBoundariesMedium, MaxApplyBoundariesMedium);

                //Generar los parámetros de las capas en Medium
                LayerAmount = UnityEngine.Random.Range(Mathf.Min(MinLayersMedium, RoomHeight - 1), RoomHeight);
                BetweenLayersSpaceInUnits = Mathf.Max(1, Mathf.FloorToInt((RoomHeight - 2) / Mathf.Max(LayerAmount, 2)));

                //Generar los parámetros para los enemigos en Medium
                MinimunEnemies = UnityEngine.Random.Range(MinMinEnemiesMedium, MaxMinEnemiesMedium);
                PlatformEnemyDensity = UnityEngine.Random.Range(MinEnemyDensityMedium, MaxEnemyDensityMedium);
                t800SpawnPropability = UnityEngine.Random.Range(MinT800ProbabilityMedium, MaxT800ProbabilityMedium);

                break;

            case RoomCR.Hard:
                //Generar los tamaños de la sala en Hard
                RoomHeight = UnityEngine.Random.Range(MinRoomHeightHard, MaxRoomHeightHard);
                RoomWidth = UnityEngine.Random.Range(MinRoomWidthHard, MaxRoomWidthHard);
                RoomLength = UnityEngine.Random.Range(MinRoomLengthHard, MaxRoomLengthHard);

                //Generar los parámetros de las plataformas en Hard
                MinPartitionSize = UnityEngine.Random.Range(MinMinPartitionSizeHard, MaxMinPartitionSizeHard);
                MinPlatformSize = UnityEngine.Random.Range(MinMinPlatformSizeHard, MaxMinPlatformSizeHard);
                MaxAllowedDivisions = UnityEngine.Random.Range(MinMaxDivisionsHard, MaxMaxDivisonsHard);
                ApplyBoundaries = UnityEngine.Random.Range(MinApplyBoundariesHard, MaxApplyBoundariesHard);

                //Generar los parámetros de las capas en Hard
                LayerAmount = UnityEngine.Random.Range(Mathf.Min(MinLayersHard, RoomHeight - 1), RoomHeight);
                BetweenLayersSpaceInUnits = Mathf.Max(1, Mathf.FloorToInt((RoomHeight - 2) / Mathf.Max(LayerAmount, 2)));

                //Generar los parámetros para los enemigos en Hard
                MinimunEnemies = UnityEngine.Random.Range(MinMinEnemiesHard, MaxMinEnemiesHard);
                PlatformEnemyDensity = UnityEngine.Random.Range(MinEnemyDensityHard, MaxEnemyDensityHard);
                t800SpawnPropability = UnityEngine.Random.Range(MinT800ProbabilityHard, MaxT800ProbabilityHard);

                break;

            case RoomCR.GodsScar:
                //Generar los tamaños de la sala en GodsScar
                RoomHeight = UnityEngine.Random.Range(MinRoomHeightGodsScar, MaxRoomHeightGodsScar);
                RoomWidth = UnityEngine.Random.Range(MinRoomWidthGodsScar, MaxRoomWidthGodsScar);
                RoomLength = UnityEngine.Random.Range(MinRoomLengthGodsScar, MaxRoomLengthGodsScar);

                //Generar los parámetros de las plataformas en GodsScar
                MinPartitionSize = UnityEngine.Random.Range(MinMinPartitionSizeGodsScar, MaxMinPartitionSizeGodsScar);
                MinPlatformSize = UnityEngine.Random.Range(MinMinPlatformSizeGodsScar, MaxMinPlatformSizeGodsScar);
                MaxAllowedDivisions = UnityEngine.Random.Range(MinMaxDivisionsGodsScar, MaxMaxDivisonsGodsScar);
                ApplyBoundaries = UnityEngine.Random.Range(MinApplyBoundariesGodsScar, MaxApplyBoundariesGodsScar);

                //Generar los parámetros de las capas en GodsScar
                LayerAmount = UnityEngine.Random.Range(Mathf.Min(MinLayersGodsScar, RoomHeight - 1), RoomHeight);
                BetweenLayersSpaceInUnits = Mathf.Max(1, Mathf.FloorToInt((RoomHeight - 2) / Mathf.Max(LayerAmount, 2)));

                //Generar los parámetros para los enemigos en GodsScar
                MinimunEnemies = UnityEngine.Random.Range(MinMinEnemiesGodsScar, MaxMinEnemiesGodsScar);
                PlatformEnemyDensity = UnityEngine.Random.Range(MinEnemyDensityGodsScar, MaxEnemyDensityGodsScar);
                t800SpawnPropability = UnityEngine.Random.Range(MinT800ProbabilityGodsScar, MaxT800ProbabilityGodsScar);

                break;

            default:
                break;
        }

        //Asignación de los parámetros generados
        RoomGenerator.SetRoomSize(RoomWidth, RoomHeight, RoomLength);
        RoomGenerator.SetPlatfomGenerationParameters(MinPartitionSize, MinPlatformSize, MaxAllowedDivisions, ApplyBoundaries);
        RoomGenerator.SetLayerGenerationParameters(LayerAmount, BetweenLayersSpaceInUnits);
        RoomGenerator.SetEnemyGenerationParameters(MinimunEnemies, PlatformEnemyDensity, t800SpawnPropability);
        //Solicitud de generación de la sala
        RoomGenerator.GenerateRoomAtPosition(Vector3.zero);

        //Asignación de variables necesarias y colocación de objetos al sitio
        EnemyAmount = RoomGenerator.GetEnemyAmout();
        Player.GetComponent<player_movement>().SetOriginalRotation();
        Player.transform.position = new Vector3((float)(RoomWidth * BlockSize) / 2, 1, (float)(RoomLength * BlockSize) / 2);
        Altar.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z + BlockSize / 4);
        Portal.transform.position = new Vector3(Player.transform.position.x, RoomHeight * BlockSize - 2, Player.transform.position.z);
        Altar.SetActive(true);
        Portal.SetActive(false);
    }

    public void PlayerChangedGravity()
    {
        if (InCombat) GravityChanges++;
    }

    public void PlayerDoubleJumped()
    {
        if (InCombat) DoubleJumps++;
    }

    public void PlayerDashed()
    {
        if (InCombat) Dashes++;
    }

    public void PlayerTookDamage(int DamageAmount)
    {
        if (InCombat) DamageTaken += DamageAmount;
    }

    public void PlayerHealed(int HealAmount)
    {
        if (InCombat) HealthHealed += HealAmount;
    }

    public void PlayerShot()
    {
        if (InCombat) ShotsFired++;
    }

    public void PlayerHitTarget()
    {
        if (InCombat) ShotsHit++;
    }

    private void CalculateNextRoomCR()
    {
        PreviousRoomCR = CurrentRoomCR;

        //Inicializar variables para guardar los valores normalizados
        float CombatDurationNormalized = 0;
        float AirTimeNormalized = 0;
        float DamageAndHealNormalized = 0;
        float AccuracyNormalized = 0;
        float GravityChangesNormalized = 0;
        float DoubleJumpsNormalized = 0;
        float DashesNormalized = 0;

        switch (CurrentRoomCR)
        {
            case RoomCR.Easy:
                CombatDurationNormalized = NormalizeValue((1 / CombatDuration), (1 / EasyMaxCombatDuration), (1 / EasyMinCombatDuration));
                AirTimeNormalized = NormalizeValue(AirTime / CombatDuration, EasyMinAirTimePercentage, EasyMaxAirTimePercentage);
                DamageAndHealNormalized = NormalizeValue(HealthHealed - DamageTaken, EasyMinDamageAndHeal, EasyMaxDamageAndHeal);
                AccuracyNormalized = NormalizeValue((float)ShotsHit / ShotsFired, EasyMinAccuracy, EasyMaxAccuracy);
                GravityChangesNormalized = NormalizeValue(GravityChanges, EasyMinGravityChanges, EasyMaxGravityChanges);
                DoubleJumpsNormalized = NormalizeValue(DoubleJumps, EasyMinDoubleJumps, EasyMaxDoubleJumps);
                DashesNormalized = NormalizeValue(Dashes, EasyMinDashes, EasyMaxDashes);
                break;
            case RoomCR.Medium:
                CombatDurationNormalized = NormalizeValue((1 / CombatDuration), (1 / MediumMaxCombatDuration), (1 / MediumMinCombatDuration));
                AirTimeNormalized = NormalizeValue(AirTime / CombatDuration, MediumMinAirTimePercentage, MediumMaxAirTimePercentage);
                DamageAndHealNormalized = NormalizeValue(HealthHealed - DamageTaken, MediumMinDamageAndHeal, MediumMaxDamageAndHeal);
                AccuracyNormalized = NormalizeValue((float)ShotsHit / ShotsFired, MediumMinAccuracy, MediumMaxAccuracy);
                GravityChangesNormalized = NormalizeValue(GravityChanges, MediumMinGravityChanges, MediumMaxGravityChanges);
                DoubleJumpsNormalized = NormalizeValue(DoubleJumps, MediumMinDoubleJumps, MediumMaxDoubleJumps);
                DashesNormalized = NormalizeValue(Dashes, MediumMinDashes, MediumMaxDashes);
                break;
            case RoomCR.Hard:
                CombatDurationNormalized = NormalizeValue((1 / CombatDuration), (1 / HardMaxCombatDuration), (1 / HardMinCombatDuration));
                AirTimeNormalized = NormalizeValue(AirTime / CombatDuration, HardMinAirTimePercentage, HardMaxAirTimePercentage);
                DamageAndHealNormalized = NormalizeValue(HealthHealed - DamageTaken, HardMinDamageAndHeal, HardMaxDamageAndHeal);
                AccuracyNormalized = NormalizeValue((float)ShotsHit / ShotsFired, HardMinAccuracy, HardMaxAccuracy);
                GravityChangesNormalized = NormalizeValue(GravityChanges, HardMinGravityChanges, HardMaxGravityChanges);
                DoubleJumpsNormalized = NormalizeValue(DoubleJumps, HardMinDoubleJumps, HardMaxDoubleJumps);
                DashesNormalized = NormalizeValue(Dashes, HardMinDashes, HardMaxDashes);
                break;
            case RoomCR.GodsScar:
                CombatDurationNormalized = NormalizeValue((1 / CombatDuration), (1 / GodsScarMaxCombatDuration), (1 / GodsScarMinCombatDuration));
                AirTimeNormalized = NormalizeValue(AirTime / CombatDuration, GodsScarMinAirTimePercentage, GodsScarMaxAirTimePercentage);
                DamageAndHealNormalized = NormalizeValue(HealthHealed - DamageTaken, GodsScarMinDamageAndHeal, GodsScarMaxDamageAndHeal);
                AccuracyNormalized = NormalizeValue((float)ShotsHit / ShotsFired, GodsScarMinAccuracy, GodsScarMaxAccuracy);
                GravityChangesNormalized = NormalizeValue(GravityChanges, GodsScarMinGravityChanges, GodsScarMaxGravityChanges);
                DoubleJumpsNormalized = NormalizeValue(DoubleJumps, GodsScarMinDoubleJumps, GodsScarMaxDoubleJumps);
                DashesNormalized = NormalizeValue(Dashes, GodsScarMinDashes, GodsScarMaxDashes);
                break;
            default:
                break;
        }


        float CombatDurationPerformance = CombatDurationWeight * CombatDurationNormalized; //A menor duración del combate, más puntuación.
        float AirTimePerformace = AirTimeWeight * AirTimeNormalized; //A más tiempo en el aire, mayor puntuación.
        float DamageAndHealPerformace = DamageAndHealWeight * DamageAndHealNormalized;
        float AccuracyPerformance = AccuracyWeight * AccuracyNormalized;
        float GravityChangesPerformance = GravityChangesWeight * GravityChangesNormalized;
        float DoubleJumpsPerformance = DoubleJumpsWeight * DoubleJumpsNormalized;
        float DashesPerformance = DashesWeight * DashesNormalized;

        float PlayerPerformance = CombatDurationPerformance + AirTimePerformace + DamageAndHealPerformace + AccuracyPerformance + GravityChangesPerformance + DoubleJumpsPerformance + DashesPerformance;

        switch (PlayerPerformance)
        {
            case < 0.25f:
                ReduceCR();
                break;
            case > 0.75f:
                IncreaseCR();
                break;
            default:
                break;
        }
    }

    private float NormalizeValue(float Value, float Min, float Max)
    {
        return (Value - Min) / (Max - Min);
    }

    private void ReduceCR()
    {
        switch (CurrentRoomCR)
        {
            case RoomCR.Easy:
                break;
            case RoomCR.Medium:
                CurrentRoomCR = RoomCR.Easy;
                break;
            case RoomCR.Hard:
                CurrentRoomCR = RoomCR.Medium;
                break;
            case RoomCR.GodsScar:
                CurrentRoomCR = RoomCR.Hard;
                break;
            default:
                break;
        }
    }

    private void IncreaseCR()
    {
        switch (CurrentRoomCR)
        {
            case RoomCR.Easy:
                CurrentRoomCR = RoomCR.Medium;
                break;
            case RoomCR.Medium:
                CurrentRoomCR = RoomCR.Hard;
                break;
            case RoomCR.Hard:
                CurrentRoomCR = RoomCR.GodsScar;
                break;
            case RoomCR.GodsScar:
                break;
            default:
                break;
        }
    }

    public void BeginCombat()
    {
        CombatDuration = 0; AirTime = 0; //Reseteo de los datos de tiempo
        DamageTaken = 0; HealthHealed = 0; //Reseteo de los datos de vida
        ShotsFired = 0; ShotsHit = 0; //Reseteo de los datos de precisión
        GravityChanges = 0; DoubleJumps = 0; Dashes = 0; //Reseteo de los datos de acciones de movimiento

        Altar.SetActive(false);
        Portal.SetActive(false);
        RoomGenerator.ActivateEnemies();
        InCombat = true;
        Music.GetComponents<AudioSource>()[0].enabled = false;
        Music.GetComponents<AudioSource>()[1].enabled = true;
    }

    public void EnemyDead()
    {
        EnemyAmount--;
        if (EnemyAmount <= 0)
        {
            EndCombat();
        }
    }

    public void EndCombat()
    {
        InCombat = false;
        Portal.SetActive(true);
        Music.GetComponents<AudioSource>()[1].enabled = false;
        Music.GetComponents<AudioSource>()[0].enabled = true;
    }

    public void StatsScreen()
    {
        InfiniteSoulsStatsMenu.SetActive(true);
        float AccuracyPercentageForStats = (float)ShotsHit / ShotsFired;
        InfiniteSoulsStatsMenu.GetComponent<StatsMenu>().ShowStats(CombatDuration, AirTime, Mathf.Round(AccuracyPercentageForStats * 100), DamageTaken, HealthHealed, GravityChanges, DoubleJumps, Dashes, CurrentRoomCR);
        CalculateNextRoomCR();
        InfiniteSoulsStatsMenu.GetComponent<StatsMenu>().SetNextRoomCR(CurrentRoomCR);
    }
}

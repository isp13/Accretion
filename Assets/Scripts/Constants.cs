using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float GravityPower = 70; // default is 27

    public static Dictionary<string, int> HierarchyDict = new Dictionary<string, int> { 
        { "Asteroid" , 0}, { "DwarfPlanet", 1 }, { "Planet", 2 }, { "DwarfStar", 3 },
        { "Star", 4 }, { "Giant Star", 5 }, {"neutron  star", 6 }, {"Black hole", 6 } };


    public static string PlayersNextObject = "DwarfPlanet";

    // Asteroids stats
    public static float AsteroidStartingMass = 1f;
    public static int AsteroidCriticalMass = 10;
    public static int AsteroidScale = 1;

    // Dwarf planet stats
    public static int DwarfPlanetsStartingMass = 10;
    public static int DwarfPlanetCriticalMass = 20;
    public static int DwarfPlanetScale = 5;

    // Planet stats
    public static int PlanetsStartingMass = 20;
    public static int PlanetCriticalMass = 50;
    public static int PlanetScale = 10;

    public static int DwarfStarCriticalMass = 200000;



    public static int MaxGravitationalDistance = 50;

    
    

    public static int LowerSecondsGenPlanet = 2; // нижняя граница в секундах в промежутке генерирования планет
    public static int UpperSecondsGenPlanet = 6; // верхняя граница в секундах в промежутке генерирования планет

    public static int DistanceToGenerateObjects = 150;

    public static float TrailDisapearTime = 1f;

    public static float maxSpeed = 30;
    public static float minSpeed = 10;

    public static string PlanetPrefabsFolder = "Assets/#OnePotatoKingdom_FULL/Prefabs/#Planets";
    public static string AsteroidPrefabsFolder = "Assets/PACKS VOLS/Mobile Astre Pack vol. 1/Prefabs";
    public static string ColorfulMaterialsFolder = "Assets/#OnePotatoKingdom_FULL/Materials/Materials/";

    public static bool PlayerIsMoving = false;


    // не нарушая порядок, словари должны быть внизу
    // СЛОВАРИ НЕ ЗАПОЛНЕНЫ ДО КОНЦА
    public static Dictionary<string, int> HierarchyMinMass = new Dictionary<string, int> {
        { "Asteroid" , 0}, { "DwarfPlanet", AsteroidCriticalMass }, { "Planet", DwarfPlanetCriticalMass}, { "DwarfStar", PlanetCriticalMass},
        { "Star", 1 }, { "Giant Star", 1 }, {"neutron  star", 1 }, {"Black hole", 1 } };

    public static Dictionary<string, int> HierarchyMaxMass = new Dictionary<string, int> {
        { "Asteroid" , AsteroidCriticalMass}, { "DwarfPlanet", DwarfPlanetCriticalMass }, { "Planet", PlanetCriticalMass}, { "DwarfStar", DwarfStarCriticalMass},
        { "Star", 1 }, { "Giant Star", 1 }, {"neutron  star", 1 }, {"Black hole", 1 } };

}

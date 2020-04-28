using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float GravityPower = 70; // default is 27

    public static Dictionary<string, int> HierarchyDict = new Dictionary<string, int> { 
        { "Asteroid" , 0}, { "DwarfPlanet", 1 }, { "Planet", 2 }, { "DwarfStar", 3 },
        { "Star", 4 }, { "GiantStar", 5 }, {"NeutronStar", 6 }, {"BlackHole", 6 } };


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

    public static int DwarfStarStartingMass = 50;
    public static int DwarfStarCriticalMass = 100;

    public static int StarStartingMass = 100;
    public static int StarCriticalMass = 150;

    public static int GiantStarStartingMass = 150;
    public static int GiantStarCriticalMass = 250;

    public static int NeutronStarStartingMass = 250;
    public static int NeutronStarCriticalMass = 350;

    public static int BlackHoleStartingMass = 350;
    public static int BlackHoleCriticalMass = 1000;



    public static int MaxGravitationalDistance = 50;

    
    

    public static int LowerSecondsGenPlanet = 2; // нижняя граница в секундах в промежутке генерирования планет
    public static int UpperSecondsGenPlanet = 6; // верхняя граница в секундах в промежутке генерирования планет

    public static int DistanceToGenerateObjects = 100;

    public static float TrailDisapearTime = 1f;

    public static float maxSpeed = 30;
    public static float minSpeed = 10;

    public static string ColorfulMaterialsFolder = "Assets/Resources/#OnePotatoKingdom_FULL/Materials/Materials/";
    public static string StarsMaterialsFolder = "Assets/Stars/";
    

    public static bool PlayerIsMoving = false;


    // не нарушая порядок, словари должны быть внизу
    // СЛОВАРИ НЕ ЗАПОЛНЕНЫ ДО КОНЦА
    public static Dictionary<string, int> HierarchyMinMass = new Dictionary<string, int> {
        { "Asteroid" , 0}, { "DwarfPlanet", AsteroidCriticalMass }, { "Planet", DwarfPlanetCriticalMass}, { "DwarfStar", PlanetCriticalMass},
        { "Star", DwarfStarCriticalMass }, { "GiantStar", StarCriticalMass }, {"NeutronStar", GiantStarCriticalMass }, {"BlackHole", NeutronStarCriticalMass } };

    public static Dictionary<string, int> HierarchyMaxMass = new Dictionary<string, int> {
        { "Asteroid" , AsteroidCriticalMass}, { "DwarfPlanet", DwarfPlanetCriticalMass }, { "Planet", PlanetCriticalMass}, { "DwarfStar", DwarfStarCriticalMass},
        { "Star", StarCriticalMass }, { "GiantStar", GiantStarCriticalMass }, {"NeutronStar", NeutronStarCriticalMass }, {"BlackHole", BlackHoleCriticalMass } };

}

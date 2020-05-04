using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float GravityPower = 140; // default is 27 // ,было 70 // но тк изменилась гравитация, поднялв  два раза

    public static Dictionary<string, int> HierarchyDict = new Dictionary<string, int> { 
        { "Asteroid" , 0}, { "DwarfPlanet", 1 }, { "Planet", 2 }, { "DwarfStar", 3 },
        { "Star", 4 }, { "GiantStar", 5 }, {"NeutronStar", 6 }, {"BlackHole", 6 } };


    public static string PlayersNextObject = "Dwarf Planet";
    
    // Asteroids stats
    public static float AsteroidStartingMass = 1f;
    public static int AsteroidCriticalMass = 10;
    public static int AsteroidScale = 1;
    public static int AsteroidMainCameraDistance = 30;
    // Dwarf planet stats
    public static int DwarfPlanetsStartingMass = 10;
    public static int DwarfPlanetCriticalMass = 25;
    public static float DwarfPlanetScale = 10f;
    public static float ColliderRadius_DwarfPlanet = 0.54f;
    public static int PlanetMainCameraDistance = 70;
    // Planet stats
    public static int PlanetsStartingMass = 20;
    public static int PlanetCriticalMass = 50;
    public static float PlanetScale = 13f;
    public static float ColliderRadius_Planet = 0.55f;

    public static int DwarfStarStartingMass = 50;
    public static int DwarfStarCriticalMass = 100;
    public static float DwarfStarScale = 13f;
    public static float ColliderRadius_DwarfStar = 0.55f;

    public static int StarStartingMass = 100;
    public static int StarCriticalMass = 150;
    public static float StarScale = 14f;
    public static float ColliderRadius_Star = 0.6f;

    public static int GiantStarStartingMass = 150;
    public static int GiantStarCriticalMass = 250;
    public static float GiantStarScale = 16f;
    public static float ColliderRadius_GiantStar = 0.65f;

    public static int NeutronStarStartingMass = 250;
    public static int NeutronStarCriticalMass = 350;
    public static float NeutronStarScale = 10f;
    public static float ColliderRadius_NeutronStar = 0.6f;

    public static int BlackHoleStartingMass = 350;
    public static int BlackHoleCriticalMass = 1000;
    public static float BlackHoleScale = 10f;
    public static float ColliderRadius_BlackHole = 0.6f;


    public static int MaxGravitationalDistance = 45;

    
    

    public static int LowerSecondsGenPlanet = 2; // нижняя граница в секундах в промежутке генерирования планет
    public static int UpperSecondsGenPlanet = 8; // верхняя граница в секундах в промежутке генерирования планет

    public static int DistanceToGenerateObjects = 250;

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

    public static Dictionary<string, string[]> LegalToSpawn = new Dictionary<string, string[]> {
        { "Asteroid" , new string[] {"Asteroid", "DwarfPlanet" } },
        {"DwarfPlanet" , new string[] {"Asteroid", "DwarfPlanet", "Planet" } },
        {"Planet" , new string[] {"Asteroid", "DwarfPlanet", "Planet", "DwarfStar" } },
        {"DwarfStar" , new string[] { "DwarfPlanet", "Planet", "DwarfStar", "Star" } },
        {"Star" , new string[] { "DwarfPlanet", "Planet", "DwarfStar", "Star", "GiantStar" } },
        {"GiantStar" , new string[] { "DwarfPlanet", "Planet", "DwarfStar", "Star", "GiantStar", "NeutronStar" } },
        {"NeutronStar" , new string[] {"Planet", "DwarfStar", "Star", "GiantStar", "NeutronStar", "BlackHole" } },
        {"BlackHole" , new string[] { "DwarfPlanet", "Planet", "DwarfStar", "Star", "GiantStar", "NeutronStar" , "BlackHole" } }
    };

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float GravityPower = 100; // default is 27

    public static int AsteroidCriticalMass = 200;
    public static int AsteroidScale = 1;

    public static int DwarfPlanetCriticalMass = 500;

    public static int PlanetCriticalMass = 20000;
    public static int PlanetScale = 10;

    public static int StarCriticalMass = 200000;


    public static int MaxGravitationalDistance = 50;

    public static int PlanetsStartingMass = 1;
    public static float AsteroidStartingMass = 0.5f;

    public static int LowerSecondsGenPlanet = 2; // нижняя граница в секундах в промежутке генерирования планет
    public static int UpperSecondsGenPlanet = 6; // верхняя граница в секундах в промежутке генерирования планет

    public static int DistanceToGenerateObjects = 150;

    public static int TrailDisapearTime = 2;

    public static string PlanetPrefabsFolder = "Assets/#OnePotatoKingdom_FULL/Prefabs/#Planets";
    public static string AsteroidPrefabsFolder = "Assets/PACKS VOLS/Mobile Astre Pack vol. 1/Prefabs";

    public static bool PlayerIsMoving = false;


}

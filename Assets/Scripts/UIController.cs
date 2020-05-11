using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public Text planetName;
    public Text Mass;
    public Text Vector;
    public Text Coords;
    public Text nextObject;
    public GameObject player;
    private GravitationalBody scr;

    // Start is called before the first frame update
    void Start()
    {
        scr = player.GetComponent<GravitationalBody>();

        planetName.text = "Type: planet";
        Mass.text = "Mass: -1";
        Vector.text = "Vector2: <0,0,0>";
        Coords.text = "Coordinates: <0,0,0>";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        planetName.text = "" + scr.ShareObjectData("name");
        Mass.text = "Mass: " + scr.ShareObjectData("mass");;
        Vector.text = "Vector2: <" + scr.ShareObjectData("vector") + ">" ;
        Coords.text = "Coordinates: <" + scr.ShareObjectData("coords") + ">";
        nextObject.text = "Next: " + Constants.PlayersNextObject;
    }

    void FetchInfoLabels() { 

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSet : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        // Turn off v-sync
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}

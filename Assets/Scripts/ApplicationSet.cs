using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSet : MonoBehaviour
{
    /// <summary>
    /// 0 - отключение vsync, else - включение
    /// </summary>
    private int VSyncCount {
        set { 
            QualitySettings.vSyncCount = value;
        }
    }

    /// <summary>
    /// Желаемая частота обновления экрана. ios - максимум 60 fps
    /// </summary>
    private int TargetFrameRate
    {
        set
        {
            Application.targetFrameRate = value;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        // Turn off v-sync
        VSyncCount = Constants.vSyncCount;
        // устанавливаем максимальную частоту обновления экрана
        TargetFrameRate = Constants.targetFrameRate;
    }
}

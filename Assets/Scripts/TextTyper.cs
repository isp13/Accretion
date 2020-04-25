using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Text uiText;
    public string firstText = "";
    IEnumerator Start()
    {
        
        for (int i = 0; i < firstText.Length; i++)
        {
            yield return new WaitForSeconds(0.23f);
            uiText.text = firstText.Substring(0, i + 1);
        }

        yield return new WaitForSeconds(1.5f);
        uiText.text = "";
    }
}

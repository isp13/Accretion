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
    public string secondText = "";

    IEnumerator Start()
    {
        
        for (int i = 0; i < firstText.Length; i++)
        {
            yield return new WaitForSeconds(0.08f);
            uiText.text = firstText.Substring(0, i + 1);
        }

        yield return new WaitForSeconds(1.5f);
        uiText.text = "";

        yield return new WaitForSeconds(2f);


        for (int i = 0; i < secondText.Length; i++)
        {
            yield return new WaitForSeconds(0.04f);
            uiText.text = secondText.Substring(0, i + 1);
        }

        yield return new WaitForSeconds(2f);
        uiText.text = "";
    }



    public IEnumerator TypeText(string text)
    {
        uiText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(0.04f);
            uiText.text = text.Substring(0, i + 1);
        }

        yield return new WaitForSeconds(2f);
        uiText.text = "";
    }

    public IEnumerator TypeText(string text, string text2)
    {
        uiText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(0.04f);
            uiText.text = text.Substring(0, i + 1);
        }

        yield return new WaitForSeconds(2f);
        uiText.text = "";

        
        for (int i = 0; i < text2.Length; i++)
        {
            yield return new WaitForSeconds(0.04f);
            uiText.text = text2.Substring(0, i + 1);
        }

        yield return new WaitForSeconds(2f);
        uiText.text = "";
    }
}

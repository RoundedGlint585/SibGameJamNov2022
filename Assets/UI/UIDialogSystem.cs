using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialogSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] senetences;
    private int index;
    public float typingDelay;

    public GameObject continueButton;

    IEnumerator Type()
    {
        foreach (char letter in senetences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingDelay);
        }
        //yield return new WaitForSeconds(2f);
    }

    public void NextSentence() 
    {
        continueButton.SetActive(false);

        if (index < senetences.Length - 1)
        {
            index ++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
 
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplay.text == senetences[index])
        {
            continueButton.SetActive(true);
        }
    }
}

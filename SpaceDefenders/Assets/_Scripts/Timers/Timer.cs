using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [HideInInspector] public int seconds = 0;
    [HideInInspector] public int minutes = 0;


    void OnEnable()
    {
        StartCoroutine(Time());
    }
    void Update()
    {
        if(seconds >= 60)
        {
            seconds = 0;
            minutes++;
        }

        #region DisplayText
        if (minutes < 10 && seconds < 10) GetComponent<TextMeshProUGUI>().text = "0" + minutes + ":0" + seconds;
        if (minutes < 10 && seconds >= 10) GetComponent<TextMeshProUGUI>().text = "0" + minutes + ":" + seconds;
        if (minutes >= 10 && seconds < 10) GetComponent<TextMeshProUGUI>().text = minutes + ":0" + seconds;
        if (minutes >= 10 && seconds >= 10) GetComponent<TextMeshProUGUI>().text = minutes + ":" + seconds;
        #endregion
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(1);

        seconds++;

        StartCoroutine(Time());
    }
}

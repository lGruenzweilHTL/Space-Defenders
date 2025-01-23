using System.Collections;
using UnityEngine;
using TMPro;
using SpaceDefenders.GameManagement;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TimeModeGameManager manager;
    [SerializeField] private float availableTime = 60f;

    void Awake()
    {
        StartCoroutine(Count());
    }

    void Update()
    {
        GetComponent<TMP_Text>().text = "Time: " + availableTime.ToString();

        if (availableTime <= 0) manager.GameOver();
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(1f);

        availableTime--;

        StartCoroutine(Count());
    }
}

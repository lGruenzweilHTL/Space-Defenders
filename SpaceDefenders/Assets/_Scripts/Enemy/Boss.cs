using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum State
    {
        Intro,
        Phase1,
        Phase2,
        Outro
    }

    private State currentState = State.Intro;

    void Start()
    {
        Debug.Log(currentState);
    }
}

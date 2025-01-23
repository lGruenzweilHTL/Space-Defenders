using UnityEngine;

[CreateAssetMenu(fileName ="HighscoreComm", menuName = "ScriptableObjects/HighscoreCommunicator")]
public class HighscoreCommunicator : ScriptableObject
{
    public int DefendMode;
    public int SurvivalMode;
    public int TimeMode;
    
    public ulong awardedXP;
}
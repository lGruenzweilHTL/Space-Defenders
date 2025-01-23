using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int goldCount;
    public bool postProcessingEnabled = true;
    public bool cameraShakeEnabled = true;
    public bool vibrationsEnabled = true;
}
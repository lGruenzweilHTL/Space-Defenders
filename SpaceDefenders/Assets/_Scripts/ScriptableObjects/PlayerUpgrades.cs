using UnityEngine;

[CreateAssetMenu(fileName = "Player UpgradeStats", menuName = "ScriptableObjects/PlayerUpgradeStats")]
public class PlayerUpgrades : ScriptableObject
{
    [Space, Range(0, 4)] public int player_speedLevel;
    [Space, Range(0, 4)] public int player_shieldLevel;
    [Space, Range(0, 4)] public int player_damageLevel;
    
    public void SetUpgradeIndex(int index, int value)
    {
        switch (index)
        {
            case 0:
                player_speedLevel = value;
                break;
            case 1:
                player_shieldLevel = value;
                break;
            case 2:
                player_damageLevel = value;
                break;
        }
    }
}
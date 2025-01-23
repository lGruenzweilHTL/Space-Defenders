using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private UpgradeItemData data;

    void Awake() //use Awake because it also triggers when GameObject is inactive
    {
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        levelText.text = "Level: " + data.Value.ToString();
    }

    public void Toogle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

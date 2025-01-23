using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private bool debugWindowOpen;
    [SerializeField] private GameObject debugWindow;

    private List<GameObject> hitboxDrawers;
    private bool showHitboxes;

    [SerializeField] private GameObject minimap;

    [SerializeField] private PlayerStats playerStats;

    void Update()
    {
        if(debugWindowOpen)
        {
            debugWindow.SetActive(true);
        }
        else
        {
            debugWindow.SetActive(false);
        }


        hitboxDrawers = new List<GameObject>();

        hitboxDrawers.Add(GameObject.Find("Player"));
        hitboxDrawers.Add(GameObject.Find("Tower"));
        hitboxDrawers.Add(GameObject.Find("MoonShop"));

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            hitboxDrawers.Add(enemy);
        }
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            hitboxDrawers.Add(enemy);
        }
        foreach (var hitboxDrawer in hitboxDrawers)
        {
            if (hitboxDrawer != null) hitboxDrawer.GetComponent<DrawPolygonCollider2D>().isShowingHitboxes = showHitboxes;
        }
    }

    public void ToggleDebugWindow()
    {
        if(debugWindowOpen)
        {
            debugWindowOpen = false;
        }
        else
        {
            debugWindowOpen = true;
        }
    }

    public void ToggleHitboxes(bool toggle)
    {
        showHitboxes = toggle;
    }

    public void ToggleMinimap(bool show)
    {
        minimap.SetActive(show);
    }

    public void GoldAmmt(string ammt)
    {
        int defValue = 0;
        int result = defValue;
        int.TryParse(ammt, out result);

        playerStats.goldCount = result;
    }
}

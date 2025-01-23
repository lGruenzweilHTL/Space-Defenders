using LootLocker.Requests;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class LootlockerManager : MonoBehaviour {
    public const ulong XP_PER_GAME = 25;
    
    public Leaderboard[] leaderboards;
    public HighscoreCommunicator scoreComm;

    [SerializeField] private TMP_Text currLvlText, nextLvlText, xpText;
    [SerializeField] private Slider xpSlider;

    bool successfulLogin = true;
    [SerializeField] private GameObject loginWarningIcon;
    [SerializeField] private GameObject connectInfo;

    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Transform inventoryParent;

    [SerializeField] private TMP_Text currNameText;

    [SerializeField] private Sprite[] upgradeTextures;
    
    [Space, SerializeField] private PlayerUpgrades playerUpgrades;

    [FormerlySerializedAs("upgradeIndex")] [Header("Debug Upgrade values")] [SerializeField]
    private int debugUpgradeIndex;

    [SerializeField] private int debugUpgradeLevel;

    #region INIT

    void Start() {
        TryConnect();
    }

    public void TryConnect() {
        StartCoroutine(Setup());
    }

    private IEnumerator Setup() {
        yield return Login();
        if (successfulLogin) {
            // Start new PlayerProgression if not active
            SubmitXP(0);

            //Initialize everything else
            yield return InitLeaderboards();
            InitUpgrades();
            InitXP();
            InitName();
        }
    }

    void InitXP() {
        const string PROGRESSION_KEY = "p_lvl";

        LootLockerSDKManager.GetPlayerProgression(PROGRESSION_KEY, (response) => {
            if (!response.success) {
                Debug.LogError("GetPlayerProgression Failed: " + response.Error);
            }

            currLvlText.text = response.step.ToString();
            nextLvlText.text = (response.step + 1).ToString();
            SubmitXP(scoreComm.awardedXP);
        });
    }

    IEnumerator InitLeaderboards() {
        foreach (var lb in leaderboards) {
            switch (lb.leaderboardKey) {
                case "LB_Def":
                    if (scoreComm.DefendMode != 0) yield return lb.Submit(scoreComm.DefendMode);
                    break;

                case "LB_Sur":
                    if (scoreComm.SurvivalMode != 0) yield return lb.Submit(scoreComm.SurvivalMode);
                    break;

                case "LB_Time":
                    if (scoreComm.TimeMode != 0) yield return lb.Submit(scoreComm.TimeMode);
                    break;
            }

            yield return lb.FetchTopScores();
        }
    }

    void InitName() {
        LootLockerSDKManager.GetPlayerName(response => {
            if (response.success) {
                if (response.name != "") {
                    currNameText.text = "Current Name: " + response.name;
                }
                else {
                    currNameText.text = "You currently don't have a name";
                }
            }
            else {
                Debug.LogError("Initializing NameText failed: " + response.Error);
            }
        });
    }

    void InitUpgrades() {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(UPGRADE_KEY, response => {
            if (!response.success) {
                Debug.LogError("GetPlayerPersistentStorage failed" + response.Error);
                return;
            }
            
            string value = response.payload != null ? response.payload.value : "";
            Debug.Log("Upgrades: " + value);
            
            // Check if the player already has upgrades
            if (value.Length == NumUpgrades) {
                UpdateUpgradeInventory();
                return;
            }
            
            // If not, initialize the upgrades
            LootLockerSDKManager.UpdateOrCreateKeyValue(UPGRADE_KEY, new string('0', NumUpgrades), true, response => {
                if (response.success) {
                    UpdateUpgradeInventory();
                    Debug.Log("Successfully initialized upgrades");
                }
                else {
                    Debug.LogError("Initializing upgrades failed: " + response.Error);
                }
            });
        });
    }

    #endregion

    #region XP

    [ContextMenu("(Debug) Add150XP")]
    void Add150XP() {
        SubmitXP(150);
    }


    void SubmitXP(ulong amountOfPoints) {
        string progressionKey = "p_lvl";

        LootLockerSDKManager.AddPointsToPlayerProgression(progressionKey, amountOfPoints, response => {
            if (!response.success) {
                Debug.LogError("SubmitXP Failed: " + response.Error);
            }
            
            Debug.Log($"Successfully awarded {amountOfPoints} XP to player");

            // If the awarded_tiers array contains any items that means the player leveled up
            // There can also be multiple level-ups at once
            if (response.awarded_tiers.Any()) {
                foreach (var awardedTier in response.awarded_tiers) {
                    Debug.Log($"Reached level {awardedTier.step}!");

                    foreach (var assetReward in awardedTier.rewards.asset_rewards) {
                        Debug.Log($"Rewarded with an asset, id: {assetReward.asset_id}!");
                    }

                    //Level
                    currLvlText.text = awardedTier.step.ToString();
                    nextLvlText.text = (awardedTier.step + 1).ToString();
                }
            }

            //XP
            if (response.next_threshold != null) {
                xpText.text = (response.points - response.previous_threshold).ToString() + "/" + (response.next_threshold - response.previous_threshold).ToString();
                xpSlider.maxValue = (float)response.next_threshold - response.previous_threshold;
                xpSlider.value = response.points - response.previous_threshold;
            }
            else {
                xpSlider.value = 0;
                xpText.text = "Max Level Reached !";
                Debug.Log("Max Level Reached");
            }
        });
    }

    #endregion

    public void SetPlayerName(string name) {
        LootLockerSDKManager.SetPlayerName(name, (response) => {
            if (response.success) {
                Debug.Log("Successfully set player name");
                currNameText.text = "Current Name: " + name;
            }
            else {
                Debug.LogError("Setting Player Name Failed" + response.Error);
            }
        });
        StartCoroutine(InitLeaderboards());
    }

    IEnumerator Login() {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) => {
            if (response.success) {
                Debug.Log("Player logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                successfulLogin = true;
                loginWarningIcon.SetActive(false);
                connectInfo.SetActive(false);
                done = true;
            }
            else {
                Debug.LogError("Could not start session" + response.Error);
                successfulLogin = false;
                loginWarningIcon.SetActive(true);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    #region UpgradeInventory

    [ContextMenu("(Debug) SetUpgradeItem")]
    public void SetDebugUpgradeItem() {
        SetUpgradeItem(debugUpgradeIndex, debugUpgradeLevel);
    }

    private static int NumUpgrades => UpgradeData.Length;
    private const string UPGRADE_KEY = "Upgrades";

    private static readonly (string, string)[] UpgradeData = {
        ("Damage", "The damage a single shot does"),
        ("Speed", "The speed of the player"),
        ("Shield", "The strength of the shield")
    };

    public void SetUpgradeItem(int index, int level) {
        if (index < 0 || index >= NumUpgrades) {
            Debug.LogError("Invalid index");
            return;
        }

        StringBuilder currentUpgrades = new();
        LootLockerSDKManager.GetSingleKeyPersistentStorage(UPGRADE_KEY, response => {
            if (response.success) {
                currentUpgrades = new(response.payload.value);

                currentUpgrades[index] = level.ToString()[0];
                LootLockerSDKManager.UpdateOrCreateKeyValue(UPGRADE_KEY,
                    currentUpgrades.ToString(),
                    true,
                    response => {
                        if (response.success) {
                            UpdateUpgradeInventory();
                            Debug.Log($"Successfully updated upgrades to {currentUpgrades}");
                        }
                        else {
                            Debug.LogError("Update Upgrade Items failed: " + response.Error);
                        }
                    });
            }
            else {
                Debug.LogError("Get UpgradeItem failed: " + response.Error);
            }
        });
    }

    void UpdateUpgradeInventory() {
        //Clear all slots
        while (inventoryParent.childCount > 0) {
            DestroyImmediate(inventoryParent.GetChild(0).gameObject);
        }

        LootLockerSDKManager.GetSingleKeyPersistentStorage(UPGRADE_KEY, response => {
            if (!response.success) {
                Debug.LogError("GetPlayerPersistentStorage failed" + response.Error);
                return;
            }

            // In format: "123"
            string upgrades = response.payload.value;

            for (int i = 0; i < NumUpgrades; i++) {
                int level = int.Parse(upgrades[i].ToString());

                if (level == 0) continue;

                //Instantiate item
                GameObject item = Instantiate(inventoryItemPrefab, inventoryParent);

                //Set all values
                UpgradeItemData data = item.GetComponent<UpgradeItemData>();
                data.Name = UpgradeData[i].Item1;
                data.Value = level;
                data.Description = UpgradeData[i].Item2;

                //Set correct sprite
                item.GetComponent<Image>().sprite = upgradeTextures[level - 1];
                
                //Set correct upgrade level
                playerUpgrades.SetUpgradeIndex(i, level);
            }
        });
    }

    #endregion

    private IEnumerator LoadTexture(string url, Image iconIMG) {
        iconIMG.enabled = false; //híde image until texture is loaded (no white background until load)

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
        iconIMG.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);

        iconIMG.enabled = true;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

namespace SpaceDefenders.GameManagement
{
    public class TimeModeGameManager : MonoBehaviour
    {
        [SerializeField] private HighscoreCommunicator scoreComm;

        [SerializeField] private TMP_Text killCounter;

        [HideInInspector] public int enemysKilled = 0;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private GameObject postProcessObj;

        private GameObject player;
        private Player_Health pHealth;
        private GameObject playerHealthBar;
        private Camera_FollowPlayer camFollow;

        void Update() { killCounter.text = "Kills: " + enemysKilled; }

        void Start()
        {
            player = GameObject.Find("Player");
            playerHealthBar = GameObject.Find("Player_Canvas");
            pHealth = player.GetComponent<Player_Health>();
            camFollow = Camera.main.GetComponent<Camera_FollowPlayer>();

            postProcessObj.SetActive(playerStats.postProcessingEnabled);
        }

        public void GameOver()
        {
            scoreComm.TimeMode = enemysKilled;
            scoreComm.awardedXP = LootlockerManager.XP_PER_GAME;
            SceneManager.LoadScene("GameOver");
        }

        public void RespawnPlayer()
        {
            player.SetActive(false);
            playerHealthBar.SetActive(false);

            camFollow.playerDead = true;
            pHealth.playerDead = true;
            pHealth.PlayerHealth = 100;

            StartCoroutine(RespawnTimer());
        }
        IEnumerator RespawnTimer()
        {
            yield return new WaitForSeconds(5);

            player.transform.position = Vector2.zero;
            player.SetActive(true);
            playerHealthBar.SetActive(true);

            camFollow.playerDead = false;
            pHealth.playerDead = false;
            pHealth.PlayerHealth = pHealth.playerHealthMax;
        }
    }
}
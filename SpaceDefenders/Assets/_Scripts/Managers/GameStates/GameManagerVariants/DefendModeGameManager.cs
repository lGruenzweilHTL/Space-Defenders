using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SpaceDefenders.GameManagement
{
    public class DefendModeGameManager : MonoBehaviour
    {
        [SerializeField] private HighscoreCommunicator scoreComm;

        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Timer timer;
        [SerializeField] private GameObject postProcessObj;
        [Header("Respawn")]
        [SerializeField] private Vector3 respawnPos;

        private GameObject player;
        private Player_Health pHealth;
        private GameObject playerHealthBar;
        private Camera_FollowPlayer camFollow;

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
            scoreComm.DefendMode = timer.minutes * 60 + timer.seconds;
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

            player.transform.position = respawnPos;
            player.SetActive(true);
            playerHealthBar.SetActive(true);

            camFollow.playerDead = false;
            pHealth.playerDead = false;
            pHealth.PlayerHealth = pHealth.playerHealthMax;
        }
    }
}
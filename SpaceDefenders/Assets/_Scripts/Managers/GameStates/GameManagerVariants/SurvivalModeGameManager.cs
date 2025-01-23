using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceDefenders.GameManagement
{
    public class SurvivalModeGameManager : MonoBehaviour
    {
        [SerializeField] private HighscoreCommunicator scoreComm;

        [SerializeField] private Timer timer;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private GameObject postProcessObj;

        void Start ()
        {
            postProcessObj.SetActive(playerStats.postProcessingEnabled);
        }

        public void GameOver()
        {
            scoreComm.SurvivalMode = timer.minutes * 60 + timer.seconds;
            scoreComm.awardedXP = LootlockerManager.XP_PER_GAME;
            SceneManager.LoadScene("GameOver");
        }
    }
}
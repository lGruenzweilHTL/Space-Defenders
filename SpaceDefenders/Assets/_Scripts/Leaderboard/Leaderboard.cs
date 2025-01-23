using LootLocker.Requests;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class Leaderboard : MonoBehaviour
{
    [FormerlySerializedAs("retreiveScoreAmmt")] [Tooltip("The amount of scores to be shown on the leaderboard including the playerScore")]
    public int retrieveScoreAmmt;

    public TMP_Text playerScores, playerNames;
    public string leaderboardKey;

    public IEnumerator Submit(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.LogError("Score Upload Failed: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopScores()
    {
        print("Fetching Top Scores for leaderboard: " + leaderboardKey);
        bool done = false;
        #region Get Player Rank and Name
        int playerRank = 0;
        LootLockerSDKManager.GetMemberRank(leaderboardKey, PlayerPrefs.GetString("PlayerID"), (response) => 
        {
            if (response.statusCode == 200)
            {
                print("GetMemberRank Successful");
                playerRank = response.rank;
            }
            else
            {
                Debug.LogError("GetMemberRank failed: " + response.Error);
            }
        });

        string PlayerName = "";
        LootLockerSDKManager.GetPlayerName((response) => 
        {
            if (response.success)
            {
                if (string.IsNullOrEmpty(response.name))
                {
                    Debug.Log("The player has no name");
                }
                else
                {
                    Debug.Log("The player has the name: " + response.name);
                    PlayerName = response.name;
                }
            }
            else
            {
                Debug.LogError("Error getting player name");
            }
        });
        #endregion
        #region Get Top Scores
        LootLockerSDKManager.GetScoreList(leaderboardKey, retrieveScoreAmmt, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                //if the player is good enough to be listed on top
                if (playerRank <= retrieveScoreAmmt)
                {
                    //Fetch Scores for the Top players
                    for (int i = 0; i < members.Length; i++)
                    {
                        tempPlayerNames += members[i].rank + ". ";
                        if (members[i].player.name != "")
                        {
                            tempPlayerNames += members[i].player.name;
                        }
                        else
                        {
                            tempPlayerNames += "Player " + members[i].player.id;
                        }
                        if (members[i].member_id == PlayerPrefs.GetString("PlayerID")) tempPlayerNames += " (You)";

                        tempPlayerScores += members[i].score + "\n";
                        tempPlayerNames += "\n";
                    }
                }

                //if the player is not good enough to be listed on the top
                else if (playerRank > retrieveScoreAmmt)
                {
                    //fetch scores for Top5 players
                    for (int i = 0; i < members.Length - 1; i++)
                    {
                        tempPlayerNames += members[i].rank + ". ";
                        if (members[i].player.name != "")
                        {
                            tempPlayerNames += members[i].player.name;
                        }
                        else
                        {
                            tempPlayerNames += "Player " + members[i].player.id;
                        }

                        tempPlayerScores += members[i].score + "\n";
                        tempPlayerNames += "\n";
                    }

                    //Add the Player at the end
                    tempPlayerNames += playerRank + ". ";
                    if(PlayerName != "")
                    {
                        tempPlayerNames += PlayerName + " (You)";
                    }
                    else
                    {
                        tempPlayerNames += "Player " + PlayerPrefs.GetString("PlayerID") + " (You)";
                    }
                }

                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.LogError("Get Score list failed" + response.Error);
                done = true;
            }
        });
        #endregion
        yield return new WaitWhile(() => done == false);
    }
}
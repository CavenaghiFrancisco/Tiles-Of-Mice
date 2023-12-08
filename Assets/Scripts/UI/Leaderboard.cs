using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    string leaderboardID = "19157";
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    public TMP_InputField playerNameInputfield;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    public void TurnOffInput()
    {
        playerNameInputfield.gameObject.SetActive(false);
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
        {
            if (response.success)
            {
                StartCoroutine(SubmitScoreRoutine(66666, playerNameInputfield.text));
            }
            else
            {
                Debug.Log("Could not set player name" + response.errorData);
            }
        });
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload, string name)
    {
        bool done = false;
        string playerID = name;
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
                StartCoroutine(FetchTopHighscoresRoutine());
            }
            else
            {
                Debug.Log("Failed" + response);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed" + response.errorData);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
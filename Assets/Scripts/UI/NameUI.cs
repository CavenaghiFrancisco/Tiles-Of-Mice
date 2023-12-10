using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputField;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Button bttn;

    private void Start()
    {
        StartCoroutine(Leaderboard.LoginRoutine());
        text.text = PlayerPrefs.GetString("Player", "BR1E");
    }

    private void Update()
    {
        if(inputField.text.Length == 0)
        {
            bttn.interactable = false;
        }
        else
        {
            bttn.interactable = true;
        }
    }

    public void ChangeName()
    {
        string name = inputField.text;

        inputField.text = "";

        PlayerPrefs.SetString("Player", name);

        text.text = name;

        Leaderboard.SetPlayerName(name);
    }
}

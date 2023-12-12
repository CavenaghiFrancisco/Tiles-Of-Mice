using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private GameObject timeline;
    [SerializeField] private GameObject leaderboard;

    private void Start()
    {
        StartCoroutine(ChargeScene());
    }

    private IEnumerator ChargeScene()
    {
        panel.gameObject.SetActive(true);
        while (panel.color.a > 0f)
        {
            panel.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        yield return null;
        timeline.SetActive(true);
        panel.gameObject.SetActive(false);
    }

    public void ChangeScene(int index)
    {
        StartCoroutine(FadeScene(index));
    }

    private IEnumerator FadeScene(int index)
    {
        panel.gameObject.SetActive(true);
        while (panel.color.a < 0.99f)
        {
            panel.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        yield return null;
        SceneManager.LoadScene(index);
        yield return null;
    }

    public void ActiveLeaderboard()
    {
        leaderboard.SetActive(true);
    }

}

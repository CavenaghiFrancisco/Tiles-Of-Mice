using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private List<GameObject> cams;

    private IEnumerator Fade(int index)
    {
        yield return new WaitForSeconds(1.8f);
        panels[index].SetActive(true);
        yield return null;
    }

    public void SetActivePanelAndCam(int index)
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject cam in cams)
        {
            cam.SetActive(false);
        }
        cams[index].SetActive(true);
        StartCoroutine(Fade(index));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

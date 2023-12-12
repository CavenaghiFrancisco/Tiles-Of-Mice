using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private List<GameObject> cams;
    [SerializeField] private GameObject panel;

    private void Start()
    {
        OptionsManager.SetVolumes();
    }

    private IEnumerator Fade(int index)
    {
        yield return new WaitForSeconds(1.8f);
        panels[index].SetActive(true);
        yield return null;
    }

    private IEnumerator FadeGameplay()
    {
        panel.SetActive(true);
        Image img = panel.GetComponent<Image>();
        while(img.color.a < 0.98f)
        {
            img.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        yield return null;
        SceneManager.LoadScene(1);
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
        StartCoroutine(FadeGameplay());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

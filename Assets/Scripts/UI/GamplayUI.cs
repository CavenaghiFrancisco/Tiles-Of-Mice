using UnityEngine;
using UnityEngine.UI;

namespace TOM
{
    public class GamplayUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private CanvasGroup panelInitial = null;
        [SerializeField] private CanvasGroup panelGameplay = null;
        [SerializeField] private CanvasGroup panelPause = null;
        [SerializeField] private CanvasGroup panelGameOver = null;

        [Header("Buttons")]
        [SerializeField] private Button buttonStart = null;
        [SerializeField] private Button buttonRestart = null;
        [SerializeField] private Button buttonResume = null;
        [SerializeField] private Button buttonEndGame = null;

        [Header("In Game UI")]
        [SerializeField] private Image healthBar = null;

        private void Awake()
        {
            TurnOffPanel(panelGameplay);
            TurnOffPanel(panelGameOver);
            TurnOffPanel(panelPause);

            Player.OnLifeModified += ChangeHPValue;
            WaveController.OnTestWaveLimitArrive += ShowGameOver;

            GameManager.OnPause += ShowPausePanel;
            GameManager.OnResume += HidePausePanel;

            buttonRestart.onClick.AddListener(RestartGame);
            buttonEndGame.onClick.AddListener(RestartGame);

            buttonResume.onClick.AddListener(HidePausePanel);
            buttonStart.onClick.AddListener(StartGame);
        }

        private void Start()
        {
            TurnOnPanel(panelInitial);
            GameManager.PauseGame();
        }

        private void OnDestroy()
        {
            Player.OnLifeModified -= ChangeHPValue;
            WaveController.OnTestWaveLimitArrive -= ShowGameOver;

            GameManager.OnPause  -= ShowPausePanel;
            GameManager.OnResume -= HidePausePanel;

            buttonRestart.onClick.RemoveAllListeners();
            buttonEndGame.onClick.RemoveAllListeners();
            buttonStart.onClick.RemoveAllListeners();
            buttonResume.onClick.RemoveAllListeners();
        }

        private void StartGame()
        {
            TurnOffPanel(panelInitial);
            TurnOffPanel(panelPause);
            TurnOffPanel(panelGameOver);
            TurnOnPanel(panelGameplay);
            GameManager.ResumeGame();
        }

        private void TurnOnPanel(CanvasGroup panel)
        {
            panel.alpha = 1;
            panel.blocksRaycasts = true;
            panel.interactable = true;
        }
        private void TurnOffPanel(CanvasGroup panel)
        {
            panel.alpha = 0;
            panel.blocksRaycasts = false;
            panel.interactable = false;
        }

        private void ChangeHPValue(int maxValue, int actualValue)
        {
            healthBar.fillAmount = (float)actualValue / (float)maxValue;
        }

        private void ShowGameOver()
        {
            TurnOffPanel(panelGameplay);
            TurnOffPanel(panelInitial);
            TurnOffPanel(panelPause);
            TurnOnPanel(panelGameOver);
        }
        private void RestartGame()
        {
            GameManager.ReloadGame();
        }

        private void ShowPausePanel()
        {
            TurnOffPanel(panelGameplay);
            TurnOffPanel(panelInitial);
            TurnOffPanel(panelGameOver);
            TurnOnPanel(panelPause);
        }

        private void HidePausePanel()
        {
            TurnOffPanel(panelPause);
            TurnOffPanel(panelInitial);
            TurnOffPanel(panelGameOver);
            GameManager.ResumeGame();
            TurnOnPanel(panelGameplay);
        }

    }
}
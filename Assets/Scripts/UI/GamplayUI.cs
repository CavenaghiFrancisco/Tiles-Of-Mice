using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TOM
{
    public class GamplayUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelGameplay = null;
        [SerializeField] private CanvasGroup panelInitial = null;
        [SerializeField] private Image healthBar = null;

        private void Awake()
        {
            TurnOffPanel(panelGameplay);
            Player.OnLifeModified += ChangeHPValue;
        }
        private void Start()
        {
            StartCoroutine(GameInitTimer());
        }
        private void OnDestroy()
        {
            Player.OnLifeModified -= ChangeHPValue;
        }

        private IEnumerator GameInitTimer()
        {
            TurnOnPanel(panelInitial);
            Time.timeScale = 0;
            float timer = 0;
            while (timer < 2)
            {
                timer += Time.fixedDeltaTime;
                yield return null;
            }
            TurnOffPanel(panelInitial);
            TurnOnPanel(panelGameplay);
            Time.timeScale = 1;
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

    }
}
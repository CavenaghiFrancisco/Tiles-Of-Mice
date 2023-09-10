using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image lifeUI;
    [SerializeField] private Image maxLifeUI;
    [SerializeField] private Player player;

    void Start()
    {
        player.OnLifeModified += UpdateLifeUI;
    }

    private void UpdateLifeUI(float life)
    {
        lifeUI.fillAmount = life / player.GetMaxLife();
    }

    private void OnDestroy()
    {
        player.OnLifeModified -= UpdateLifeUI;
    }

}

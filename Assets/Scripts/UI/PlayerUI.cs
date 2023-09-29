using System.Collections;
using System.Collections.Generic;
using TOM;
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

    private void UpdateLifeUI(int life)
    {
        lifeUI.fillAmount = (float)life / (float)player.GetMaxLife();
    }

    private void OnDestroy()
    {
        player.OnLifeModified -= UpdateLifeUI;
    }

}

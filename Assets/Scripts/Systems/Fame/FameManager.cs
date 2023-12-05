using System.Collections;
using System.Collections.Generic;
using TOM;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class FameManager : MonoBehaviour
{
    [SerializeField] private FameParameters parameters;
    [SerializeField] private GameObject rankStarFolder;
    [SerializeField] private Image fameBar;
    public int Fame { private set; get; }
    public int Rank { private set; get; }

    private List<Image> starList = new List<Image>();

    private int testingWave = 0;

    private int targetFame = 0;
    private int actualFame = 0;
    private void Awake()
    {
        FillStarList();
        ResetSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            GetFame(testingWave);
            testingWave++;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Fame += 50;
            SetSliderValue(Fame);
            if (Rank < 10)
            {
                if (Fame > targetFame)
                {
                    RankUp();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetSystem();
        }
    }
    public void GetFame(int waveCounter)
    {
        Fame += waveCounter + parameters.fameRotationMultiplier * (int)(waveCounter / 30);

        SetSliderValue(Fame);

        //Animacion de recibir fama
        //Sonido de recibir fama

        if (Rank < 10)
        {
            if (Fame > targetFame)
            {
                RankUp();
            }
        }
    }

    private void RankUp()
    {
        Rank++;
        Fame = 0;
        //Animacion de pasar de rango
        //Sonido de pasar de rango
        TurnOnStar();
    }

    private void TurnOffStars()
    {
        foreach (Image star in starList)
        {
            star.gameObject.SetActive(false);
        }

    }

    private void TurnOnStar()
    {
        starList[Rank - 1].gameObject.SetActive(true);
    }

    private void FillStarList()
    {
        foreach (Image star in rankStarFolder.GetComponentsInChildren<Image>())
        {
            starList.Add(star);
        }
    }
    private void SetFameMaxValue()
    {
        targetFame = parameters.fameNeededBase + parameters.fameNeededMultiplier * (Rank + 1);
    }

    private void ResetSystem()
    {
        Fame = 0;
        Rank = 0;
        testingWave = 0;
        TurnOffStars();
        SetFameMaxValue();
        SetSliderValue(0);

        Debug.Log("Reseteo el sistema de Fama!");
    }

    private float GetNormalizedFame(int amount)
    {
        float aux = (float)amount / (float)targetFame;
        Debug.Log("Paso de " + amount + "/" + targetFame + " a " + aux + "%");
        return aux;
    }

    private void SetSliderValue(int amount)
    {
        fameBar.fillAmount = GetNormalizedFame(amount);

    }
}

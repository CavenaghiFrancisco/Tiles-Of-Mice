using System.Collections;
using System.Collections.Generic;
using TOM;
using UnityEngine;
using UnityEngine.UI;

public class FameManager : MonoBehaviour
{
    [SerializeField] private FameParameters parameters;
    [SerializeField] private GameObject rankStarFolder;
    [SerializeField] private Slider fameBar;
    public int Fame { private set; get; }
    public int Rank { private set; get; }

    private List<Image> starList = new List<Image>();

    private int testingWave = 0;
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
            fameBar.value = Fame;
            if (Rank < 10)
            {
                if (Fame > fameBar.maxValue)
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

        fameBar.value = Fame;

        //Animacion de recibir fama
        //Sonido de recibir fama

        if (Rank < 10)
        {
            if (Fame > fameBar.maxValue)
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
            fameBar.value = 0;
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
        fameBar.maxValue = parameters.fameNeededBase + parameters.fameNeededMultiplier * (Rank + 1);
    }

    private void ResetSystem()
    {
        Fame = 0;
        Rank = 0;
        testingWave = 0;
        TurnOffStars();
        SetFameMaxValue();
    }


}

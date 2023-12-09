using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using TOM;

public class FameManager : MonoBehaviour
{
    [SerializeField] private FameParameters parameters;
    [SerializeField] private GameObject rankStarFolder;
    [SerializeField] private Image fameBar;

    [SerializeField] private TMP_Text textTotalFame;
    [SerializeField] private TMP_Text textAditionalFame;
    [SerializeField] private CanvasGroup panelAditionalFame;

    public int Fame { private set; get; }

    private int lvlFame = 0; //cantidad total de fama de la wave

    private int actualWave = 0;

    private int totalFameGained = 0;//Sumatoria de fama del player

    private bool isSafeTime = true;
    private bool isfirstTime = true;

    private float timer = 0.0f;

    private void Awake()
    {
        //FillStarList();
        ResetSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetSystem();
        }
        if (!GameManager.IsPaused)
        {
            timer += Time.deltaTime;
            if (CheckSafeTime())
            {
                UpdateFameBar();
            }
        }
    }
    public void GetFame()
    {
        int prevFameAmount = totalFameGained;
        int gainAmount = (int)(lvlFame * fameBar.fillAmount);

        totalFameGained += gainAmount; // Eso da la cantidad exacta de puntos

        SetUpNextFameLevel();
        textTotalFame.text = totalFameGained.ToString();
        textAditionalFame.text = "+" + gainAmount.ToString();
        if (isfirstTime)
        {
            isfirstTime = false;
        }
        else
        {
            panelAditionalFame.alpha = 1;
        }

        Debug.Log("Fama> Antes tenia " + prevFameAmount + " y ahora tengo " + totalFameGained);
    }

    private void SetFameMaxValue()
    {
        lvlFame = (int)(parameters.baseFameObtained * actualWave * parameters.fameGivenMultiplier);
    }
    private void SetFameMaxValue(int forcedWave)
    {
        lvlFame = (int)(parameters.baseFameObtained * forcedWave * parameters.fameGivenMultiplier);
    }

    private void ResetSystem()
    {
        actualWave = 0;
        SetFameMaxValue();
        ResetFameBar();

        isSafeTime = true;
        totalFameGained = 0;

        timer = 0;

        Debug.Log("Reseteo el sistema de Fama!");
    }

    private bool CheckSafeTime()
    {
        if (!isSafeTime)
        {
            return true;
        }
        if (timer > parameters.levelSafeTime)
        {
            timer -= parameters.levelSafeTime;
            isSafeTime = false;
            Debug.Log("Empiezo a perder puntos!");
            panelAditionalFame.alpha = 0;
            return true;
        }
        return false;
    }

    private void UpdateFameBar()
    {
        fameBar.fillAmount = 1.0f - (timer / (parameters.levelLosingTime + parameters.levelSafeTime));
    }

    private void ResetFameBar()
    {
        fameBar.fillAmount = 1;
    }

    private void SetUpNextFameLevel()
    {
        ResetFameBar();

        timer = 0;
        isSafeTime = true;

        actualWave++;
        SetFameMaxValue();

        Debug.Log("Inicio del nivel " + actualWave + "!");
    }
}

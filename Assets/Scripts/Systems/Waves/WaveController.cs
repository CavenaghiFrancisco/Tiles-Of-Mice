using UnityEngine;

namespace TOM
{

    public class WaveController : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyController enemyController = null;
        [SerializeField] private GamePlayParameters parameters = null;

        [SerializeField] private FameManager fameManager = null;

        [SerializeField] private int startingWave = 0;

        [SerializeField] private bool isTestingBuild = false;
        [SerializeField] private TMPro.TextMeshProUGUI waveText;

        private WaveParameters actualWave;//Muestra la proxima wave
        private WaveParameters nextWave;//Muestra la que va a remplazar a la proxima wave

        private int differentWaveCount = 0;
        private int rotations = 0;
        private int waveCount = 0;

        private int wavesElapsed = 0;

        public static System.Action OnTestWaveLimitArrive;

        private void Awake()
        {
            actualWave = parameters.waveList[startingWave];
            nextWave = parameters.waveList[startingWave + 1];
            enemyController.OnAllEnemiesKilled += SetNextWave;
            PlayerPrefs.SetInt("Waves", 0);
        }

        private void OnDestroy()
        {
            enemyController.OnAllEnemiesKilled -= SetNextWave;
            PlayerPrefs.SetInt("Waves", wavesElapsed);
        }

        private void Start()
        {
            waveText.text = "Wave\n" + wavesElapsed.ToString();
            differentWaveCount = parameters.waveList.Count;
            rotations = 0;
            SetNextWave();
        }

        private void StartWave(WaveParameters wave)
        {
            //Animacion de inicio de wave
            //sfx de inicio de wave

            enemyController.TurnOnEnemiesOnLevel
            (
                wave.waveLevel + parameters.levelAugmentAmount * (rotations - 1),
                wave.CRAmount,
                wave.TRAmount,
                wave.enemyDelay
            );
            waveCount++;
            wavesElapsed++;
            waveText.text = "Wave\n" + wavesElapsed.ToString();
            Debug.Log("Empezando el nivel " + wavesElapsed + "!");
            Debug.Log("CR Spawneadas: " + wave.CRAmount);
            Debug.Log("TR Spawneadas: " + wave.TRAmount);
        }

        private void EndWave()
        {
            AkSoundEngine.PostEvent("Play_wave_over", gameObject);
            AkSoundEngine.PostEvent("Play_random_crowd", gameObject);
            AkSoundEngine.PostEvent("Play_random_crowd", gameObject);
            AkSoundEngine.PostEvent("Play_random_crowd", gameObject);
            fameManager.GetFame();
        }

        private void ForceWave(int wave)
        {
            enemyController.KillEnemies();
            StartWave(parameters.waveList[wave % differentWaveCount]);
        }

        private void SetNextWave()
        {
            EndWave();
            if (actualWave.waveID == 1)
            {
                rotations++;
            }

            StartWave(actualWave);
            actualWave = nextWave;
            if (isTestingBuild)
            {
                if (actualWave.waveID == 9)
                {
                    GameManager.PauseGame();
                    OnTestWaveLimitArrive?.Invoke();
                }
            }

            nextWave = parameters.waveList[actualWave.waveID % differentWaveCount];
        }


    }

}
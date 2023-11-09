using UnityEngine;

namespace TOM
{

    public class WaveController : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyController enemyController = null;
        [SerializeField] private GamePlayParameters parameters = null;

        [SerializeField] private int startingWave = 0;

        private WaveParameters actualWave;//Muestra la proxima wave
        private WaveParameters nextWave;//Muestra la que va a remplazar a la proxima wave

        private int differentWaveCount = 0;
        private int rotations = 0;
        private int waveCount = 0;

        private void Awake()
        {
            actualWave = parameters.waveList[startingWave];
            nextWave = parameters.waveList[startingWave + 1];
            enemyController.OnEnemyThreshold += SetNextWave;
        }

        private void OnDestroy()
        {
            enemyController.OnEnemyThreshold -= SetNextWave;
        }

        private void Start()
        {
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
                wave.waveLevel + parameters.levelAugmentAmount * (rotations-1),
                wave.enemyAmount,
                wave.enemyDelay,
                wave.waveThreshold
            );
            waveCount++;
        }

        private void EndWave()
        {
            //Dar fama
            //FameController.ReceiveExp(actualWaveID);//Estatica para no referenciarlo aca.
            //Posibles cambios:
            // - Agregarlo como serializado y llamar el metodo local.
            // - Hacer un Action estatico para llamarlo desde el fame controller.
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
            nextWave = parameters.waveList[actualWave.waveID % differentWaveCount];
        }


    }

}
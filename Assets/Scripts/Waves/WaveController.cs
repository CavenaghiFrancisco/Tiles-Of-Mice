using UnityEngine;

namespace TOM
{
    
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyController enemyController = null;
        [SerializeField] private GamePlayParameters parameters = null;

        [SerializeField] private int startingWave = 0;

        private WaveParameters actualWave;
        private WaveParameters nextWave;

        private int actualWaveID = 0;
        private int differentWaveCount = 0;
        private int rotations = 0;

        private void Awake()
        {
            actualWaveID = startingWave;
            actualWave = parameters.waveList[actualWaveID];
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

        private void StartWave()
        {
            enemyController.TurnOnEnemiesOnLevel
            (
                actualWave.waveLevel + parameters.levelAugmentAmount * rotations,
                actualWave.enemyAmount, 
                actualWave.enemyDelay, 
                actualWave.waveThreshold
            );
        }

        private void EndWave()
        {
            //Dar fama
            //FameController.ReceiveExp(actualWaveID);//Estatica para no referenciarlo aca.
            //Posibles cambios:
            //  - Agregarlo como serializado y llamar el metodo local.
            //  - Hacer un Action estatico para llamarlo desde el fame controller.
        }

        private void ForceWave(int wave)
        {
            enemyController.KillEnemies();
            actualWaveID = wave;
            StartWave();
        }

        private void SetNextWave()
        {
            EndWave();
            actualWaveID++;
            if (actualWaveID % differentWaveCount == 0)
            {
                rotations++;
            }
            actualWave = parameters.waveList[actualWaveID % differentWaveCount];
            StartWave();
        }


    }

}
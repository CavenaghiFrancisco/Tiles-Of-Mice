using UnityEngine;

namespace TOM
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyController enemyController = null;
        [SerializeField] private GamePlayParameters parameters = null;

        private void Start()
        {
            enemyController?.TurnOnLevel(parameters.waveList[0]);
        }
    }

}
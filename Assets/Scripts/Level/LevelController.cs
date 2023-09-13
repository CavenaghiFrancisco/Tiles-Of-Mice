using UnityEngine;

namespace TOM
{
    public enum Levels
    {
        Demo,
        Integration,
        CyberRoachOnly,
        GregorioOnly,
        SV23Only,
        CRandSV,
        CRandGregorio,
        SVandGregorio,
        AllEnemies
    }
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Enemy.EnemyController enemyController = null;
        [SerializeField] private Levels levelToPlay;

        private void Start()
        {
            enemyController?.TurnOnLevel((int)levelToPlay);
        }
    }

}
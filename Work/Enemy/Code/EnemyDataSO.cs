using UnityEngine;

namespace Enemies
{
    public enum EnemyStats
    {
        Health,
        Speed,
        AttackDamage,
        AttackSpeed,
        AttackRange
    }
    /// <summary>
    /// 100 ~ 199: 체력, 200 ~ 299: 속도, 300 ~ 399: 공격력, 400 ~ 499: 공격속도, 500 ~ 599: 공격범위
    /// </summary>
    public enum EnemyBuffReason
    {
        #region EnemyBuffReason
        Health = 100,
        Speed = 200,
        AttackSpeed = 400,
        AttackDamage = 300,
        AttackRange = 500,
        #endregion
        SlowerTower = 201,
        // 스킬, 버프 등등
        // 알잘딱
    }
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SO/EnemyData")]
    public class EnemyDataSO : ScriptableObject
    {
        public string EnemyName;
        public float Health;
        public float Speed;
        public int DropGold = 100;
        // AttackVFX
    }
}
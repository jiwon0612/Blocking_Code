using Enemies;

public class EnemyBuff
{
    public EnemyBuffReason Reason { get; }
    public float Value { get; }
    public float Duration { get; set; }

    public EnemyBuff(EnemyBuffReason reason, float value, float duration)
    {
        Reason = reason;
        Value = value;
        Duration = duration;
    }
}

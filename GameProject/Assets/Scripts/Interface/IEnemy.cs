/// <summary> 敵インターフェース </summary>
public interface IEnemy
{
    /// <summary> 被弾 </summary>
    /// <param name="power">攻撃力</param>
    void AddDamage(int power);
}

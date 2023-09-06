using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 監視カメラ </summary>
public class SurveillanceSenser : MonoBehaviour
{
    [Header("呼び寄せる警備員たち")] [SerializeField] List<EnemyBase> callEnemyList;
    /// <summary> 敵呼び寄せ </summary>
    /// <param name="player">侵入者</param>
    void CallEnemy(Transform player)
    {
        // 待機している警備員たちを呼び寄せる
        foreach(var enemy in callEnemyList)
        {
            if (enemy != null) enemy.SetFoundPlayer(true);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが近づいたら
        if (collision.tag == Dictionary.PLAYER_TAG) CallEnemy(collision.transform);
    }
}

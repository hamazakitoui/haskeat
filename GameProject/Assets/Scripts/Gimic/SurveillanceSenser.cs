using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 監視カメラ </summary>
public class SurveillanceSenser : MonoBehaviour
{
    // 呼び寄せる範囲
    [SerializeField] float sight = 5.0f;
    /// <summary> 敵呼び寄せ </summary>
    /// <param name="player">侵入者</param>
    void CallEnemy(Transform player)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Dictionary.ENEMY_TAG); // 全ての敵を検索
        foreach(var enemy in enemies)
        {
            // 全ての敵との距離を調べ一定距離以内にいるなら呼び寄せ
            if (Vector3.SqrMagnitude(enemy.transform.position - player.position) < sight * sight)
            {
                EnemyBase e = enemy.GetComponent<EnemyBase>(); // 敵クラス取得
                if (e != null) e.SetFoundPlayer(true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが近づいたら
        if (collision.tag == Dictionary.PLAYER_TAG) CallEnemy(collision.transform);
    }
}

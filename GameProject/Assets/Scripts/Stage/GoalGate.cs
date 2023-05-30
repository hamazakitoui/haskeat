using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゴールゲート </summary>
public class GoalGate : MonoBehaviour
{
    [SerializeField] StageManager stage; // ステージマネージャー
    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに当たったら
        if (collision.tag == Dictionary.PLAYER_TAG)
        {
            if (stage.IsRunAway) stage.GameClear(); // 逃亡状態ならゲームクリア
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        // プレイヤーに当たったら
        if (collision.tag == Dictionary.PLAYER_TAG)
        {
            if (stage.IsRunAway) stage.GameClear(); // 逃亡状態ならゲームクリア
        }
    }
}

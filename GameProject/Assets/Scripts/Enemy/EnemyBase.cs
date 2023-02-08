using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 敵基本クラス </summary>
public class EnemyBase : MonoBehaviour
{
    // 鈍足経過時間
    private float slowDelta;
    // 残りHP
    protected int nowHP;
    // 現在の移動速度
    protected float nowMoveSpeed;
    protected bool foundPlayer = false; // プレイヤー発見フラグ
    protected Rigidbody2D rigidbody2; // RigidBody2Dコンポーネント
    protected Animator animator; // アニメーターコンポーネント
    protected Transform playerPos; // プレイヤー位置
    protected ActionHandler handler; // 行動変数
    protected delegate void ActionHandler(); // 行動関数
    // 最大HP
    [SerializeField] private int maxHP = 1;
    // 移動速度、視野
    [SerializeField] protected float moveSpeed = 3.0f, sight = 5.0f;
    /// <summary> 鈍足関数 </summary>
    private void Slow()
    {
        if (nowMoveSpeed == moveSpeed) return; // 移動速度が変わっていないなら無視
        slowDelta -= Time.deltaTime;
        if (slowDelta <= 0) nowMoveSpeed = moveSpeed; // 移動速度を戻す
    }
    /// <summary> 初期処理 </summary>
    protected void StartSet()
    {
        nowHP = maxHP; // HP初期化
        nowMoveSpeed = moveSpeed; // 移動速度初期化
        // コンポーネント取得
        {
            rigidbody2 = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        // 行動追加
        {
            handler += Attack; // 攻撃追加
            handler += Slow; // 鈍足関数追加
        }
    }
    /// <summary> 基礎被弾 </summary>
    /// <param name="power">攻撃力</param>
    protected void BaseDamage(int power)
    {
        if (nowHP <= 0) return; // HPが0以下なら処理しない
        nowHP -= power; // ダメージ
        if (nowHP <= 0) Destroy(gameObject); // HPが0以下になったら死亡
    }
    /// <summary> 移動 </summary>
    protected virtual void Move()
    {

    }
    /// <summary> 攻撃 </summary>
    protected virtual void Attack()
    {

    }
    /// <summary> 移動速度変更 </summary>
    /// <param name="speed">新しい移動速度</param> <param name="time">変更時間</param>
    public void  SetMoveSpeed(float speed,float time)
    {
        nowMoveSpeed = speed; // 移動速度変更
        slowDelta = time; // 鈍足時間初期化
    }
    /// <summary> 移動停止 </summary>
    /// <param name="time">停止時間</param>
    public void MoveStop(float time)
    {
        nowMoveSpeed = 0; // 移動速度を0に
        slowDelta = time; // 停止時間初期化
    }
    /// <summary> 移動速度 </summary>
    public float GetMoveSpeed
    {
        get { return moveSpeed; }
    }
    /// <summary> プレイヤーセッター </summary>
    public Transform SetPlayer { set { playerPos = value; } }
}

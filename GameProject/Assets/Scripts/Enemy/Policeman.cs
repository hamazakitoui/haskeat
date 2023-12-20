using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 警官 </summary>
public class Policeman : EnemyBase, IEnemy
{
    int oldState = 0; // 前の状態
    float foundDelta = 0.0f, shotDelta = 0.0f, animDelta = 0.0f;
    // 見失いフラグ
    bool lostFlag = false;
    // アニメーションステート
    readonly string stateAnim = "State";
    Vector3 shotDirection; // 発射方向
    // 発見角度、見失う時間、発射間隔
    [SerializeField] float foundRad = 30.0f, lostSpan = 3.0f, shotSpan = 3.0f;
    [SerializeField] float rotSpan = 5.0f; // 回転間隔
    // 発射速度
    [SerializeField] float shotPower = 5.0f;
    [SerializeField] GameObject bullet; // 弾
    // Start is called before the first frame update
    void Start()
    {
        StartSet(); // 初期処理
        // 関数登録
        handler += PlayerFound;
        handler += AnimationRotate;
    }

    // Update is called once per frame
    void Update()
    {
        handler(); // 行動
    }
    /// <summary> プレイヤー発見 </summary>
    void PlayerFound()
    {
        // 発見可能なら
        if (!player.notfound)
        {
            Transform player = !decoyFlag ? this.player.transform : Decoy;
            Vector3 dir = player.position - transform.position; // プレイヤーとの方向
            Vector3 fd = Vector3.right; // 視認方向
            if (transform.localScale.x < 0) fd = Vector3.left; // 左向きなら左を向く
            else
            {
                // 索敵方向設定
                switch (animator.GetInteger(stateAnim))
                {
                    case 1:
                        fd = Vector3.down;
                        break;
                    case 2:
                        fd = Vector3.up;
                        break;
                    default:
                        break;
                }
            }
            // 視認範囲を計算
            float r = Mathf.Acos(Vector3.Dot(fd, dir.normalized)) * Mathf.Rad2Deg;
            bool temp = foundPlayer; // 一時保存
            shotDirection = fd; // 発射方向セット
            foundPlayer = r < foundRad && dir.magnitude < sight; // 発見かどうか更新
            if (temp && !foundPlayer)
            {
                foundPlayer = true;
                lostFlag = true;
            }
        }
        // 発見不可なら
        else
        {
            if (foundPlayer) lostFlag = true; // 発見状態なら見失う
        }
        // 見失い中なら
        if (lostFlag)
        {
            foundDelta += Time.deltaTime;
            if (foundDelta >= lostSpan)
            {
                foundPlayer = false;
                lostFlag = false;
                foundDelta = 0;
            }
        }
    }
    /// <summary> 攻撃 </summary>
    protected override void Attack()
    {
        if (!foundPlayer) return; // プレイヤーを見つけていないなら
        shotDelta += Time.deltaTime;
        // 一定時間ごとに弾を発射
        if (shotDelta > shotSpan)
        {
            // 弾を発射
            Rigidbody2D rb = Instantiate(bullet, transform.position, Quaternion.identity).
                GetComponent<Rigidbody2D>();
            rb.AddForce(shotDirection * shotPower, ForceMode2D.Impulse); // 発射
            shotDelta = 0.0f;
        }
    }
    /// <summary> 回転アニメーション </summary>
    void AnimationRotate()
    {
        if (foundPlayer) return; // プレイヤーを見つけてたら無視
        animDelta += Time.deltaTime;
        if (animDelta > rotSpan)
        {
            int state = animator.GetInteger(stateAnim); // アニメ状態
            // 状態によって番号変更
            switch (state)
            {
                // 左右向き
                case 0:
                    // 左向きなら
                    if (transform.localScale.x < 0)
                    {
                        Vector3 scale = transform.localScale; // 向き
                        scale.x = -scale.x;
                        transform.localScale = scale;
                        state = 2;
                    }
                    // 前向きなら
                    else state = 1;
                    break;
                // 下向き
                case 1:
                    // 向きを反対に
                    {
                        Vector3 scale = transform.localScale; // 向き
                        scale.x = -scale.x;
                        transform.localScale = scale;
                    }
                    oldState = state;
                    state = 0;
                    break;
                // 上向き
                case 2:
                    oldState = state;
                    state = 0;
                    break;
                default:
                    break;
            }
            animator.SetInteger(stateAnim, state); // アニメ変更
            animDelta = 0;
        }
    }
    /// <summary> 被弾 </summary>
    /// <param name="power">攻撃力</param>
    void IEnemy.AddDamage(int power)
    {
        BaseDamage(power); // 被弾
    }
}

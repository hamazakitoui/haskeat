using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary> 警備員 </summary>
public class SecurityGuard : EnemyBase, IEnemy
{
    int moveIndex = 0; // 移動番号
    float foundDelta = 0.0f;
    // 移動フラグ、見失いフラグ
    bool returnMove = false, lostFlag = false;
    NavMeshAgent agent;
    // 発見角度、見失う時間
    [SerializeField] float foundRad = 30.0f, lostSpan = 3.0f;
    [SerializeField] bool isAround = false; // 周回移動
    [SerializeField] Vector3[] movePoints; // 移動先リスト
    // Start is called before the first frame update
    void Start()
    {
        StartSet(); // 初期処理
        // ナビ設定
        {
            agent = GetComponent<NavMeshAgent>(); // ナビ取得
            agent.speed = nowMoveSpeed; // 追跡速度設定
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        // デバッグ用ビルド時削除してください
        playerPos = GameObject.FindGameObjectWithTag(Dictionary.PLAYER_TAG).transform;
        // 関数登録
        handler += PlayerFound;
        handler += Move;
        handler += AnimationChange;
    }

    // Update is called once per frame
    void Update()
    {
        handler(); // 行動
    }
    /// <summary> プレイヤー発見 </summary>
    void PlayerFound()
    {
        Vector3 dir = playerPos.position - transform.position; // プレイヤーとの方向
        Vector3 fd = Vector3.right; // 視認方向
        if (transform.localScale.x < 0) fd = Vector3.left; // 左向きなら左を向く
        else
        {
            // 移動速度
            Vector2 vel = new Vector2(Mathf.Abs(agent.velocity.x), Mathf.Abs(agent.velocity.y));
            // 上下移動時
            if (vel.y > vel.x) fd = agent.velocity.y > 0 ? Vector3.up : Vector3.down;
        }
        float r = Mathf.Acos(Vector3.Dot(fd, dir.normalized)) * Mathf.Rad2Deg; // 視認範囲を計算
        bool temp = foundPlayer; // 一時保存
        foundPlayer = r < foundRad && dir.magnitude < sight; // 発見かどうか更新
        if (temp && !foundPlayer)
        {
            foundPlayer = true;
            lostFlag = true;
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
    /// <summary> 移動 </summary>
    protected override void Move()
    {
        // プレイヤーを発見したら
        if (foundPlayer)
        {
            agent.destination = playerPos.position; // 移動先設定
            agent.speed = nowMoveSpeed; // 移動速度設定
        }
        // 通常周回
        else
        {
            // 周回移動じゃないなら
            if (!isAround)
            {
                int nextPoint = moveIndex + (returnMove ? -1 : 1); // 移動先
                agent.destination = movePoints[nextPoint]; // 移動先設定
                agent.speed = nowMoveSpeed; // 移動速度設定
                // 移動先まで近づいたら
                if (Vector2.SqrMagnitude(movePoints[nextPoint] - transform.position) <= 0.1f)
                {
                    moveIndex += returnMove ? -1 : 1; // 次のポイントに変更
                    // 移動方向変更
                    if (moveIndex >= movePoints.Length - 1 || moveIndex <= 0) returnMove = !returnMove;
                }
            }
            // 周回移動なら
            else
            {
                int nextPoint = moveIndex < movePoints.Length - 1 ? moveIndex + 1 : 0; // 移動先
                               agent.destination = movePoints[nextPoint]; // 移動先設定
                agent.speed = nowMoveSpeed; // 移動速度設定
                // 移動先まで近づいたら
                if (Vector2.SqrMagnitude(movePoints[nextPoint] - transform.position) <= 0.1f)
                {
                    moveIndex++; // 次のポイントに変更
                    // 移動方向変更
                    if (moveIndex >= movePoints.Length) moveIndex = 0;
                }
            }
        }
    }
    /// <summary> アニメ変更 </summary>
    void AnimationChange()
    {
        // 左右移動時
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.normalized.y))
        {
            // 右移動時
            if (agent.velocity.x > 0)
            {
                // 左向きの時
                if (transform.localScale.x < 0)
                {
                    Vector3 scale = transform.localScale; // 向き
                    scale.x = -scale.x;
                    transform.localScale = scale;
                }
                // 視界の角度設定
                sightRenderer.rectTransform.rotation = Quaternion.Euler(0, 0, foundRad / 2);
            }
            // 左移動時
            else
            {
                // 右向きの時
                if (transform.localScale.x > 0)
                {
                    Vector3 scale = transform.localScale; // 向き
                    scale.x = -scale.x;
                    transform.localScale = scale;
                }
                // 視界の角度設定
                sightRenderer.rectTransform.rotation = Quaternion.Euler
                    (0, 0, Dictionary.DEG_MAX / 2 + foundRad / 2);
            }
        }
        // 上下移動時
        else
        {
            // 向きが反転していたら
            if (transform.localScale.x < 0)
            {
                Vector3 scale = transform.localScale; // 向き
                scale.x = -scale.x;
                transform.localScale = scale;
            }
            // 視界の角度設定
            // 上移動時
            if (agent.velocity.y > 0) sightRenderer.rectTransform.rotation = Quaternion.Euler
                    (0, 0, Dictionary.DEG_MAX / 4 + foundRad / 2);
            // 下移動時
            else sightRenderer.rectTransform.rotation = Quaternion.Euler
                    (0, 0, Dictionary.DEG_MAX * 3 / 4 + foundRad / 2);
        }
        Vector2 velocity = agent.velocity.normalized; // 移動速度
        animator.SetBool(vxAnim, Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y)); // 横移動アニメ変更
        animator.SetFloat(vyAnim, agent.velocity.y); // 縦移動アニメ変更
    }
    /// <summary> 被弾 </summary>
    /// <param name="power">攻撃力</param>
    void IEnemy.AddDamage(int power)
    {
        BaseDamage(power); // 被弾
    }
}

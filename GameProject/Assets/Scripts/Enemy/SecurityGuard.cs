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
        // ナビ設定
        {
            agent = GetComponent<NavMeshAgent>(); // ナビ取得
            agent.speed = nowMoveSpeed; // 追跡速度設定
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.isStopped = false;
        }
        playerPos = GameObject.FindGameObjectWithTag(Dictionary.PLAYER_TAG).transform;
        // 関数登録
        handler += PlayerFound;
    }

    // Update is called once per frame
    void Update()
    {
        handler(); // 行動
    }
    void FixedUpdate()
    {
        Move(); // 移動
    }
    /// <summary> プレイヤー発見 </summary>
    void PlayerFound()
    {
        Vector3 dir = playerPos.position - transform.position; // プレイヤーとの方向
        // 視認範囲を計算
        float r = Mathf.Acos(Vector3.Dot
            (transform.localScale.x > 0 ? Vector3.right : Vector3.left, dir.normalized)) * Mathf.Rad2Deg;
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
        agent.isStopped = !foundPlayer; // 追跡設定更新
        // プレイヤーを発見したら
        if (foundPlayer)
        {
            //agent.SetDestination(playerPos.position); // プレイヤー位置に向かって移動
            //agent.nextPosition = transform.position;
            //agent.speed = nowMoveSpeed; // 移動速度設定
            Debug.Log("fffref");
        }
        // 通常周回
        else
        {
            // 周回移動じゃないなら
            if (!isAround)
            {
                int nextPoint = moveIndex + (returnMove ? -1 : 1); // 移動先
                // 移動先まで距離があるなら
                if (Vector2.SqrMagnitude(movePoints[nextPoint] - transform.position) > 0.1f)
                {
                    // 移動速度
                    Vector2 moveVec = Vector2.MoveTowards
                        (transform.position, movePoints[nextPoint], nowMoveSpeed * Time.deltaTime);
                    rigidbody2.MovePosition(moveVec); // 移動
                }
                // 移動先まで近づいたら
                else
                {
                    rigidbody2.MovePosition(movePoints[nextPoint]); // 移動
                    moveIndex += returnMove ? -1 : 1; // 次のポイントに変更
                    // 移動方向変更
                    if (moveIndex >= movePoints.Length - 1 || moveIndex <= 0) returnMove = !returnMove;
                }
            }
            // 周回移動なら
            else
            {
                int nextPoint = moveIndex < movePoints.Length - 1 ? moveIndex + 1 : 0; // 移動先
                // 移動先まで距離があるなら
                if (Vector2.SqrMagnitude(movePoints[nextPoint] - transform.position) > 0.1f)
                {
                    // 移動速度
                    Vector2 moveVec = Vector2.MoveTowards
                        (transform.position, movePoints[nextPoint], nowMoveSpeed * Time.deltaTime);
                    rigidbody2.MovePosition(moveVec); // 移動
                }
                // 移動先まで近づいたら
                else
                {
                    rigidbody2.MovePosition(movePoints[nextPoint]); // 移動
                    moveIndex++; // 次のポイントに変更
                    // 移動方向変更
                    if (moveIndex >= movePoints.Length) moveIndex = 0;
                }
            }
        }
    }
    /// <summary> 被弾 </summary>
    /// <param name="power">攻撃力</param>
    void IEnemy.AddDamage(int power)
    {
        BaseDamage(power); // 被弾
    }
}

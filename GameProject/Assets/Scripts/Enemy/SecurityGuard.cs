using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary> 警備員 </summary>
public class SecurityGuard : EnemyBase, IEnemy
{
    int moveIndex = 0; // 移動番号
    bool returnMove = false; // 移動フラグ
    NavMeshAgent agent;
    [SerializeField] float foundRad = 30.0f; // 発見角度
    [SerializeField] bool isAround = false; // 周回移動
    [SerializeField] Vector3[] movePoints; // 移動先リスト
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // ナビ取得
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
        foundPlayer = r < foundRad && dir.magnitude < sight; // 発見かどうか更新
    }
    /// <summary> 移動 </summary>
    protected override void Move()
    {
        // プレイヤーを発見したら
        if (foundPlayer)
        {

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
                        (transform.position, movePoints[nextPoint], moveSpeed * Time.deltaTime);
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

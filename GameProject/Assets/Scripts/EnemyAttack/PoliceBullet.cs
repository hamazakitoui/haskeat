using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceBullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーか壁に当たったら削除
        if (collision.tag == Dictionary.PLAYER_TAG || collision.tag == Dictionary.WALL_TAG)
            Destroy(gameObject);
    }
}

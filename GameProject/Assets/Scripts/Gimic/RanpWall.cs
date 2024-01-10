using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RanpWall : MonoBehaviour
{
    [Header("変更するタイル")] [SerializeField] Tilemap tilemap;
    [Header("ライト")] [SerializeField] Light ranp;
    /// <summary> ランプ変更 </summary>
    /// <param name="value">変更後の値</param>
    private void RanpChange(bool value)
    {
        ranp.gameObject.SetActive(!value); // ランプ表示変更
        Color color = tilemap.color; // タイルの色
        color.a = value ? 1.0f : 0.0f; // アルファ値を変更
        tilemap.color = color;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        decoy d = collision.GetComponent<decoy>(); // デコイ検索
        if (d != null)
        {
            RanpWall[] ranps = FindObjectsOfType<RanpWall>(); // 全てのランプ検索
            // ランプ消灯 & 他のランプ点灯
            foreach(var ranp in ranps)
            {
                ranp.RanpChange(ranp == this);
            }
        }
    }
}

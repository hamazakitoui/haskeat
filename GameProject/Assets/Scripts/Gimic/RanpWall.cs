using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RanpWall : MonoBehaviour
{
    // アニメーションステート名
    readonly string stateAnim = "State";
    Animator animator; // アニメーター
    [Header("変更するタイル")] [SerializeField] Tilemap[] tilemap;
    [Header("ライト")] [SerializeField] Light[] ranp;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // アニメーター取得
    }
    /// <summary> ランプ変更 </summary>
    /// <param name="value">変更後の値</param> <param name="colorkind">色</param>
    private void RanpChange(decoy.Colorkind colorkind)
    {
        if (colorkind == decoy.Colorkind.purple) return; // デコイなら無視
        Color lightColor = Color.white; // 変更後の色
        // インクの色で灯りを変更
        switch (colorkind)
        {
            // 赤色
            case decoy.Colorkind.red:
                lightColor = Color.red;
                break;
            // 青色
            case decoy.Colorkind.brue:
                lightColor = Color.blue;
                break;
            // 黄色
            case decoy.Colorkind.yellow:
                lightColor = Color.yellow;
                break;
            default:
                break;
        }
        // 灯りの色変更
        foreach (var r in ranp)
        {
            r.color = lightColor;
        }
        animator.SetInteger(stateAnim, (int)colorkind); // アニメ変更
        // タイル表示
        for (int t = 0; t < tilemap.Length; t++)
        {
            Color color = tilemap[t].color; // タイルの色
            color.a = t == (int)colorkind ? 1.0f : 0.0f; // アルファ値を変更
            tilemap[t].color = color;
        }
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
                ranp.RanpChange(d.state);
            }
        }
    }
}

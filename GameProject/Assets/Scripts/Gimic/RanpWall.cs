using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ListArray;

public class RanpWall : MonoBehaviour
{
    // アニメーションステート名
    readonly string stateAnim = "State";
    Animator animator; // アニメーター
    [Header("ライト変更時の時間")] [SerializeField] float changeTime = 1.0f;
    [Header("灯りの瞬間光度")] [SerializeField] float maxRanpRange = 100;
    [Header("変更するタイル")] [SerializeField] List<ListArrayTilemap> tilemap;
    [Header("表示変更する絵")] [SerializeField] List<ListArrayGameObject> pictures;
    [Header("自身の灯り")] [SerializeField] Light selfRanp;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // アニメーター取得
    }
    /// <summary> ランプ変更 </summary>
    /// <param name="colorkind">色</param> <param name="l">変更されたランプ</param>
    private void RanpChange(decoy.Colorkind colorkind, Light l)
    {
        if (colorkind == decoy.Colorkind.purple) return; // デコイなら無視
        StartCoroutine(RanpColorFade(colorkind, l)); // ランプの色を変更
        animator.SetInteger(stateAnim, (int)colorkind); // アニメ変更
    }
    /// <summary> ランプの色変更 </summary>
    /// <param name="colorkind">変更後のランプの色</param> <param name="l">変更されたランプ</param>
    /// <returns></returns>
    private IEnumerator RanpColorFade(decoy.Colorkind colorkind, Light l)
    {
        // 変更前の灯りの色、変更後の色
        Color oldColor = selfRanp.color, lightColor = Color.white;
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
        // 経過時間、元の灯りの大きさ
        float time = 0.0f, oldRange = selfRanp.range;
        // 色を変更
        while (time < changeTime)
        {
            Color addC = Color.Lerp(oldColor, lightColor, time / changeTime); // 変更する色
            selfRanp.color = addC;
            // 変更された灯りなら一瞬だけ大きくし、その後元に戻す
            if (selfRanp == l) selfRanp.range += time <= changeTime / 2 ?
                    (maxRanpRange - oldRange) / (changeTime / 2) :
                    (oldRange - maxRanpRange) / (changeTime / 2);
            time += Time.deltaTime;
            yield return 0;
        }
        selfRanp.color = lightColor; // 色変更
        if (selfRanp == l) selfRanp.range = oldRange; // 変更された灯りなら元に戻す
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        decoy d = collision.GetComponent<decoy>(); // デコイ検索
        Debug.Log($"デコイある? {d != null}");
        if (d != null)
        {
            RanpWall[] ranps = FindObjectsOfType<RanpWall>(); // 全てのランプ検索
            // ランプ消灯 & 他のランプ点灯
            foreach(var ranp in ranps)
            {
                ranp.RanpChange(d.state, selfRanp);
            }
            // タイル表示切り替え
            for (int t = 0; t < tilemap.Count; t++)
            {
                for(int u = 0; u < tilemap[t].Length; u++)
                {
                    Color color = tilemap[t][u].color; // タイルの色
                    color.a = t == (int)d.state ? 1.0f : 0.0f; // アルファ値を変更
                    tilemap[t][u].color = color;
                }
            }
            // 絵画表示切替
            for(int p = 0; p < pictures.Count; p++)
            {
                for(int q = 0; q < pictures[p].Length; q++)
                {
                    pictures[p][q].SetActive(p == (int)d.state);
                }
            }
        }
    }
}

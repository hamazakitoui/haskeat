using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 読み込みテキスト制御スクリプト </summary>
public class LoadingText : BaseMeshEffect
{
    int index = 0; // 現在動かす文字の番号
    // 文字動作用変数
    float wordDelta = 0;
    // 動かす文章
    string loadingText = null;
    // 文字の頂点数
    const int VERTEX_MAX = 6;
    // 文字動作間隔
    const float WORD_SPAN = 0.085f;
    Graphic wordGraphic = null; // 描画用変数
    // 文字を動かす移動量
    [SerializeField] float wordVelocity = 20;
    // Update is called once per frame
    void Update()
    {
        Move(); // アニメーション
    }
    /// <summary> 全体動作 </summary>
    void Move()
    {
        wordDelta += Time.deltaTime;
        if (wordDelta <= WORD_SPAN) return; // 一定時間経過するまで処理しない
        // Graphicクラスを取得していないなら取得
        if (wordGraphic == null) wordGraphic = base.GetComponent<Graphic>();
        wordGraphic.SetVerticesDirty(); // アニメ再生
        wordDelta = 0;
    }
    /// <summary> テキストのポリゴンの各頂点にアクセス </summary>
    /// <param name="vh">アクセス用クラス</param>
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return; // アクティブじゃないなら処理しない
        List<UIVertex> vertices = new List<UIVertex>(); // 各頂点リスト
        vh.GetUIVertexStream(vertices); // アクセス開始
        WordMove(ref vertices); // 文字を動かす
        vh.Clear(); // アクセスした情報を消す
        vh.AddUIVertexTriangleStream(vertices); // ポリゴンを三角形としてとらえ各頂点にアクセス
    }
    /// <summary> 文字を動かす </summary>
    /// <param name="vertices">各頂点リスト</param>
    void WordMove(ref List<UIVertex> vertices)
    {
        Vector3 dir = Vector3.up; // 移動方向
        dir.y *= wordVelocity; // 移動量を計算
        // 対応する文字の各頂点を動かす
        // c += VERTEXは頂点数計算で1文字単位で計算
        for(int c = 0; c < vertices.Count; c += VERTEX_MAX)
        {
            // 動かす文字の場合
            if (c == index * VERTEX_MAX)
            {
                // 各頂点の数だけ計算
                for(int i = 0; i < VERTEX_MAX; i++)
                {
                    UIVertex vertex = vertices[c + i]; // 動かす頂点情報
                    vertex.position += dir; // 移動量計算
                    vertices[c + i] = vertex; // 計算した値を反映させる
                }
                break;
            }
        }
        // 動かす文章が設定されていなければ取得する
        if (loadingText == null) loadingText = GetComponent<Text>().text;
        index = index < loadingText.Length - 1 ? index + 1 : 0; // 次に動かす文字番号を設定
    }
}

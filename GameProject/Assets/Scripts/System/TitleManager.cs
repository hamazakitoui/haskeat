using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    bool isLoad = false; // 開始フラグ
    // キーコード
    [SerializeField] KeyCode startKey;
    [SerializeField] GameObject PaintBall; // ペイントアニメーション
    [SerializeField] SpriteRenderer Renderer; // 絵
    [SerializeField] SceneObject GameScene; // ゲームシーン
    [SerializeField] Sprite[] pictures; // 絵の配列
    // Start is called before the first frame update
    void Start()
    {
        PaintBall.SetActive(false); // ペイント非表示
        Renderer.sprite = pictures[Random.Range(0, pictures.Length)]; // 絵を乱数で設定
    }

    // Update is called once per frame
    void Update()
    {
        GameStart(); // ゲーム開始
    }
    /// <summary> ゲーム開始 </summary>
    void GameStart()
    {
        if (isLoad) return; // 既に読み込んでいるなら
        // 開始キーが押されたら
        if (Input.GetKeyDown(startKey))
        {
            PaintBall.SetActive(true); // ペイント表示
            FadeSceneManager.Instance.LoadScene(GameScene); // シーン移動
            isLoad = true;
        }
    }
}

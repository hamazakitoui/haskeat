using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    bool isLoad = false; // 開始フラグ
    // キーコード
    [SerializeField] KeyCode startKey;
    [SerializeField] SceneObject GameScene; // ゲームシーン
    // Start is called before the first frame update
    void Start()
    {
        
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
            FadeSceneManager.Instance.LoadScene(GameScene); // シーン移動
            isLoad = true;
        }
    }
}

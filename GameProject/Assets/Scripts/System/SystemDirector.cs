using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> システムディレクター </summary>
public class SystemDirector : MonoBehaviour
{
    bool isLoad = false; // 読み込みフラグ
    // シーン読み込み時間
    [SerializeField] float loadIntervl = 2.0f;
    [SerializeField] SceneObject defaultScene; // デフォルトロードシーン
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary> シーン読み込み </summary>
    public void LoadScene()
    {
        if (isLoad) return; // 既に読み込み中なら処理しない
        FadeSceneManager.Instance.LoadScene(defaultScene, loadIntervl); // シーン読み込み
        isLoad = true;
    }
    /// <summary> シーン読み込み </summary>
    /// <param name="scene">読み込むシーン</param>
    public void LoadScene(SceneObject scene)
    {
        if (isLoad) return; // 既に読み込み中なら処理しない
        FadeSceneManager.Instance.LoadScene(scene, loadIntervl); // シーン読み込み
        isLoad = true;
    }
}

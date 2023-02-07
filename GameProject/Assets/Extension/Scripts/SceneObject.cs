using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Inspector用シーン代入変数クラス </summary>
[System.Serializable]
public class SceneObject
{
    // シーン名
    [SerializeField] private string sceneName;
    /// <summary> string型変換関数 </summary>
    /// <param name="scene">変換オブジェクト</param>
    public static implicit operator string(SceneObject scene) { return scene.sceneName; }
    /// <summary> string型変換コンストラクタ </summary>
    /// <param name="name">変換シーン名</param>
    public static implicit operator SceneObject(string name)
    {
        return new SceneObject() { sceneName = name };
    }
}

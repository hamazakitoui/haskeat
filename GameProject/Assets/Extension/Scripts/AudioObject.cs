using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Inspector用音楽ファイル代入クラス </summary>
[System.Serializable]
public class AudioObject
{
    [SerializeField] private string audioName; // 音楽名
    /// <summary> string型変換関数 </summary>
    /// <param name="audio">変換オブジェクト</param>
    public static implicit operator string(AudioObject audio) { return audio.audioName; }
    /// <summary> string型変換コンストラクタ </summary>
    /// <param name="name">変換音楽ファイル</param>
    public static implicit operator AudioObject(string name)
    {
        return new AudioObject() { audioName = name };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary> 音楽オブジェクトエディタ </summary>
[CustomPropertyDrawer(typeof(AudioObject))]
public class AudioObjectEditor : PropertyDrawer
{
    // プロパティ名
    private readonly string nameName = "audioName";
    // パス名
    private readonly string bgmPath = "BGM/", sePath = "SE/";
    private readonly string audioPath = "Assets/Extension/Resources/Audio/";
    // 拡張子配列
    private readonly string[] fileKindList = { ".mp3" };
    /// <summary> 音楽ファイル検索 </summary>
    /// <param name="name">ファイル名</param>
    /// <returns>検索された音楽ファイル</returns>
    private AudioClip GetAudioObject(string name)
    {
        // シーン名が空ならNullを返す
        if (string.IsNullOrEmpty(name)) return null;
        // ファイル検索
        for(int a = 0; a < fileKindList.Length; a++)
        {
            // BGMフォルダから検索
            AudioClip clip = AssetDatabase.LoadAssetAtPath
                (audioPath + bgmPath + name + fileKindList[a], typeof(AudioClip)) as AudioClip;
            // 見つからなければSEフォルダから検索
            if (clip == null) clip = AssetDatabase.LoadAssetAtPath
                    (audioPath + sePath + name + fileKindList[a], typeof(AudioClip)) as AudioClip;
            if (clip != null) return clip; // 検索されれば検索されたファイルを検索
        }
        // 発見されなければ警告
        Debug.Log($"{name}は使えません.Resourcesフォルダにこのファイルを追加してください");
        return null;
    }
    /// <summary> 描画 </summary>
    /// <param name="position">描画座標</param> <param name="property">描画プロパティ</param>
    /// <param name="label">表示ラベル</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty nameProp = property.FindPropertyRelative(nameName); // 音楽名プロパティ取得
        AudioClip audioObj = GetAudioObject(nameProp.stringValue); // 音楽オブジェクト
        // Inspector上のオブジェクトフィールド
        Object clip = EditorGUI.ObjectField(position, label, audioObj, typeof(AudioClip), false);
        if (clip == null) nameProp.stringValue = ""; // フィールドが無いなら名前プロパティの中身を空にする
        else
        {
            // オブジェクト名と名前プロパティの中身が違うなら
            if (clip.name != nameProp.stringValue)
            {
                AudioClip obj = GetAudioObject(clip.name); // オブジェクト検索
                // 検索し見つからなかったら警告
                if (obj == null) Debug.LogWarning
                        ($"{clip.name}は使えません.Resourcesフォルダにファイル追加してください");
                else nameProp.stringValue = clip.name; // 名前プロパティの中身をオブジェクト名にする
            }
        }
    }
}
#endif

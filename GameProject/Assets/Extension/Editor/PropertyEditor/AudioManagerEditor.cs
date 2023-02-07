using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary> Audio Manager エディタ拡張 </summary>
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    // 最小値、最大値
    readonly float MinValune = 0f, MaxValune = 1f;
    // プロパティ名
    readonly string BGM_VolName = "bgmVolume", SeVolName = "seVolume", TtfName = "TimeToFade";
    readonly string CfrName = "CrossFadeRatio", DebugName = "DebugMode";
    // BGM音量プロパティ、SE音量プロパティ、フェード時間プロパティ、ループ重複実行割合プロパティ
    // デバッグモードプロパティ
    SerializedProperty bgmVolProp, seVolProp, ttfProp, cfrProp, debugProp;
    // アイコン用テクスチャ
    Texture soundTex, timeTex, crossTex, debugTex;
    void OnEnable()
    {
        // 各プロパティ取得
        bgmVolProp = serializedObject.FindProperty(BGM_VolName);
        seVolProp = serializedObject.FindProperty(SeVolName);
        ttfProp = serializedObject.FindProperty(TtfName);
        cfrProp = serializedObject.FindProperty(CfrName);
        debugProp = serializedObject.FindProperty(DebugName);
        // テクスチャ取得
        soundTex = EditorLibrary.IconTextureLibrary.SoundTexture;
        timeTex = EditorLibrary.IconTextureLibrary.TimeTexture;
        crossTex = EditorLibrary.IconTextureLibrary.CrossRatioTexture;
        debugTex = EditorLibrary.IconTextureLibrary.DebugTexture;
    }
    public override void OnInspectorGUI()
    {
        AudioManager audioManager = target as AudioManager; // 参照するクラス
        serializedObject.Update(); // 最新状態に更新
        // 音量プロパティ表示
        SliderProperty(bgmVolProp, new GUIContent("BGM音量", soundTex));
        SliderProperty(seVolProp, new GUIContent("SE音量", soundTex));
        SliderProperty(cfrProp, new GUIContent("重複実行割合", crossTex)); // 重複実行割合プロパティ表示
        // フェード時間プロパティ表示
        ttfProp.floatValue = EditorGUILayout.
            FloatField(new GUIContent("フェード時間", timeTex), ttfProp.floatValue);
        // デバッグモード表示
        debugProp.boolValue = EditorGUILayout.Toggle
            (new GUIContent("デバッグ", debugTex), debugProp.boolValue);
        serializedObject.ApplyModifiedProperties(); // 変更内容を反映
    }
    /// <summary> プロパティ スライダー表示 </summary>
    /// <param name="property">表示プロパティ</param>
    /// <param name="lavel">表示ラベル</param>
    void SliderProperty(SerializedProperty property, GUIContent lavel)
    {
        EditorGUILayout.Slider(property, MinValune, MaxValune, lavel);
    }
}
#endif

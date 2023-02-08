using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary> 警備員エディタ </summary>
[CustomEditor(typeof(SecurityGuard))]
public class SgEditor : Editor
{
    // 選択番号
    int mpIndex = 0;
    // プロパティ名
    readonly string mpName = "movePoints";
    // プロパティ
    SerializedProperty mpProp;
    void OnEnable()
    {
        mpProp = serializedObject.FindProperty(mpName);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update(); // 更新
        MovePointSet(); // 移動先設定
        serializedObject.ApplyModifiedProperties(); // 反映
    }
    /// <summary> 移動先設定 </summary>
    void MovePointSet()
    {
        if (mpProp.arraySize <= 0) return; // 要素が1つもないなら処理しない
        // 配列番号取得
        mpIndex = EditorGUILayout.IntSlider
            (new GUIContent("移動先番号"), mpIndex, 0, mpProp.arraySize - 1);
        SerializedProperty property = mpProp.GetArrayElementAtIndex(mpIndex); // 要素取得
        // オブジェクト代入
        Transform pos = EditorGUILayout.ObjectField
            (new GUIContent("移動先代入"), null, typeof(Transform), true) as Transform;
        if (pos != null) property.vector3Value = pos.position; // 位置座標代入
    }
}
#endif

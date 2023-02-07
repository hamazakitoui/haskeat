using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary> シーンオブジェクトエディター </summary>
[CustomPropertyDrawer(typeof(SceneObject))]
public class SceneObjectEditor : PropertyDrawer
{
    // プロパティ名
    private readonly string nameName = "sceneName";
    /// <summary> シーン検索 </summary>
    /// <param name="name">シーン名</param>
    /// <returns></returns>
    private SceneAsset GetSceneObject(string name)
    {
        // シーン名が空ならNullを返す
        if (string.IsNullOrEmpty(name)) return null;
        // ビルド設定に登録されているシーンの数だけ処理
        for(int s = 0; s < EditorBuildSettings.scenes.Length; s++)
        {
            EditorBuildSettingsScene scene = EditorBuildSettings.scenes[s];
            // 登録番号が負の値じゃないならAsset内のパスから検索して返す
            if (scene.path.IndexOf(name) != -1)
                return AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)) as SceneAsset;
        }
        Debug.Log($"{name}は使えません.BuildSettingにこのシーンを追加してください");
        return null;
    }
    /// <summary> 描画 </summary>
    /// <param name="position">描画座標</param>
    /// <param name="property">描画プロパティ</param>
    /// <param name="label">表示ラベル</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // シーン名プロパティ
        SerializedProperty nameProp = property.FindPropertyRelative(nameName);
        SceneAsset sceneObj = GetSceneObject(nameProp.stringValue); // シーンオブジェクト
        // Inspector上のオブジェクトフィールド
        Object newScene = EditorGUI.ObjectField(position, label, sceneObj, typeof(SceneAsset), false);
        // フィールドが無いなら名前プロパティの中身を空にする
        if (newScene == null) nameProp.stringValue = "";
        else
        {
            // オブジェクト名と名前プロパティの中身が違うなら
            if (newScene.name != nameProp.stringValue)
            {
                // オブジェクトを検索する
                var obj = GetSceneObject(newScene.name);
                // 検索し見つからなかったら警告
                if (obj == null) Debug.LogWarning
                        ($"{newScene.name}は使えません.プロジェクトのBuildSettingに追加してください");
                // 名前プロパティの中身をオブジェクト名にする
                else nameProp.stringValue = newScene.name;
            }
        }
    }
}
#endif

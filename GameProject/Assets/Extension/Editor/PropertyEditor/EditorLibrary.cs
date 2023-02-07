using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
namespace UnityEditor
{
    /// <summary> エディタ拡張ライブラリ </summary>
    public struct EditorLibrary
    {
        // フォルダ名
        private static readonly string assets = "Assets", resources = "Resources";
        // 多次元配列プロパティ名
        private static readonly string storage = "storage";
        // 配列String型テキストボックスサイズ
        private const float ARRAY_TEXT_WIDTH = 250, ARRAY_TEXT_HEIGHT = 30;
        /// <summary> Resourcesフォルダ生成 </summary>
        [MenuItem("Assets/Create/Resources Folder", priority = 20)]
        private static void CreateResourcesFolder()
        {
            AssetDatabase.CreateFolder(assets, "Resources");
        }
        /// <summary> Scriptsフォルダ生成 </summary>
        [MenuItem("Assets/Create/Scripts Folder", priority = 21)]
        private static void CreateScriptsFolder()
        {
            AssetDatabase.CreateFolder(assets, "Scripts");
        }
        /// <summary> Spritesフォルダ生成 </summary>
        [MenuItem("Assets/Create/Sprites Folder", priority = 22)]
        private static void CreateSpritesFolder()
        {
            AssetDatabase.CreateFolder(assets, "Sprites");
        }
        /// <summary> Resourcesフォルダ以下のパスを取得 </summary>
        [MenuItem("Assets/Copy Resources Path", priority = 19)]
        private static void CopyResourcesPath()
        {
            // パスを取得
            int id = Selection.activeInstanceID; // パスを探したいオブジェクトのID取得
            string path = AssetDatabase.GetAssetPath(id); // IDからパスを取得
            string[] folderPath = path.Split('/'); // フォルダーごとに分類
            int index = -1; // ResourcesFolderのパス番号
            // Resourcesフォルダを検索
            for(int f = 0; f < folderPath.Length; f++)
            {
                // Resourcesフォルダを発見したら検索終了
                if (folderPath[f] == resources)
                {
                    index = f;
                    break;
                }
            }
            if (index < 0) return; // Resourcesフォルダーが見つからなかったら処理しない
            string result = ""; // コピーするパス
            // パスを連結
            for (int p = index + 1; p < folderPath.Length; p++)
            {
                string folder = null; // フォルダ名
                // 最後の拡張子はパスに含まない
                if (p == folderPath.Length - 1)
                {
                    string[] end = folderPath[p].Split('.');
                    folder = end[0];
                }
                // それ以外は / を追加して連結
                else folder = folderPath[p] + "/";
                result += folder;
            }
            // パスをコピー
            GUIUtility.systemCopyBuffer = result;
        }
        /// <summary> Resourcesフォルダ生成可能判定 </summary>
        /// <returns></returns>
        [MenuItem("Assets/Create/Resources Folder", validate = true)]
        private static bool CanCreateResources()
        {
            return !AssetDatabase.IsValidFolder("Assets/Resources");
        }
        /// <summary> Scriptsフォルダ生成可能判定 </summary>
        /// <returns></returns>
        [MenuItem("Assets/Create/Scripts Folder", validate = true)]
        private static bool CanCreateScripts()
        {
            return !AssetDatabase.IsValidFolder("Assets/Scripts");
        }
        /// <summary> Spritesフォルダ生成可能判定 </summary>
        /// <returns></returns>
        [MenuItem("Assets/Create/Sprites Folder", validate = true)]
        private static bool CanCreateSprites()
        {
            return !AssetDatabase.IsValidFolder("Assets/Sprites");
        }
        /// <summary> 配列要素数変更 </summary>
        /// <param name="array">配列プロパティ</param>
        /// <param name="lavel">表示ラベル</param>
        public static void ArrayLengthChange(SerializedProperty array, GUIContent label)
        {
            int size = array.arraySize; // 現在の要素数取得
            size = EditorGUILayout.IntField(label, size); // 入力フィールド表示
            // 要素数が変更されたなら反映させて更新
            if (size != array.arraySize)
            {
                array.arraySize = size;
                array.serializedObject.ApplyModifiedProperties();
                array.serializedObject.Update();
            }
        }
        /// <summary> List表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="type">配列の型</param>
        /// <param name="label">表示ラベル</param>
        public static void DrawList(SerializedProperty array, System.Type type, GUIContent label)
        {
            using(new EditorGUI.IndentLevelScope(1))
            {
                // 配列が存在しないなら無しと表示
                if (array == null || array.arraySize <= 0) EditorGUILayout.LabelField("NULL");
                // 要素を1つずつ表示
                for(int i = 0; i < array.arraySize; i++)
                {
                    SerializedProperty property = array.GetArrayElementAtIndex(i); // 要素取得
                    // 表示
                    using(new EditorGUILayout.HorizontalScope())
                    {
                        // Int型配列の場合
                        if (type == typeof(int))
                            property.intValue = EditorGUILayout.IntField
                                (new GUIContent($"{label.text} {i}", label.image), property.intValue);
                        // Float型配列の場合
                        else if (type == typeof(float))
                            property.floatValue = EditorGUILayout.FloatField
                                (new GUIContent($"{label.text} {i}", label.image), property.floatValue);
                        // String型配列の場合
                        else if (type == typeof(string))
                        {
                            EditorGUILayout.LabelField(new GUIContent($"{label.text} {i}", label.image));
                            property.stringValue = EditorGUILayout.TextArea(property.stringValue,
                                GUILayout.Width(ARRAY_TEXT_WIDTH), GUILayout.Height(ARRAY_TEXT_HEIGHT));
                        }
                        // Vector2型配列の場合
                        else if (type == typeof(Vector2))
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                Transform pos = null; // 位置代入用Transform
                                // Transform取得
                                pos = EditorGUILayout.ObjectField
                                    (label, pos, typeof(Transform), true) as Transform;
                                // 何か代入されたら
                                if (pos != null) property.vector2Value = pos.position;
                                // プロパティ表示
                                property.vector2Value = EditorGUILayout.Vector2Field
                                    (new GUIContent($"{label.text} {i}", label.image),
                                    property.vector2Value);
                            }
                        }
                        // Vector3型配列の場合
                        else if (type == typeof(Vector3))
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                Transform pos = null; // 位置代入用Transform
                                // Transform取得
                                pos = EditorGUILayout.ObjectField
                                    (label, pos, typeof(Transform), true) as Transform;
                                // 何か代入されたら
                                if (pos != null) property.vector3Value = pos.position;
                                // プロパティ表示
                                property.vector3Value = EditorGUILayout.Vector3Field
                                    (new GUIContent($"{label.text} {i}", label.image),
                                    property.vector3Value);
                            }
                        }
                        // オブジェクト系配列の場合
                        else EditorGUILayout.ObjectField
                                (property, type, new GUIContent($"{label.text} {i}", label.image));
                    }
                }
            }
        }
        /// <summary> Listをスライダー形式で表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="type">配列の型</param>
        /// <param name="label">表示ラベル</param> <param name="index">要素番号</param>
        /// <param name="indexLabel">要素番号表示ラベル</param>
        public static void DrawList(SerializedProperty array, System.Type type, GUIContent label,
            ref int index, GUIContent indexLabel)
        {
            // 配列が存在しないなら表示しない
            if (array == null || array.arraySize <= 0) return;
            // 要素番号取得
            index = EditorGUILayout.IntSlider(indexLabel, index, 0, array.arraySize - 1);
            // 選択した要素番号の要素取得
            SerializedProperty selectedProp = array.GetArrayElementAtIndex(index);
            // 要素表示
            // Int型配列の場合
            if (type == typeof(int))
                selectedProp.intValue = EditorGUILayout.IntField
                    (new GUIContent($"{label.text} {index}", label.image), selectedProp.intValue);
            // Float型配列の場合
            else if (type == typeof(float))
                selectedProp.floatValue = EditorGUILayout.FloatField
                    (new GUIContent($"{label.text} {index}", label.image), selectedProp.floatValue);
            // String型配列の場合
            else if (type == typeof(string))
            {
                EditorGUILayout.LabelField(new GUIContent($"{label.text} {index}", label.image));
                selectedProp.stringValue = EditorGUILayout.TextArea(selectedProp.stringValue,
                    GUILayout.Width(ARRAY_TEXT_WIDTH), GUILayout.Height(ARRAY_TEXT_HEIGHT));
            }
            // Vector2型配列の場合
            else if (type == typeof(Vector2))
            {
                Transform pos = null; // 位置代入用Transform
                // Transform取得
                pos = EditorGUILayout.ObjectField
                    (label, pos, typeof(Transform), true) as Transform;
                // 何か代入されたら
                if (pos != null) selectedProp.vector2Value = pos.position;
                // プロパティ表示
                selectedProp.vector2Value = EditorGUILayout.Vector2Field
                    (new GUIContent($"{label.text} {index}", label.image), selectedProp.vector2Value);
            }
            // Vector3型配列の場合
            else if (type == typeof(Vector3))
            {
                Transform pos = null; // 位置代入用Transform
                // Transform取得
                pos = EditorGUILayout.ObjectField
                    (label, pos, typeof(Transform), true) as Transform;
                // 何か代入されたら
                if (pos != null) selectedProp.vector3Value = pos.position;
                // プロパティ表示
                selectedProp.vector3Value = EditorGUILayout.Vector3Field
                    (new GUIContent($"{label.text} {index}", label.image), selectedProp.vector3Value);
            }
            // オブジェクト系配列の場合
            else EditorGUILayout.ObjectField
                    (selectedProp, type, new GUIContent($"{label.text} {index}", label.image));
        }
        /// <summary> Int型Listの要素をスライダー形式で表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="min">要素の最小値</param>
        /// <param name="max">要素の最大値</param> <param name="label">表示ラベル</param>
        public static void DrawIntSliderList(SerializedProperty array, int min, int max, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                // 配列が存在しないなら無しと表示
                if (array == null || array.arraySize <= 0)
                {
                    EditorGUILayout.LabelField("NULL");
                    return;
                }
                // 要素を1つずつ表示
                for (int i = 0; i < array.arraySize; i++)
                {
                    SerializedProperty property = array.GetArrayElementAtIndex(i); // 要素取得
                    // 表示
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        property.intValue = EditorGUILayout.IntSlider
                            (new GUIContent($"{label.text} {i}", label.image), property.intValue,
                            min, max);
                    }
                }
            }
        }
        /// <summary> Int型Listとその要素をスライダー形式で表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="label">表示ラベル</param>
        /// <param name="index">要素番号</param> <param name="min">要素の最小値</param>
        /// <param name="max">要素の最大値</param> <param name="indexLabel">要素番号表示ラベル</param>
        public static void DrawIntSliderList(SerializedProperty array, GUIContent label, ref int index,
            int min, int max, GUIContent indexLabel)
        {
            // 配列が存在しないなら表示しない
            if (array == null || array.arraySize <= 0) return;
            // 要素番号取得
            index = EditorGUILayout.IntSlider(indexLabel, index, 0, array.arraySize - 1);
            // 選択した要素番号の要素取得
            SerializedProperty selectedProp = array.GetArrayElementAtIndex(index);
            // 要素表示
            selectedProp.intValue = EditorGUILayout.IntSlider
                (new GUIContent($"{label.text} {index}", label.image), selectedProp.intValue, min, max);
        }
        /// <summary> Float型Listの要素をスライダー形式で表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="min">要素の最小値</param>
        /// <param name="max">要素の最大値</param> <param name="label">表示ラベル</param>
        public static void DrawFloatSliderList(SerializedProperty array, float min, float max,
            GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                // 配列が存在しないなら無しと表示
                if (array == null || array.arraySize <= 0)
                {
                    EditorGUILayout.LabelField("NULL");
                }
                // 要素を1つずつ表示
                for (int i = 0; i < array.arraySize; i++)
                {
                    SerializedProperty property = array.GetArrayElementAtIndex(i); // 要素取得
                    // 表示
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        property.floatValue = EditorGUILayout.Slider
                            (new GUIContent($"{label.text} {i}", label.image), property.floatValue,
                            min, max);
                    }
                }
            }
        }
        /// <summary> Float型Listとその要素をスライダー形式で表示 </summary>
        /// <param name="array">List プロパティ</param> <param name="label">表示ラベル</param>
        /// <param name="index">要素番号</param> <param name="min">要素の最小値</param>
        /// <param name="max">要素の最大値</param> <param name="indexLabel"></param>
        public static void DrawFloatSliderList(SerializedProperty array, GUIContent label, ref int index,
            float min, float max, GUIContent indexLabel)
        {
            // 配列が存在しないなら表示しない
            if (array == null || array.arraySize <= 0) return;
            // 要素番号取得
            index = EditorGUILayout.IntSlider(indexLabel, index, 0, array.arraySize - 1);
            // 選択した要素番号の要素取得
            SerializedProperty selectedProp = array.GetArrayElementAtIndex(index);
            // 要素表示
            selectedProp.floatValue = EditorGUILayout.Slider
                (new GUIContent($"{label.text} {index}", label.image), selectedProp.floatValue, min, max);
        }
        /// <summary> 2次元配列やジャグ配列表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="type">配列の型</param>
        /// <param name="label">親要素表示ラベル</param> <param name="childLabel">子要素表示ラベル</param>
        public static void Draw2D_List(SerializedProperty array, System.Type type, GUIContent label,
            GUIContent childLabel)
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                // 配列が存在しないなら無しと表示
                if (array == null || array.arraySize <= 0)
                {
                    EditorGUILayout.LabelField("NULL");
                    return;
                }
                // 要素を1つずつ表示
                for (int i = 0; i < array.arraySize; i++)
                {
                    // 要素取得
                    SerializedProperty selectedProp = array.GetArrayElementAtIndex(i).
                        FindPropertyRelative(storage);
                    // 表示
                    ArrayLengthChange(selectedProp, label);   // 子要素の要素数変更
                    DrawList(selectedProp, type, childLabel); // 子要素のList表示
                }
            }
        }
        /// <summary> 2次元配列やジャグ配列表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="type">配列の型</param>
        /// <param name="label">表示ラベル</param> <param name="index">要素番号</param>
        /// <param name="childLabel">子要素表示ラベル</param>
        public static void Draw2D_List(SerializedProperty array, System.Type type, GUIContent label,
            ref int index, GUIContent childLabel)
        {
            // 配列が存在しないなら表示しない
            if (array == null || array.arraySize <= 0) return;
            // 要素番号取得
            index = EditorGUILayout.IntSlider(label, index, 0, array.arraySize - 1);
            // 選択した要素番号の要素取得
            SerializedProperty selectedProp = array.GetArrayElementAtIndex(index).
                FindPropertyRelative(storage);
            // 子要素表示
            ArrayLengthChange(selectedProp, childLabel);   // 子要素の要素数変更
            DrawList(selectedProp, type, childLabel); // 子要素のList表示
        }
        /// <summary> 2次元配列やジャグ配列表示 </summary>
        /// <param name="array">Listプロパティ</param> <param name="type">配列の型</param>
        /// <param name="label">表示ラベル</param> <param name="index">要素番号</param>
        /// <param name="childLabel">子要素表示ラベル</param>
        /// <param name="childIndex">子要素の要素番号</param>
        /// <param name="childIndexLabel">子要素の要素表示ラベル</param>
        public static void Draw2D_List(SerializedProperty array, System.Type type, GUIContent label,
            ref int index, GUIContent childLabel, ref int childIndex, GUIContent childIndexLabel)
        {
            // 配列が存在しないなら表示しない
            if (array == null || array.arraySize <= 0) return;
            // 要素番号取得
            index = EditorGUILayout.IntSlider(label, index, 0, array.arraySize - 1);
            // 選択した要素番号の要素取得
            SerializedProperty selectedProp = array.GetArrayElementAtIndex(index).
                FindPropertyRelative(storage);
            // 子要素表示
            ArrayLengthChange(selectedProp, childLabel); // 子要素の要素数変更
            DrawList(selectedProp, type, childLabel, ref childIndex, childIndexLabel); // 子要素のList表示
        }
        /// <summary> テクスチャライブラリ </summary>
        public struct IconTextureLibrary
        {
            /// <summary> テクスチャパス </summary>
            private static string TexturePath { get { return "Editor/Texture/"; } }
            /// <summary> ポイント追加テクスチャ </summary>
            public static Texture AddPointTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "AddPoint"); }
            }
            /// <summary> 重複テクスチャ </summary>
            public static Texture CrossRatioTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "Cross"); }
            }
            /// <summary> デバッグテクスチャ </summary>
            public static Texture DebugTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "Debug"); }
            }
            /// <summary> ラベルテクスチャ </summary>
            public static Texture LabelTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "Label"); }
            }
            /// <summary> サイズ変更テクスチャ </summary>
            public static Texture MoveSizeTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "MoveSize"); }
            }
            /// <summary> 筆テクスチャ </summary>
            public static Texture PaintTexure
            {
                get { return Resources.Load<Texture>(TexturePath + "Color"); }
            }
            /// <summary> 音テクスチャ </summary>
            public static Texture SoundTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "Sound"); }
            }
            /// <summary> 画像テクスチャ </summary>
            public static Texture TextureTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "Texture"); }
            }
            /// <summary> 時計テクスチャ </summary>
            public static Texture TimeTexture
            {
                get { return Resources.Load<Texture>(TexturePath + "Time"); }
            }
        }
    }
}
#endif

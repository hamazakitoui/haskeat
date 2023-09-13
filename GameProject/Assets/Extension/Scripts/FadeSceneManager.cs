using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> シーン読み込みマネージャー </summary>
public class FadeSceneManager : MonoBehaviour
{
    private float fadeAlpha = 0; // フェード中のアルファ値
    private bool isFade = false; // シーンロードフラグ // デフォルト読み込み時間
    // 遷移係数、フェード最小アルファ値、フェード最大アルファ値
    private readonly float fadeIntervalMul = 0.5f, fadeMinAlpha = 0, fadeMaxAlpha = 1;
    // オブジェクト名
    private readonly string backName = "Back", loadName = "LoadText", iconName = "LoadIcon";
    // フェード用UI背景
    private Image BackGround;
    // 読み込みテキスト、読み込みアイコン
    private GameObject LoadObj, Icon;
    [SerializeField] private float loadTime = 1f;
    [SerializeField] private bool DebugMode = false; // デバッグモード
    [SerializeField] private Color fadeColor = Color.black; // フェード用背景色
    // 読み込み時表示用UI
    [SerializeField] private GameObject FadeCanvas;
    // デバッグシーンリスト
    [SerializeField] private List<SceneObject> DebugScene;
    private static FadeSceneManager instance; // インスタンス保存用変数
    /// <summary> シングルトンインスタンス </summary>
    public static FadeSceneManager Instance
    {
        get
        {
            // インスタンスがないなら
            if (instance == null)
            {
                instance = FindObjectOfType<FadeSceneManager>();
                // インスタンスが見つからないならエラー処理
                if (instance == null) Debug.LogError($"{typeof(FadeSceneManager)}が見つかりません!");
            }
            return instance;
        }
    }
    void Awake()
    {
        if (this != Instance)
        {
            Destroy(FadeCanvas); // フェード用のUIを削除
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (this != Instance) return; // 念のため
        DontDestroyOnLoad(this.FadeCanvas); // 暗幕が勝手に駆除されないよう処理
        // 背景取得
        {
            BackGround = this.FadeCanvas.transform.Find(backName).GetComponent<Image>();
            BackGround.color = fadeColor;
        }
        // テキスト取得
        {
            Transform load = this.FadeCanvas.transform.Find(loadName); // テキスト検索
            // テキストがあれば
            if (load != null)
            {
                LoadObj =load.gameObject;
                LoadObj.SetActive(false);
            }
        }
        // アイコン取得
        {
            Transform icon = this.FadeCanvas.transform.Find(iconName); // アイコン検索
            // アイコンがあれば
            if (icon != null)
            {
                Icon = icon.gameObject;
                Icon.SetActive(false);
            }
        }
        this.FadeCanvas.SetActive(false); // フェード用のUIを非表示
    }
    void OnGUI()
    {
        if (!this.DebugMode) return; // デバッグモードじゃないなら処理しない
        // デバッグシーンがない場合
        if (DebugScene.Count <= 0)
        {
            GUI.Box(NoSceneBox, new GUIContent("Fade Debug")); // 枠表示
            GUI.Label(NoSceneLabel, new GUIContent("デバッグ用シーン無し")); // ラベル表示
            return;
        }
        GUI.Box(SceneBox, new GUIContent("Fade Debug")); // 枠表示
        // 現在のシーン表示
        GUI.Label(SceneLabel, new GUIContent($"現在のシーン : {SceneManager.GetActiveScene().name}"));
        for(int s = 0; s < DebugScene.Count; s++)
        {
            // ボタンが押されたらシーンを読み込み
            if (GUI.Button(ButtonRect(s), new GUIContent("Load")))
                LoadScene(DebugScene[s]);
            GUI.Label(SceneButtonLabel(s), DebugScene[s]); // シーン名表示
        }
    }
    #region デバッグ用関数
    /// <summary> ボタン用枠 </summary>
    /// <param name="count">ボタン番号</param>
    /// <returns></returns>
    Rect ButtonRect(int count)
    {
        return new Rect(20, 80 + count * 25, 100, 25);
    }
    /// <summary> ボタン用シーンラベル </summary>
    /// <param name="count">シーン番号</param>
    /// <returns></returns>
    Rect SceneButtonLabel(int count)
    {
        return new Rect(130, 80 + count * 25, 1000, 25);
    }
    /// <summary> デバッグ用シーン枠 </summary>
    Rect SceneBox { get { return new Rect(10, 25, 350, 30 * DebugScene.Count + 50); } }
    /// <summary> デバッグ用シーンラベル </summary>
    Rect SceneLabel { get { return new Rect(20, 50, 300, 25); } }
    /// <summary> デバッグ用シーンが無い場合 </summary>
    Rect NoSceneBox { get { return new Rect(10, 25, 200, 50); } }
    /// <summary> デバッグ用シーンが無い場合 </summary>
    Rect NoSceneLabel { get { return new Rect(20, 50, 200, 25); } }
    #endregion
    /// <summary> シーン読み込み </summary>
    /// <param name="scene">読み込むシーン</param>
    public void LoadScene(string scene)
    {
        LoadScene(scene, loadTime); // 通常時間で読み込み
    }
    /// <summary> シーン読み込み </summary>
    /// <param name="scene">読み込むシーン</param> <param name="interval">読み込み時間</param>
    /// <param name="wait">読み込み待機時間</param>
    public void LoadScene(string scene, float interval, float wait = 1.0f)
    {
        if (isFade) return; // 既に読み込んでいるなら処理しない
        StartCoroutine(LoadSceneFading(scene, interval, wait)); // シーン遷移開始
    }
    /// <summary> シーン読み込み 読み込み画面ループ </summary>
    /// <param name="scene">読み込みシーン</param>
    public void CloseScene(string scene)
    {
        CloseScene(scene, loadTime); // 通常時間で読み込み
    }
    /// <summary> シーン読み込み 読み込み画面ループ </summary>
    /// <param name="scene">読み込みシーン</param> <param name="interval">読み込み時間</param>
    /// <param name="wait">読み込み待機時間</param>
    public void CloseScene(string scene, float interval, float wait = 1.0f)
    {
        if (isFade) return; // 既に読み込んでいるなら処理しない
        StartCoroutine(CloseSceneFadig(scene, interval, wait)); // シーン遷移開始
    }
    /// <summary> 読み込み画面非表示 </summary>
    public void OpenScene()
    {
        OpenScene(loadTime); // 通常時間で読み込み画面を非表示にする
    }
    /// <summary> 読み込み画面非表示 </summary>
    /// <param name="interval">非表示までの時間</param>
    public void OpenScene(float interval)
    {
        if (!isFade) return; // 読み込んでいないなら処理しない
        StartCoroutine(OpenSceneFading(interval)); // 読み込み画面非表示開始
    }
    /// <summary> シーン遷移コルーチン </summary>
    /// <param name="scene">遷移シーン</param> <param name="interval">読み込み時間</param>
    /// <param name="wait">読み込み待機時間</param>
    /// <returns></returns>
    private IEnumerator LoadSceneFading(string scene, float interval, float wait)
    {
        isFade = true; // 読み込み開始
        FadeCanvas.SetActive(true); // フェード用UI表示
        BackGround.transform.SetAsLastSibling(); // フェード用Canvasを最後に表示
        float orgCmrFov = Camera.main.fieldOfView; // 元の画角
        float time = 0; // 遷移用変数
        //AudioManager.Instance.StopBGM(); // BGM停止
        // 読み込み画面を少しずつ出す
        // 読み込み時間の一定割合の時間で表示
        while (time <= interval * fadeIntervalMul)
        {
            // アルファ値に現在の経過時間の遷移時間の割合をアルファ値の最小値から最大値の間に収める
            fadeAlpha = Mathf.Lerp(fadeMinAlpha, fadeMaxAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // フェード用背景UIの色
            color.a = fadeAlpha; // アルファ値変更
            BackGround.color = color; // 背景自体の色に反映
            // 画角を小さくする
            Camera.main.fieldOfView -= orgCmrFov / (interval * fadeIntervalMul) * Time.deltaTime;
            time += Time.deltaTime;
            yield return 0;
        }
        // テキスト関係
        if (LoadObj != null)
        {
            LoadObj.SetActive(true); // 読み込みテキスト表示
            LoadObj.transform.SetAsLastSibling(); // 読み込みテキストを最後に表示
        }
        // アイコン関係
        if (Icon != null)
        {
            Icon.SetActive(true); // アイコン表示
            Icon.transform.SetAsLastSibling(); // 読み込みアイコンを最後に表示
        }
        // 読み込み時間の一定割合の時間一時停止
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(scene); // シーン切り替え
        time = 0; // 経過時間リセット
        if (LoadObj != null) LoadObj.SetActive(false); // 読込テキスト非表示
        if (Icon != null) Icon.SetActive(false); // アイコン非表示
        // 読み込み画面を少しずつ透明に
        // 読み込み時間の一定割合の時間で非表示
        while (time <= interval * fadeIntervalMul)
        {
            // アルファ値に現在の経過時間の遷移時間の割合をアルファ値の最小値から最大値の間に収める
            fadeAlpha = Mathf.Lerp(fadeMaxAlpha, fadeMinAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // フェード用背景UIの色
            color.a = fadeAlpha; // アルファ値変更
            BackGround.color = color; // 背景自体の色に反映
            time += Time.deltaTime;
            yield return 0;
        }
        FadeCanvas.SetActive(false); // フェード用UI非表示
        isFade = false; // 読み込み終了
    }
    /// <summary> シーン遷移読み込みまでのコルーチン </summary>
    /// <param name="scene">読み込みシーン</param> <param name="interval">読み込み時間</param>
    /// <param name="wait">読み込み待機時間</param>
    /// <returns></returns>
    private IEnumerator CloseSceneFadig(string scene, float interval, float wait)
    {
        isFade = true; // 読み込み開始
        FadeCanvas.SetActive(true); // フェード用UI表示
        BackGround.transform.SetAsLastSibling(); // フェード用Canvasを最後に表示
        float time = 0; // 遷移用変数
        AudioManager.Instance.StopBGM(); // BGM停止
        // 読み込み画面を少しずつ出す
        while (time <= interval)
        {
            // アルファ値に現在の経過時間の遷移時間の割合をアルファ値の最小値から最大値の間に収める
            fadeAlpha = Mathf.Lerp(fadeMinAlpha, fadeMaxAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // フェード用背景UIの色
            color.a = fadeAlpha; // アルファ値変更
            BackGround.color = color; // 背景自体の色に反映
            time += Time.deltaTime;
            yield return 0;
        }
        // テキスト関係
        if (LoadObj != null)
        {
            LoadObj.SetActive(true); // 読み込みテキスト表示
            LoadObj.transform.SetAsLastSibling(); // 読み込みテキストを最後に表示
        }
        // アイコン関係
        if (Icon != null)
        {
            Icon.SetActive(true); // アイコン表示
            Icon.transform.SetAsLastSibling(); // 読み込みアイコンを最後に表示
        }
        // 読み込み時間の一定割合の時間一時停止
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(scene); // シーン切り替え
    }
    /// <summary> 読み込み画面非表示コルーチン </summary>
    /// <param name="interval">非表示までの時間</param>
    /// <returns></returns>
    private IEnumerator OpenSceneFading(float interval)
    {
        float time = 0; // 経過時間
        if (LoadObj != null) LoadObj.SetActive(false); // 読込テキスト非表示
        if (Icon != null) Icon.SetActive(false); // アイコン非表示
        // 読み込み画面を少しずつ透明に
        while (time <= interval * fadeIntervalMul)
        {
            // アルファ値に現在の経過時間の遷移時間の割合をアルファ値の最小値から最大値の間に収める
            fadeAlpha = Mathf.Lerp(fadeMaxAlpha, fadeMinAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // フェード用背景UIの色
            color.a = fadeAlpha; // アルファ値変更
            BackGround.color = color; // 背景自体の色に反映
            time += Time.deltaTime;
            yield return 0;
        }
        FadeCanvas.SetActive(false); // フェード用UI非表示
        isFade = false; // 読み込み終了
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゲーム内のBGM、SEの制御用スクリプト </summary>
public class AudioManager : MonoBehaviour
{
    // BGM用Audio数
    private const int BGM_AUDIO_MAX = 2;
    // 最大BGM重複実行割合
    private const float MAX_CROSS_FADE_RATIO = 1f;
    // BGM、SE用パス
    private readonly string BGM_Path = "Audio/BGM", SePath = "Audio/SE";
    // デバッグ用ラベル
    private readonly string DebugLavel = "Audio Debug";
    // コルーチン中断用コルーチン
    private IEnumerator BgmFadeInCoroutine, BgmFadeOutCoroutine;
    // デバッグ用GUIの全体の形及びサイズ
    private Rect GUI_ErrorBox = new Rect(10, 10, 200, 75);
    // デバッグ用GUI_Lavelの形及びサイズ
    private Rect GUI_BGM_ErrorLavel = new Rect(15, 35, 200, 20);
    private Rect GUI_SE_ErrorLavel = new Rect(15, 50, 200, 20);
    // BGM再生用Audio、SE再生用Audio
    private List<AudioSource> BgmAudio = null, SeAudio = null;
    // 再生可能BGM、SEリスト
    private Dictionary<string, AudioClip> BgmClipDict = null, SeClipDict = null;
    // BGM再生音量、SE再生音量
    [SerializeField] private float bgmVolume = 1f, seVolume = 1f;
    // BGMフェード時間、BGMフェード重複実行割合
    [SerializeField] private float TimeToFade = 1f, CrossFadeRatio = 1f;
    // デバッグ
    [SerializeField] private bool DebugMode = false;
    private static AudioManager instance; // インスタンス保存用変数
    /// <summary> シングルトン </summary>
    public static AudioManager Instance
    {
        get
        {
            // インスタンスが無かったら
            if (instance == null)
            {
                // このスクリプトのオブジェクトを検索
                instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
                // 検索してもなかったらエラーメッセージ
                if (instance == null) Debug.LogError($"{typeof(AudioManager)} is nothing");
            }
            return instance;
        }
    }
    void Awake()
    {
        // シングルトン用処理
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        // BGM用のAudioを2つ用意
        {
            BgmAudio = new List<AudioSource>(); // BGM Audioリスト初期化
            for (int a = 0; a < BGM_AUDIO_MAX; a++)
            {
                BgmAudio.Add(this.gameObject.AddComponent<AudioSource>());
            }
        }
        SeAudio = new List<AudioSource>(); // SEソースのリストを用意
        // BGM関連初期化
        {
            // BGM初期設定
            foreach (AudioSource audio in BgmAudio)
            {
                audio.playOnAwake = false; // 初期再生防止
                audio.volume = 0; // 音量をミュートに
                audio.loop = true; // ループ再生するよう設定
            }
            // BGMを検索
            BgmClipDict = new Dictionary<string, AudioClip>();
            foreach (AudioClip bgm in Resources.LoadAll<AudioClip>(BGM_Path))
            {
                BgmClipDict.Add(bgm.name, bgm);
            }
        }
        // SEを検索
        {
            SeClipDict = new Dictionary<string, AudioClip>();
            foreach (AudioClip se in Resources.LoadAll<AudioClip>(SePath))
            {
                SeClipDict.Add(se.name, se);
            }
        }
        // 有効なAudioListenerが無いなら生成
        if (FindObjectsOfType(typeof(AudioListener)).All(o => !(o as AudioListener).enabled))
            this.gameObject.AddComponent<AudioListener>();
    }
    void OnGUI()
    {
        if (!this.DebugMode) return; // デバッグモードじゃないなら処理しない
        // 再生するファイルが無いなら
        if (this.BgmClipDict.Count + this.SeClipDict.Count == 0)
        {
            GUI.Box(GUI_ErrorBox, DebugLavel); // Box型のデバッグ表示
            // BGMファイルが無い場合
            if (this.BgmClipDict.Count == 0) GUI.Label(GUI_BGM_ErrorLavel, "BGM clipが見つかりません");
            // SEファイルが無い場合
            if (this.SeClipDict.Count == 0) GUI.Label(GUI_SE_ErrorLavel, "SE clipが見つかりません");
            return;
        }
        // 枠表示
        GUI.Box(GUI_Box, DebugLavel);
        int count = 0; // デバッグ用カウンター
        // BGM用ラベル
        GUI.Label(GUI_Lavel(ref count), $"BGM音量 : {bgmVolume.ToString("F2")}");
        GUI.Label(GUI_Lavel(ref count), $"フェード時間 : {TimeToFade.ToString("F2")}");
        GUI.Label(GUI_Lavel(ref count), $"重複実行割合 : {CrossFadeRatio.ToString("F2")}");
        // SE用ラベル
        GUI.Label(GUI_Lavel(ref count), $"SE音量 : {seVolume.ToString("F2")}");
        count = 0;
        // 再生ボタン
        // BGM
        foreach(AudioClip bgm in BgmClipDict.Values)
        {
            if (GUI.Button(GUI_PlayButton(count), "BGM再生")) PlayBGM(bgm.name);
            // 再生するBGMが一致しているか判定
            bool current = (CurrentBGM_Audio != null && CurrentBGM_Audio.clip == BgmClipDict[bgm.name]);
            // 表示ラベル
            string head = current ? "X" : " ";
            string lavel = string.Format($"[{head}] {bgm.name}");
            GUI.Label(GUI_PlayLavel(count), lavel);
            count++;
        }
        // SE
        foreach(AudioClip se in SeClipDict.Values)
        {
            if (GUI.Button(GUI_PlayButton(count), "SE再生")) PlaySE(se.name);
            // 表示ラベル
            string txt = string.Format($"{se.name}");
            GUI.Label(GUI_PlayLavel(count), txt);
            count++;
        }
        // 停止ボタン
        // BGM
        if (GUI.Button(GUI_StopButton(ref count), "BGM停止")) StopBGM_Immediately();
        // SE
        if (GUI.Button(GUI_StopButton(ref count), "SE停止")) StopSE_Immediately();
        // 現在再生しているSE数
        int playSeSource = SeAudio.Count(s => s.isPlaying);
        string playlavel = string.Format($"SEを{playSeSource}個再生しています"); // 表示ラベル
        // 再生しているSEが1つなら
        if (playSeSource == 1) GUI.Label(GUI_SE_PlayingSource(count), playlavel);
        // SEを複数再生しているなら
        else if (playSeSource > 1) GUI.Label(GUI_SE_PlayingSource(count), playlavel);
    }
    /// <summary> BGMフェードイン中断 </summary>
    private void StopBgmFadeIn()
    {
        // フェードイン中なら停止
        if (BgmFadeInCoroutine != null) StopCoroutine(BgmFadeInCoroutine);
        BgmFadeInCoroutine = null;
    }
    /// <summary>/ BGMフェードアウト中断 </summary>
    private void StopBgmFadeOut()
    {
        // フェードアウト中なら停止
        if (BgmFadeOutCoroutine != null) StopCoroutine(BgmFadeOutCoroutine);
        BgmFadeOutCoroutine = null;
    }
    /// <summary> デバッグ用ラベル </summary>
    /// <param name="count">デバッグカウンター</param>
    /// <returns></returns>
    private Rect GUI_Lavel(ref int count)
    {
        return new Rect(20, 30 + count++ * 20, 180, 20);
    }
    /// <summary> デバッグ用再生ボタンGUI </summary>
    /// <param name="count">デバッグカウンター</param>
    /// <returns></returns>
    private Rect GUI_PlayButton(int count)
    {
        return new Rect(20, 120 + count * 25, 70, 20);
    }
    /// <summary> デバッグ用再生ラベル </summary>
    /// <param name="count">デバッグカウンター</param>
    /// <returns></returns>
    private Rect GUI_PlayLavel(int count)
    {
        return new Rect(100, 120 + count * 25, 1000, 20);
    }
    /// <summary> デバッグ用停止ボタンGUI </summary>
    /// <param name="count">デバッグカウンター</param>
    /// <returns></returns>
    private Rect GUI_StopButton(ref int count)
    {
        return new Rect(20, 120 + count++ * 25, 180, 20);
    }
    /// <summary> 現在再生しているSEのデバッグ用GUIラベル </summary>
    /// <param name="count">デバッグカウンター</param>
    /// <returns></returns>
    private Rect GUI_SE_PlayingSource(int count)
    {
        return new Rect(20, 120 + count * 25, 1000, 20);
    }
    /// <summary> デバッグ用GUIの枠 </summary>
    private Rect GUI_Box
    {
        get { return new Rect(10, 10, 200, 170 + 25 * (BgmClipDict.Count + SeClipDict.Count)); }
    }
    /// <summary> BGMフェードイン再生開始 </summary>
    /// <param name="bgm">再生するAudioSource</param> <param name="timeToFade">フェードイン完了時間</param>
    /// <param name="fromVolume">初期音量</param> <param name="toVolume">フェードイン完了時音量</param>
    /// <param name="delay">フェードイン完了までの待ち時間</param>
    /// <returns></returns>
    private IEnumerator BgmFadeInPlay
        (AudioSource bgm, float timeToFade, float fromVolume, float toVolume, float delay)
    {
        // 待ち時間待機
        if (delay > 0) yield return new WaitForSeconds(delay);
        float startTime = Time.time; // 開始時間
        bgm.Play();
        while (true)
        {
            float spent = Time.time - startTime; // 現在までのかかった時間
            // 完了時間を過ぎたら音量を調節して終了
            if (spent > timeToFade)
            {
                bgm.volume = toVolume;
                BgmFadeInCoroutine = null;
                break;
            }
            float rate = spent / timeToFade; // 音量調節割合
            // 再生音量
            float volume = Mathf.Lerp(fromVolume, toVolume, rate);
            bgm.volume = volume;
            yield return null;
        }
    }
    /// <summary> BGMフェードアウト && 停止 </summary>
    /// <param name="bgm">停止するAudioSource</param> <param name="timeToFade">フェードアウト完了時間</param>
    /// <param name="fromVolume">初期音量</param> <param name="toVolume">フェードアウト完了時音量</param>
    /// <returns></returns>
    private IEnumerator BgmFadeOutStop(
        AudioSource bgm, float timeToFade, float fromVolume, float toVolume)
    {
        float startTime = Time.time; // 開始時間
        while (true)
        {
            float spent = Time.time - startTime; // 現在までのかかった時間
            // 完了時間を過ぎたら音量を調節して終了
            if (spent > timeToFade)
            {
                bgm.volume = toVolume;
                bgm.Stop();
                BgmFadeOutCoroutine = null;
                break;
            }
            float rate = spent / timeToFade; // 音量調節割合
            // 再生音量
            float volume = Mathf.Lerp(fromVolume, toVolume, rate);
            bgm.volume = volume;
            yield return null;
        }
    }
    /// <summary> BGM再生 </summary>
    /// <param name="bgmName">BGM名</param>
    public void PlayBGM(string bgmName)
    {
        PlayBGM(bgmName, bgmVolume); // BGM再生
    }
    /// <summary> BGM再生 </summary>
    /// <param name="bgmName">BHM名</param> <param name="volume">再生音量</param>
    public void PlayBGM(string bgmName, float volume)
    {
        // BGMが存在しなかったら警告
        if (!BgmClipDict.ContainsKey(bgmName))
        {
            Debug.LogError(string.Format($"{bgmName}が見つかりません"));
            return;
        }
        // 既に指定されたBGMが再生していたら処理しない
        if ((CurrentBGM_Audio != null) && (CurrentBGM_Audio.clip == BgmClipDict[bgmName])) return;
        // フェード中なら停止
        StopBgmFadeOut();
        StopBgmFadeIn();
        // 現在再生中のBGMを停止
        StopBGM();
        // 待ち時間のフェード重複実行割合に応じた数値を待ち時間にする
        float fadeRatio = MAX_CROSS_FADE_RATIO - CrossFadeRatio;
        float startDelay = TimeToFade * fadeRatio;
        // BGM再生開始
        CurrentBGM_Audio = SubBGM_AudioSource; // 再生するSourceを変更
        CurrentBGM_Audio.clip = BgmClipDict[bgmName];
        BgmFadeInCoroutine = BgmFadeInPlay
            (CurrentBGM_Audio, TimeToFade, CurrentBGM_Audio.volume, volume, startDelay);
        StartCoroutine(BgmFadeInCoroutine);
    }
    /// <summary> SE再生 </summary>
    /// <param name="seName">SE名</param>
    /// <returns>再生しているソース</returns>
    public AudioSource PlaySE(string seName)
    {
        return PlaySE(seName, seVolume, 1f, false);
    }
    /// <summary> SE再生 </summary>
    /// <param name="seName">SE名</param> <param name="loop">ループ再生</param>
    /// <returns>再生しているソース</returns>
    public AudioSource PlaySE(string seName,bool loop)
    {
        return PlaySE(seName, seVolume, 1f, loop);
    }
    /// <summary> SE再生 </summary>
    /// <param name="seName">SE名</param> <param name="volume">再生音量</param>
    /// <param name="pitch">再生ピッチ</param> <param name="loop">ループ再生</param>
    /// <returns>再生しているソース</returns>
    public AudioSource PlaySE(string seName, float volume, float pitch, bool loop)
    {
        // SEが見つからなかったら警告
        if (!SeClipDict.ContainsKey(seName))
        {
            Debug.LogError(string.Format($"{seName}が見つかりません"));
            return null;
        }
        // 音量を0から1以内に収める
        volume = Mathf.Clamp01(volume);
        // 使用していないAudioSourceを探す
        AudioSource source = SeAudio.FirstOrDefault(s => !s.isPlaying);
        // 空いているAudioSourceが無いなら
        if (source == null)
        {
            source = this.gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false; // 初期再生防止
            SeAudio.Add(source);
        }
        source.clip = SeClipDict[seName]; // 再生するSEをセット
        source.loop = loop; // ループ再生設定
        source.volume = volume; // 音量設定
        source.pitch = pitch; // ピッチ設定
        source.Play(); // SE再生
        return source; // 再生しているソースを返す
    }
    /// <summary> 全てのBGM、SEを緊急停止 </summary>
    public void StopImmediately()
    {
        StopBGM_Immediately();
        StopSE_Immediately();
    }
    /// <summary> BGM停止 </summary>
    public void StopBGM()
    {
        // 現在再生中のAudioSourceが無かったら処理しない
        if (CurrentBGM_Audio == null) return;
        BgmFadeOutCoroutine = BgmFadeOutStop(CurrentBGM_Audio, TimeToFade, CurrentBGM_Audio.volume, 0f);
        StartCoroutine(BgmFadeOutCoroutine);
    }
    /// <summary> BGM緊急停止 </summary>
    public void StopBGM_Immediately()
    {
        // フェードコルーチンを削除
        BgmFadeInCoroutine = null;
        BgmFadeOutCoroutine = null;
        foreach(AudioSource source in BgmAudio)
        {
            source.Stop();
        }
        CurrentBGM_Audio = null;
    }
    /// <summary> 全てのSE緊急停止 </summary>
    public void StopSE_Immediately()
    {
        foreach(var source in SeAudio)
        {
            source.Stop();
        }
    }
    /// <summary> 特定のSEを緊急停止 </summary>
    /// <param name="seName">停止させるSE名</param>
    public void StopSE_Immediately(string seName)
    {
        foreach(var source in SeAudio)
        {
            // 再生しているSE名と一致しなら停止
            if (source.clip.name == seName)
            {
                source.Stop();
                break;
            }
        }
    }
    /// <summary> BGM、SEを全て一時停止 </summary>
    public void Pause()
    {
        PauseBGM(); // BGMを全て一時停止
        PauseSE();  // SEを全て一時停止
    }
    /// <summary> BGM一時停止 </summary>
    public void PauseBGM()
    {
        // 現在再生中のAudioSourceが無かったら処理しない
        if (CurrentBGM_Audio == null) return;
        CurrentBGM_Audio.Pause(); //BGM停止
    }
    /// <summary> 全てのSE一時停止 </summary>
    public void PauseSE()
    {
        // 全てのSE停止
        foreach(var source in SeAudio)
        {
            source.Pause();
        }
    }
    /// <summary> 特定のSEを一時停止 </summary>
    /// <param name="seName">SE名</param>
    public void PauseSE(string seName)
    {
        // 全SEソースを検索
        foreach(var source in SeAudio)
        {
            // SE名と一致していたら一時停止
            if (source.clip.name == seName)
            {
                source.Pause();
                break;
            }
        }
    }
    /// <summary> 全てのBGM、SE再開 </summary>
    public void Resume()
    {
        ResumeBGM(); // BGMを全て再開
        ResumeSE();  // SEを全て再開
    }

    /// <summary> 全てのBGM再開 </summary>
    public void ResumeBGM()
    {
        // 現在再生中のAudioSourceが無かったら処理しない
        if (CurrentBGM_Audio == null) return;
        CurrentBGM_Audio.UnPause(); // BGM再開
    }
    /// <summary> 全てのSE再開 </summary>
    public void ResumeSE()
    {
        // 全てのSEを再開
        foreach(var source in SeAudio)
        {
            source.UnPause();
        }
    }
    /// <summary> 現在再生中のAudioSource </summary>
    public AudioSource CurrentBGM_Audio { get; private set; } = null;
    /// <summary> 再生待機中のAudioSource </summary>
    public AudioSource SubBGM_AudioSource
    {
        get
        {
            // 現在再生中以外のAudioSourceを返す
            if (this.BgmAudio == null) return null;
            foreach(AudioSource source in this.BgmAudio)
            {
                if (source != this.CurrentBGM_Audio) return source;
            }
            return null;
        }
    }
}

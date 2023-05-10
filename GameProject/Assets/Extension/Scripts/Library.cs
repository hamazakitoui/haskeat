using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> メソッドライブラリ </summary>
public struct Library
{
    // 一時停止対象物理演算回転速度、一時停止対象アニメーション速度
    private static float[] pause_rb2Aglvels = null, pauseAnimationTimes = null;
    // 一時停止対象の物理演算速度
    private static Vector2[] pause_rb2velocities = null;
    // 一時停止対象スクリプト
    private static MonoBehaviour[] pauseBehaviours = null;
    // 一時停止対象アニメーションコンポーネント
    private static Animator[] pauseAnimators = null;
    // 一時対象物理演算コンポーネント
    private static Rigidbody2D[] pause_rb2 = null;
    /// <summary> 文字出力コルーチン </summary>
    /// <param name="message">表示文章</param> <param name="span">出力間隔</param>
    /// <param name="text">出力テキスト</param>
    /// <returns></returns>
    private static IEnumerator PrintMessage(string message, float span, Text text)
    {
        IsPrintMessage = true;
        int count = 0; // 現在出力している文字数
        // 1文字ずつ表示
        while (count < message.Length)
        {
            string msg = message.Substring(0, count++); // 表示する文章
            text.text = msg;
            yield return new WaitForSeconds(span); // 一定時間待機
        }
        IsPrintMessage = false;
    }
    /// <summary> 一時停止 </summary>
    public static void Pause2D()
    {
        // 一時停止するスクリプトが既に存在するなら処理しない
        if (pauseBehaviours != null) return;
        // 停止対象のスクリプトを停止
        pauseBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach(var com in pauseBehaviours)
        {
            com.enabled = false;
        }
        // 停止対象のアニメーション停止
        pauseAnimators = GameObject.FindObjectsOfType<Animator>();
        pauseAnimationTimes = new float[pauseAnimators.Length];
        for(int a = 0; a < pauseAnimators.Length; a++)
        {
            pauseAnimationTimes[a] = pauseAnimators[a].speed;
            pauseAnimators[a].speed = 0;
        }
        // 停止対象の物理演算コンポーネント停止
        pause_rb2 = GameObject.FindObjectsOfType<Rigidbody2D>();
        pause_rb2velocities = new Vector2[pause_rb2.Length];
        pause_rb2Aglvels = new float[pause_rb2.Length];
        for(int r = 0; r < pause_rb2.Length; r++)
        {
            // 対象の物理演算コンポーネントが眠っていないなら移動速度と回転速度を保存してから停止
            if (!pause_rb2[r].IsSleeping())
            {
                pause_rb2velocities[r] = pause_rb2[r].velocity;
                pause_rb2Aglvels[r] = pause_rb2[r].angularVelocity;
                pause_rb2[r].Sleep();
            }
        }
    }
    public static void Pause2D(MonoBehaviour[] notPauseBehaviour)
    {
        // 一時停止するスクリプトが既に存在するなら処理しない
        if (pauseBehaviours != null) return;
        // 停止対象のスクリプトを停止
        pauseBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach (var com in pauseBehaviours)
        {
            bool pause = true; // 一時停止フラグ
            // 除外対象か判別して停止か判断
            foreach(var notcom in notPauseBehaviour)
            {
                if (com == notcom)
                {
                    pause = false;
                    break;
                }
            }
            if (pause) com.enabled = false;
        }
        // 停止対象のアニメーション停止
        pauseAnimators = GameObject.FindObjectsOfType<Animator>();
        pauseAnimationTimes = new float[pauseAnimators.Length];
        for (int a = 0; a < pauseAnimators.Length; a++)
        {
            bool pause = true; // 一時停止フラグ
            // 除外対象か判別して停止か判断
            foreach (var notcom in notPauseBehaviour)
            {
                if (!notcom.gameObject.activeSelf) continue; // 非アクティブならスキップ
                Animator npanim = notcom.GetComponent<Animator>(); // 判別用Animatorクラス
                // 除外対象だったら
                if (npanim != null && npanim == pauseAnimators[a])
                {
                    pause = false;
                    break;
                }
            }
            if (pause)
            {
                pauseAnimationTimes[a] = pauseAnimators[a].speed;
                pauseAnimators[a].speed = 0;
            }
            else pauseAnimationTimes[a] = -1;
        }
        // 停止対象の物理演算コンポーネント停止
        pause_rb2 = GameObject.FindObjectsOfType<Rigidbody2D>();
        pause_rb2velocities = new Vector2[pause_rb2.Length];
        pause_rb2Aglvels = new float[pause_rb2.Length];
        for (int r = 0; r < pause_rb2.Length; r++)
        {
            bool pause = true; // 一時停止フラグ
            // 除外対象か判別して停止か判断
            foreach (var notcom in notPauseBehaviour)
            {
                Rigidbody2D nprb = notcom.GetComponent<Rigidbody2D>(); // 判別用RigidBody2Dクラス
                if (nprb != null)
                {
                    // 除外対象だったら
                    if (nprb == pause_rb2[r])
                    {
                        pause = false;
                        break;
                    }
                }
            }
            // 対象の物理演算コンポーネントが眠っていないなら移動速度と回転速度を保存してから停止
            if (!pause_rb2[r].IsSleeping())
            {
                if (pause)
                {
                    pause_rb2velocities[r] = pause_rb2[r].velocity;
                    pause_rb2Aglvels[r] = pause_rb2[r].angularVelocity;
                    pause_rb2[r].Sleep();
                }
                // 除外対象なら
                else
                {
                    pause_rb2velocities[r] = Vector2.zero;
                    pause_rb2Aglvels[r] = float.NaN;
                }
            }
        }
    }
    /// <summary> 再開 </summary>
    public static void Resume2D()
    {
        // 再開させるスクリプトが無いなら処理しない
        if (pauseBehaviours == null) return;
        // 停止対象のスクリプトを有効化
        foreach (var com in pauseBehaviours)
        {
            com.enabled = true;
        }
        // 停止対象のアニメーション再開
        for(int a = 0; a < pauseAnimators.Length; a++)
        {
            if (pauseAnimationTimes[a] >= 0) pauseAnimators[a].speed = pauseAnimationTimes[a];
        }
        // 物理演算を再開
        for (int r = 0; r < pause_rb2.Length; r++)
        {
            // 物理演算コンポーネントが破棄されていなかったら再開
            if (pause_rb2[r] == null) continue;
            if (pause_rb2velocities[r] != Vector2.zero && pause_rb2Aglvels[r] != float.NaN)
            {
                pause_rb2[r].WakeUp();
                pause_rb2[r].velocity = pause_rb2velocities[r];
                pause_rb2[r].angularVelocity = pause_rb2Aglvels[r];
            }
        }
        // 停止対象のコンポーネント削除
        pauseBehaviours = null;
        pauseAnimators = null;
        pauseAnimationTimes = null;
        pause_rb2 = null;
        pause_rb2velocities = null;
        pause_rb2Aglvels = null;
    }
    /// <summary> 文章を1文字ずつ表示 </summary>
    /// <param name="message">表示文章</param> <param name="text">出力テキスト</param>
    /// <param name="behaviour">実行スクリプト</param>
    public static void PrintMessage(string message, Text text, MonoBehaviour behaviour)
    {
        // 現在文字を出力中なら実行しない
        if (IsPrintMessage) return;
        PrintMessage(message, 0.05f, text, behaviour);
    }
    /// <summary> 文字を1文字ずつ表示 </summary>
    /// <param name="message">表示文章</param> <param name="span">出力間隔</param>
    /// <param name="text">出力テキスト</param> <param name="behaviour">実行スクリプト</param>
    public static void PrintMessage(string message, float span, Text text, MonoBehaviour behaviour)
    {
        // 現在文字を出力中なら実行しない
        if (IsPrintMessage) return;
        behaviour.StartCoroutine(PrintMessage(message, span, text));
    }
    /// <summary> カメラ名 </summary>
    public static string CameraName { get { return "Main Camera"; } }
    /// <summary> 文章出力フラグ </summary>
    public static bool IsPrintMessage { get; private set; } = false;
}

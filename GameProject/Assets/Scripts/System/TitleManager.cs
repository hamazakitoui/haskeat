using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    bool isLoad = false; // 開始フラグ
    // キーコード
    [SerializeField] KeyCode startKey;
    [SerializeField] KeyCode tutorialKey;
    [SerializeField] GameObject PaintBall; // ペイントアニメーション
    [SerializeField] SpriteRenderer Renderer; // 絵
    [SerializeField] SceneObject GameScene; // ゲームシーン
    [SerializeField] SceneObject tutorialScene; // ゲームシーン
    [SerializeField] Sprite[] pictures; // 絵の配列
    [SerializeField] bool IsTitle;
    [SerializeField] Image SelectArrow;
    public bool Effectend;
    const int maxselect = 2;
    int nowselect = 0;
    Vector3 Arrowstart;
    [SerializeField]Wordfade textfade;
    int[] textpos = new int[3] { -111, -150, -200 };
    enum selectname
    {
        start,
        tutorial,
        end
    };
    selectname selectnum = selectname.start;
    // Start is called before the first frame update
    void Start()
    {

        if (IsTitle)
        {
            PaintBall.SetActive(false); // ペイント非表示
            Renderer.sprite = pictures[Random.Range(0, pictures.Length)]; // 絵を乱数で設定
            Arrowstart = SelectArrow.GetComponent<RectTransform>().anchoredPosition;
        }
        SelectArrow.GetComponent<RectTransform>().anchoredPosition = new Vector3(Arrowstart.x, textpos[(int)selectnum]);

        Debug.Log(textpos[0]);

    }

    // Update is called once per frame
    void Update()
    {

        if (!textfade.ISSelectFadeIn)
        {
            return;
        }
        if (IsTitle)
        {
            gameselect();
            GameStart(); // ゲーム開始

        }
        else
        {
            SceneLoad();
        }

    }
    /// <summary> ゲーム開始 </summary>
    void GameStart()
    {
        if (isLoad) return; // 既に読み込んでいるなら
        // 開始キーが押されたら
        if (Input.GetKeyDown(startKey))
        {
            PaintBall.SetActive(true); // ペイント表示
            isLoad = true;
            switch (selectnum)
            {
                case selectname.start:
                    FadeSceneManager.Instance.LoadScene(GameScene);
                    break;
                case selectname.tutorial:
                    FadeSceneManager.Instance.LoadScene(tutorialScene);
                    break;
                case selectname.end:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
                    break;
            }

        }

    }
    void SceneLoad()
    {
        if (isLoad) return;
        if (Input.GetKeyDown(startKey) )
        {
            FadeSceneManager.Instance.LoadScene(GameScene); // シーン移動
            isLoad = true;
        }
    }
    void gameselect()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if ((int)selectnum > 0)
            {
                selectnum--;

            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if ((int)selectnum < maxselect)
                selectnum++;
        }
        SelectArrow.GetComponent<RectTransform>().anchoredPosition = new Vector3(Arrowstart.x, textpos[(int)selectnum]);
        Debug.Log((int)selectnum);
    }
}

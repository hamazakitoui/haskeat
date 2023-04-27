using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pausemenu : AnimationEffect
{
    public bool IsPause=false;
    enum PauseState
    {
        backStageSelect,//ステージセレクトに戻る
        backTitle,//タイトルへ戻る
        Resume,//閉じる
    };
    [SerializeField] Text[] Pausetext;
    [SerializeField] Image SelectArrow;
    Vector2 ArrowPos;
    int state = 0;
    const int minstatenum = 0;
    const int Maxstatenum = 2;
    [SerializeField] GameObject Mission;
    [SerializeField] string[] Scenename;
    PauseState nowState = PauseState.backStageSelect;
    // Start is called before the first frame update
    void Start()
    {
        ArrowPos = SelectArrow.GetComponent<RectTransform>().anchoredPosition;
        Mission.GetComponent<Animator>().Play("CheckMision");
    }

    // Update is called once per frame
    void Update()
    {
        Pausestart();
        if (IsPause)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (state != 2)
                {
                    FadeSceneManager.Instance.LoadScene(Scenename[state]);
                }
                else
                {
                    Mission.GetComponent<Animator>().Play("MisionNotActive");
                }
            }
            else
            {
                switch (keycheck())
                {
                    case (int)PauseState.backStageSelect:
                        ArrowPos.y = Pausetext[0].GetComponent<RectTransform>().anchoredPosition.y;
                        break;
                    case (int)PauseState.backTitle:
                        ArrowPos.y = Pausetext[1].GetComponent<RectTransform>().anchoredPosition.y;
                        break;
                    case (int)PauseState.Resume:
                        ArrowPos.y = Pausetext[2].GetComponent<RectTransform>().anchoredPosition.y;
                        break;
                }
                SelectArrow.GetComponent<RectTransform>().anchoredPosition = ArrowPos;
            }
        }
    }
    int keycheck()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (state > minstatenum)
            {
                state--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (state < Maxstatenum)
            {
                state++;
            }
        }
        return state;
    }
    void Pausestart()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!IsPause)
            {
                MonoBehaviour[] NotPause = new MonoBehaviour[1];
                NotPause[0] = this;
                Library.Pause2D(NotPause);
                IsPause = true;
            }

        }
    }
}

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
    [SerializeField] GameObject[] Pause;
    [SerializeField] Image SelectArrow;
    Vector2 ArrowPos;
    int state = 0;
    const int minstatenum = 0;
    const int Maxstatenum = 2;
    [SerializeField] GameObject Mission;
    [SerializeField] GameObject MissionCanvas;
    Animator MissionAnim;
    [SerializeField] string[] Scenename;
    PauseState nowState = PauseState.backStageSelect;
    [SerializeField] List<MonoBehaviour> NPList;
    // Start is called before the first frame update
    void Start()
    {
        ArrowPos = SelectArrow.GetComponent<RectTransform>().anchoredPosition;
        gameObject.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        MissionAnim = Mission.GetComponent<Animator>();
        NPList.Add(this);
        int a = NPList.Count;
        //foreach(var m in NPList)
        //{
        //    a += m.transform.childCount;
        //}
        MonoBehaviour[] g = new MonoBehaviour[NPList.Count];
        for (int i = 0; i < g.Length+1; i++)
        {
            g[i] = NPList[i];
        }
        //for (int i=0;i<NPList.Count ;i++)
        //{
        //    for(int j=0;j<NPList[i].transform.childCount+1; j++)
        //    {
        //        g[i + j] = NPList[i].transform.GetChild(j).GetComponent<MonoBehaviour>();
        //        if (j == NPList[i].transform.childCount) g[i + j] = NPList[i];
        //    }
        //}
        Library.Pause2D(g);
      
       
        //Mission.GetComponent<Animator>().Play("CheckMision");
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
                    Debug.Log(state);
                    MissionAnim.Play("resumption");
                    
                    if (MissionAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
                    {
                        Debug.Log("Pauseend");
                        IsPause = false;
                        state = 0;
                        gameObject.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
                        Library.Resume2D();
                    }
                    
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

            gameObject.transform.GetChild(0).GetComponent<Canvas>().enabled = true;
            Mission.gameObject.SetActive(true);
            
            if (!IsPause)
            {
                MonoBehaviour[] NotPause = new MonoBehaviour[Pause.Length + 2];
                for (int i = 0; i < Pause.Length; i++)
                {
                    NotPause[i] = Pause[i].GetComponent<MonoBehaviour>();
                }
                NotPause[Pause.Length] = this;
                NotPause[Pause.Length + 1] = SelectArrow;
                Mission.GetComponent<Animator>().Play("CheckMision");
                Library.Pause2D(NotPause);

                IsPause = true;
            }

        }
    }
}

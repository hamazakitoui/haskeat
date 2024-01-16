using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Alert : MonoBehaviour
{
    [SerializeField] GameObject[] Enemy;      //エネミーさん集団のマンション[]は配列
    bool isfound;
    bool isAlert;
    EnemyBase[] Base;                         //部屋
    float Alertnum;                              //アラート回数
    int MaxAlert = 4;                              //最大回数
    [SerializeField] StageManager Stagemane;
    [SerializeField] GameObject Escapeanim;
    [SerializeField] AudioClip StageBGM;
    [SerializeField] AudioClip discoveryBGM;
    bool IsChengeBGM = true;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM(StageBGM.name);
        Base = new EnemyBase[Enemy.Length];
        for (int i = 0; i < Enemy.Length; i++)        //エネミーさんをカウント
        {
            Base[i] = Enemy[i].GetComponent<EnemyBase>();
            GetComponent<Animator>().Play("StayAlert");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Stagemane.IsRunAway)
        {
            Escapeanim.SetActive(true);
            GetComponent<Animator>().Play("Alert");
            for (int i = 0; i < Enemy.Length; i++)        //エネミーさんをカウント
            {
                Base[i].Discovery();
            }
            return;
        }
        bool checkfound = false;
        for (int i = 0; i < Enemy.Length; i++)
        {
            if (Base[i].Getfound)
            {
                if (!isfound)
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM(discoveryBGM.name);
                    GetComponent<Animator>().Play("Alert");
                    isfound = true;
                    isAlert = true;
                }
                checkfound |= Base[i].Getfound;
                //時間計測
            }
        }

        if (!checkfound)
        {
            isfound = false;
        }
        if (isfound == true)
        {

            if (Alertnum > MaxAlert)
            {
                GetComponent<Animator>().Play("StayAlert");
                Alertnum = 0;
            }
            else if (isAlert == true)
            {
                Alertnum += Time.deltaTime;


            }
            IsChengeBGM = false;
        }
        else
        {
            if (!IsChengeBGM)
            {
                AudioManager.Instance.PlayBGM(StageBGM.name);
                IsChengeBGM = true;
            }
        }
    }
}

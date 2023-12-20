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

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log(Alertnum);
        bool checkfound = false;
        for (int i = 0; i < Enemy.Length; i++)
        {
            if (Base[i].Getfound)
            {
                if(!isfound)
                {
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
        if(isfound==true)
        {
            if (Alertnum > MaxAlert)
            {
                GetComponent<Animator>().Play("StayAlert");
                Alertnum = 0;
            }
            else if(isAlert==true)
            {
                Alertnum += Time.deltaTime;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wordfade : MonoBehaviour
{
    [SerializeField] Text Ztext;
    [SerializeField] Text[] Selecttext;
    [SerializeField] Image SelectImage;
    [SerializeField] GameObject Titlemane;
    bool StartTextFade;
    bool keycheck;
    public bool ISSelectFadeIn;
    float clearcolor = 1;
    bool Isfade;
    const float Fadenum = 0.008f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ISSelectFadeIn)
        {
            return;
        }
        //開始が書かれているtextがフェードアウトしたとき（最初は入らない）
        if (StartTextFade)
        {
            //フェードが完了していない場合
            if (clearcolor < 1)
            {
                //textをどんどんフェードインさせていく値
                clearcolor += Fadenum;
                //フェードインさせるtexを参照しフェードインさせる
                for (int i = 0; i < Selecttext.Length; i++)
                {
                    //フェードインさせるテキストの色を参照
                    Color co = Selecttext[i].color;
                    //textをフェードインさせる処理
                    Selecttext[i].color = new Color(co.r, co.g, co.b, clearcolor);
                }
                //一緒に選択する矢印もフェードさせる。
                Color Imagecolor = SelectImage.color;
                SelectImage.color = new Color(Imagecolor.r, Imagecolor.g, Imagecolor.b, clearcolor);

            }
            else if (clearcolor > 1)
            {
                ISSelectFadeIn = true;
            }
        }
        //開始が書かれているtextが書かれていた場合これ以降の処理に入らないようにする
        if (StartTextFade)
        {
            return;
        }
        //Zキーが押された場合
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //キー入力のboolをtrueに
            keycheck = true;
            Ztext.GetComponent<LoadingText>().enabled = false;
        }
        //キー入力がされた場合
        if (keycheck)
        {
            //textの色の値を参照
            Color co = Ztext.color;
            //フェードの値を減らしていく
            clearcolor -= Fadenum;

            Debug.Log(clearcolor);
            //fedoが終わった場合の処理
            if (clearcolor < 0)
            {
                //完全に透明になったかの確認フラグをtrueに
                StartTextFade = true;
                //次のために値を0に初期化
                clearcolor = 0;
            }
            //textをフェードアウト
            Ztext.color = new Color(co.r, co.g, co.b, clearcolor);
        }

        //StartCoroutine("flash");

    }
}

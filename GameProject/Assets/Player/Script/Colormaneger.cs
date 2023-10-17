using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colormaneger : MonoBehaviour
{
    [SerializeField] ParticleSystem Coloreffect;
    enum Pldirction
    {
        up,
        Down,
        Left,
        Right
    };

    enum Colorchenge
    {
        red,
        brue,
        yellow,
        purple
    }
    public int colornum;
    [SerializeField] Sprite[] SprayImaga;
    bool IsEffectStart;
    [SerializeField] GameObject Effectobject;
    Pldirction nowstate = Pldirction.up;
    [SerializeField] GameObject Player;
    Vector3 nowdirction;
    Color red = new Color(1, 0, 0, 1);
    Color brue = new Color(0, 0, 1, 1);
    Color yellow = new Color(1, 1, 0, 1);
    Color purple = new Color(0, 0, 0, 1);
    public int[] uselimitnum = new int[4];
    [SerializeField] GameObject[] Colorobject = new GameObject[4];
    [SerializeField] Text[] UesNumtext;
    const float hide = -931.5f;
    const float use = -764.4f;
    Vector2 StartPos;
    int BeforColor = 0;
    bool checkChengebutton;
    RectTransform[] Rect = new RectTransform[4];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Colorobject.Length; i++)
        {
            Rect[i] = Colorobject[i].GetComponent<RectTransform>();
            if (i != 0)
            {
                Rect[i].anchoredPosition = new Vector2(hide, Rect[i].anchoredPosition.y);
            }
        }
        StartPos.y = Rect[0].anchoredPosition.y;
        StartPos.x = Rect[1].anchoredPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        colorchenge();
        if (checkChengebutton)
        {
            StartCoroutine("MoveColor");
        }

        UesNumtext[colornum].text = "×" + uselimitnum[colornum].ToString();

        if (Player.GetComponent<PlMoveAction>().IsZbuttonCheck)
        {
            EffectStart();
        }
    }
    //現在のプレイヤーの向きを取得
    int CheckPldir(Vector3 Pldir)
    {

        if (Pldir.y < 0)
        {
            nowstate = Pldirction.Down;
            return 1;
        }
        else if (Pldir.y > 0)
        {
            nowstate = Pldirction.up;
            return 0;
        }
        else if (Pldir.x < 0)
        {
            nowstate = Pldirction.Left;
            return 3;
        }
        else if (Pldir.x > 0)
        {
            nowstate = Pldirction.Right;
            return 2;
        }
        return 0;
    }
    //色の切替処理
    void colorchenge()
    {
        if (checkChengebutton)
        {
            return;
        }
        //1のキーが押された場合
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (BeforColor != colornum)
            {
                BeforColor = colornum;
            }
            checkChengebutton = true;
            if (colornum<(int)Colorchenge.purple)
            {
                colornum++;
            }
            else
            {
                colornum = 0;
            }
            
        }
        //先に前に出ていた色を取得
        //if (BeforColor != colornum)
        //{
        //    BeforColor = colornum;
        //}
    }
    IEnumerator MoveColor()
    {
        //押された数字の色UIの位置を取得
        Vector2 ColorPos = Rect[colornum].anchoredPosition;
        //数字が押される前の色UIの位置を取得
        Vector2 BeforPos = Rect[BeforColor].anchoredPosition;
        float movedistance = -1;
        while (checkChengebutton)
        {
            //現在の位置から色UIが隠れるまでの距離を求める
            movedistance = ColorPos.x - use;
            //色UIの移動開始
            Rect[colornum].anchoredPosition += new Vector2(30 * Time.deltaTime, 0);
            Rect[BeforColor].anchoredPosition -= new Vector2(30 * Time.deltaTime, 0);
            //Debug.Log("aaa");
            //移動完了の処理
            if (movedistance < 0)
            {
                checkChengebutton = true;
                Rect[BeforColor].anchoredPosition = StartPos;
            }
            else
            {
                checkChengebutton = false;
            }

            if (checkChengebutton)
            {
                yield return 0;
            }
            else
            {
                break;
            }

        }
        checkChengebutton = false;
        //Debug.Log("bbb");
        yield return null;
    }
    //色のエフェクトを出す処理
    void EffectStart()
    {

        if (!IsEffectStart)
        {
            //現在プレイヤーが向いている向きを取得
            nowdirction = Player.GetComponent<PlMoveAction>().direction;
            CheckPldir(nowdirction);
            //今選ばれている色のイメージを取得、変更
            Effectobject.GetComponent<SpriteRenderer>().sprite = SprayImaga[colornum];
            //エフェクトの色を取得
            var EffectColor = Coloreffect.main;
            //現在の色にエフェクトの色を変更する。
            switch (colornum)
            {
                case (int)Colorchenge.red:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(red);
                    break;
                case (int)Colorchenge.brue:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(brue);
                    break;
                case (int)Colorchenge.yellow:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(yellow);
                    break;
                case (int)Colorchenge.purple:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(purple);
                    break;
            }
        }
        //向いている方向に向きをエフェクトオブジェクトを合わせる。
        switch (nowstate)
        {
            //上向きの場合
            case Pldirction.up:
                //エフェクトが開始されていなかった場合
                if (!IsEffectStart)
                {
                    //エフェクトオブジェクトをプレイヤーの上方向に合わせる
                    Effectobject.transform.position = Player.transform.position + Vector3.up;
                    //向きに合った回転を加える
                    Effectobject.transform.Rotate(0, 0, -90);
                    //エフェクトの開始
                    IsEffectStart = true;
                    Coloreffect.Play();
                }
                //エフェクトオブジェクトが一定以上回転した場合処理を止める。
                if (Effectobject.transform.localEulerAngles.z <= 180 && IsEffectStart)
                {
                    Effectobject.gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    //回転させた向きを元に戻す。
                    Effectobject.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Player.GetComponent<PlMoveAction>().IsZbuttonCheck = false;
                }
                break;
            case Pldirction.Down:
                if (!IsEffectStart)
                {
                    Effectobject.transform.position = Player.transform.position + Vector3.down;
                    IsEffectStart = true;
                    Effectobject.transform.Rotate(0, 0, 90);
                    Coloreffect.Play();
                }
                if (Effectobject.transform.localEulerAngles.z >= 340 && IsEffectStart)
                {
                    Effectobject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    Effectobject.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Player.GetComponent<PlMoveAction>().IsZbuttonCheck = false;
                }

                break;
            case Pldirction.Left:
                if (!IsEffectStart)
                {
                    Effectobject.transform.localScale = new Vector3(-1, 1, 1);
                    Effectobject.transform.position = Player.transform.position + Vector3.left;
                    IsEffectStart = true;
                    Effectobject.transform.Rotate(0, 0, 180);
                    Coloreffect.Play();
                }
                if (Effectobject.transform.localEulerAngles.z <= 90 && IsEffectStart)
                {
                    Effectobject.transform.localScale = new Vector3(1, 1, 1);
                    Effectobject.gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    Effectobject.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Player.GetComponent<PlMoveAction>().IsZbuttonCheck = false;
                }

                break;
            case Pldirction.Right:
                if (!IsEffectStart)
                {
                    Effectobject.transform.position = Player.transform.position + Vector3.right;
                    Effectobject.transform.localScale = new Vector3(-1, 1, 1);

                    IsEffectStart = true;
                    Coloreffect.Play();
                }
                if (Effectobject.transform.localEulerAngles.z <= 268 && IsEffectStart)
                {
                    Effectobject.transform.localScale = new Vector3(1, 1, 1);
                    Effectobject.gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    Effectobject.transform.localEulerAngles = new Vector3(0, 0, 0);
                    Player.GetComponent<PlMoveAction>().IsZbuttonCheck = false;
                }
                break;
        }
        //回転させる。
        Effectobject.transform.RotateAround(Player.transform.position, Vector3.forward, -3);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    [SerializeField] StageManager movecheck;
    [SerializeField] GameObject[] tutorialImage;
    [SerializeField] GameObject trapkind;
    [SerializeField] GameObject messageImage;
    [SerializeField] GameObject letter;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject[] hollowImage;
    [TextArea] [SerializeField] string[] message;
    [SerializeField] Text messagetext;
    int textnum = 0;
    bool textend;
    bool nowtutorial;
    int nownum = 5;
    int faze;
    bool kind;
    PlMoveAction Plscript;
    [SerializeField] int[] stopnum = new int[4];
    [SerializeField] Vector3[] Raystart;
    [SerializeField] float RayLenth;
    [SerializeField] GameObject key;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] int[] attentionnum;
    [SerializeField] AudioClip TutorialBGM;
    enum tutorialfall
    {
        move,    //移動
        destroy,//美術品の削除
        trap,　 //罠の配置と切替
        avoid,　//回避
        end,
        speak,
    }
    tutorialfall nowfaze = tutorialfall.speak;
    bool nowmessage;
    // Start is called before the first frame update
    void Start()
    {
        Plscript = Player.GetComponent<PlMoveAction>();
        Player.GetComponent<PlMoveAction>().IsStop = true;
        Library.PrintMessage(message[textnum], messagetext, this);
        textnum++;
        AudioManager.Instance.PlayBGM(TutorialBGM.name);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Camera.main.transform.position);
        tutorialFaze();
        nowmessage = Library.IsPrintMessage;
    }
    bool keycheck()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            return true;
        }
        return false;
    }
    void textdisplay()
    {
        if (textnum == message.Length)
        {
            messageImage.SetActive(false);
            nownum = 4;
            return;
        }
        if (textnum >= 11 && textnum < 12)
            {
                tutorialImage[faze].SetActive(true);
            }
        //テキストが一定値に達した場合
        if (textnum >= stopnum[faze] && textnum < stopnum[faze] + 1)
        {

            //Playerが行動可能か
            if (textnum >= 12 && textnum < 13)
            {
                tutorialImage[faze].SetActive(false);
                trapkind.SetActive(true);
                nowtutorial = true;
                kind = true;
            }
            else
            {
                nowtutorial = true;
                //チュートリアルの画像を見せる
                tutorialImage[faze].SetActive(true);
            }
        }
        if (!kind)
        {
            if (textnum >= attentionnum[faze] && textnum < attentionnum[faze] + 1)
            {
                hollowImage[faze].SetActive(true);
            }
            else if (textnum > attentionnum[faze])
            {
                hollowImage[faze].SetActive(false);
            }

        }
        if (textnum <= message.Length)
        {
            Library.PrintMessage(message[textnum], messagetext, this);
            textnum++;
        }
    }
    void tutorialFaze()
    {
        if (Plcheck())
        {
            faze++;
            Plscript.IsStop = true;
            Pldirction();
            Library.PrintMessage(message[textnum], messagetext, this);
            textnum++;
            messageImage.SetActive(true);
            nowtutorial = false;
            nownum = 5;
            Player.GetComponent<PlMoveAction>().rigid2D.velocity = new Vector2(0, 0);
            Debug.Log(faze);
        }
        switch (nownum)
        {
            //動かすチュートリアル
            case (int)tutorialfall.move:
                TutorialStart();
                break;
            //絵を汚すチュートリアル
            case (int)tutorialfall.destroy:
                TutorialStart();
                break;
            //罠についてのチュートリアル
            case (int)tutorialfall.trap:
                TutorialStart();
                break;
            //回避のチュートリアル
            case (int)tutorialfall.avoid:
                TutorialStart();
                break;
            case (int)tutorialfall.end:
                if (!nowmessage)
                {
                    Plscript.IsStop = false;
                    messageImage.SetActive(false);
                }
                break;
            case (int)tutorialfall.speak:

                //テキストを進める
                if (keycheck() && !textend)
                {
                    if (nowtutorial)
                    {
                        nownum = faze;
                        //messageImage.SetActive(false);
                    }
                    else
                    {
                        if (!nowmessage && !textend)
                        {
                            textdisplay();
                        }
                    }
                }
                break;
        }
    }
    bool Plcheck()
    {
        bool result = false;
        RaycastHit2D[] hit = Physics2D.RaycastAll(Raystart[faze], Vector3.up, RayLenth, hitLayer);
        foreach (RaycastHit2D hit2D in hit)
        {
            result |= hit2D.transform.gameObject;
        }
        Debug.DrawRay(Raystart[faze], Vector3.up, Color.red);
        return result;
    }

    void TutorialStart()
    {
        if (!nowmessage)
        {
            Plscript.IsStop = false;
            tutorialImage[faze].SetActive(false);
            messageImage.SetActive(false);
            if (kind)
            {
                trapkind.SetActive(false);
            }
        }
        else if (textnum == message.Length)
        {
            messageImage.SetActive(false);
            textend = true;
        }
    }
    void Pldirction()
    {
        if (Plscript.direction.x == 1)
        {
            Player.GetComponent<PlMoveAction>().PlAnim.Play("PlRightstay");
        }
        else if (Plscript.direction.x == -1)
        {
            Player.GetComponent<PlMoveAction>().PlAnim.Play("PlLeftstay");
        }
        else if (Plscript.direction.y == 1)
        {
            Player.GetComponent<PlMoveAction>().PlAnim.Play("PlUpstay");
        }
        else if (Plscript.direction.y == -1)
        {
            Player.GetComponent<PlMoveAction>().PlAnim.Play("Plstay");
        }

    }
}
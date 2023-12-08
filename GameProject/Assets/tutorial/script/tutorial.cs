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
    [SerializeField] GameObject Stagemaneger;
    [TextArea] [SerializeField] string[] message;
    [SerializeField] Text messagetext;
    [SerializeField] Image textback;
    [SerializeField] float CameraMovespeedX;
    [SerializeField] float CameraMovespeedY;
    [SerializeField] float ChengeSizeSpeed;
    int textnum = 0;
    bool textend;
    bool cameramove;
    bool backCamera;
    bool nowtutorial;
    Vector2 CameraTransform;
    const float MaxPos = 5;
    const float Size = 1.5f;
    const float MaxPosY = 0.7f;
    float camerasize;
    float camerastop;
    int nownum = 4;
    int faze;
    [SerializeField] int[] stopnum = new int[4];
    [SerializeField] Vector3[] Raystart;
    [SerializeField] float RayLenth;
    [SerializeField] GameObject key;
    [SerializeField] LayerMask hitLayer;
    enum tutorialfall
    {
        move,    //移動
        destroy,//美術品の削除
        trap,　 //罠の配置と切替
        avoid,　//回避
        speak,
    }
    tutorialfall nowfaze = tutorialfall.speak;
    // Start is called before the first frame update
    void Start()
    {
        //Player.GetComponent<PlMoveAction>().IsStop = true;
        //カメラの写している範囲を取得
        camerasize = Camera.main.orthographicSize;
        //カメラの初期位置を取得
        CameraTransform = Camera.main.transform.localPosition;
        Library.PrintMessage(message[textnum], messagetext, this);
        textnum++;
        Debug.Log(Camera.main.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Camera.main.transform.position);
        tutorialFaze();

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
        //テキストが一定値に達した場合
        if (textnum >= stopnum[faze] && textnum < stopnum[faze] + 1)
        {
            //Playerが行動可能か
            nowtutorial = true;
            //チュートリアルの画像を見せる
            tutorialImage[faze].SetActive(true);
        }
        if (textnum >= 1 && textnum < 2)
        {
            
            
        }
        //メッセージがすべて終わっているか？
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
            Debug.Log("check");
            nownum++;
        }
        switch (nownum)
        {
            //動かすチュートリアル
            case (int)tutorialfall.move:
                //チュートリアルの開始
                if (keycheck())
                {
                    Player.GetComponent<PlMoveAction>().IsStop = false;
                    tutorialImage[faze].SetActive(false);
                    messageImage.SetActive(false);

                }
                break;
            //絵を汚すチュートリアル
            case (int)tutorialfall.destroy:
                break;
            //罠についてのチュートリアル
            case (int)tutorialfall.trap:
                break;
            //回避のチュートリアル
            case (int)tutorialfall.avoid:
                break;
            case (int)tutorialfall.speak:

                //カメラが動いていない場合
                if (!cameramove)
                {
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
                            textdisplay();

                        }
                    }
                }
                else if (cameramove)
                {
                    Cameramove();
                }
                break;
        }
    }
    void Cameramove()
    {
        //カメラを元の位置に戻す
        if (backCamera)
        {
            //戻すときの処理
            if (Camera.main.orthographicSize < camerasize)
            {
                Camera.main.orthographicSize += ChengeSizeSpeed;
                Debug.Log("aaa");
            }
            if (Camera.main.transform.localPosition.x >= CameraTransform.x)
            {
                Camera.main.transform.localPosition -= new Vector3(CameraMovespeedX, 0, 0);

            }
            //カメラ位置の補正
            else
            {
                Camera.main.orthographicSize = camerasize;
                Camera.main.transform.localPosition = new Vector3(CameraTransform.x, CameraTransform.y, -10);
                cameramove = false;
            }
            if (Camera.main.transform.localPosition.y >= MaxPosY)
            {
                Camera.main.transform.localPosition += new Vector3(0, CameraMovespeedY, 0);

            }
            return;
        }
        //カメラのズーム処理
        else
        {
            if (Camera.main.orthographicSize > Size)
            {
                Camera.main.orthographicSize -= ChengeSizeSpeed;

            }
            if (Camera.main.transform.localPosition.x < MaxPos)
            {
                Camera.main.transform.localPosition += new Vector3(CameraMovespeedX, 0, 0);
            }
            else
            {
                camerastop += Time.deltaTime;
                Camera.main.transform.localPosition = new Vector3(MaxPos, -MaxPosY, -10);
                if (camerastop >= 4.5f)
                {
                    backCamera = true;
                }

            }
            if (Camera.main.transform.localPosition.y >= -MaxPosY)
            {
                Camera.main.transform.localPosition += new Vector3(0, -CameraMovespeedY, 0);

            }
        }

    }
    bool Plcheck()
    {
        bool result = false;
        RaycastHit2D[] hit = Physics2D.RaycastAll(Raystart[faze], Vector3.up, RayLenth, hitLayer);
        foreach (RaycastHit2D hit2D in hit)
        {
            result |= hit2D.transform.gameObject;
            //Debug.Log(hit2D.transform.gameObject);
        }
        Debug.DrawRay(Raystart[faze], Vector3.up, Color.red);
        return result;
    }
}
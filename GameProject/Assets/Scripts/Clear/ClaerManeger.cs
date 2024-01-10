using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaerManeger : MonoBehaviour
{
    [SerializeField] Text messagetext;
    float AnimCount;
    int CountMax=2;
    string Scenename = "TitleScene";
    [SerializeField] string message = "ステージクリア！！";
    bool AnimEnd;
    bool ismessage;
    bool TextStart = false;
    // Start is called before the first frame update
    void Start()
    {
        messagetext.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ismessage = Library.IsPrintMessage;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (AnimEnd)
            {
                FadeSceneManager.Instance.LoadScene(Scenename);
            }
        }
        if (AnimCount < CountMax)
        {
            AnimCount += Time.deltaTime;
        }
        else
        {
            messagetext.enabled=true;
        }


    }

}

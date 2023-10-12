using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    [SerializeField] StageManager movecheck;
    [SerializeField] GameObject tutorialImage;
    [SerializeField] GameObject colortutorial;
    [SerializeField] GameObject letter;
    [SerializeField] GameObject Player;
    bool Isindicate;
    bool indicateend=true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Player.GetComponent<PlMoveAction>().isLoad);
        //チュートリアルが表示されているか
        if (!Isindicate)
        {
            Debug.Log("tutorial");
            indicate();
        }
        //ボタンが押されたか
        else if (keycheck())
        {
            //ボタンが押されたときにチュートリアルが表示されているか
            if (indicateend)
            {
                Player.GetComponent<PlMoveAction>().isLoad = false;
                Debug.Log("end");
                
                indicateend = false;
            }
            else
            {
                Debug.Log("start");
                indicateend = true;
                
            }
            tutorialImage.SetActive(indicateend);
            colortutorial.SetActive(indicateend);

        }
        //else
        //{
        //    Player.GetComponent<PlMoveAction>().isLoad = true;
        //}

    }
    void indicate()
    {
        if (letter.activeSelf==false)
        {
            Player.GetComponent<PlMoveAction>().isLoad = true;
            Isindicate = true;
            tutorialImage.SetActive(true);
            colortutorial.SetActive(true);
        }
    }
    bool keycheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }
}

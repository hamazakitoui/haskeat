using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    [SerializeField] StageManager movecheck;
    [SerializeField] GameObject tutorialImage;
    [SerializeField] GameObject colortutorial;
    bool Isindicate;
    bool indicateend=true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Isindicate)
        {
            Debug.Log("tutorial");
            indicate();
        }
        else if (keycheck())
        {
            
            if (indicateend)
            {
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

    }
    void indicate()
    {
        if (movecheck.IsGameStart)
        {
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
    IEnumerator StartDlay()
    {
        yield return null;
    }
}

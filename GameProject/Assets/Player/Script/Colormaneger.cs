using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colormaneger : MonoBehaviour
{
    public int colornum;
    public enum nowcolor
    {
        red,
        brue,
        yellow,
        purple
    };
    nowcolor colorstate = nowcolor.red;
    public int[] uselimitnum = new int[4];
    [SerializeField] Sprite[] Color = new Sprite[4];
    [SerializeField] GameObject[] Colorobject = new GameObject[4];
    [SerializeField] Text[] UesNumtext;
    const float hide = -931.5f;
    const float use = -764.4f;
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
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Rect[BeforColor]);
        //Rect[BeforColor].anchoredPosition = new Vector2(hide, Rect[BeforColor].anchoredPosition.y);
        //switch (colorchenge())
        //{
        //    case (int)nowcolor.red:

        //        Rect[0].anchoredPosition = new Vector3(use, Rect[0].anchoredPosition.y);

        //        break;
        //    case (int)nowcolor.brue:
        //        StartCoroutine("MoveColor");
        //        break;
        //    case (int)nowcolor.yellow:
        //        StartCoroutine("MoveColor");
        //        //if (Rect[2].anchoredPosition.x < use)
        //        //{
        //        //    Rect[BeforColor].anchoredPosition -= new Vector2(100 * Time.deltaTime, 0);
        //        //    Rect[2].anchoredPosition += new Vector2(100 * Time.deltaTime, 0);
        //        //}
        //        //Rect[2].anchoredPosition = new Vector3(use, Rect[2].anchoredPosition.y);
        //        break;
        //    case (int)nowcolor.purple:
        //        StartCoroutine("MoveColor");
        //        //if (Rect[3].anchoredPosition.x < use)
        //        //{
        //        //    Rect[3].anchoredPosition += new Vector2(100 * Time.deltaTime, 0);
        //        //}
        //        //Rect[3].anchoredPosition = new Vector3(use, Rect[3].anchoredPosition.y);
        //        break;
        //}
       
        colorchenge();
        if (checkChengebutton)
        {
            StartCoroutine("MoveColor");
        }
        UesNumtext[colornum].text = "×" + uselimitnum[colornum].ToString();


    }
    void colorchenge()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (BeforColor != colornum)
            {
                BeforColor = colornum;
            }
            checkChengebutton = true;
            colornum = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (BeforColor != colornum)
            {
                BeforColor = colornum;
            }
            checkChengebutton = true;
            colornum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (BeforColor != colornum)
            {
                BeforColor = colornum;
            }
            checkChengebutton = true;
            colornum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (BeforColor != colornum)
            {
                BeforColor = colornum;
            }
            checkChengebutton = true;
            colornum = 3;
        }
    }
    IEnumerator MoveColor()
    {
        Vector2 ColorPos = Rect[colornum].anchoredPosition;
        Vector2 beforColorPos = Rect[BeforColor].anchoredPosition;
        float movedistance = -1;
        while (ColorPos.x<=use)
        {
            movedistance = ColorPos.x - use;
            Rect[colornum].anchoredPosition += new Vector2(20 * Time.deltaTime, 0);
            Rect[BeforColor].anchoredPosition -= new Vector2(20 * Time.deltaTime, 0);
            Debug.Log("aaa");
            yield return 0;
        }
        checkChengebutton = false;
        yield return null;
    }

    

}

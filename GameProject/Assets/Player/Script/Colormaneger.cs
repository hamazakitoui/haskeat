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
    public int[] uselimitnum=new int[4];
    [SerializeField]Sprite[] Color=new Sprite[4];
    [SerializeField] GameObject Colorobject;
    [SerializeField] Text UesNumtext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (colorchenge())
        {
            case (int)nowcolor.red:
                Colorobject.GetComponent<SpriteRenderer>().sprite = Color[0];
                

                break;
            case (int)nowcolor.brue:
                Colorobject.GetComponent<SpriteRenderer>().sprite = Color[1];
                break;
            case (int)nowcolor.yellow:
                Colorobject.GetComponent<SpriteRenderer>().sprite = Color[2];
                break;
            case (int)nowcolor.purple:
                Colorobject.GetComponent<SpriteRenderer>().sprite = Color[3];
                break;
        }
        UesNumtext.text = "残り"+uselimitnum[colorchenge()].ToString();


    }
    int colorchenge()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            colornum = 0;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            colornum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            colornum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            colornum = 3;
        }
        return colornum;
    }
}

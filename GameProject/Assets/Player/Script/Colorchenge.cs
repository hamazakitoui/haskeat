using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorchenge : MonoBehaviour
{
    int nowcolor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int colorchenge()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nowcolor = 1;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nowcolor = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nowcolor = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nowcolor = 4;
        }
        return nowcolor;
    }
}

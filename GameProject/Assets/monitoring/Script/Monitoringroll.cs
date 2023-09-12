using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitoringroll : MonoBehaviour
{
    float Waiktime;
    [SerializeField] float maxroll;
    [SerializeField] float rollspeed;
    bool fast = true;
    int rollnum;
    [SerializeField]bool wait;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotation();
    }
    void rotation()
    {
        Vector3 nowroll = transform.rotation.eulerAngles;

        if (fast)
        {
            if (nowroll.z > maxroll / 2)
            {
                fast = false;
                //Debug.Log("aaa");
                rollnum = 0;
                rollspeed *= -1;
                wait = true;
            }
            else
            {
                rollnum += (int)rollspeed;
                
                transform.Rotate(0, 0, rollspeed);

            }

        }
        else
        {

            //Debug.Log(i);

            if (rollnum < maxroll)
            {
                rollnum += Mathf.Abs((int)rollspeed);
                if (rollnum >= maxroll)
                {
                    Debug.Log("aa");
                    rollnum = 0;
                    rollspeed *= -1;
                }
                transform.Rotate(0, 0, rollspeed);
            }

        }
    }
}

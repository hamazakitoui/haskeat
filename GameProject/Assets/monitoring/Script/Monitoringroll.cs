using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitoringroll : MonoBehaviour
{
    float Waittime;
    [SerializeField] float MaxWaik;
    [SerializeField] float maxroll;
    [SerializeField] float rollspeed;
    bool fast = true;
    int rollnum;
    float scale;
    const int startmax = 90;
    [SerializeField] bool Wait;
    [SerializeField] bool rightroll;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.x;
        if (rightroll)
        {
            rollspeed *= -1;
        }
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
                if (rollnum > startmax)
                {
                    fast = false;
                    //Debug.Log("aaa");
                    rollnum = 0;
                    rollspeed *= -1;
                    Wait = true;
                }
                else
                {
                    rollnum += Mathf.Abs((int)rollspeed);

                    transform.Rotate(0, 0, rollspeed);

                }
        }
        else
        {

            //Debug.Log(i);

            if (rollnum < maxroll && !Wait)
            {
                rollnum += Mathf.Abs((int)rollspeed);
                if (rollnum >= maxroll)
                {
                    Debug.Log("aa");
                    rollnum = 0;
                    rollspeed *= -1;
                    Wait = true;
                }
                    transform.Rotate(0, 0, rollspeed);
            }
            else if (Wait)
            {
                Waittime += Time.deltaTime;
                if (Waittime >= MaxWaik)
                {
                    Debug.Log("bbb");
                    Waittime = 0;
                    Wait = false;
                }
            }

        }
    }
}

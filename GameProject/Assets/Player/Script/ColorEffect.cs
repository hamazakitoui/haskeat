using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEffect : MonoBehaviour
{
    const float Left = 90;
    const float Right = -90;
    const float Up = 0;
    const float Down = 180;
    enum Pldirction
    {
        up,
        Down,
        Left,
        Right
    };
    bool IsEffectStart;
    Pldirction nowstate = Pldirction.up;

    [SerializeField] GameObject Player;
    Vector3 nowdirction;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        Debug.Log(nowstate);
        nowdirction = Player.GetComponent<PlMoveAction>().direction;
        if (!IsEffectStart)
        {
            CheckPldir(nowdirction);
        }
        
            switch (nowstate)
        {
            case Pldirction.up:
                if (!IsEffectStart)
                {       
                    transform.position = Player.transform.position + Vector3.up;
                    transform.Rotate(0, 0, 90);
                    IsEffectStart = true;
                }
                if (transform.localEulerAngles.z<=0&&IsEffectStart)
                {
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                }
                break;
            case Pldirction.Down:
                if (!IsEffectStart)
                {
                    transform.position = Player.transform.position + Vector3.down;
                    IsEffectStart = true;
                    transform.Rotate(-1, 0, -90);
                }
                Debug.Log(transform.localEulerAngles.z);
                if (transform.localEulerAngles.z < 180&&IsEffectStart)
                {
                    Debug.Log("aaa");
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                }

                break;
            case Pldirction.Left:
                if (!IsEffectStart)
                {
                    transform.position = Player.transform.position + Vector3.left;
                    IsEffectStart = true;
                    transform.Rotate(0, 0, 180);
                }
                Debug.Log(transform.localEulerAngles.z);
                if (transform.localEulerAngles.z <= 90&&IsEffectStart)
                {
                    Debug.Log("aaa");
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                }

                break;
            case Pldirction.Right:
                if (!IsEffectStart)
                {
                    transform.position = Player.transform.position + Vector3.right;
                    IsEffectStart = true;
                    transform.Rotate(-1, 0, 0);
                }
                Debug.Log(transform.localEulerAngles.z);
                if (transform.localEulerAngles.z <= 268&&IsEffectStart)
                {
                    Debug.Log("aaa");
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                }

                break;
        }
        transform.RotateAround(Player.transform.position, Vector3.forward, -1);
    }
    int CheckPldir(Vector3 Pldir)
    {
       
        if (Pldir.y<0)
        {
            nowstate = Pldirction.Down;
            return 1;
        }
        else if(Pldir.y>0)
        {
            nowstate = Pldirction.up;
            return 0;
        }
        else if (Pldir.x<0)
        {
            nowstate = Pldirction.Left;
            return 3;
        }
        else if (Pldir.x>0)
        {
            nowstate = Pldirction.Right;
            
            return 2;
        }
        return 0;
    }

}

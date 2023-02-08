using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlMoveAction : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    BoxCollider2D Collsion;
    Rigidbody2D rigid2D;
    const int NowMove = 1;
    float InputX;
    float InputY;
    int nowcolor;
    [SerializeField] GameObject[] coloreffect=new GameObject[4];
    //enum Plstate
    //{
    //    stay,
    //    move,
    //    damege,

    //}
    // Start is called before the first frame update
    void Start()
    {
        //必要なコンポーネントを取得
        Collsion = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");
        float speedX = 0;
        float speedY = 0;
        if (InputX == NowMove)
        {
            speedX = MoveSpeed;
        }
        else if (InputX == -NowMove)
        {
            speedX = -MoveSpeed;
        }

        if (InputY == NowMove)
        {
            speedY = MoveSpeed;
        }
        else if (InputY == -NowMove)
        {
            speedY = -MoveSpeed;
        }
        
       
        rigid2D.velocity = new Vector2(speedX, speedY);
        //Move();
       
    }
    private void Move()
    {
        float speedX = 0;
        float speedY=0;
        if (InputX == NowMove)
        {
            speedX = MoveSpeed;
        }
        else if (InputX == -NowMove)
        {
            speedX = -MoveSpeed;
        }

        if (InputY == NowMove)
        {
            speedY = MoveSpeed;
        }
        else if (InputY == -NowMove)
        {
            speedY = -MoveSpeed;
        }
        rigid2D.velocity = new Vector2(speedX, speedY);
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
        }else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nowcolor = 4;
        }
        return nowcolor;
    }
    bool checkfront()
    {
        Vector3 chkPos = transform.position;
        bool result = false;
        result|=Physics2D.Raycast(chkPos.x+InputX,)
        return false;
        
    }


}

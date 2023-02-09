using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlMoveAction : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float RayLength;
    [SerializeField] LayerMask NothitLayer;
    [SerializeField] float HP;
    [SerializeField] float invincibleTime;
    BoxCollider2D Collsion;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid2D;
    const int NowMove = 1;
    float InputX;
    float InputY;
    int nowcolor;
    Vector3 dirctionkeap;
    [SerializeField] GameObject[] coloreffect = new GameObject[4];
    enum Plstate
    {
        none,
        invincible,

    }
    Plstate state = Plstate.none;
    // Start is called before the first frame update
    void Start()
    {
        //必要なコンポーネントを取得
        Collsion = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("flash");
    }

    // Update is called once per frame
    void Update()
    {

        //移動を検知
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");
        //スピード代入
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

        //移動
        rigid2D.velocity = new Vector2(speedX, speedY);
        //Move();

    }
    private void Move()
    {
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
    bool checkfront()
    {
        //rayの初期位置
        Vector3 RaystartPos = transform.position;
        Vector3 direction = Vector3.up;
        direction = new Vector3((int)InputX, (int)InputY);
        if (direction != Vector3.zero)
        {
            dirctionkeap = direction;

        }
        else if (direction == Vector3.zero)
        {
            direction = dirctionkeap;
        }
        bool result = false;
        result |= Physics2D.Raycast(RaystartPos, direction, RayLength, NothitLayer);
        Debug.DrawRay(RaystartPos, direction, Color.red);
        return result;

    }
    IEnumerator flash()
    {

        Color co = spriteRenderer.color;
        Color Before = co;
        int count = 0;


        Debug.Log("フェード中");
        while (count < 10)
        {
            count++;

            spriteRenderer.color = new Color(co.r, co.g, co.b,0);
            yield return new WaitForSeconds(0.07f);
            spriteRenderer.color = new Color(co.r,co.g,co.b,1);
            yield return new WaitForSeconds(0.07f);
        }
    }


}

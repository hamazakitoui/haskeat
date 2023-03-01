using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlMoveAction : MonoBehaviour
{
    [SerializeField] float MoveSpeed;       //移動速度
    [SerializeField] float RayLength;　　　 //raycastの長さ
    [SerializeField] LayerMask hitLayer;    //当たるLayer
    [SerializeField] float HP;　　　　　　　//プレイヤーのHP
    [SerializeField] float invinciblenum;　 //何回点滅するか
    [SerializeField] Colormaneger Colorscript;
    [SerializeField] StageManager StageManager;
    [SerializeField] Slider HPSlider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] float Actionmovenum;
    BoxCollider2D Collsion;　　　　　　　　 //当たり判定の保存用関数
    SpriteRenderer spriteRenderer;　　　　　//画像のコンポーネント取得関数
    Rigidbody2D rigid2D;                    //重力取得
    const int Nowdrection = 1;                  //
    float InputX;
    float InputY;
    Vector3 dirctionkeap;
    Vector3 direction;
    RaycastHit2D[] hit2;
    float stamina;
    const float StaminaMax = 100;
    bool IsAction;
    Animator PlAnim;
    [SerializeField] float staminarecoverynum;
    [SerializeField] GameObject[] coloreffect = new GameObject[4];
    bool movestart = true;
    string NowAnim = "None";
    bool Ismove;
    GameObject art;
    [SerializeField] Camera main;
    enum Pldirection
    {
        none,
        Up,
        Down,
        Right,
        Left,
        Diagonal
    };
    Pldirection state = Pldirection.none;
    // Start is called before the first frame update
    void Start()
    {
        main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        //必要なコンポーネントを取得
        Collsion = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        PlAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //StartCoroutine("flash");
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManager.IsGameStart)
        {
            //移動を検知
            InputX = Input.GetAxisRaw("Horizontal");
            InputY = Input.GetAxisRaw("Vertical");
            //スピード代入
            float speedX = 0;
            float speedY = 0;
            if (movestart)
            {
                direction = Vector3.up;
                if (InputX != 0 || InputY != 0)
                {
                    movestart = false;
                }
            }
            else
            {
                direction = new Vector3((int)InputX, (int)InputY);
            }
            //止まる前の向きを取得
            if (direction != Vector3.zero)
            {
                dirctionkeap = direction;
                Ismove = true;
            }

            //止まっているときに向きを代入
            else if (direction == Vector3.zero)
            {
                direction = dirctionkeap;
                Ismove = false;
                if (direction.y == 1)
                {
                    PlAnim.Play("PlUpstay");
                }
                else if (direction.y == -1)
                {

                    PlAnim.Play("Plstay");
                }
                else if (direction.x == 1)
                {
                    PlAnim.Play("PlRightstay");
                }
                else if (direction.x == -1)
                {
                    PlAnim.Play("PlLeftstay");
                }
            }
            //Debug.Log(direction);

            if (!IsAction)
            {
                if (InputX == Nowdrection)
                {
                    speedX = MoveSpeed;
                    if (speedY == 0)
                    {
                        PlAnim.Play("PlRight");
                    }

                }
                else if (InputX == -Nowdrection)
                {
                    speedX = -MoveSpeed;
                    if (speedY == 0)
                    {
                        PlAnim.Play("PlLeft");
                    }
                }

                if (InputY == Nowdrection)
                {
                    speedY = MoveSpeed;
                    if (speedX == 0)
                    {
                        PlAnim.Play("PlUp");
                    }
                }
                else if (InputY == -Nowdrection)
                {
                    speedY = -MoveSpeed;
                    if (speedX == 0)
                    {
                        Debug.Log("下入力");
                        PlAnim.Play("PlDown");
                    }
                }

                rigid2D.velocity = new Vector2(speedX, speedY);
            }
            Debug.Log(checkfront());
            if (checkfront())
            {
                Debug.Log(art.gameObject);
                //目の前に美術品があれば美術品を破壊もしくは塗りつぶす
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (Colorscript.uselimitnum[Colorscript.colornum] > 0)
                    {
                        //美術品か確認
                        Art checkart = art.GetComponent<Art>();
                        //美術品であれば
                        if (checkart != null)
                        {
                            Debug.Log("チェック");
                            //破壊した数を追加
                            StageManager.AddDestroyArt(checkart.GetArtType);
                            //目の前にあるのが絵画だった場合
                            if (checkart.GetArtType == ArtType.Picture)
                            {
                                art.SetActive(false);
                            }
                            //壺だった場合の処理
                            if (checkart.GetArtType == ArtType.Pot)
                            {
                                Destroy(art.gameObject);
                            }
                            int nowcolor = Colorscript.colornum;
                            Colorscript.uselimitnum[nowcolor]--;
                        }
                    }
                }
            }
            //残り残量がある場合エフェクトを出す
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!checkfront())
                {
                    if (Colorscript.uselimitnum[Colorscript.colornum] > 0)
                    {
                        int nowcolor = Colorscript.colornum;
                        GameObject paintEffect = null;
                        paintEffect = Instantiate(coloreffect[nowcolor]);
                        paintEffect.transform.position = transform.position + (direction * 2);
                        Colorscript.uselimitnum[nowcolor]--;
                    }
                    else
                    {
                        int nowcolor = Colorscript.colornum;
                        Colorscript.uselimitnum[nowcolor]=0;
                    }

                }
                else if (art.tag == "wall")
                {

                }
                else
                {

                }
            }
            stamina = staminaSlider.value;
            if (stamina <= StaminaMax)
            {
                staminaSlider.value += Time.deltaTime * staminarecoverynum;
            }
            //アクション開始
            if (Input.GetKeyDown(KeyCode.X))
            {
                //スタミナ量チェック
                if (stamina >= 0 && !Ismove||!IsAction)
                if (stamina >= 25 && !Ismove&&!IsAction)
                {
                    staminaSlider.value -= 25;
                    //Debug.Log("コルーチンチェック");
                    StartCoroutine(Action(direction));
                }
            }
        }
        //移動
    }

    IEnumerator Action(Vector3 movedirction)
    {
        IsAction = true;
        //当たり判定を消す
        Collsion.enabled = false;
        //動く前の位置を取得
        Vector3 PlmovePos = transform.position;
        Debug.Log(movedirction);
        //移動後の位置を取得
        Vector3 moveend = transform.position + movedirction;
        //移動量チェック
        float checkmovedistance = 1;
        //float debug = 0;
        //どれだけ移動したかの変数
        //const float _TIME = 360.0f / 50;
        while (checkmovedistance >= 0.1f)
        {
            //debug = Input.GetAxisRaw("Vertical");

            checkmovedistance = Vector3.Distance(transform.position, moveend);
            transform.position += movedirction * Actionmovenum * Time.deltaTime;
            yield return 0;
        }
        //当たり判定を戻す
        Collsion.enabled = true;
        IsAction = false;
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵に当たった時に点滅開始
        if (collision.tag == "Enemy")
        {
            StartCoroutine("flash");

        }
    }
    bool checkfront()
    {
        //rayの初期位置
        Vector3 RaystartPos = transform.position;
        //結果を宣言
        bool result = false;
        //raycastを飛ばす
        foreach (RaycastHit2D hit2D in Physics2D.RaycastAll(RaystartPos, direction, RayLength, hitLayer))
        {
            art = hit2D.transform.gameObject;

            if (hit2D.transform.gameObject.tag == "Picture" || hit2D.transform.gameObject.tag == "Pot")
            {
                //art = hit2D.transform.gameObject;


            }
            else
            {

            }
            result |= hit2D.transform.gameObject;
            //Debug.Log(hit2D.transform.gameObject);
        }


        //rayを表示する
        Debug.DrawRay(RaystartPos, direction, Color.red);
        return result;

    }
    //点滅と無敵の処理
    IEnumerator flash()
    {
        //色を取得
        Color co = spriteRenderer.color;
        HPSlider.value -= 10;
        //点滅回数の宣言
        int count = 0;
        //当たり判定を消す
        Collsion.enabled = false;
        //規定回数まで点滅を繰り返す
        while (count < 10)
        {
            count++;

            spriteRenderer.color = new Color(co.r, co.g, co.b, 0);
            yield return new WaitForSeconds(0.07f);
            spriteRenderer.color = new Color(co.r, co.g, co.b, 1);
            yield return new WaitForSeconds(0.07f);
        }
        //最後に当たり判定を戻す
        Collsion.enabled = true;
    }


}

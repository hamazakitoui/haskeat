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
    [SerializeField] GameObject EffctSpray;
    [SerializeField] float MaxDestroyTime;
    BoxCollider2D Collsion;　　　　　　　　 //当たり判定の保存用関数
    SpriteRenderer spriteRenderer;　　　　　//画像のコンポーネント取得関数
    public Rigidbody2D rigid2D;                    //重力取得
    const int Nowdrection = 1;                  //
    float InputX;
    float InputY;
    Vector3 dirctionkeap;
    public Vector3 direction;
    RaycastHit2D[] hit2;
    float stamina;
    const float StaminaMax = 100;
    bool IsAction;
    public Animator PlAnim;
    [SerializeField] float staminarecoverynum;
    [SerializeField] GameObject[] coloreffect = new GameObject[4];
    bool movestart = true;
    bool Ismove;
    GameObject art;
    public bool IsZbuttonCheck;
    [SerializeField] Camera main;
    public List<GameObject> paintEffect = new List<GameObject>();
    [SerializeField] float ColorDestroytime = 5;
    const float MOVE_AFTER_CREATE_INSTANCE_SPAN = 0.1f;
    const float MOVE_AFTER_ACTIVE_TIME = 0.1f;
    const float MOVE_AFTER_DESTROY_SPAN = 0.01f;
    [SerializeField] AudioClip SpraySE;
    [SerializeField] SceneObject gameover;
    public bool IsStop;
    bool damege = false;
    EnemyBase Base;
    bool Ischecklettor;
    public bool notfound = false;
    [SerializeField] bool IsTutorial;
    [SerializeField] GameObject[] Artpaint;
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
        //main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        IsStop = false;
        //必要なコンポーネントを取得
        Collsion = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
        PlAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //StartCoroutine("flash");
        EffctSpray.SetActive(false);
        Base = GetComponent<EnemyBase>();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsStop)
        {
            return;
        }
        else if (!IsTutorial)
        {
            Ischecklettor = StageManager.IsGameStart;
        }
        if (HPSlider.value <= 0)
        {
            FadeSceneManager.Instance.LoadScene(gameover);
            IsStop = true;
        }
        if (Ischecklettor)
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
                        //Debug.Log("下入力");
                        PlAnim.Play("PlDown");
                    }
                }

                rigid2D.velocity = new Vector2(speedX, speedY);
            }
            //Debug.Log(checkfront());
            if (checkfront())
            {
                if (art.tag == "tutorial")
                {
                    Debug.Log("aaa");
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        FadeSceneManager.Instance.LoadScene("TitleScene");
                    }
                }

                //目の前に美術品があれば美術品を破壊もしくは塗りつぶす
                if (Input.GetKeyDown(KeyCode.Z))
                {

                    int nowcolor = Colorscript.colornum;
                    if (Colorscript.uselimitnum[Colorscript.colornum] > 0)
                    {
                        //美術品か確認
                        Art checkart = art.GetComponent<Art>();
                        //美術品であれば
                        if (checkart != null)
                        {
                            //破壊した数を追加
                            StageManager.AddDestroyArt(checkart.GetArtType);
                            //目の前にあるのが絵画だった場合
                            if (checkart.GetArtType == ArtType.Picture)
                            {
                                GameObject Spray = Instantiate(Artpaint[nowcolor]);
                                art.GetComponent<BoxCollider2D>().enabled = false;
                                Spray.transform.position = art.transform.position;
                            }
                            //壺だった場合の処理
                            if (checkart.GetArtType == ArtType.Pot)
                            {
                                Destroy(art.gameObject);
                            }
                            nowcolor = Colorscript.colornum;
                            Colorscript.uselimitnum[nowcolor]--;
                        }

                    }
                }
            }
            else if (!checkfront())
            {
                //残り残量がある場合エフェクトを出す
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    IsZbuttonCheck = true;
                    int nowcolor = Colorscript.colornum;
                    //Debug.Log("bbb");
                    if (Colorscript.uselimitnum[Colorscript.colornum] > 0)
                    {
                        //スプレーのエフェクトを表示
                        EffctSpray.SetActive(true);
                        GameObject Effect = null;
                        //SEを開始
                        AudioManager.Instance.PlaySE(SpraySE.name, false);
                        //罠を生成
                        Effect = Instantiate(coloreffect[nowcolor]);
                        //罠にスクリプトをアタッチ
                        Effect.AddComponent<decoy>();
                        //罠の位置を調整
                        Effect.transform.position = transform.position + (direction * 2);
                        //出したエフェクトを追加
                        paintEffect.Add(Effect);
                        if (nowcolor == 0)
                        {
                            Effect.GetComponent<decoy>().state = decoy.Colorkind.red;
                        }
                        else if (nowcolor == 1)
                        {
                            Effect.GetComponent<decoy>().state = decoy.Colorkind.brue;
                        }
                        else if (nowcolor == 2)
                        {
                            Effect.GetComponent<decoy>().state = decoy.Colorkind.yellow;
                        }
                        else if (nowcolor == 3)
                        {
                            //生成したオブジェクトをデコイに設定
                            Effect.GetComponent<decoy>().state = decoy.Colorkind.purple;
                            //敵のタイプを取得
                            EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
                            foreach (var e in enemies)
                            {
                                e.SetDecoy(paintEffect[paintEffect.Count - 1].transform);
                            }

                        }
                        Colorscript.uselimitnum[nowcolor]--;
                        Debug.Log(Colorscript.uselimitnum[nowcolor]);
                        //else
                        //{
                        //    Colorscript.uselimitnum[nowcolor] = 0;
                        //}
                    }
                    else if (art.tag == "wall")
                    {
                        Debug.Log("wall");
                    }
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
                if (stamina >= 25 && !IsAction && !checkfront())
                {
                    staminaSlider.value -= 10;
                    //Debug.Log("コルーチンチェック");
                    StartCoroutine(MoveAfterMotion(transform.position, transform.position + direction));
                    StartCoroutine(Action(direction));

                }
            }
        }
    }

    IEnumerator Action(Vector3 movedirction)
    {
        IsAction = true;
        rigid2D.velocity = new Vector2(0, 0);
        //当たり判定を消す
        Collsion.enabled = false;
        //移動後の位置を取得
        Vector3 moveend = transform.position + movedirction;
        //移動量チェック
        float checkmovedistance = 1;
        //float debug = 0;
        //どれだけ移動したかの変数
        float _TIME = movedirction.magnitude / MoveSpeed;
        float nowtime = 0;

        while (_TIME < nowtime)
        {

            //debug = Input.GetAxisRaw("Vertical");
            checkmovedistance = Vector3.Distance(transform.position, moveend);
            Debug.Log(checkmovedistance);
            nowtime++;
            if (checkmovedistance >= 0.1)
            {
                if (_TIME < nowtime)
                {
                    transform.position += movedirction * Actionmovenum * Time.deltaTime;

                }
                else
                {
                    yield return 0;
                    //transform.position = PlmovePos+moveend;
                }
            }
            yield return 0;
        }
        //当たり判定を戻す
        Collsion.enabled = true;
        transform.position = moveend;
        IsAction = false;
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵に当たった時に点滅開始
        if (collision.tag == "Enemy")
        {
            if (damege)
            {
                return;
            }
            StartCoroutine("flash");

        }
        if (collision.tag == "redsplay")
        {
            Debug.Log("invisible");
            notfound = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "invisible")
        {
            notfound = false;
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
        damege = true;
        HPSlider.value -= 25;
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
        damege = false;
    }
    private IEnumerator MoveAfterMotion(Vector3 startPos, Vector3 endPos)
    {
        // 終点と始点との距離、終点と始点との角度
        float length, dirRad;
        // 終点と始点の方向関連
        {
            Vector3 dir = endPos - startPos; // 終点と始点との方向
            length = dir.magnitude; // 距離取得
            dirRad = Mathf.Atan2(dir.y, dir.x); // 角度取得
        }
        List<SpriteRenderer> afterMotionList = new List<SpriteRenderer>(); // 残像リスト
        //AudioSource moveAudio = AudioManager.Instance.PlaySE(moveSe); // 移動音再生
        yield return null; // 1フレーム待機
        // 残像生成
        SpriteRenderer Plsprite = GetComponent<SpriteRenderer>();
        for (float d = length; d > 0.0f; d -= MOVE_AFTER_CREATE_INSTANCE_SPAN)
        {
            GameObject w = new GameObject("fgfh");
            SpriteRenderer renderer = w.AddComponent<SpriteRenderer>(); // 残像取得
            renderer.sprite = Plsprite.sprite;
            renderer.sortingOrder = Plsprite.sortingOrder - 1;
            // 生成位置
            Vector3 pos = endPos - new Vector3(Mathf.Cos(dirRad) * d, Mathf.Sin(dirRad) * d);
            pos.z = Vector3.zero.z; // Z座標を一定値に
            renderer.transform.position = pos; // 位置設定
            afterMotionList.Add(renderer); // リストに追加
        }
        yield return new WaitForSeconds(MOVE_AFTER_ACTIVE_TIME); // 一定時間表示
        // 一定時間ごとに残像削除
        while (afterMotionList.Count > 0)
        {
            yield return new WaitForSeconds(MOVE_AFTER_DESTROY_SPAN); // 一定時間待機
            StartCoroutine(AfterMotionFade(afterMotionList[0])); // 残像透過開始
            afterMotionList.RemoveAt(0); // 先頭の要素を削除s
        }
        // 移動音停止
        //if (moveAudio != null && moveAudio.isPlaying && moveAudio.clip.name == moveSe) moveAudio.Stop();
    }
    IEnumerator AfterMotionFade(SpriteRenderer motion)
    {
        //float MotionTime;
        Color MotionColor = motion.color;
        motion.color = new Color(MotionColor.r, MotionColor.g, MotionColor.b, 0);
        yield return new WaitForSeconds(0.01f);
        motion.color = new Color(MotionColor.r, MotionColor.g, MotionColor.b, 1);
        Destroy(motion.gameObject);
        yield return null;

    }

}

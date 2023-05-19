using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem Coloreffect;
    enum Pldirction
    {
        up,
        Down,
        Left,
        Right
    };
    [SerializeField] Colormaneger Colorscript;
    enum Colorchenge
    {
        red,
        brue,
        yellow,
        purple
    }
    [SerializeField] Sprite[] SprayImaga;
    bool IsEffectStart;
    Pldirction nowstate = Pldirction.up;
    [SerializeField] GameObject Player;
    Vector3 nowdirction;
    Color red = new Color(1, 0, 0, 1);
    Color brue = new Color(0, 0, 1, 1);
    Color yellow = new Color(1, 1, 0, 1);
    Color purple = new Color(0.6f, 0.3f, 1, 1);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        nowdirction = Player.GetComponent<PlMoveAction>().direction;
        if (!IsEffectStart)
        {
            CheckPldir(nowdirction);
            GetComponent<SpriteRenderer>().sprite = SprayImaga[Colorscript.colornum];
            var EffectColor = Coloreffect.main;
            switch (Colorscript.colornum)
            {
                case (int)Colorchenge.red:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(red);
                    break;
                case (int)Colorchenge.brue:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(brue);
                    break;
                case (int)Colorchenge.yellow:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(yellow);
                    break;
                case (int)Colorchenge.purple:
                    EffectColor.startColor = new ParticleSystem.MinMaxGradient(purple);
                    break;
            }
        }

        switch (nowstate)
        {
            case Pldirction.up:
                if (!IsEffectStart)
                {
                    transform.position = Player.transform.position + Vector3.up;
                    transform.Rotate(0, 0, -90);
                    IsEffectStart = true;
                    Coloreffect.Play();
                }
                if (transform.localEulerAngles.z <= 180 && IsEffectStart)
                {
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                break;
            case Pldirction.Down:
                if (!IsEffectStart)
                {
                    transform.position = Player.transform.position + Vector3.down;
                    IsEffectStart = true;
                    transform.Rotate(0, 0, 90);
                    Coloreffect.Play();
                }
                if (transform.localEulerAngles.z <= 0 && IsEffectStart)
                {
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }

                break;
            case Pldirction.Left:
                if (!IsEffectStart)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.position = Player.transform.position + Vector3.left;
                    IsEffectStart = true;
                    transform.Rotate(0, 0, 180);
                    Coloreffect.Play();
                }
                if (transform.localEulerAngles.z <= 90 && IsEffectStart)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }

                break;
            case Pldirction.Right:
                if (!IsEffectStart)
                {
                    transform.position = Player.transform.position + Vector3.right;
                    transform.localScale = new Vector3(-1, 1, 1);

                    IsEffectStart = true;
                    Coloreffect.Play();
                }
                if (transform.localEulerAngles.z <= 268 && IsEffectStart)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    gameObject.SetActive(false);
                    IsEffectStart = false;
                    Coloreffect.Stop();
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                break;
        }
        transform.RotateAround(Player.transform.position, Vector3.forward, -1);
    }
    int CheckPldir(Vector3 Pldir)
    {

        if (Pldir.y < 0)
        {
            nowstate = Pldirction.Down;
            return 1;
        }
        else if (Pldir.y > 0)
        {
            nowstate = Pldirction.up;
            return 0;
        }
        else if (Pldir.x < 0)
        {
            nowstate = Pldirction.Left;
            return 3;
        }
        else if (Pldir.x > 0)
        {
            nowstate = Pldirction.Right;

            return 2;
        }
        return 0;
    }

}

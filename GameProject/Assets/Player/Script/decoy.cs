﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decoy : MonoBehaviour
{
    float Maxtime = 50;

    public enum Colorkind
    {
        red,
        brue,
        yellow,
        purple
    };
    public Colorkind state = Colorkind.red;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, Maxtime);
    }
    private void OnDestroy()
    {
        if ((int)state != 3)
        {
            return;
        }
        GameObject Player = GameObject.Find("Player");

        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        foreach (var e in enemies)
        {
            e.SetPlayer = Player.transform;
        }
        Player.GetComponent<PlMoveAction>().paintEffect.RemoveAt(Player.GetComponent<PlMoveAction>().paintEffect.Count - 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        if (collision.tag != "Player")
        {


            switch (state)
            {
                case Colorkind.red:

                    break;
                case Colorkind.brue:

                    collision.gameObject.GetComponent<EnemyBase>().SetMoveSpeed(1f, Maxtime);

                    break;
                case Colorkind.yellow:

                    collision.gameObject.GetComponent<EnemyBase>().MoveStop(Maxtime);

                    break;
                case Colorkind.purple:
                    break;
            }
        }
    }

}

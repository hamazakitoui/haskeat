using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject Door;
    [SerializeField] GameObject Door2;
    [SerializeField] GameObject LookObject;
    public bool KeyAnimend;
    bool Getkey;
    Animator Lookanim;
    // Start is called before the first frame update
    private void Start()
    {
        Lookanim = LookObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (KeyAnimend)
        {
            Door.transform.GetComponent<Animator>().Play("doorAnimation");
            Door2.transform.GetComponent<Animator>().Play("doorAnimation");
            
            Destroy(gameObject);
            Destroy(LookObject);
            return;

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //gameObject.SetActive(false);
            Destroy(GetComponent<BoxCollider2D>());
            Lookanim.Play("Lookanim");
        }
    }
}

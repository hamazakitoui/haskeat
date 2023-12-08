using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] GameObject ZText;
    [SerializeField] Key key;
    /// <summary> オブジェクト非表示 </summary>
    public void SetActive()
    {
        gameObject.SetActive(false);
    }
    public void Effectend()
    {
        Target.GetComponent<TitleManager>().Effectend = true;
    }
    public void cageend()
    {
        ZText.SetActive(true);
    }
    public void Open()
    {
        key.GetComponent<Key>().KeyAnimend = true ;
    }
}

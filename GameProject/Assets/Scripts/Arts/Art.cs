using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 芸術品 </summary>
public class Art : MonoBehaviour
{
    // アニメ名
    readonly string breakAnim = "Break";
    [SerializeField] ArtType type; // 芸術品のタイプ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary> 画像変更 </summary>
    public void ChangeSprite()
    {
        Animator animator = GetComponent<Animator>(); // アニメーターコンポーネント取得
        if (animator == null) return; // アニメーターコンポーネントがないなら
        animator.SetTrigger(breakAnim); // アニメ変更
    }
    /// <summary> 芸術品の種類 </summary>
    public ArtType GetArtType { get { return type; } }
}

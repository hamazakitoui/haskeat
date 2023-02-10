using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // 破壊した美術品数
    private int pictureSum = 0, potSum = 0, sculpSum = 0, mpSum = 0, msSum = 0;
    // ノルマ
    [SerializeField] int picNolma, potNolma, sculpNolma, mpNolma, msNolma;
    [SerializeField] ClearCondition Condition; // クリア条件
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary> 芸術品破壊 </summary>
    /// <param name="type">破壊した芸術品のタイプ</param>
    public void AddDestroyArt(ArtType type)
    {
        switch (type)
        {
            // 絵画の場合
            case ArtType.Picture:
                pictureSum++;
                break;
            // 壺の場合
            case ArtType.Pot:
                potSum++;
                break;
            // 彫刻の場合
            case ArtType.Sculpture:
                sculpSum++;
                break;
            // 人喰い絵画の場合
            case ArtType.MonsterPicture:
                mpSum++;
                break;
            // 妖魔彫刻の場合
            case ArtType.MonsterSculpture:
                msSum++;
                break;
            default:
                break;
        }
        // クリア条件
        switch (Condition)
        {

            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // 破壊した美術品数
    private int pictureSum = 0, potSum = 0, sculpSum = 0, mpSum = 0, msSum = 0;
    private bool runAway = false; // 逃亡フラグ
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
        // ノルマを達成したか
        {
            bool clear = false; // クリアフラグ
            // クリア条件
            switch (Condition)
            {
                // 絵画を破壊
                case ClearCondition.Picture:
                    if (pictureSum >= picNolma) clear = true;
                    break;
                // 壺を破壊
                case ClearCondition.Pot:
                    if (potSum >= potNolma) clear = true;
                    break;
                // 彫刻を破壊
                case ClearCondition.Sculpture:
                    if (sculpSum >= sculpNolma) clear = true;
                    break;
                default:
                    break;
            }
            if (!runAway && clear) runAway = true; // ノルマを達成したら逃亡する
        }
    }
    /// <summary> ゲームクリア </summary>
    void GameClear()
    {
        Debug.Log("GameClear");
    }
}

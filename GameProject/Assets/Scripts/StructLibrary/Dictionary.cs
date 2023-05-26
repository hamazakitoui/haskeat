using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 辞書 </summary>
public struct Dictionary
{
    /// <summary> プレイヤータグ </summary>
    public const string PLAYER_TAG = "Player";
    /// <summary> エネミータグ </summary>
    public const string ENEMY_TAG = "Enemy";
    /// <summary> 赤スプレー </summary>
    public const string RS_TAG = "redsplay";
    /// <summary> 壁 </summary>
    public const string WALL_TAG = "wall";
}
/// <summary> クリア条件 </summary>
public enum ClearCondition
{
    /// <summary> 絵画を破壊 </summary>
    Picture,
    /// <summary> 壺を破壊 </summary>
    Pot,
    /// <summary>彫刻を破壊  </summary>
    Sculpture,
    /// <summary> 絵画だけ破壊 </summary>
    OnlyPicture,
    /// <summary> 壺だけ破壊 </summary>
    OnlyPot,
    /// <summary> 彫刻だけ破壊 </summary>
    OnlySculpture,
    /// <summary> 絵画と壺を破壊 </summary>
    PictureAndPot,
    /// <summary> 絵画と彫刻を破壊 </summary>
    PictureAndSculpture,
    /// <summary> 壺と彫刻を破壊 </summary>
    PotAndSculpture,
    /// <summary> 付喪神を破壊 </summary>
    Monster,
    /// <summary> 付喪神だけ破壊 </summary>
    OnlyMonster,
    /// <summary> 全ての美術品を破壊 </summary>
    AllArts
}
/// <summary> 美術品の種類 </summary>
public enum ArtType
{
    /// <summary> 絵画 </summary>
    Picture,
    /// <summary> 壺 </summary>
    Pot,
    /// <summary> 彫刻 </summary>
    Sculpture,
    /// <summary> 人喰い絵画 </summary>
    MonsterPicture,
    /// <summary> 妖魔彫刻 </summary>
    MonsterSculpture
}

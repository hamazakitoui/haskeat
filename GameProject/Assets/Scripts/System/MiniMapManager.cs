using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

/// <summary> ミニマップ </summary>
public class MiniMapManager : MonoBehaviour
{
    // 確認方向
    readonly Vector3Int[] dirs =
    {
        Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up, new Vector3Int(-1, -1, 0),
        new Vector3Int(1, -1, 0), new Vector3Int(1, 1, 0), new Vector3Int(-1, 1, 0)
    };
    [SerializeField] Image mapImage; // マップ画像
    // 地面と壁のタイルマップ
    [SerializeField] Tilemap Ground, Wall;
    // マップの色
    [SerializeField] Color groundColor, wallColor, noneColor = Color.clear;
    Texture2D mapTexture = null; // マップテクスチャ
    // Start is called before the first frame update
    void Start()
    {
        Vector3Int mapSize = Wall.size; // マップサイズ
        // テクスチャ生成
        {
            // テクスチャ生成
            mapTexture = new Texture2D(mapSize.x, mapSize.y, TextureFormat.ARGB32, false);
            mapTexture.filterMode = FilterMode.Point; // 画像ぼやけ防止
        }
        Vector3Int origin = Wall.origin; // 中心座標
        // 画像生成
        for(int y = 0; y < mapSize.y; y++)
        {
            for(int x = 0; x < mapSize.x; x++)
            {
                Vector3Int cell = origin + new Vector3Int(x, y, 0); // タイル座標
                cell.z = 0; // Z座標を0固定
                Color color = noneColor; // 設定する色
                // 壁があったら
                if (Wall.GetTile(cell) != null)
                {
                    // 各方向に地面があるか確認
                    for(int d = 0; d < dirs.Length; d++)
                    {
                        if (Ground.GetTile(cell + dirs[d]) != null) color = wallColor;
                    }
                }
                else if (Ground.GetTile(cell) != null) color = groundColor; // 地面があったら
                mapTexture.SetPixel(x, y, color); // ピクセルを埋める
            }
        }
        mapTexture.Apply(); // マップ確定
        // 画像設定
        {
            mapImage.rectTransform.sizeDelta = new Vector2(mapSize.x, mapSize.y);
            mapImage.sprite = Sprite.Create(mapTexture, new Rect
                (Vector2.zero, new Vector2(mapSize.x, mapSize.y)), Vector2.zero);
        }
    }
    void OnDestroy()
    {
        Destroy(mapTexture); // テクスチャ削除
    }
}

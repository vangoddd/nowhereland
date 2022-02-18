using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTexture : MonoBehaviour {
  public MapSO map;

  public Sprite[] tile_alternate;
  public Sprite[] tile_bitmap;
  public float alternate_chance = 0.95f;

  private int bitmaskValue = 15;

  public int tileId;

  private int x, y;

  private SpriteRenderer sr;
  // Start is called before the first frame update
  void Start() {
    sr = GetComponent<SpriteRenderer>();
    x = Mathf.FloorToInt(transform.position.x);
    y = Mathf.FloorToInt(transform.position.y);

    //check bitmask value
    if (tileId != 0) {
      CalculateBitmask();

      switch (bitmaskValue) {
        case 6:
          sr.sprite = tile_bitmap[0];
          break;
        case 14:
          sr.sprite = tile_bitmap[1];
          break;
        case 12:
          sr.sprite = tile_bitmap[2];
          break;
        case 7:
          sr.sprite = tile_bitmap[3];
          break;
        case 13:
          sr.sprite = tile_bitmap[4];
          break;
        case 3:
          sr.sprite = tile_bitmap[5];
          break;
        case 11:
          sr.sprite = tile_bitmap[6];
          break;
        case 9:
          sr.sprite = tile_bitmap[7];
          break;
        case 4:
          sr.sprite = tile_bitmap[8];
          break;
        case 5:
          sr.sprite = tile_bitmap[9];
          break;
        case 1:
          sr.sprite = tile_bitmap[10];
          break;
        case 2:
          sr.sprite = tile_bitmap[11];
          break;
        case 10:
          sr.sprite = tile_bitmap[12];
          break;
        case 8:
          sr.sprite = tile_bitmap[13];
          break;
        case 0:
          sr.sprite = tile_bitmap[14];
          break;
      }
    }

    if (tileId == 0) {
      CalculateWaterBitmask();
      switch (bitmaskValue) {
        case 6:
          sr.sprite = tile_bitmap[0];
          break;
        case 7:
          sr.sprite = tile_bitmap[1];
          break;
        case 3:
          sr.sprite = tile_bitmap[2];
          break;
        case 0:
          bitmaskValue = 15;
          break;
      }
    }


    //replace normal tile with alternate
    if (Random.value > alternate_chance && bitmaskValue == 15) {
      if (tile_alternate.Length != 0) {
        int index = Random.Range(0, tile_alternate.Length);
        sr.sprite = tile_alternate[index];
      }
    }

  }

  void CalculateBitmask() {
    int mapSize = map.mapSize;
    int top, right, bot, left;
    top = (y + 1 >= mapSize) ? 0 : map.tileMap[x][y + 1];
    right = (x + 1 >= mapSize) ? 0 : map.tileMap[x + 1][y];
    bot = (y - 1 < 0) ? 0 : map.tileMap[x][y - 1];
    left = (x - 1 < 0) ? 0 : map.tileMap[x - 1][y];

    bitmaskValue = (top + 2 * right + 4 * bot + 8 * left);
  }

  void CalculateWaterBitmask() {
    int mapSize = map.mapSize;
    int top, tright, tleft;
    if (y + 1 >= mapSize) return;
    top = map.tileMap[x][y + 1];
    tright = (x + 1 >= mapSize) ? 0 : map.tileMap[x + 1][y + 1];
    tleft = (x - 1 < 0) ? 0 : map.tileMap[x - 1][y + 1];

    bitmaskValue = (tleft + 2 * top + 4 * tright);
  }

}

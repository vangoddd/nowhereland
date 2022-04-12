using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalTexture : MonoBehaviour {
  public SpriteRenderer sr;
  public Sprite[] sprites;

  public void SetFace(int face) {
    sr.sprite = sprites[face];
  }

  void Start() {
    ChunkHandlerScript.addObjectToChunk(gameObject);
  }

}

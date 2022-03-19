using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Harvestable {
  public bool ready = true;

  public Sprite[] sprites;
  public SpriteRenderer sr;

  void Start() {
    UpdateSprite();
  }

  void UpdateSprite() {
    if (ready) {
      sr.sprite = sprites[0];
    } else {
      sr.sprite = sprites[1];
    }
  }

  public override void Interact(GameObject player) {
    base.Interact(player);
    Debug.Log("Player interacted with Bush " + gameObject.name + player.transform.position.x.ToString());

    ready = false;
    interactable = false;
    UpdateSprite();
  }
}

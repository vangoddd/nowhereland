using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Harvestable {
  public bool ready = true;

  public Sprite[] sprites;
  public SpriteRenderer sr;

  void Start() {
    InitializeObject();
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
    ItemSpawner.Instance.spawnDrops(transform.position, drops);

    ready = false;
    interactable = false;
    UpdateSprite();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : WorldObject {
  public int regenRate;
  private int regenCounter = 0;
  public bool ready = true;

  public Sprite readySprite;
  public Sprite cooldownSprite;

  public SpriteRenderer sr;

  public TimeSO _timeSO;

  public bool disableHitboxOnCooldown = true;
  public BoxCollider2D boxCollider;

  void Start() {
    InitializeObject();
    if (status == -1) status = regenCounter;
    UpdateSprite();

    _timeSO.OnDayChange.AddListener(DayListener);
  }

  void OnDestroy() {
    _timeSO.OnDayChange.RemoveListener(DayListener);
  }

  void UpdateSprite() {
    if (ready) {
      sr.sprite = readySprite;
      if (disableHitboxOnCooldown) {
        boxCollider.enabled = true;
        sr.sortingOrder = 0;
      }
    } else {
      sr.sprite = cooldownSprite;
      if (disableHitboxOnCooldown) {
        boxCollider.enabled = false;
        sr.sortingOrder = -1;
      }
    }
  }

  public override bool ToolRangeCheck() {
    if (!interactable) return false;
    return true;
  }

  public override void Interact(GameObject player) {
    base.Interact(player);
    ItemSpawner.Instance.spawnDrops(transform.position - offset, drops);

    regenCounter = regenRate;
    status = regenCounter;
    SetReady(false);
  }

  [ContextMenu("Tick Day")]
  private void DayListener(int day) {
    //Debug.Log(gameObject.name + " Listened for day change event : " + transform.position.x + " " + transform.position.y);
    if (regenCounter > 0) {
      regenCounter--;
      if (regenCounter <= 0) {
        SetReady(true);
      }
    }
    status = regenCounter;
  }

  private void SetReady(bool readyStatus) {
    ready = readyStatus;
    interactable = readyStatus;
    UpdateSprite();
  }

  public override void OnDataLoad() {
    regenCounter = status;
    SetReady(regenCounter == 0);
  }
}

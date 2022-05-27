using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItem : MonoBehaviour {
  public Inventory _inventory;
  public ItemInteraction _itemInteraction;
  public SpriteRenderer sr;
  public PlayerMovement pm;
  public Animator _animator;

  public int facing;

  void Start() {
    ApplyData(0);
  }

  void OnEnable() {
    _itemInteraction.OnItemUse.AddListener(ApplyData);
    _itemInteraction.OnUnequip.AddListener(ApplyData);
  }
  void OnDisable() {
    _itemInteraction.OnItemUse.RemoveListener(ApplyData);
    _itemInteraction.OnUnequip.RemoveListener(ApplyData);
  }

  public void ApplyData(int slot) {
    if (_inventory.handSlot != null) {
      sr.sprite = _inventory.handSlot.itemData.sprite;
    } else {
      sr.sprite = null;
    }
  }

  void Update() {
    Vector2 moveDir = pm.getMoveDir();
    _animator.SetFloat("moveX", moveDir.x);
    _animator.SetFloat("moveY", moveDir.y);

    if (facing == 0) {
      transform.localPosition = new Vector3(-0.4f, 0.3f, 0f);
    } else if (facing == 1) {
      transform.localPosition = new Vector3(0.2f, 0.3f, 0f);
    } else if (facing == 2) {
      transform.localPosition = new Vector3(0.4f, 0.3f, 0f);
    } else if (facing == 3) {
      transform.localPosition = new Vector3(-0.2f, 0.3f, 0f);
    }
  }
}

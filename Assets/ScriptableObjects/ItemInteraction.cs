using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ItemInteraction : ScriptableObject {
  public UnityEvent<Item> OnItemPickup;
  public UnityEvent<int> OnItemSlotClicked;
  public UnityEvent<int> OnItemUse;

  private void OnEnable() {
    if (OnItemPickup == null) {
      OnItemPickup = new UnityEvent<Item>();
    }

    if (OnItemSlotClicked == null) {
      OnItemSlotClicked = new UnityEvent<int>();
    }

    if (OnItemUse == null) {
      OnItemUse = new UnityEvent<int>();
    }
  }

  public void PickupItem(Item item) {
    OnItemPickup.Invoke(item);
  }

  public void ItemSlotClicked(int index) {
    OnItemSlotClicked.Invoke(index);
  }

  public void UseItem(int index) {
    OnItemUse.Invoke(index);
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ItemInteraction : ScriptableObject {
  public UnityEvent<ItemScript> OnItemPickup;
  public UnityEvent<int> OnItemSlotClicked;
  public UnityEvent<int> OnItemUse;
  public UnityEvent<int> OnUnequip;

  public UnityEvent<int, int> OnSlotSwap;

  private void OnEnable() {
    if (OnItemPickup == null) {
      OnItemPickup = new UnityEvent<ItemScript>();
    }

    if (OnItemSlotClicked == null) {
      OnItemSlotClicked = new UnityEvent<int>();
    }

    if (OnItemUse == null) {
      OnItemUse = new UnityEvent<int>();
    }

    if (OnUnequip == null) {
      OnUnequip = new UnityEvent<int>();
    }

    if (OnSlotSwap == null) {
      OnSlotSwap = new UnityEvent<int, int>();
    }
  }

  public void PickupItem(ItemScript item) {
    OnItemPickup.Invoke(item);
  }

  public void ItemSlotClicked(int index) {
    OnItemSlotClicked.Invoke(index);
  }

  public void UseItem(int index) {
    OnItemUse.Invoke(index);
  }

  public void Unequip(int index) {
    OnUnequip.Invoke(index);
  }

  public void SlotSwap(int from, int to) {
    OnSlotSwap.Invoke(from, to);
  }

}

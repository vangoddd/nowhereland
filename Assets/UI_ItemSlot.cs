using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour {
  public int slotIndex;
  public ItemInteraction _itemInteraction;

  public void OnSlotClick() {
    _itemInteraction.ItemSlotClicked(slotIndex);
  }
}

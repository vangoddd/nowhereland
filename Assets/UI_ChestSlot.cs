using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ChestSlot : UI_InventorySlot {
  public override void OnDrop(PointerEventData eventData) {

    if (eventData.pointerDrag != null) {
      UI_InventorySlot slot = eventData.pointerDrag.gameObject.GetComponent<UI_InventorySlot>();

      //if dragged item is from inv, add item into chest
      if (slot != null && !(slot is UI_ChestSlot)) {
        _itemInteraction.InvToChest(slot.slotIndex, slotIndex);
      }

      //if dragged item is from chest, swap chest item position
      if (slot != null && slot is UI_ChestSlot) {
        _itemInteraction.ChestSwap(slot.slotIndex, slotIndex);
      }

    }
  }
}

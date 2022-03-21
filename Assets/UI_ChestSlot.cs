using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ChestSlot : UI_InventorySlot {
  public override void OnDrop(PointerEventData eventData) {
    if (eventData.pointerDrag != null) {
      UI_InventorySlot slot = eventData.pointerDrag.gameObject.GetComponent<UI_InventorySlot>();
      if (slot != null) {
        //_itemInteraction.SlotSwap(slot.slotIndex, slotIndex);
      }
    }
  }
}

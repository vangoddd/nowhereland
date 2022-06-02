using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UI_dropSlot : MonoBehaviour, IDropHandler {
  public ItemInteraction _itemInteraction;

  public virtual void OnDrop(PointerEventData eventData) {
    if (eventData.pointerDrag != null) {
      UI_InventorySlot slot = eventData.pointerDrag.gameObject.GetComponent<UI_InventorySlot>();
      if (slot != null && !(slot is UI_ChestSlot)) {
        _itemInteraction.DropItem(slot.slotIndex);
      }
    }
  }
}

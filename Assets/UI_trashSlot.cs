using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UI_trashSlot : MonoBehaviour, IDropHandler {
  public ItemInteraction _itemInteraction;

  public virtual void OnDrop(PointerEventData eventData) {
    if (eventData.pointerDrag != null) {
      UI_InventorySlot slot = eventData.pointerDrag.gameObject.GetComponent<UI_InventorySlot>();
      if (slot != null) {
        _itemInteraction.DeleteItem(slot.slotIndex);
      }
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Overlay : MonoBehaviour, IPointerClickHandler {
  public UI_Inventory inv;

  public void OnPointerClick(PointerEventData eventData) {
    inv.CloseInventory();
  }
}

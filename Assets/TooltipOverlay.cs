using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipOverlay : MonoBehaviour, IPointerClickHandler {
  public UI_Tooltip tooltip;

  public void OnPointerClick(PointerEventData eventData) {
    //tooltip.ShowPanel(false);
  }
}
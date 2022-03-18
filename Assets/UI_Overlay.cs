using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Overlay : MonoBehaviour, IPointerClickHandler {
  public GameObject parent;

  public void OnPointerClick(PointerEventData eventData) {
    parent.SetActive(false);
  }
}

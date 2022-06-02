using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContinueOverlay : MonoBehaviour, IPointerClickHandler {

  public GameEvent OnLoadingClicked;
  public void OnPointerClick(PointerEventData eventData) {
    OnLoadingClicked.Raise();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractButton : MonoBehaviour
// , IPointerDownHandler, IPointerUpHandler 
{
  public GameEvent OnInteractButtonClick;

  public void OnClick() {
    OnInteractButtonClick.Raise();
  }
}

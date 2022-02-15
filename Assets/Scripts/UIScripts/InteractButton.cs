using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  public PlayerMovement pm;

  public void OnPointerDown(PointerEventData eventData)
  {
    transform.localScale = transform.localScale * 0.95f;
    pm.MoveInteract();
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    transform.localScale = Vector3.one;
  }
}

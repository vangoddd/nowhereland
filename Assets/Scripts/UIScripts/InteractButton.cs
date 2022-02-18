using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractButton : MonoBehaviour
// , IPointerDownHandler, IPointerUpHandler 
{

  [SerializeField] private InteractSO _interactSO;

  // public void OnPointerDown(PointerEventData eventData) {
  //   transform.localScale = transform.localScale * 0.95f;
  //   _interactSO.DoInteractMove();
  // }

  // public void OnPointerUp(PointerEventData eventData) {
  //   transform.localScale = Vector3.one;
  // }
}

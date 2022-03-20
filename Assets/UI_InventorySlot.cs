using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {
  public int slotIndex;
  public ItemInteraction _itemInteraction;
  public Image image;
  public Sprite emptySprite;
  public TextMeshProUGUI tmPro;

  private Vector2 originalPos;

  [SerializeField] private RectTransform rt;
  [SerializeField] private Canvas canvas;
  [SerializeField] private CanvasGroup canvasGroup;

  public bool isEmpty = true;

  void Start() {
    originalPos = rt.anchoredPosition;
  }

  public void OnSlotClick() {
    Debug.Log("clicked slot " + slotIndex);
  }

  public void setText(string text) {
    tmPro.text = text;
  }

  public void setSlotEmpty() {
    image.sprite = emptySprite;
    tmPro.text = "";
    isEmpty = true;
  }

  //Drag handling
  public void OnDrag(PointerEventData eventData) {
    rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
  }

  public void OnBeginDrag(PointerEventData eventData) {
    if (isEmpty) {
      eventData.pointerDrag = null;
    } else {
      transform.SetAsLastSibling();
      canvasGroup.blocksRaycasts = false;
    }
  }

  public void OnEndDrag(PointerEventData eventData) {
    rt.anchoredPosition = originalPos;
    canvasGroup.blocksRaycasts = true;
  }

  public void OnDrop(PointerEventData eventData) {
    if (eventData.pointerDrag != null) {
      UI_InventorySlot slot = eventData.pointerDrag.gameObject.GetComponent<UI_InventorySlot>();
      if (slot != null) {
        _itemInteraction.SlotSwap(slot.slotIndex, slotIndex);
      }

      //tools or armor dropped into inventory
      UI_InventoryEquippableContent equippable = eventData.pointerDrag.gameObject.GetComponent<UI_InventoryEquippableContent>();
      if (equippable != null) {

      }
    }
  }
}

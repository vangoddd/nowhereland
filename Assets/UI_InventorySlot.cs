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

  public virtual void OnSlotClick() {
    _itemInteraction.ShowTooltip(slotIndex);
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

  public virtual void OnDrop(PointerEventData eventData) {
    if (eventData.pointerDrag != null) {

      //if item is from inventory, swap normally
      UI_InventorySlot slot = eventData.pointerDrag.gameObject.GetComponent<UI_InventorySlot>();
      if (slot != null && !(slot is UI_ChestSlot)) {
        _itemInteraction.SlotSwap(slot.slotIndex, slotIndex);
      }

      //if item is from chest, add item from chest to inv
      if (slot != null && slot is UI_ChestSlot) {
        _itemInteraction.ChestToInv(slot.slotIndex, slotIndex);
      }
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelectionScript : MonoBehaviour {
  [SerializeField] private RectTransform rectTransform;

  public ItemInteraction _itemInteraction;

  private int initialX = -60;
  private int gap = 24;

  public int selection = 0;

  void OnEnable() {
    _itemInteraction.OnItemSlotClicked.AddListener(ChangeSelection);
  }

  void OnDisable() {
    _itemInteraction.OnItemSlotClicked.RemoveListener(ChangeSelection);
  }

  public void ChangeSelection(int index) {
    if (selection == index) {
      _itemInteraction.UseItem(index);
      Debug.Log("Player using item at index : " + index);
      return;
    }
    rectTransform.localPosition = new Vector3(initialX + (index * gap), rectTransform.localPosition.y, 0f);
    selection = index;
  }

}

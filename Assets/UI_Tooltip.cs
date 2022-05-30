using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Tooltip : MonoBehaviour {
  public RectTransform panelRectTransform;
  public RectTransform backgroundRT;
  public TextMeshProUGUI itemName, type, desc;
  public Image image;

  public float originalPanelSizeY = 34.39f;

  public void UpdateData(Item item) {
    itemName.text = item.itemData.name;
    type.text = item.itemData.name;
    desc.text = item.itemData.description;

    image.sprite = item.itemData.sprite;
    Canvas.ForceUpdateCanvases();
    // LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRT);
    // LayoutRebuilder.ForceRebuildLayoutImmediate(desc.GetComponent<RectTransform>());
  }

  public void ResizePanel() {
    backgroundRT.sizeDelta = new Vector2(backgroundRT.sizeDelta.x, originalPanelSizeY + desc.GetComponent<RectTransform>().sizeDelta.y);
    // Canvas.ForceUpdateCanvases();
    // LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRT);
    // LayoutRebuilder.ForceRebuildLayoutImmediate(desc.GetComponent<RectTransform>());

  }

  //   public void ShowPanel(bool show) {
  //     if (show) {
  //       backgroundRT.GetComponent<CanvasGroup>().alpha = 1f;
  //       backgroundRT.GetComponent<CanvasGroup>().blocksRaycasts = true;
  //     } else {
  //       backgroundRT.GetComponent<CanvasGroup>().alpha = 0f;
  //       backgroundRT.GetComponent<CanvasGroup>().blocksRaycasts = false;
  //     }
  //   }
}

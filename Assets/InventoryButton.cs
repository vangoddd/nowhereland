using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour {

  public GameEvent OnInventoryOpen;

  public void OnClick() {
    OnInventoryOpen.Raise();
  }
}

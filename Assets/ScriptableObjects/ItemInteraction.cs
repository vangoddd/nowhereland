using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ItemInteraction : ScriptableObject {
  public UnityEvent<Item> OnItemPickup;

  private void OnEnable() {
    if (OnItemPickup == null) {
      OnItemPickup = new UnityEvent<Item>();
    }
  }

  public void PickupItem(Item item) {
    OnItemPickup.Invoke(item);
  }

}

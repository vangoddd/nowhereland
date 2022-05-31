using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Item Interaction")]
public class ItemInteraction : ScriptableObject {
  public UnityEvent<ItemScript> OnItemPickup;
  public UnityEvent<int> OnItemSlotClicked;
  public UnityEvent<int> OnItemUse;
  public UnityEvent<int> OnUnequip;
  public UnityEvent<int> OnItemDrop;
  public UnityEvent<int> OnItemDelete;

  public UnityEvent<int> OnChestOpen;
  public UnityEvent<int> OnShowTooltip;

  public UnityEvent<int, int> OnSlotSwap;

  public UnityEvent<int, int> OnInvToChest;
  public UnityEvent<int, int> OnChestToInv;
  public UnityEvent<int, int> OnChestSwap;

  public UnityEvent<Placeable, int> OnItemPlaceAction;

  public UnityEvent<bool> OnPlaceableSelected;

  public UnityEvent OnPickupNull;

  private void OnEnable() {
    if (OnItemPickup == null) {
      OnItemPickup = new UnityEvent<ItemScript>();
    }

    if (OnItemSlotClicked == null) {
      OnItemSlotClicked = new UnityEvent<int>();
    }

    if (OnItemUse == null) {
      OnItemUse = new UnityEvent<int>();
    }

    if (OnShowTooltip == null) {
      OnShowTooltip = new UnityEvent<int>();
    }

    if (OnUnequip == null) {
      OnUnequip = new UnityEvent<int>();
    }

    if (OnSlotSwap == null) {
      OnSlotSwap = new UnityEvent<int, int>();
    }

    if (OnChestOpen == null) {
      OnChestOpen = new UnityEvent<int>();
    }

    if (OnInvToChest == null) {
      OnInvToChest = new UnityEvent<int, int>();
    }

    if (OnChestToInv == null) {
      OnChestToInv = new UnityEvent<int, int>();
    }

    if (OnChestSwap == null) {
      OnChestSwap = new UnityEvent<int, int>();
    }

    if (OnItemPlaceAction == null) {
      OnItemPlaceAction = new UnityEvent<Placeable, int>();
    }

    if (OnPlaceableSelected == null) {
      OnPlaceableSelected = new UnityEvent<bool>();
    }

    if (OnPickupNull == null) {
      OnPickupNull = new UnityEvent();
    }

    if (OnItemDrop == null) {
      OnItemDrop = new UnityEvent<int>();
    }

    if (OnItemDelete == null) {
      OnItemDelete = new UnityEvent<int>();
    }

  }

  public void PickupItem(ItemScript item) {
    OnItemPickup.Invoke(item);
  }

  public void ItemSlotClicked(int index) {
    OnItemSlotClicked.Invoke(index);
  }

  public void UseItem(int index) {
    OnItemUse.Invoke(index);
  }

  public void Unequip(int index) {
    OnUnequip.Invoke(index);
  }

  public void SlotSwap(int from, int to) {
    OnSlotSwap.Invoke(from, to);
  }

  public void OpenChest(int chestId) {
    OnChestOpen.Invoke(chestId);
  }

  public void ChestToInv(int from, int to) {
    OnChestToInv.Invoke(from, to);
  }

  public void InvToChest(int from, int to) {
    OnInvToChest.Invoke(from, to);
  }

  public void ChestSwap(int from, int to) {
    OnChestSwap.Invoke(from, to);
  }

  public void PlaceItemAction(Placeable item, int index) {
    OnItemPlaceAction.Invoke(item, index);
  }

  public void PlaceableSelected(bool selection) {
    OnPlaceableSelected.Invoke(selection);
  }

  public void PickupNull() {
    OnPickupNull.Invoke();
  }

  public void DropItem(int slot) {
    OnItemDrop.Invoke(slot);
  }

  public void DeleteItem(int slot) {
    OnItemDelete.Invoke(slot);
  }

  public void ShowTooltip(int slot) {
    OnShowTooltip.Invoke(slot);
  }

}

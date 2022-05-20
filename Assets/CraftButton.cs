using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftButton : MonoBehaviour {
  public GameEvent OnStarterCraft;
  public void OnCraftButtonClick() {
    OnStarterCraft.Raise();
  }
}

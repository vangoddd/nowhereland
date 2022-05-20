using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveListener : MonoBehaviour {
  public void SaveGame() {
    SaveSystem.Instance.SaveGame();
  }

  public void DeleteSave() {
    SaveSystem.Instance.DeleteSave();
  }
}

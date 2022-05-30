using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton : MonoBehaviour {
  public GameObject PauseOverlay;

  public void OnResume() {
    PauseOverlay.SetActive(false);
  }

  public void OnSave() {
    SaveSystem.Instance.SaveGame();
    OnResume();
  }

  public void OnQuit() {
    SceneManager.UnloadSceneAsync(1);
    SceneManager.LoadScene(0);
  }
}

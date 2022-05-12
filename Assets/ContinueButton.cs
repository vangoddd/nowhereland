using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour {
  public GameObject loadingScreen;
  public StartMode mode;

  public void onContinueClick() {
    mode.loadGame = true;
    //SceneManager.UnloadSceneAsync(1);
    SceneManager.LoadSceneAsync(1);
    loadingScreen.SetActive(true);
    Destroy(this.gameObject);
  }
}

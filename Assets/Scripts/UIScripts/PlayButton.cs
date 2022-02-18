using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
  public GameObject loadingScreen;

  public void onPlayClick() {
    //SceneManager.UnloadSceneAsync(1);
    SceneManager.LoadSceneAsync(1);
    loadingScreen.SetActive(true);
    Destroy(this.gameObject);
  }
}

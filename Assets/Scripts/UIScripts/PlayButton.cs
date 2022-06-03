using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
  public GameObject loadingScreen;
  public GameObject FadeScreen;

  public void onPlayClick() {
    Instantiate(FadeScreen).GetComponent<ScreenFade>().SetAction(StartLoading);
  }

  public void StartLoading() {
    SceneManager.LoadSceneAsync(1);
    Instantiate(loadingScreen);
  }
}

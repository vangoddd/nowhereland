using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton : MonoBehaviour {
  public GameObject PauseOverlay;
  public GameObject LoadingScreen;
  public GameObject FadeScreen;

  public void OnResume() {
    PauseOverlay.SetActive(false);
  }

  public void OnSave() {
    SaveSystem.Instance.SaveGame();
    OnResume();
  }

  public void OnQuit() {
    Instantiate(FadeScreen).GetComponent<ScreenFade>().SetAction(OnQuitAction);
  }

  public void OnQuitAction() {
    SceneManager.LoadSceneAsync(0);
    //Instantiate(LoadingScreen);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour {
  public GameObject loadingScreen;
  public StartMode mode;
  public Button button;

  public void onContinueClick() {
    mode.loadGame = true;
    //SceneManager.UnloadSceneAsync(1);
    SceneManager.LoadSceneAsync(1);
    loadingScreen.SetActive(true);
    Destroy(this.gameObject);
  }

  void Start() {
    string path = Application.persistentDataPath + "/save.dat";
    button.interactable = File.Exists(path);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class ContinueButton : MonoBehaviour {
  public GameObject loadingScreen;
  public StartMode mode;
  public Button button;

  public TextMeshProUGUI survivedText;

  public void onContinueClick() {
    mode.loadGame = true;
    //SceneManager.UnloadSceneAsync(1);
    SceneManager.LoadSceneAsync(1);
    loadingScreen.SetActive(true);
    Destroy(this.gameObject);
  }

  void Start() {
    string path = Application.persistentDataPath + "/save.dat";
    if (File.Exists(path)) {
      button.interactable = true;

      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);
      SaveGameData data = formatter.Deserialize(stream) as SaveGameData;

      survivedText.text = data._worldData.day.ToString() + " Days";
    } else {
      button.interactable = false;
      survivedText.text = "";
    }
  }
}

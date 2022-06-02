using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUI : MonoBehaviour {
  public LoadingEvent _loadingEvent;
  public RectTransform loadingtext;
  public GameObject TouchToContinueScreen;
  public GameObject LoadingBar;

  public TextMeshProUGUI loadingText;

  private void OnEnable() {
    _loadingEvent.OnLoadingFinishedEvent.AddListener(OnLoadingFinished);
  }

  private void OnDisable() {
    _loadingEvent.OnLoadingFinishedEvent.RemoveListener(DisableLoading);
  }

  private void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }

  public void OnLoadingFinished() {
    TouchToContinueScreen.SetActive(true);
    loadingtext.gameObject.SetActive(false);
    LoadingBar.gameObject.SetActive(false);
  }

  public void DisableLoading() {
    gameObject.SetActive(false);
    Destroy(gameObject);
  }

  void Update() {
    loadingText.text = _loadingEvent.loadingStatus;
  }
}

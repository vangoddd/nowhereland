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

  float loadingProgress;
  public Image progressBar;

  public GameObject ScreenFade;

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
    Instantiate(ScreenFade).GetComponent<ScreenFade>().SetAction(DisableLoadingAction);
  }

  public void DisableLoadingAction() {
    gameObject.SetActive(false);
    Destroy(this.gameObject);
  }

  void Update() {
    if (_loadingEvent.loadingStatus != "") {
      loadingText.text = _loadingEvent.loadingStatus;
    }
    loadingProgress = (float)_loadingEvent.loadingCount / (float)_loadingEvent.loadingTotal;
    progressBar.fillAmount = loadingProgress;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {
  public LoadingEvent _loadingEvent;
  public RectTransform loadingtext;
  private void OnEnable() {
    _loadingEvent.OnLoadingFinishedEvent.AddListener(DisableLoading);
    loadingtext.anchoredPosition += new Vector2(0f, -5f);
    LeanTween.moveY(loadingtext, loadingtext.anchoredPosition.y + 10f, 2f).setEaseInOutSine().setLoopPingPong();
  }

  private void OnDisable() {
    _loadingEvent.OnLoadingFinishedEvent.RemoveListener(DisableLoading);
  }

  private void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }

  private void DisableLoading() {
    gameObject.SetActive(false);
    Destroy(gameObject);
  }
}

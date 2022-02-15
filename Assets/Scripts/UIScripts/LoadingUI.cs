using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
  public LoadingEvent _loadingEvent;
  private void OnEnable()
  {
    _loadingEvent.OnLoadingFinishedEvent.AddListener(DisableLoading);
  }

  private void OnDisable()
  {
    _loadingEvent.OnLoadingFinishedEvent.RemoveListener(DisableLoading);
  }

  private void Awake()
  {
    DontDestroyOnLoad(this.gameObject);
  }

  private void DisableLoading()
  {
    gameObject.SetActive(false);
  }
}

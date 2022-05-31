using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Loading Event")]
public class LoadingEvent : ScriptableObject {
  public bool isFinished = false;

  public UnityEvent OnLoadingFinishedEvent;

  private void OnEnable() {
    if (OnLoadingFinishedEvent == null) {
      OnLoadingFinishedEvent = new UnityEvent();
    }
  }

  public void FinishLoading() {
    isFinished = true;
    OnLoadingFinishedEvent.Invoke();
  }
}

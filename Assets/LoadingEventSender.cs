using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingEventSender : MonoBehaviour {
  public LoadingEvent _loadingEvent;
  void Start() {
    _loadingEvent.FinishLoading();
  }

}

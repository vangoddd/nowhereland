using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
  private static TimeManager _instance;

  public static TimeManager Instance {
    get {
      if (_instance is null) {

      }
      return _instance;
    }
  }

  private void Awake() {
    _instance = this;
  }

  private float timer = 0f;

  [SerializeField] private TimeSO _timeSO;

  private void Update() {
    timer += Time.deltaTime;
    if (timer >= _timeSO.secondsPerTick) {
      timer -= _timeSO.secondsPerTick;
      _timeSO.Tick();
    }
  }
}

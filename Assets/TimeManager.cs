using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour {
  private static TimeManager _instance;
  public bool isPaused = false;

  public InputActionAsset playerAction;

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

  public void PauseGame() {
    Time.timeScale = 0;
    playerAction.Disable();
    isPaused = true;
  }

  public void ResumeGame() {
    Time.timeScale = 1;
    playerAction.Enable();
    isPaused = false;
  }

  public void togglePause() {
    if (isPaused) {
      ResumeGame();
    } else {
      PauseGame();
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class TimeSO : ScriptableObject {
  public UnityEvent<int> OnDayChange;
  public UnityEvent<int> OnTick;
  public UnityEvent OnNightChange;
  public UnityEvent<WorldData> OnGameLoad;

  public int tick, day;
  public float secondsPerDay;
  public float secondsPerTick;
  public float nightRatio;

  private int ticksPerDay;
  private int tickToNight;

  public float transitionTime = 5f;

  public bool isDay = true;

  private void OnEnable() {
    ResetValues();
  }

  public void ResetValues() {
    tick = 0;
    day = 1;
    isDay = true;

    ticksPerDay = Mathf.FloorToInt(secondsPerDay / secondsPerTick);
    tickToNight = Mathf.FloorToInt(ticksPerDay * (1f - nightRatio));

    if (OnDayChange == null) {
      OnDayChange = new UnityEvent<int>();
    }

    if (OnTick == null) {
      OnTick = new UnityEvent<int>();
    }

    if (OnNightChange == null) {
      OnNightChange = new UnityEvent();
    }

    if (OnGameLoad == null) {
      OnGameLoad = new UnityEvent<WorldData>();
    }
  }

  public void Tick() {
    tick++;
    if (tick >= tickToNight && isDay) ChangeToNight();
    if (tick >= ticksPerDay) ChangeDay();
    OnTick.Invoke(tick);
  }

  public void ChangeDay() {
    isDay = true;
    tick = 0;
    day++;
    OnDayChange.Invoke(day);
  }

  public void ChangeToNight() {
    isDay = false;
    OnNightChange.Invoke();
  }

  public void ApplyLoadedData(WorldData data) {
    tick = data.tick;
    day = data.day;
    isDay = data.isDay;

    OnGameLoad.Invoke(data);
  }
}

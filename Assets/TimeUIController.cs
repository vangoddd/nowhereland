using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUIController : MonoBehaviour {
  [SerializeField] TextMeshProUGUI day;
  [SerializeField] TextMeshProUGUI timeOfDay;
  [SerializeField] TimeSO _timeSO;

  void OnEnable() {
    _timeSO.OnDayChange.AddListener(changeDay);
    _timeSO.OnGameLoad.AddListener(LoadEvent);
    _timeSO.OnIngameHourTick.AddListener(IngameHourTickListener);

    changeDay(_timeSO.day);
  }
  void OnDisable() {
    _timeSO.OnDayChange.RemoveListener(changeDay);
    _timeSO.OnGameLoad.RemoveListener(LoadEvent);
    _timeSO.OnIngameHourTick.RemoveListener(IngameHourTickListener);
  }

  void changeDay(int dayData) {
    day.text = "Day " + dayData.ToString();
  }

  void LoadEvent(WorldData data) {
    day.text = "Day " + data.day.ToString();
  }

  void IngameHourTickListener(string time) {
    timeOfDay.text = time;
  }

  public void UpdateUIOnSceneLoad() {
    changeDay(_timeSO.day);
    IngameHourTickListener(_timeSO.CalculateInGameHour(_timeSO.inGameMinuteCounter));
  }
}

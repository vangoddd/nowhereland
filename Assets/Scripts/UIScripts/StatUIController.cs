using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIController : MonoBehaviour {
  public Image healthImage, hungerImage, thristImage;
  public PlayerStats _playerStats;
  [SerializeField] private TimeSO _timeSO;

  [SerializeField] private RectTransform daylightWheel;

  //subscribe to event
  void OnEnable() {
    _playerStats.OnStatChangeEvent.AddListener(UpdateStat);
    _timeSO.OnDayChange.AddListener(ChangeWheelToDay);
    _timeSO.OnNightChange.AddListener(ChangeWheelToNight);
  }

  void OnDisable() {
    _playerStats.OnStatChangeEvent.RemoveListener(UpdateStat);
    _timeSO.OnDayChange.RemoveListener(ChangeWheelToDay);
    _timeSO.OnNightChange.RemoveListener(ChangeWheelToNight);
  }

  void UpdateStat(PlayerStatData data) {
    healthImage.fillAmount = GetFloatPercentage(data.health, _playerStats.maxStat);
    hungerImage.fillAmount = GetFloatPercentage(data.hunger, _playerStats.maxStat);
    thristImage.fillAmount = GetFloatPercentage(data.thirst, _playerStats.maxStat);
  }

  private float GetFloatPercentage(float val, float maxVal) {
    return (val / maxVal);
  }

  void ChangeWheelToNight() {
    LeanTween.value(gameObject, 0f, 180f, _timeSO.transitionTime).setOnUpdate((float val) => {
      if (Mathf.FloorToInt(val) % (180 / 5) == 0) daylightWheel.eulerAngles = new Vector3(0f, 0f, val);
    });
  }

  void ChangeWheelToDay(int day) {
    LeanTween.value(gameObject, 180f, 360f, _timeSO.transitionTime).setOnUpdate((float val) => {
      if (Mathf.FloorToInt(val) % (180 / 5) == 0) daylightWheel.eulerAngles = new Vector3(0f, 0f, val);
    });
  }

 

}

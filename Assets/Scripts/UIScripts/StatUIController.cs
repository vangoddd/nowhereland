using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIController : MonoBehaviour
{
  public Image healthImage, hungerImage, thristImage;
  public PlayerStats _playerStats;

  //subscribe to event
  void OnEnable()
  {
    _playerStats.OnStatChangeEvent.AddListener(UpdateStat);
  }

  void OnDisable()
  {
    _playerStats.OnStatChangeEvent.RemoveListener(UpdateStat);
  }

  void UpdateStat(PlayerStatData data)
  {
    healthImage.fillAmount = GetFloatPercentage(data.health, _playerStats.maxStat);
    hungerImage.fillAmount = GetFloatPercentage(data.hunger, _playerStats.maxStat);
    thristImage.fillAmount = GetFloatPercentage(data.thirst, _playerStats.maxStat);
  }

  private float GetFloatPercentage(float val, float maxVal)
  {
    return (val / maxVal);
  }

}

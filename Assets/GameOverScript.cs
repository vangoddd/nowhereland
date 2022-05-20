using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScript : MonoBehaviour {
  public TextMeshProUGUI daySurvived;
  public TimeSO _timeSO;

  void OnEnable() {
    UpdateText();
  }

  void UpdateText() {
    daySurvived.text = "Survived : " + _timeSO.day.ToString() + "days,";
  }
}

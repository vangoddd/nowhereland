using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DaylightController : MonoBehaviour {
  [SerializeField] private Light2D sun;
  [SerializeField] private TimeSO _timeSO;

  [SerializeField] private Color nightColor = new Color(0.5f, 0.7f, 1f);
  [SerializeField] private Color dayColor = Color.white;

  [SerializeField] private float nightIntensity = 0.15f;
  [SerializeField] private float dayIntensity = 1f;

  // Start is called before the first frame update
  void OnEnable() {
    _timeSO.OnDayChange.AddListener(ChangeToDayTime);
    _timeSO.OnNightChange.AddListener(ChangeToNightTime);
  }

  void OnDisable() {
    _timeSO.OnDayChange.RemoveListener(ChangeToDayTime);
    _timeSO.OnNightChange.RemoveListener(ChangeToNightTime);
  }

  void ChangeToDayTime(int day) {
    // sun.color = dayColor;
    // sun.intensity = 1f;
    LeanTween.value(gameObject, nightIntensity, dayIntensity, _timeSO.transitionTime).setOnUpdate((float val) => {
      sun.intensity = val;
    });

    LeanTween.value(gameObject, nightColor, dayColor, _timeSO.transitionTime).setOnUpdate((Color val) => {
      sun.color = dayColor;
    });
  }

  void ChangeToNightTime() {
    // sun.color = nightColor;
    // sun.intensity = 0.15f;

    LeanTween.value(gameObject, dayIntensity, nightIntensity, _timeSO.transitionTime).setOnUpdate((float val) => {
      sun.intensity = val;
    });

    LeanTween.value(gameObject, dayColor, nightColor, _timeSO.transitionTime).setOnUpdate((Color val) => {
      sun.color = dayColor;
    });
  }

}

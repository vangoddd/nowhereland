using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SO/Health Interaction")]
public class HealthInteraction : ScriptableObject {
  public UnityEvent<float> OnPlayerHurt;

  private void OnEnable() {
    if (OnPlayerHurt == null) {
      OnPlayerHurt = new UnityEvent<float>();
    }
  }

  public void HurtPlayer(float amt) {
    OnPlayerHurt.Invoke(amt);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
  public PlayerStats _playerStat;
  public float temp;

  public bool godmode = false;

  void OnEnable()
  {
    _playerStat.OnPlayerDie.AddListener(Die);
  }
  void OnDisable()
  {
    _playerStat.OnPlayerDie.RemoveListener(Die);
  }

  void Update()
  {
    if (godmode) return;

    float reductionAmt = -2 * Time.deltaTime;
    _playerStat.addStat(new PlayerStatData(reductionAmt, reductionAmt, reductionAmt));
  }

  void Die()
  {
    _playerStat.OnPlayerDie.RemoveListener(Die);
    Debug.Log("Player died.");
  }

  public void ToggleGodMode()
  {
    godmode = !godmode;
    Debug.Log("Godmode : " + godmode);
  }
}

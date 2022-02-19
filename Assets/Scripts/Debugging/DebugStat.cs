using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugStat : MonoBehaviour {
  public Text text;
  private string health, hunger, thirst, temp;

  private PlayerStatScript ps;
  // Start is called before the first frame update
  void Start() {
    ps = GameManagerScript.Instance.player.GetComponent<PlayerStatScript>();
  }

  // Update is called once per frame
  void Update() {
    // health = ps.health.ToString();
    // hunger = ps.hunger.ToString();
    // thirst = ps.thirst.ToString();

    text.text = "Health " + health + "\nHunger " + hunger + "\nthirst " + thirst;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour {
  public GameObject cursorGraphic;
  public PlayerMovement pm;

  void Update() {
    cursorGraphic.SetActive(pm.isMoving());
    GameObject target = pm.isAttacking();
    if (target != null) cursorGraphic.transform.position = target.transform.position;
    else cursorGraphic.transform.position = RoundVector(pm.GetTargetPos());
  }

  Vector2 RoundVector(Vector2 pos) {
    return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
  }
}

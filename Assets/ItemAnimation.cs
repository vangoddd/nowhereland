using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour {
  void Start() {
    float offset = Mathf.Repeat(transform.position.x, 2f);
    int tweenId = LeanTween.moveY(gameObject, transform.position.y + .2f, 2f).setPassed(0.5f).setEaseInOutSine().setLoopPingPong().id;
    LTDescr descr = LeanTween.descr(tweenId);
    descr.setPassed(offset);
  }
}

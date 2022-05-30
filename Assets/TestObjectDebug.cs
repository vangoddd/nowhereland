using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectDebug : MonoBehaviour {
  public GameObject playerObject;

  void Start() {
    transform.position = playerObject.transform.position;
  }


  void Update() {

  }
}

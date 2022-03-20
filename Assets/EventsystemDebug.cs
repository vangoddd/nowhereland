using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EventsystemDebug : MonoBehaviour {
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    // if (EventSystem.current.IsPointerOverGameObject()) {
    //   Debug.Log("Mouse Over: " + EventSystem.current.currentSelectedGameObject.name);
    // }

    PointerEventData pointerData = new PointerEventData(EventSystem.current) {
      pointerId = -1,
    };

    pointerData.position = Mouse.current.position.ReadValue();
    List<RaycastResult> results = new List<RaycastResult>();

    EventSystem.current.RaycastAll(pointerData, results);

    if (results.Count > 0) {
      foreach (RaycastResult r in results) {
        Debug.Log(r.gameObject.name);
      }
    }
  }
}

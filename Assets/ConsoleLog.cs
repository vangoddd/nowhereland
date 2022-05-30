using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleLog : MonoBehaviour {
  string error;
  public Text text;

  int counter = 0;

  void OnEnable() {
    Application.logMessageReceived += HandleLog;
    text.text = "";
  }

  void OnDisable() {
    Application.logMessageReceived -= HandleLog;
  }

  void HandleLog(string logString, string stackTrace, LogType type) {

    //if (type == LogType.Error) {
    error = error + "\n" + logString;
    text.text = error;
    //}

    counter++;
    if (counter > 5) {
      error = logString;
      counter = 0;
    }
  }
}

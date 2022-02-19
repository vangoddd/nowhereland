using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour {
  [SerializeField]
  bool showConsole;
  string input;

  public static DebugCommand TEST;
  public static DebugCommand GODMODE;
  public static DebugCommand<float> CHANGESPEED;
  public static DebugCommand<float, float> TP;
  public List<object> commandList;

  void Awake() {
    TEST = new DebugCommand("test", "Just a test command", "test", () => {
      Debug.Log("Test command working, NICE");
    });

    GODMODE = new DebugCommand("godmode", "toggle godmode", "godmode", () => {
      PlayerStatScript playerStat = GameManagerScript.Instance.player.GetComponent<PlayerStatScript>();
      playerStat.ToggleGodMode();

    });

    CHANGESPEED = new DebugCommand<float>("changespeed", "change player speed", "changespeed <float>", (x) => {
      GameManagerScript.Instance.player.GetComponent<PlayerMovement>().setSpeed(x);
    });

    TP = new DebugCommand<float, float>("tp", "teleport to pos", "changespeed <float> <float>", (x, y) => {
      GameManagerScript.Instance.player.GetComponent<PlayerMovement>().Teleport(x, y);
    });

    commandList = new List<object>{
          TEST,
          GODMODE,
          CHANGESPEED,
          TP,
      };
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Semicolon)) {
      Debug.Log("pressing semicolon");
      showConsole = !showConsole;
    }

    if (Input.GetKeyDown(KeyCode.Return)) {
      if (showConsole) {
        HandleInput();
        input = "";
        showConsole = !showConsole;
      }

    }


  }

  private void OnGUI() {
    if (!showConsole) { return; }

    float y = 0;

    GUI.Box(new Rect(0, y, Screen.width, 30), "");

    input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
  }

  void HandleInput() {
    string[] prop = input.Split(' ');

    for (int i = 0; i < commandList.Count; i++) {
      DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
      if (input.Contains(commandBase.commandId)) {
        if (commandList[i] as DebugCommand != null) {
          (commandList[i] as DebugCommand).Invoke();
        } else if (commandList[i] as DebugCommand<float> != null) {
          (commandList[i] as DebugCommand<float>).Invoke(float.Parse(prop[1]));
        } else if (commandList[i] as DebugCommand<float, float> != null) {
          (commandList[i] as DebugCommand<float, float>).Invoke(float.Parse(prop[1]), float.Parse(prop[2]));
        }
      }
    }
  }
}

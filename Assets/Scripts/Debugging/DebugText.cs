using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
  public MapGenerator mg;
  public Text text;
  public PlayerStats _stat;
  public TimeSO _time;

  public float deltaTime;

  void Update() {
    text.text = "Seed : " + mg.seed + ", mapsize : " + mg.mapSize +
    "\nchunk : " + ChunkHandlerScript.getChunkFromPosition(_stat.position.x, _stat.position.y)
    + "\npos : " + (int)_stat.position.x + " " + (int)_stat.position.y
    + "\ntick : " + _time.tick
    + "\nfps : " + getFPS()
    + "\nversion : " + Application.version;
  }

  string getFPS() {
    deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    float fps = 1.0f / deltaTime;
    return Mathf.Ceil(fps).ToString();
  }
}

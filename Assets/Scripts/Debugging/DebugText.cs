using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
  public MapGenerator mg;
  public Text text;
  // Start is called before the first frame update
  void Start()
  {
    text.text = "Seed : " + mg.seed;
  }

  // Update is called once per frame
  void Update()
  {

  }
}

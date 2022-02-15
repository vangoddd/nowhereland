using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
  private static GameManagerScript _instance;

  public static GameManagerScript Instance
  {
    get
    {
      if (_instance is null)
      {

      }
      return _instance;
    }
  }

  public GameObject player;
  public MapGenerator mapGenerator;

  private void Awake()
  {
    _instance = this;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
  public GameObject player;

  public float lerpValue = 10f;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void FixedUpdate()
  {
    Vector3 finalPos = player.transform.position;
    finalPos.z = transform.position.z;
    Vector3 lerp = Vector3.Lerp(transform.position, finalPos, lerpValue * Time.deltaTime);

    transform.position = lerp;
    // Vector3 playerpos = player.transform.position;
    // playerpos.z = transform.position.z;
    // transform.position = playerpos;
  }
}

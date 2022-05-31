using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    Resize();
  }

  // Update is called once per frame
  void Update()
  {

  }

  void Resize()
  {
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr == null) return;

    transform.localScale = new Vector3(1, 1, 1);

    float width = sr.sprite.bounds.size.x;
    float height = sr.sprite.bounds.size.y;

    float worldScreenHeight = Camera.main.orthographicSize * 2f;
    float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

    Vector3 scale = transform.localScale;
    float scaleAmt;
    // if (worldScreenHeight / worldScreenWidth >= 1)
    // {
    //   scaleAmt = worldScreenWidth / width
    // }
    // else
    // {
    //   scaleAmt = worldScreenHeight / height;
    // }
    scaleAmt = Mathf.Max(worldScreenWidth / width, worldScreenHeight / height);
    scale.x = scaleAmt;
    scale.y = scaleAmt;

    transform.localScale = scale;

  }

}

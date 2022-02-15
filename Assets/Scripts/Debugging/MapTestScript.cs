using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTestScript : MonoBehaviour
{
  public MapGenerator mapGenerator;
  public RawImage _rawImage;
  // Start is called before the first frame update
  void Start()
  {
    _rawImage = GetComponent<RawImage>();
  }

  // Update is called once per frame
  void Update()
  {
    _rawImage.texture = mapGenerator.mapTexture;
  }
}

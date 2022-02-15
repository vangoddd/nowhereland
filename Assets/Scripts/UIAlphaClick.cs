using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAlphaClick : MonoBehaviour
{
  private Image image;
  private void OnEnable()
  {
    image = GetComponent<Image>();

    if (image)
    {
      image.alphaHitTestMinimumThreshold = 0.001f;
    }
  }
}

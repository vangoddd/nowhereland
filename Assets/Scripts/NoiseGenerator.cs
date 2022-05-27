using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator {
  public List<List<float>> rawNoiseData;
  private int size;

  public void GenerateNoise(int mapSize, int seed, float magnification, int octaves, float persistance, float lacunarity) {
    rawNoiseData = new List<List<float>>();
    size = mapSize;

    System.Random pseudoRandom = new System.Random(seed.ToString().GetHashCode());

    Vector2[] offsets = new Vector2[octaves];
    for (int i = 0; i < octaves; i++) {
      int x_offset = pseudoRandom.Next(-10000, 10000);
      int y_offset = pseudoRandom.Next(-10000, 10000);
      offsets[i] = new Vector2(x_offset, y_offset);
    }

    float minHeight = 0f;
    float maxHeight = 0f;

    float halfSize = mapSize / 2f;

    for (int x = 0; x < mapSize; x++) {
      rawNoiseData.Add(new List<float>());
      for (int y = 0; y < mapSize; y++) {

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++) {
          float sampleX = (x - halfSize) / magnification * frequency + offsets[i].x;
          float sampleY = (y - halfSize) / magnification * frequency + offsets[i].y;

          float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
          //float perlinValue = Mathf.PerlinNoise(x, y);

          noiseHeight += perlinValue * amplitude;

          amplitude *= persistance; // 0 < persistance < 1
          frequency *= lacunarity; // lacunarity > 1
        }

        if (x == 0 && y == 0) {
          minHeight = noiseHeight;
          maxHeight = noiseHeight;
        }

        if (noiseHeight < minHeight) {
          minHeight = noiseHeight;
        }

        if (noiseHeight > maxHeight) {
          maxHeight = noiseHeight;
        }

        rawNoiseData[x].Add(noiseHeight);

        //Debug.Log("Noise height :" + noiseHeight);

      }
    }

    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        rawNoiseData[x][y] = Mathf.InverseLerp(minHeight, maxHeight, rawNoiseData[x][y]);
      }
    }
  }

  public void CalculateDistribution(int accuracy){
    int multiplier = 1;
    for(int i = 0; i < accuracy; i++){
      multiplier *= 10;
    }

    int[] occurence = new int[multiplier+1];

    for(int x = 0; x < size; x++){
      for(int y = 0; y < size; y++){
        occurence[Mathf.RoundToInt(rawNoiseData[x][y] * (float) multiplier)]++;
      }
    }

    for(int x = 0; x <= multiplier; x++){
      Debug.Log(occurence[x]);
    }


    // float f = 10.123456F;
    // float fc = (float)Math.Round(f * 100f) / 100f;
    // MessageBox.Show(fc.ToString());
  }
}

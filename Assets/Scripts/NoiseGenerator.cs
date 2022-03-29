using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator {
  public List<List<float>> rawNoiseData;

  public void GenerateNoise(int mapSize, int seed, float magnification, int octaves, float persistance, float lacunarity) {
    rawNoiseData = new List<List<float>>();

    System.Random pseudoRandom = new System.Random(seed.ToString().GetHashCode());

    Random.InitState(seed.ToString().GetHashCode());

    int x_offset = pseudoRandom.Next(-10000, 10000);
    int y_offset = pseudoRandom.Next(-10000, 10000);

    float minHeight = float.MaxValue;
    float maxHeight = float.MinValue;

    for (int x = 0; x < mapSize; x++) {
      rawNoiseData.Add(new List<float>());
      for (int y = 0; y < mapSize; y++) {

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++) {
          float sampleX = (x + x_offset) / magnification * frequency;
          float sampleY = (y + y_offset) / magnification * frequency;

          float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
          //float perlinValue = Mathf.PerlinNoise(x, y);

          noiseHeight += perlinValue * amplitude;

          amplitude *= persistance; // 0 < persistance < 1
          frequency *= lacunarity; // lacunarity > 1
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
}

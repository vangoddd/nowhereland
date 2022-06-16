using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class FileIOHandler : MonoBehaviour {
  public static FileIOHandler Instance;

  public string basePath = @"v:\nowhereland test\";
  public string TexturePath = @"v:\nowhereland test\tex\";
  string fileExt = ".txt";

  void Start() {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  public void WriteMapData(MapSO map) {
    string outputPath = basePath + map.seed.ToString() + fileExt;

    Debug.Log("Writing map data to : " + outputPath);

    using (StreamWriter sw = new StreamWriter(outputPath)) {
      sw.WriteLine(getListFrom2DArray(map.tileMap, map.mapSize));
      sw.WriteLine(getFinalTileIdList(map));
      sw.WriteLine(getWorldObjectList(map));
    }
  }

  string getListFrom2DArray(List<List<int>> data, int mapSize) {
    string list = "";

    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        list += data[x][y].ToString();
      }
    }

    return list;
  }

  string getFinalTileIdList(MapSO map) {
    string list = "";

    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        if (map.tileMap[x][y] == 1) {
          list += (map.tileMap[x][y] + map.biomes[x][y]).ToString();
        } else {
          list += map.tileMap[x][y].ToString();
        }
      }
    }

    return list;
  }

  string getWorldObjectList(MapSO map) {
    string[,] list = new string[map.mapSize, map.mapSize];
    string finalList = "";

    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        list[x, y] = "-1";
      }
    }

    foreach (WorldObjectData item in map.worldObjectDatas) {
      list[(int)item.position[0], (int)item.position[1]] = item.objectID.ToString("00");
    }

    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        finalList += list[x, y];
      }
    }

    return finalList;
  }

  public void SaveTextureImage(Texture2D tex, int seed) {
    byte[] bytes = tex.EncodeToPNG();
    var dirPath = basePath + seed.ToString() + ".png";
    File.WriteAllBytes(dirPath, bytes);
  }

  public void SaveAllTexture(List<Texture2D> texList, int seed) {
    int counter = 0;
    foreach (Texture2D tex in texList) {
      byte[] bytes = tex.EncodeToPNG();
      var dirPath = TexturePath + seed.ToString() + "_" + counter.ToString() + ".png";
      File.WriteAllBytes(dirPath, bytes);
      counter++;
    }
  }
}

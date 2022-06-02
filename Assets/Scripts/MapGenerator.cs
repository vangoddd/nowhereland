using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
  private int currentSeed = 0;

  [Header("Save system")]
  public bool loadFromSave = false;
  public bool randomSeed = false;
  [Space(10)]

  [Header("Base Noise Setting")]
  public int mapSize;
  public int seed = 0;
  public int octaves;
  public float magnification;
  public float presistance;
  public float lacunarity;
  public float cutoff;
  [Space(10)]

  [Header("Biome Noise Setting")]
  public int biome_octaves;
  public float biome_magnification;
  public float biome_presistance;
  public float biome_lacunarity;
  [Space(10)]

  [Header("Generated Textures")]
  public Texture2D noise1;
  public Texture2D noise2;
  public Texture2D noise3;
  public Texture2D noiseBiome1;
  public Texture2D noiseBiome2;
  //map texture
  public Texture2D mapTexture;
  [Space(10)]

  [Header("References")]
  public MapSO map;
  public TilesetDatabase tileset;
  public WorldObjectDB worldObjectDB;
  public GameObject[] decals;
  public WorldObjectDB[] naturalObjPerBiomeDB;

  public float[] biomeWeight;
  public float[] biomeObjectSpawnrate;
  public SetpieceData[] setpieceDatas;
  public GameEvent OnTextureCreated;

  [Space(10)]

  Color[] gradientPixels;
  public Texture2D levelGradient;

  private List<WorldObjectData> worldObjectDatas = new List<WorldObjectData>();
  //To notify loading screen if the generation is complete
  public LoadingEvent _loadingEvent;
  public StartMode startMode;
  public GameEvent OnChunkChanged;
  public GameEvent OnMapDataGenerated;

  private List<TileTexture> runtimeTiles = new List<TileTexture>();

  public PlayerMovement _pm;

  private bool[,] tileUnavailable;

  /*----------------------------------------*/

  void Awake() {
    map.mapSize = mapSize;
    map.ResetValues();

    tileUnavailable = new bool[mapSize, mapSize];

    loadFromSave = startMode.loadGame;
    if (!loadFromSave && randomSeed) seed = Random.Range(0, 100000);
  }

  void Start() {
    StartCoroutine(SpawnCoroutine());
  }

  IEnumerator SpawnCoroutine() {
    TimeManager.Instance.PauseGame();

    Vector2 startPos = new Vector2(mapSize / 2, mapSize / 2);
    Camera.main.transform.position = new Vector3(mapSize / 2, mapSize / 2, -100f);

    GetGradientColors();

    if (!loadFromSave) {
      InitiateSeed();
      map.mapSize = mapSize;
      GenerateMap();
      _loadingEvent.loadingStatus = "Generating Map Data";

      yield return StartCoroutine(InstantiateTiles());

      _loadingEvent.loadingStatus = "Spawning Setpiece";
      SpawnSetPiece();

      yield return null;

      OnMapDataGenerated.Raise();
      _loadingEvent.loadingStatus = "Generating Object Data";

      GenerateWorldObject();

      yield return null;
      _loadingEvent.loadingStatus = "Spawning Objects";
      yield return StartCoroutine(SpawnObjects());

      yield return null;

      _pm.gameObject.transform.position = startPos;

    } else {
      _loadingEvent.loadingStatus = "Loading Game";
      SaveSystem.Instance.LoadGame();
      InitiateSeed();
      mapSize = map.mapSize;

      yield return StartCoroutine(InstantiateTiles());

      yield return null;

      OnMapDataGenerated.Raise();
      _loadingEvent.loadingStatus = "Spawning Objects";

      yield return StartCoroutine(SpawnObjects());

      yield return null;

    }

    mapTexture = new Texture2D(map.mapSize, map.mapSize, TextureFormat.ARGB32, false);
    GenerateMapTexture();

    Camera.main.Render();

    yield return new WaitForSecondsRealtime(0.5f);
    Debug.Log("loading done");

    OnChunkChanged.Raise();
    _loadingEvent.loadingStatus = "";
    _loadingEvent.FinishLoading();
  }

  void GetGradientColors() {
    gradientPixels = levelGradient.GetPixels();
  }

  float GetGradientFloat(int x, int y) {
    float ratio = 1000f / map.mapSize;
    int newX = Mathf.RoundToInt(x * ratio);
    int newY = Mathf.RoundToInt(y * ratio);
    Color pixelColor = gradientPixels[newX * 1000 + newY];
    return pixelColor.grayscale;
  }


  void InitiateSeed() {
    if (loadFromSave) {
      seed = map.seed;
    }
    currentSeed = seed.ToString().GetHashCode();
    Random.InitState(currentSeed);

    map.seed = seed;

    Debug.Log("seed : " + seed);
  }

  float[] CalculateBiomeThreshold() {
    // Calculate biome threshold
    float[] biomeThreshold = new float[tileset.tiles.Count - 1];
    float total = 0;
    for (int i = 0; i < tileset.tiles.Count - 1; i++) {
      total += biomeWeight[i];
    }
    for (int i = 0; i < tileset.tiles.Count - 1; i++) {
      if (i == 0) biomeThreshold[i] = biomeWeight[i] / total;
      else biomeThreshold[i] = biomeThreshold[i - 1] + (biomeWeight[i] / total);
    }
    // for (int i = 0; i < tileset.tiles.Count - 1; i++) {
    //   Debug.Log(biomeThreshold[i]);
    // }
    return biomeThreshold;
  }

  void GenerateMap() {
    for (int x = 0; x < map.mapSize; x++) {
      map.tileMap.Add(new List<int>());
      map.biomes.Add(new List<int>());
    }

    //generate base noise
    NoiseGenerator baseNoise = new NoiseGenerator();
    baseNoise.GenerateNoise(map.mapSize, seed, magnification, octaves, presistance, lacunarity);
    map.rawNoiseData = baseNoise.rawNoiseData;

    noise1 = GeneratePartTexture(map.rawNoiseData);

    //multiply noise with gradient
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        float gradFloat = GetGradientFloat(x, y);
        float finalNoise = map.rawNoiseData[x][y] * gradFloat;
        //finalNoise = (finalNoise + gradFloat / 2) / 2;
        map.rawNoiseData[x][y] = finalNoise;

        //getting tile ID from noise data
        int tileId = getTileFromPerlin(finalNoise);
        map.tileMap[x].Add(tileId);
      }
    }

    noise2 = GeneratePartTexture(map.rawNoiseData);
    noise3 = GeneratePartTexturePosterized(map.rawNoiseData);

    //generate biome noise
    NoiseGenerator biomeNoise = new NoiseGenerator();
    biomeNoise.GenerateNoise(map.mapSize, seed, biome_magnification, biome_octaves, biome_presistance, biome_lacunarity);
    //biomeNoise.CalculateDistribution(1);

    noiseBiome1 = GeneratePartTexture(biomeNoise.rawNoiseData);

    float[] biomeThreshold = CalculateBiomeThreshold();

    //get biome data from biome noise
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        // int biomeID = Mathf.FloorToInt(biomeNoise.rawNoiseData[x][y] * (tileset.tiles.Count - 1));
        // if (biomeID == tileset.tiles.Count - 1) biomeID--;

        int biomeID = 0;

        for (int i = 0; i < tileset.tiles.Count - 1; i++) {
          if (biomeNoise.rawNoiseData[x][y] <= biomeThreshold[i]) {
            biomeID = i;
            break;
          }
        }

        map.biomes[x].Add(biomeID);
      }
    }

    noiseBiome2 = GenerateBiomeTexture(map.biomes);
    int radius = 12;
    for (int x = mapSize / 2 - radius; x < mapSize / 2 + radius; x++) {
      for (int y = mapSize / 2 - radius; y < mapSize / 2 + radius; y++) {
        if ((x - mapSize / 2) * (x - mapSize / 2) + (y - mapSize / 2) * (y - mapSize / 2) < radius * radius) {
          map.biomes[x][y] = 0;
          map.tileMap[x][y] = 1;
        }
      }
    }



    CountBiomes();
  }

  IEnumerator InstantiateTiles() {
    _loadingEvent.loadingStatus = "Generating Tiles";
    int counter = 0;
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        SpawnTile(map.tileMap[x][y], x, y);
        counter++;
        if (counter >= 1000) {
          counter = 0;
          yield return null;

        }
      }
    }
    GenerateDecal();

    yield return new WaitForSecondsRealtime(0.5f);

  }

  int getTileFromPerlin(float perlinValue) {
    //if water tile
    if (perlinValue < 0.35f) {
      return 0;
    } else {
      return 1;
    }
  }

  void SpawnTile(int tileId, int x, int y) {
    GameObject tilePrefab;

    if (tileId == 1) {
      tilePrefab = tileset.tiles[tileId + map.biomes[x][y]];
    } else {
      tilePrefab = tileset.tiles[tileId];
    }

    GameObject tile = Instantiate(tilePrefab);
    tile.SetActive(false);

    int chunkIndex = Mathf.FloorToInt(x / map.chunkSize) + (Mathf.FloorToInt(y / map.chunkSize) * (map.mapSize / map.chunkSize));
    map.chunks[chunkIndex].Add(tile);

    tile.transform.position = new Vector3(x, y, 0);
    tile.name = string.Format("Tile_({0}, {1})", x, y);

    runtimeTiles.Add(tile.GetComponent<TileTexture>());

  }

  void GenerateDecal() {
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        if (map.tileMap[x][y] == 0) continue;
        int currentBiome = map.biomes[x][y];
        GameObject decal;
        //check up
        if (y + 1 < mapSize && map.biomes[x][y + 1] > currentBiome && map.tileMap[x][y + 1] != 0) {
          decal = Instantiate(decals[map.biomes[x][y + 1] - 1]);
          decal.GetComponent<DecalTexture>().SetFace(0);
          decal.transform.position = new Vector2(x, y);
        }
        //check right
        if (x + 1 < mapSize && map.biomes[x + 1][y] > currentBiome && map.tileMap[x + 1][y] != 0) {
          decal = Instantiate(decals[map.biomes[x + 1][y] - 1]);
          decal.GetComponent<DecalTexture>().SetFace(1);
          decal.transform.position = new Vector2(x, y);
        }
        //check down
        if (y - 1 >= 0 && map.biomes[x][y - 1] > currentBiome && map.tileMap[x][y - 1] != 0) {
          decal = Instantiate(decals[map.biomes[x][y - 1] - 1]);
          decal.GetComponent<DecalTexture>().SetFace(2);
          decal.transform.position = new Vector2(x, y);
        }
        //check left
        if (x - 1 >= 0 && map.biomes[x - 1][y] > currentBiome && map.tileMap[x - 1][y] != 0) {
          decal = Instantiate(decals[map.biomes[x - 1][y] - 1]);
          decal.GetComponent<DecalTexture>().SetFace(3);
          decal.transform.position = new Vector2(x, y);
        }
      }
    }
  }

  void SpawnSetPiece() {
    if (loadFromSave) return;
    List<List<Vector2>> biomeTiles = new List<List<Vector2>>();

    for (int i = 0; i < tileset.tiles.Count - 1; i++) {
      biomeTiles.Add(new List<Vector2>());
    }

    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        if (map.tileMap[x][y] == 1) biomeTiles[map.biomes[x][y]].Add(new Vector2(x, y));
      }
    }

    for (int i = 0; i < tileset.tiles.Count - 1; i++) {
      if (setpieceDatas[i].objects.Length > 0) {
        for (int j = 0; j < setpieceDatas[i].objects.Length; j++) {
          for (int k = 0; k < setpieceDatas[i].objects[j].amount; k++) {
            if (biomeTiles[i].Count == 0) break;
            int randomNum = Random.Range(0, biomeTiles[i].Count);
            GameObject go = Instantiate(setpieceDatas[i].objects[j].prefab);
            go.transform.position = new Vector3(biomeTiles[i][randomNum].x, biomeTiles[i][randomNum].y, 0);
            tileUnavailable[(int)biomeTiles[i][randomNum].x, (int)biomeTiles[i][randomNum].y] = true;
            biomeTiles[i].RemoveAt(randomNum);
          }
        }
      }
    }
  }

  void GenerateWorldObject() {
    for (int x = 0; x < map.mapSize; x += 2) {
      for (int y = 0; y < map.mapSize; y += 2) {
        if (tileUnavailable[x, y]) continue;
        if (Random.value > biomeObjectSpawnrate[map.biomes[x][y]] && map.tileMap[x][y] == 1) {
          float randomChoice = Random.Range(0f, 1f);
          int randomID = 0;
          for (int i = 0; i < naturalObjPerBiomeDB[map.biomes[x][y]].spawnWeightThreshold.Length; i++) {
            if (randomChoice <= naturalObjPerBiomeDB[map.biomes[x][y]].spawnWeightThreshold[i]) {
              randomID = i;
              break;
            }
          }
          Vector2 position = new Vector2(x, y);

          int worldObjectId = worldObjectDB.objectLookup[naturalObjPerBiomeDB[map.biomes[x][y]].worldObjects[randomID]];
          worldObjectDatas.Add(new WorldObjectData(worldObjectId, position, -1));
        }
      }
    }

    map.worldObjectDatas = worldObjectDatas;
  }

  IEnumerator SpawnObjects() {
    int counter = 0;
    foreach (WorldObjectData data in map.worldObjectDatas) {
      GameObject wo = Instantiate(worldObjectDB.worldObjects[data.objectID]);
      wo.transform.position = new Vector3(data.position[0], data.position[1], 0);
      WorldObject woScript = wo.GetComponent<WorldObject>();
      woScript.status = data.status;
      if (loadFromSave) woScript.OnDataLoad();

      counter++;
      if (counter >= 1000) {
        counter = 0;
        yield return null;
      }
    }
    yield return null;
  }

  void GenerateMapTexture() {
    mapTexture.filterMode = FilterMode.Point;
    Color blue = Color.blue;
    Color green = Color.green;
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        if (map.tileMap[x][y] == 0) mapTexture.SetPixel(x, y, new Color(0.38f, 0.49f, 0.68f));
        else {
          if (map.biomes[x][y] == 0) mapTexture.SetPixel(x, y, new Color(0.53f, 0.69f, 0.57f));
          if (map.biomes[x][y] == 1) mapTexture.SetPixel(x, y, new Color(0.94f, 0.51f, 0.50f));
          if (map.biomes[x][y] == 2) mapTexture.SetPixel(x, y, new Color(0.52f, 0.52f, 0.52f));
          if (map.biomes[x][y] == 3) mapTexture.SetPixel(x, y, new Color(0.95f, 0.87f, 0.72f));
        }
      }
    }
    mapTexture.Apply();

    OnTextureCreated.Raise();
  }

  public Texture2D GeneratePartTexture(List<List<float>> data) {
    Texture2D texture = new Texture2D(map.mapSize, map.mapSize, TextureFormat.ARGB32, false);
    texture.filterMode = FilterMode.Point;

    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        texture.SetPixel(x, y, getColorFromFloat(data[x][y]));
      }
    }

    return texture;
  }

  public Texture2D GeneratePartTexturePosterized(List<List<float>> data) {
    Texture2D texture = new Texture2D(map.mapSize, map.mapSize, TextureFormat.ARGB32, false);
    texture.filterMode = FilterMode.Point;

    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        texture.SetPixel(x, y, getColorFromFloatPosterized(data[x][y], 0.35f));
      }
    }

    return texture;
  }

  public Texture2D GenerateBiomeTexture(List<List<int>> data) {
    Texture2D texture = new Texture2D(map.mapSize, map.mapSize, TextureFormat.ARGB32, false);
    texture.filterMode = FilterMode.Point;

    Color[] colorList = new Color[tileset.tiles.Count - 1];

    for (int i = 0; i < tileset.tiles.Count - 1; i++) {
      colorList[i] = Random.ColorHSV();
    }

    colorList[0] = Color.green;
    colorList[1] = Color.red;
    colorList[2] = Color.gray;
    colorList[3] = Color.yellow;


    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        texture.SetPixel(x, y, colorList[map.biomes[x][y]]);
      }
    }

    return texture;
  }

  private Color getColorFromFloat(float val) {
    return new Color(val, val, val, 1f);
  }

  private Color getColorFromFloatPosterized(float val, float cutoff) {
    if (val > cutoff) {
      return Color.white;
    } else {
      return Color.black;
    }
  }

  [ContextMenu("Apply Tile Texture")]
  public void ApplyTileTexture() {
    foreach (TileTexture tile in runtimeTiles) {
      tile.ApplyTexture();
    }
  }

  private void CountBiomes() {
    int biomeCount = tileset.tiles.Count - 1;
    int[] occurence = new int[biomeCount];
    for (int i = 0; i < biomeCount; i++) {
      occurence[i] = 0;
    }

    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        occurence[map.biomes[x][y]]++;
      }
    }

    for (int i = 0; i < biomeCount; i++) {
      Debug.Log(i + " : " + occurence[i] + " occurence");
    }
  }

}

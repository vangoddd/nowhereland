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
  public WorldObjectDB worldObjectDB, naturalObjDB;
  public GameObject[] decals;
  public WorldObjectDB[] naturalObjPerBiomeDB;

  Color[] gradientPixels;
  public Texture2D levelGradient;

  private List<WorldObjectData> worldObjectDatas = new List<WorldObjectData>();
  //To notify loading screen if the generation is complete
  public LoadingEvent _loadingEvent;
  public StartMode startMode;

  private List<TileTexture> runtimeTiles = new List<TileTexture>();

  /*----------------------------------------*/

  void Awake() {
    map.ResetValues();

    loadFromSave = startMode.loadGame;
    if (!loadFromSave && randomSeed) seed = Random.Range(0, 100000);
  }

  void Start() {
    TimeManager.Instance.ResumeGame();

    GetGradientColors();

    //map.ResetValues();

    if (!loadFromSave) {
      InitiateSeed();
      map.mapSize = mapSize;
      GenerateMap();
      GenerateWorldObject();
      SpawnObjects();
    } else {
      SaveSystem.Instance.LoadGame();
      InitiateSeed();
      mapSize = map.mapSize;
      InstantiateTiles();
      SpawnObjects();
    }

    mapTexture = new Texture2D(map.mapSize, map.mapSize, TextureFormat.ARGB32, false);
    GenerateMapTexture();

    Camera.main.Render();

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
        finalNoise = (finalNoise + gradFloat / 2) / 2;
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

    noiseBiome1 = GeneratePartTexture(biomeNoise.rawNoiseData);

    //get biome data from biome noise
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        int biomeID = Mathf.FloorToInt(biomeNoise.rawNoiseData[x][y] * (tileset.tiles.Count - 1));
        if (biomeID == tileset.tiles.Count - 1) biomeID--;

        map.biomes[x].Add(biomeID);
      }
    }

    noiseBiome2 = GenerateBiomeTexture(map.biomes);
    int radius = 30;
    for (int x = mapSize / 2 - radius; x < mapSize / 2 + radius; x++) {
      for (int y = mapSize / 2 - radius; y < mapSize / 2 + radius; y++) {
        if ((x - mapSize / 2) * (x - mapSize / 2) + (y - mapSize / 2) * (y - mapSize / 2) < radius * radius) {
          map.biomes[x][y] = 0;
          map.tileMap[x][y] = 1;
        }
      }
    }

    InstantiateTiles();
  }

  void InstantiateTiles() {
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        SpawnTile(map.tileMap[x][y], x, y);
      }
    }
    GenerateDecal();
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

  }

  void GenerateWorldObject() {
    for (int x = 0; x < map.mapSize; x += 2) {
      for (int y = 0; y < map.mapSize; y += 2) {
        if (Random.value > 0.95 && map.tileMap[x][y] == 1) {
          int randChoice = Random.Range(0, naturalObjPerBiomeDB[map.biomes[x][y]].worldObjects.Count);
          Vector2 position = new Vector2(x, y);

          int worldObjectId = worldObjectDB.objectLookup[naturalObjPerBiomeDB[map.biomes[x][y]].worldObjects[randChoice]];
          worldObjectDatas.Add(new WorldObjectData(worldObjectId, position, -1));
        }
      }
    }

    map.worldObjectDatas = worldObjectDatas;
  }

  void SpawnObjects() {
    SpawnSetPiece();

    foreach (WorldObjectData data in map.worldObjectDatas) {
      GameObject wo = Instantiate(worldObjectDB.worldObjects[data.objectID]);
      wo.transform.position = new Vector3(data.position[0], data.position[1], 0);
      WorldObject woScript = wo.GetComponent<WorldObject>();
      woScript.status = data.status;
      if (woScript is Destroyable && !(woScript is Chest)) {
        if (woScript.status != -1) ((Destroyable)woScript).health = woScript.status;
      }
    }
  }

  void GenerateMapTexture() {
    mapTexture.filterMode = FilterMode.Point;
    Color blue = Color.blue;
    Color green = Color.green;
    for (int x = 0; x < map.mapSize; x++) {
      for (int y = 0; y < map.mapSize; y++) {
        if (map.tileMap[x][y] == 0) mapTexture.SetPixel(x, y, blue);
        else mapTexture.SetPixel(x, y, green);
      }
    }
    mapTexture.Apply();
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

  private float GenerateNoiseHeight(float amp, float freq, int octaves, float presistance, float lacunarity) {
    return 0f;
  }

  [ContextMenu("Apply Tile Texture")]
  public void ApplyTileTexture() {
    foreach (TileTexture tile in runtimeTiles) {
      tile.ApplyTexture();
    }
  }

}

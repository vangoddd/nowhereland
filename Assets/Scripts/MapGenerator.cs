using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
  public MapSO map;

  public int seed = 0;
  private int currentSeed = 0;

  public static int mapSize = 300;
  public static int chunkSize = 30;

  public static List<List<GameObject>> chunks = new List<List<GameObject>>();
  public static List<List<int>> tileMap = new List<List<int>>();
  public static List<List<int>> biomes = new List<List<int>>();
  public static List<GameObject> worldObjects = new List<GameObject>();

  List<List<float>> rawNoiseData = new List<List<float>>();
  Dictionary<int, GameObject> tileset;
  Color[] gradientPixels;

  public float magnification = 12.0f;
  public float biomeMagnigication;
  public float biomeScale = 2f;

  int x_offset = 0;
  int y_offset = 0;
  int biome_x_offset = 0;
  int biome_y_offset = 0;

  public Texture2D levelGradient;

  public GameObject tile_water;
  public GameObject tile_grass;
  public GameObject tile_redland;

  public GameObject test;

  //World object to be spawned
  public GameObject[] worldObjectPrefab;

  //map texture
  public Texture2D mapTexture;

  //To notify loading screen if the generation is complete
  public LoadingEvent _loadingEvent;

  void Awake() {
    for (int i = 0; i < (mapSize * mapSize / (chunkSize * chunkSize)); i++) {
      chunks.Add(new List<GameObject>());
    }
  }

  // Start is called before the first frame update
  void Start() {
    biomeMagnigication = magnification * biomeScale;
    GetGradientColors();
    CreateTileset();
    InitiateSeed();
    GenerateMap();
    SpawnObjects();

    mapTexture = new Texture2D(mapSize, mapSize, TextureFormat.ARGB32, false);
    GenerateMapTexture();

    Camera.main.Render();

    _loadingEvent.FinishLoading();
  }

  void GetGradientColors() {
    gradientPixels = levelGradient.GetPixels();
  }

  float GetGradientFloat(int x, int y) {
    float ratio = 1000f / mapSize;
    int newX = Mathf.RoundToInt(x * ratio);
    int newY = Mathf.RoundToInt(y * ratio);
    Color pixelColor = gradientPixels[newX * 1000 + newY];
    return pixelColor.grayscale;
  }

  void CreateTileset() {
    tileset = new Dictionary<int, GameObject>();
    tileset.Add(0, tile_water);
    tileset.Add(1, tile_grass);
    tileset.Add(2, tile_redland);
  }

  void InitiateSeed() {
    currentSeed = seed.ToString().GetHashCode();
    Random.InitState(currentSeed);

    x_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
    y_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
    biome_x_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
    biome_y_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
  }

  void GenerateMap() {
    for (int x = 0; x < mapSize; x++) {
      tileMap.Add(new List<int>());
      biomes.Add(new List<int>());
      rawNoiseData.Add(new List<float>());

      for (int y = 0; y < mapSize; y++) {
        //Generate noise and save the raw noise data
        float rawNoise = Mathf.PerlinNoise((x - x_offset) / magnification, (y - y_offset) / magnification);
        float clampedNoise = Mathf.Clamp01(rawNoise);

        //Add gradient to ensure outside is water
        float gradFloat = GetGradientFloat(x, y);
        float finalNoise = clampedNoise * gradFloat;
        finalNoise = (finalNoise + gradFloat / 2) / 2;
        rawNoiseData[x].Add(finalNoise);

        //getting tile ID and saving it in a 2D array
        int tileId = getTileFromPerlin(finalNoise);
        tileMap[x].Add(tileId);

        //Generate biome data using another perlin
        float biomeNoise = Mathf.PerlinNoise((x - biome_x_offset) / biomeMagnigication, (y - biome_y_offset) / biomeMagnigication);
        biomeNoise = Mathf.Clamp01(biomeNoise);

        //divide the noise to <biome count> part
        int biomeID = Mathf.FloorToInt(biomeNoise * (tileset.Count - 1));
        if (biomeID == tileset.Count - 1) biomeID--;

        //Save biome data to 2D array
        biomes[x].Add(biomeID);
      }
    }
    InstantiateTiles();
  }

  void InstantiateTiles() {
    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        SpawnTile(tileMap[x][y], x, y);
      }
    }
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
      tilePrefab = tileset[tileId + biomes[x][y]];
    } else {
      tilePrefab = tileset[tileId];
    }

    GameObject tile = Instantiate(tilePrefab);
    tile.SetActive(false);

    int chunkIndex = Mathf.FloorToInt(x / chunkSize) + (Mathf.FloorToInt(y / chunkSize) * (mapSize / chunkSize));
    chunks[chunkIndex].Add(tile);

    tile.transform.position = new Vector3(x, y, 0);
    tile.name = string.Format("Tile_({0}, {1})", x, y);

  }

  void SpawnSetPiece() {

  }

  void SpawnObjects() {
    SpawnSetPiece();

    for (int x = 0; x < mapSize; x += 2) {
      for (int y = 0; y < mapSize; y += 2) {
        if (Random.value > 0.99 && tileMap[x][y] == 1) {
          int randChoice = Random.Range(0, worldObjectPrefab.Length);
          GameObject wo = Instantiate(worldObjectPrefab[randChoice]);
          wo.transform.position = new Vector3(x, y, 0);
          worldObjects.Add(wo);
        }
      }
    }
  }

  void GenerateMapTexture() {
    mapTexture.filterMode = FilterMode.Point;
    Color blue = Color.blue;
    Color green = Color.green;
    for (int x = 0; x < mapSize; x++) {
      for (int y = 0; y < mapSize; y++) {
        if (tileMap[x][y] == 0) mapTexture.SetPixel(x, y, blue);
        else mapTexture.SetPixel(x, y, green);
      }
    }
    mapTexture.Apply();
    //mapTexture.SetPixel(0, 0, Color.blue);
  }

  private float GenerateNoiseHeight(float amp, float freq, int octaves, float presistance, float lacunarity) {
    return 0f;
  }

}

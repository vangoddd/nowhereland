using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
  public MapSO map;
  public TilesetDatabase tileset;
  public WorldObjectDB worldObjectDB;

  private List<WorldObjectData> worldObjectDatas = new List<WorldObjectData>();

  private int currentSeed = 0;
  public int seed = 0;

  Color[] gradientPixels;

  public float magnification = 12.0f;
  public float biomeMagnigication;
  public float biomeScale = 2f;

  int x_offset = 0;
  int y_offset = 0;
  int biome_x_offset = 0;
  int biome_y_offset = 0;

  public Texture2D levelGradient;

  //map texture
  public Texture2D mapTexture;

  //To notify loading screen if the generation is complete
  public LoadingEvent _loadingEvent;

  public bool loadFromSave = false;

  void Awake() {
    for (int i = 0; i < (map.mapSize * map.mapSize / (map.chunkSize * map.chunkSize)); i++) {
      map.chunks.Add(new List<GameObject>());
    }
  }

  void Start() {
    biomeMagnigication = magnification * biomeScale;
    InitiateSeed();
    GetGradientColors();

    if (!loadFromSave) {
      GenerateMap();
      GenerateWorldObject();
      SpawnObjects();
    } else {
      SaveSystem.Instance.LoadGame();
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

    x_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
    y_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
    biome_x_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
    biome_y_offset = Mathf.FloorToInt(Random.Range(-10000f, 10000f));
  }

  void GenerateMap() {
    for (int x = 0; x < map.mapSize; x++) {
      //initializing containers
      map.tileMap.Add(new List<int>());
      map.biomes.Add(new List<int>());
      map.rawNoiseData.Add(new List<float>());

      for (int y = 0; y < map.mapSize; y++) {
        //Generate noise and save the raw noise data
        float rawNoise = Mathf.PerlinNoise((x - x_offset) / magnification, (y - y_offset) / magnification);
        float clampedNoise = Mathf.Clamp01(rawNoise);

        //Add gradient to ensure outside is water
        float gradFloat = GetGradientFloat(x, y);
        float finalNoise = clampedNoise * gradFloat;
        finalNoise = (finalNoise + gradFloat / 2) / 2;
        map.rawNoiseData[x].Add(finalNoise);

        //getting tile ID and saving it in a 2D array
        int tileId = getTileFromPerlin(finalNoise);
        map.tileMap[x].Add(tileId);

        //Generate biome data using another perlin
        float biomeNoise = Mathf.PerlinNoise((x - biome_x_offset) / biomeMagnigication, (y - biome_y_offset) / biomeMagnigication);
        biomeNoise = Mathf.Clamp01(biomeNoise);

        //divide the noise to <biome count> part
        int biomeID = Mathf.FloorToInt(biomeNoise * (tileset.tiles.Count - 1));
        if (biomeID == tileset.tiles.Count - 1) biomeID--;

        //Save biome data to 2D array
        map.biomes[x].Add(biomeID);
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

  }

  void SpawnSetPiece() {

  }

  void GenerateWorldObject() {
    for (int x = 0; x < map.mapSize; x += 2) {
      for (int y = 0; y < map.mapSize; y += 2) {
        if (Random.value > 0.99 && map.tileMap[x][y] == 1) {
          int randChoice = Random.Range(0, worldObjectDB.worldObjects.Count);
          Vector2 position = new Vector2(x, y);
          worldObjectDatas.Add(new WorldObjectData(randChoice, position));
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

      //should add obj to worldobjects from the object itself
      //wo.GetComponent<WorldObject>().objectID = data.objectID;
      //map.worldObjects.Add(wo);
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

  private float GenerateNoiseHeight(float amp, float freq, int octaves, float presistance, float lacunarity) {
    return 0f;
  }

}

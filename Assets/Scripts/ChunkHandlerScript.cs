using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandlerScript : MonoBehaviour {
  public static MapSO map;
  public MapSO _map;
  public GameObject player;
  public static List<int> activeChunks = new List<int>();

  public bool unloadChunk = true;

  private int playerChunk;

  void Start() {
    map = _map;
    playerChunk = getChunkFromPosition(player.transform.position.x, player.transform.position.y);
  }

  public void UpdateLoadedChunk() {
    setActiveChunks();
    ActivateObjects();
  }

  public static int getChunkFromPosition(float x, float y) {
    int chunkIndex = Mathf.FloorToInt(x / map.chunkSize) + (Mathf.FloorToInt(y / map.chunkSize) * (map.mapSize / map.chunkSize));
    return chunkIndex;
  }

  void setActiveChunks() {
    activeChunks.Clear();
    playerChunk = getChunkFromPosition(player.transform.position.x, player.transform.position.y);
    int bot = playerChunk - (int)(map.mapSize / map.chunkSize);
    int top = playerChunk + (int)(map.mapSize / map.chunkSize);
    activeChunks.Add(playerChunk);
    activeChunks.Add(playerChunk - 1);
    activeChunks.Add(playerChunk + 1);

    activeChunks.Add(bot);
    activeChunks.Add(bot - 1);
    activeChunks.Add(bot + 1);

    activeChunks.Add(top);
    activeChunks.Add(top - 1);
    activeChunks.Add(top + 1);
  }

  void ActivateObjects() {
    for (int i = 0; i < map.chunks.Count; i++) {
      if (!activeChunks.Contains(i)) {
        foreach (GameObject tile in map.chunks[i]) tile.SetActive(false);
      } else {
        foreach (GameObject tile in map.chunks[i]) tile.SetActive(true);
      }
    }
  }

  [ContextMenu("Activate all chunks")]
  void ActivateAllObjects() {
    for (int i = 0; i < map.chunks.Count; i++) {
      foreach (GameObject tile in map.chunks[i]) tile.SetActive(true);
    }
  }

  public static void addObjectToChunk(GameObject g) {
    int chunkIndex = getChunkFromPosition(g.transform.position.x, g.transform.position.y);
    //Debug.Log("Adding " + g.name + " to chunk " + chunkIndex);
    map.chunks[chunkIndex].Add(g);
  }

  public static void removeObjectFromChunk(GameObject g) {
    for (int i = 0; i < map.chunks.Count; i++) {
      map.chunks[i].Remove(g);
    }
  }
}

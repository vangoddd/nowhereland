using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemRecipe : ScriptableObject {
  public RecipeItem[] requiredItems;
  public RecipeItem result;
}

[System.Serializable]
public class RecipeItem {
  public ItemData item;
  public int amount;
}
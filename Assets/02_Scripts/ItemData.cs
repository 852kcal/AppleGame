using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;   
    public int itemIndex;     
    public int amount;        
    public Sprite itemIcon;   
    [TextArea]
    public string description; 
}

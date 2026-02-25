using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToSortLayer : MonoBehaviour
{
    public string _sortingLayerName = "";
    public int _sortingOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>(); 
        meshRenderer.sortingLayerName = _sortingLayerName; 
        meshRenderer.sortingOrder = _sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

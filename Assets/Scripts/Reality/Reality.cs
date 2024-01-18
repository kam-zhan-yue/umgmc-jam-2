using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Reality : MonoBehaviour
{
    public Timeline timeline;

    [ShowInInspector, ReadOnly] private RealityObject[] _realityObjects;

    public void Show()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].gameObject.SetActive(false);
        }
    }

    public void ActivateColliders()
    {
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            Debug.Log($"Activating Collider: {_realityObjects[i].name}");
            _realityObjects[i].ActivateCollider();
        }
    }

    public void DeactivateColliders()
    {
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            Debug.Log($"Deactivating Collider: {_realityObjects[i].name}");
            _realityObjects[i].DeactivateCollider();
        }
    }

    public void SetVisibleInsideMask()
    {
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].SetVisibleInsideMask();
        }
    }
    
    public void SetVisibleOutsideMask()
    {
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].SetVisibleOutsideMask();
        }
    }

    private void OnValidate()
    {
        UpdateReality();
    }

    [Button]
    public void UpdateReality()
    {
        RealityTiles[] realityTiles = FindObjectsOfType<RealityTiles>();
        List<RealityTiles> tileList = new();
        for (int i = 0; i < realityTiles.Length; ++i)
        {
            if (realityTiles[i].timeline == timeline)
            {
                tileList.Add(realityTiles[i]);
            }
        }
        
        int childCount = transform.childCount;
        int totalObjects = childCount + tileList.Count;
        _realityObjects = new RealityObject[totalObjects];
        for (int i = 0; i < childCount; ++i)
        {
            _realityObjects[i] = transform.GetChild(i).GetComponent<RealitySprite>();
        }

        for (int i = 0; i < tileList.Count; ++i)
        {
            _realityObjects[childCount + i] = tileList[i];
        }
    }
}

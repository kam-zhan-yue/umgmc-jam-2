using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class Reality : MonoBehaviour
{
    public Timeline timeline;

    [ShowInInspector, ReadOnly] private RealityTiles _realityTiles;
    [ShowInInspector, ReadOnly] private RealityObject[] _realityObjects;

    public void Show()
    {
        gameObject.SetActive(true);
        _realityTiles.gameObject.SetActive(true);
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _realityTiles.gameObject.SetActive(false);
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].gameObject.SetActive(false);
        }
    }

    public void ActivateColliders()
    {
        _realityTiles.ActivateCollider();
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].ActivateCollider();
        }
    }

    public void DeactivateColliders()
    {
        _realityTiles.DeactivateCollider();
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].DeactivateCollider();
        }
    }

    public void SetVisibleInsideMask()
    {
        _realityTiles.SetVisibleInsideMask();
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i].SetVisibleInsideMask();
        }
    }
    
    public void SetVisibleOutsideMask()
    {
        _realityTiles.SetVisibleOutsideMask();
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
        int childCount = transform.childCount;
        _realityObjects = new RealityObject[childCount];
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i] = transform.GetChild(i).GetComponent<RealitySprite>();
        }

        RealityTiles[] realityTiles = FindObjectsOfType<RealityTiles>();
        for (int i = 0; i < realityTiles.Length; ++i)
        {
            if (realityTiles[i].timeline == timeline)
            {
                _realityTiles = realityTiles[i];
                break;
            }
        }
    }
}

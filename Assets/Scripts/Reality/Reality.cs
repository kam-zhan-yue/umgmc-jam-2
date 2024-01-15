using Common;
using Sirenix.OdinInspector;
using UnityEngine;

public class Reality : MonoBehaviour
{
    public Timeline timeline;
    
    [ShowInInspector, ReadOnly] private RealityObject[] _realityObjects;

    private void Awake()
    {
        ServiceLocator.Instance.Get<IRealityManager>().RegisterReality(this);
    }

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
            _realityObjects[i].ActivateCollider();
        }
    }

    public void DeactivateColliders()
    {
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
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
    private void UpdateReality()
    {
        int childCount = transform.childCount;
        _realityObjects = new RealityObject[childCount];
        for (int i = 0; i < _realityObjects.Length; ++i)
        {
            _realityObjects[i] = transform.GetChild(i).GetComponent<RealitySprite>();
        }
    }
}

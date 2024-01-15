using Common;
using Unity.Collections;
using UnityEngine;

public class Reality : MonoBehaviour
{
    public Timeline timeline;
    
    [SerializeField, ReadOnly] private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        ServiceLocator.Instance.Get<IRealityManager>().RegisterReality(this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].gameObject.SetActive(false);
        }
    }

    public void SetVisibleInsideMask()
    {
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
    
    public void SetVisibleOutsideMask()
    {
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }

    private void OnValidate()
    {
        int childCount = transform.childCount;
        spriteRenderers = new SpriteRenderer[childCount];
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }
}

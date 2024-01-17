using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    [BoxGroup("UI Objects")]
    public PopupSettings settings;
    public RectTransform mainHolder;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    public bool isAnimating = false;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    public bool isShowing = false;
    
    public Action<Popup> onCloseButtonClicked = null;

    private void Awake()
    {
        InitPopup();
    }

    protected abstract void InitPopup();

    public virtual void ShowPopup()
    {
        isShowing = true;
        mainHolder.gameObject.SetActive(true);
    }

    public virtual void HidePopup()
    {
        isShowing = false;
        mainHolder.gameObject.SetActive(false);
    }
    
    public virtual void CloseButtonClicked()
    {
        onCloseButtonClicked?.Invoke(this);
    }

    protected void ButtonClicked()
    {
        settings.PlayButton();
    }

    public virtual void EscapeButtonClicked()
    {
        CloseButtonClicked();
    }
}
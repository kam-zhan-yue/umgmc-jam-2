using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;


/// <summary>
/// Add this class to a background image so it will act as your level's background
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    /// if this is true, the background will be following the camera 
    public bool Following = true;
    /// the offset to apply relative to the camera
    public Vector3 BackgroundOffset = Vector3.zero;

    /// test button to start following the camera
    [MMInspectorButton("StartFollowing")]
    public bool StartFollowingBtn;
    /// test button to stop following the camera
    [MMInspectorButton("StopFollowing")]
    public bool StopFollowingBtn;

    protected Transform _initialParent;
    protected float _initialOffsetZ;
    protected bool _initialized = false;
    [SerializeField] private RealitySwitcher switcher;


    [SerializeField] private GameObject pastImage;
    [SerializeField] private GameObject presentImage;
    [SerializeField] private GameObject futureImage;


    public Dictionary<Timeline, Sprite> backgrounds = new();
    private bool inRange = false;




    // private void UpdateRealityBackground()
    // {
    //     if (inRange)
    //     {
    //         Timeline currTimeline = switcher.Timeline;
    //         if (currTimeline == Timeline.Past)
    //         {
    //             pastImage.SetActive(true);
    //             presentImage.SetActive(false);
    //             futureImage.SetActive(false);
    //         }
    //         else if (currTimeline == Timeline.Present)
    //         {
    //             pastImage.SetActive(false);
    //             presentImage.SetActive(true);
    //             futureImage.SetActive(false);
    //         }
    //         else if (currTimeline == Timeline.Future)
    //         {
    //             pastImage.SetActive(false);
    //             presentImage.SetActive(false);
    //             futureImage.SetActive(true);
    //         }
    //     }
    // }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>()) // Assuming your character has the tag "Player"
        {
            inRange = true;
            Debug.Log("Enter");
            switcher = other.GetComponentInChildren<RealitySwitcher>();
            Debug.Log(switcher);
            // if (switcher)
            // {
            //     switcher.onRealityChanged.AddListener(UpdateRealityBackground);
            // }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>())
        {
            inRange = false;


            // if (switcher)
            // {
            //     switcher.onRealityChanged.RemoveListener(UpdateRealityBackground);
            // }
        }


    }


    /// <summary>
    /// On enable, we get the main camera
    /// </summary>
    protected virtual void OnEnable()
    {
        _initialParent = this.transform.parent;
        if (Following)
        {
            StartFollowing();
        }
    }

    /// <summary>
    /// Lets the background follow its camera
    /// </summary>
    public virtual void StartFollowing()
    {
        Following = true;
        this.transform.SetParent(Camera.main.transform);
        if (!_initialized)
        {
            _initialOffsetZ = this.transform.localPosition.z;
            _initialized = true;
        }

        BackgroundOffset.z = BackgroundOffset.z + _initialOffsetZ;
        this.transform.localPosition = BackgroundOffset;
    }

    /// <summary>
    /// Prevents the background from following the camera
    /// </summary>
    public virtual void StopFollowing()
    {
        Following = false;
        this.transform.SetParent(_initialParent);
    }

    /// <summary>
    /// Applies a new z offset for the background
    /// </summary>
    /// <param name="newOffset"></param>
    public virtual void SetZOffset(float newOffset)
    {
        _initialOffsetZ = newOffset;
    }
}
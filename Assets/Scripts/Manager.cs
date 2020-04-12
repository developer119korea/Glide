using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }

    public Material playerMaterial;
    public Color[] playerColors = new Color[10];
    public GameObject[] playerTrails = new GameObject[10];

    public int currentLevel = 0;
    public int menuFocus = 0;

    private Dictionary<int, Vector2> activeTouches = new Dictionary<int, Vector2>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }


    public Vector3 GetPlayerInput()
    {
        if(SaveManager.Instance.state.usingAccelerometer)
        {
            Vector3 a = Input.acceleration;
            a.y = a.z;
            Debug.Log($"using accelerometer {a}");
            return a;
        }

        Vector3 r = Vector3.zero;

        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            float mag = 0;
            r = Input.GetMouseButton(0) ? Input.mousePosition : Vector3.zero; // (Input.mousePosition - activeTouches[touch.fingerId]);
            mag = r.magnitude / 300;
            r = r.normalized * mag;
        }
        else
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    activeTouches.Add(touch.fingerId, touch.position);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (activeTouches.ContainsKey(touch.fingerId))
                    {
                        activeTouches.Remove(touch.fingerId);
                    }
                }
                else
                {
                    float mag = 0;
                    r = (touch.position - activeTouches[touch.fingerId]);
                    mag = r.magnitude / 300;
                    r = r.normalized * mag;
                }
            }
        }
        return r;
    }
}

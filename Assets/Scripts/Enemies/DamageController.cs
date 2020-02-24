using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{

    private static Canvas canvas;
    public PopupText popupResrc;
    private static PopupText staticPopup;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        if (staticPopup == null)
        {
            staticPopup = popupResrc;
            if (staticPopup == null)
            {
                Debug.LogError("no no bad");
            }
        }
        
        
    }

    public static void CreatePopup(string text, Vector3 position, Color color)
    {
        PopupText instance = Instantiate(staticPopup);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPos;
        instance.SetText(text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

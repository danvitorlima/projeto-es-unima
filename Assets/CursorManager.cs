using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D texturaCursor;
    private Vector2 cursorHotspot;

    void Start()
    {
        cursorHotspot = new Vector2 (texturaCursor.width/2, texturaCursor.height/2);
        Cursor.SetCursor(texturaCursor, cursorHotspot, CursorMode.Auto);
    }

}

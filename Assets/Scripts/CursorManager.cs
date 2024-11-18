using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D texturaCursor;
    private Vector2 cursorHotspot;
    private KeyCode tecla;

    void Start()
    {
        tecla = KeyCode.None;
        cursorHotspot = new Vector2(texturaCursor.width / 2, texturaCursor.height / 2);
        Cursor.SetCursor(texturaCursor, cursorHotspot, CursorMode.Auto);
    }

    void DetectarTecla(KeyCode _tecla)
    {
        if (Input.GetKeyDown(_tecla))
        {
            //abrindo tela
            if (tecla == KeyCode.None)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                tecla = _tecla;
            }
            //fechando tela
            else if (tecla == _tecla)
            {
                Cursor.SetCursor(texturaCursor, cursorHotspot, CursorMode.Auto);
                tecla = KeyCode.None;
            }
        }
    }

    private void Update()
    {
        DetectarTecla(KeyCode.X);
        DetectarTecla(KeyCode.P);
        DetectarTecla(KeyCode.Tab);
    }

}

using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursorTexture;
    public string sceneID;

    void Start()
    {
        Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);

        Cursor.visible = true;
    }
}

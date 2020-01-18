using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitBackground : MonoBehaviour
{
    public enum Types
    {
        BackgroundLight,
        BackgroundDark,
        Frame
    }

    [SerializeField]
    Types type = Types.BackgroundLight;

    [SerializeField]
    float frameWidth = 0.07f;
    [SerializeField]
    Board board = null;

    int previousHeight;
    int previousWidth;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Fit();
    }

    void Update()
    {
        if (previousHeight != Screen.height || previousWidth != Screen.width)
        {
            Fit();
        }
    }

    void Fit()
    {
        Camera camera = Camera.main;
        float cellSize = 0;
        int boardWidth = 0;
        int boardHeight = 0;
        if (board != null)
        {
            cellSize = board.CellSize;
            boardWidth = board.Size.x;
            boardHeight = board.Size.y;
        }


        switch (type)
        {
            case Types.BackgroundLight:
                spriteRenderer.size = new Vector2(
                    camera.aspect * camera.orthographicSize * 2.0f,
                    camera.orthographicSize * 2.0f
                    );
                spriteRenderer.size *= 2;
                spriteRenderer.transform.position = Vector3.zero;
                break;
            case Types.BackgroundDark:
                if (cellSize == 0)
                {
                    break;
                }
                spriteRenderer.size = new Vector2(
                    cellSize * boardWidth,
                    cellSize * boardHeight
                    );
                spriteRenderer.transform.position = Vector3.zero;
                break;
            case Types.Frame:
                if (cellSize == 0)
                {
                    break;
                }
                spriteRenderer.size = new Vector2(cellSize * boardWidth + frameWidth * 2, cellSize * boardHeight + frameWidth * 2);
                spriteRenderer.transform.position = Vector3.zero;
                break;
        }

        previousHeight = Screen.height;
        previousWidth = Screen.height;
    }
}

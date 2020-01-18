using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public float CellSize { get => cellSize; } 
    public Vector2Int Size { get => size; } 
    public int Combo { get => comboMultiplier == 0 ? 1 : comboMultiplier; }
    

    [SerializeField]
    Vector2Int size = new Vector2Int(5, 9);
    [SerializeField]
    float cellSize = 0.18f;
    [SerializeField]
    Gem[] gemPrefabs = new Gem[] { };

    [SerializeField]
    AudioClip matchAudioClip = null;
    [SerializeField]
    AudioClip errorAudioClip = null;

    Gem[,] gems;
    public bool IsReady { get; private set; } = false;
    int comboMultiplier = 0;

    readonly Vector2Int[] lastMove = new Vector2Int[] { Vector2Int.zero, Vector2Int.zero };
    Vector2Int selected = Vector2Int.one * -1;

    // Start is called before the first frame update
    void Start()
    {
        if(gemPrefabs.Length == 0)
        {
            Destroy(gameObject);
            Debug.LogWarning("Gem Prefabs not set befor initialize. OBJECT DESTROYED.");
            return;
        }

        InitializeBoard();
    }


    // Update is called once per frame
    void Update()
    {
        if (IsReady && GameManager.Instance.CurrentTime > 0 && !GameManager.Instance.IsPaused)
        {
            comboMultiplier = 0;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2Int position = ScreenToBoadrPositon(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    selected = position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (selected.x >= 0 && selected.x < size.x && selected.y >= 0 && selected.y < size.y)
                    {
                        float magnitude = (selected - position).magnitude;
                        if (magnitude >= 0.7) // TDDO: Property
                        {
                            if (magnitude % 1 == 0)
                            {
                                position = selected + Vector2Int.FloorToInt(((Vector2)(position - selected)).normalized);
                                DoSwapCells(selected.x, selected.y, position.x, position.y).OnComplete(() => {
                                    if (!HasMatches())
                                    {
                                        if(SFXManager.Instance != null)
                                        {
                                            SFXManager.Instance.Play(errorAudioClip);
                                        }
                                        DoSwapCells(lastMove[0].x, lastMove[0].y, lastMove[1].x, lastMove[1].y).OnComplete(() => IsReady = true);
                                    }
                                    else
                                    {
                                        RemoveMatches();
                                    }
                                });
                                IsReady = false;

                                lastMove[0] = selected;
                                lastMove[1] = position;

                                selected = Vector2Int.one * -1;
                            }
                        }
                    }
                }
            }
        }        
    }

    Vector2Int ScreenToBoadrPositon(Vector2 touchPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        return WorldToBoadrPositon(worldPosition);
    }

    Vector2Int WorldToBoadrPositon(Vector3 worldPosition)
    {
        Vector2 leftBottom = CalculatePosition(0, 0);

        return new Vector2Int(
            Mathf.RoundToInt((worldPosition.x - leftBottom.x) / cellSize),
            Mathf.RoundToInt((worldPosition.y - leftBottom.y) / cellSize)
            );
    }

    Vector3 CalculatePosition(int x, int y)
    {
        return new Vector3(
                -0.5f * cellSize * size.x + 0.5f * cellSize + x * cellSize,
                -0.5f * cellSize * size.y + 0.5f * cellSize + y * cellSize
                );
    }

    void InitializeBoard()
    {
        gems = new Gem[size.x, size.y];

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (x < 2 && y < 2)
                {
                    CreateGem(x, y);
                }
                else
                {
                    CreateGem(x, y);
                    Gem.Colors gemColor = gems[x, y].Color;

                    while (
                        (x >= 2 && (gems[x - 1, y].Color == gemColor && gems[x - 2, y].Color == gemColor)) 
                        || 
                        (y >= 2 && (gems[x, y - 1].Color == gemColor && gems[x, y - 2].Color == gemColor))
                        )
                    {
                        DestroyGem(x, y);
                        CreateGem(x, y);
                        gemColor = gems[x, y].Color;
                    }
                }
            }
        }

        if (!CheckIsMatchPossible())
        {
            InitializeBoard();
        }
        else
        {
            IsReady = true;
        }
    }


    Tween DoCreateGem(int x, int y)
    {
        CreateGem(x, y);
        gems[x, y].transform.localScale = Vector3.zero;
        return gems[x, y].transform.DOScale(Vector3.one, GameManager.Instance.GemCreateTime);
    }

    void CreateGem(int x, int y, int color)
    {
        Vector3 gemPosition = CalculatePosition(x, y);

        gems[x, y] = Instantiate(gemPrefabs[color], gemPosition, Quaternion.identity, transform);
    }

    void CreateGem(int x, int y, Gem.Colors color)
    {
        CreateGem(x, y, (int)color);
    }

    void CreateGem(int x, int y)
    {
        int randomGemIndex = Random.Range(0, gemPrefabs.Length);
        CreateGem(x, y, randomGemIndex);
    }

    Tween DoDestroyGem(int x, int y)
    {
        if (gems[x, y] != null)
        {
            return gems[x, y].transform.DOScale(Vector3.zero, GameManager.Instance.GemDestroyTime).OnComplete(() => DestroyGem(x, y));
        }
        return null;
    }

    void DestroyGem(int x, int y)
    {
        Destroy(gems[x, y].gameObject);
        gems[x, y] = null;
    }


    Tween DoSwapCells(int x1, int y1, int x2, int y2)
    {
        if (x1 < 0 || x1 >= size.x ||
            x2 < 0 || x2 >= size.x ||
            y1 < 0 || y1 >= size.y ||
            y2 < 0 || y2 >= size.y)
        {
            return null;
        }

        Gem tempGem = gems[x2, y2];
        gems[x2, y2] = gems[x1, y1];
        gems[x1, y1] = tempGem;

        Sequence moveSequence = DOTween.Sequence();
        if (gems[x1, y1] != null)
        {
            Vector3 startValue = gems[x1, y1].transform.position;
            Vector3 endValue = CalculatePosition(x1, y1);
            IsReady = false;

            moveSequence.Insert(0, gems[x1, y1].transform.DOMove(endValue, Vector3.Distance(startValue, endValue) / GameManager.Instance.GemMoveSpeed));
        }
        if (gems[x2, y2] != null)
        {
            Vector3 startValue = gems[x2, y2].transform.position;
            Vector3 endValue = CalculatePosition(x2, y2);
            IsReady = false;

            moveSequence.Insert(0, gems[x2, y2].transform.DOMove(endValue, Vector3.Distance(startValue, endValue) / GameManager.Instance.GemMoveSpeed));
        }

        return moveSequence;
    }

    void Mix()
    {
        Sequence destroySequence = DOTween.Sequence();

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                destroySequence.Insert(0, DoDestroyGem(x, y));
            }
        }

        destroySequence.OnComplete(() =>
        {
            Sequence createSequence = DOTween.Sequence();
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    createSequence.Insert(0, DoCreateGem(x, y));
                }
            }

            createSequence.OnComplete(() =>
            {
                if (HasMatches())
                {
                    RemoveMatches();
                }
                else
                {
                    IsReady = true;
                }
            });
        });
    }

    bool CheckIsMatchPossible()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (x == 0 && y == 0) 
                {
                    if (gems[x, y].Color == gems[x + 1, y].Color && gems[x + 2, y + 1].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                    if (gems[x, y].Color == gems[x, y + 1].Color && gems[x + 1, y + 2].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                }
                else if (x == 0 && y == size.y - 1)
                {
                    if (gems[x, y].Color == gems[x + 1, y].Color && gems[x + 2, y - 1].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                    if (gems[x, y].Color == gems[x, y - 1].Color && gems[x + 1, y - 2].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                }
                else if (x == size.x - 1 && y == 0)
                {
                    if (gems[x, y].Color == gems[x - 1, y].Color && gems[x - 2, y + 1].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                    if (gems[x, y].Color == gems[x, y + 1].Color && gems[x - 1, y + 2].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                }
                else if (x == size.x - 1 && y == size.y - 1)
                {
                    if (gems[x, y].Color == gems[x - 1, y].Color && gems[x - 2, y - 1].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                    if (gems[x, y].Color == gems[x, y - 1].Color && gems[x - 1, y - 2].Color == gems[x, y].Color)
                    {
                        return true;
                    }
                }
                else if (x == 0 && y < size.y - 1)
                {
                    if ( y < size.y - 2)
                    {
                        if (gems[x, y].Color == gems[x, y + 1].Color && (gems[x + 1, y - 1].Color == gems[x, y].Color || gems[x + 1, y + 2].Color == gems[x, y].Color))
                        {
                            return true;
                        }
                    }
                    if(gems[x, y - 1].Color == gems[x, y + 1].Color && gems[x + 1, y].Color == gems[x, y - 1].Color) {
                        return true;
                    }
                }
                else if (x == size.x - 1 && y < size.y - 1)
                {
                    if (y < size.y - 2)
                    {
                        if (gems[x, y].Color == gems[x, y + 1].Color && (gems[x - 1, y - 1].Color == gems[x, y].Color || gems[x - 1, y + 2].Color == gems[x, y].Color))
                        {
                            return true;
                        }
                    }
                    if (gems[x, y - 1].Color == gems[x, y + 1].Color && gems[x - 1, y].Color == gems[x, y - 1].Color)
                    {
                        return true;
                    }
                }
                else if (y == 0 && x < size.x - 1)
                {
                    if (x < size.x - 2)
                    {
                        if (gems[x, y].Color == gems[x + 1, y].Color && (gems[x - 1, y + 1].Color == gems[x, y].Color || gems[x + 2, y + 1].Color == gems[x, y].Color))
                        {
                            return true;
                        }
                    }
                    if (gems[x - 1, y].Color == gems[x + 1, y].Color && gems[x, y + 1].Color == gems[x - 1, y].Color)
                    {
                        return true;
                    }
                }
                else if (y == size.y - 1 && x < size.x - 1)
                {
                    if (x < size.x - 2)
                    {
                        if (gems[x, y].Color == gems[x + 1, y].Color && (gems[x - 1, y - 1].Color == gems[x, y].Color || gems[x + 2, y - 1].Color == gems[x, y].Color))
                        {
                            return true;
                        }
                    }
                    if (gems[x - 1, y].Color == gems[x + 1, y].Color && gems[x, y - 1].Color == gems[x - 1, y].Color)
                    {
                        return true;
                    }
                }
                else if (x > 0 && y > 0)
                {
                    // 3x3
                    if (x < size.x - 1 && y < size.y - 1)
                    {
                        if (gems[x, y - 1].Color == gems[x, y + 1].Color && (gems[x - 1, y].Color == gems[x, y - 1].Color || gems[x + 1, y].Color == gems[x, y - 1].Color))
                        {
                            return true;
                        }
                        if (gems[x - 1, y].Color == gems[x + 1, y].Color && (gems[x, y - 1].Color == gems[x - 1, y].Color || gems[x, y + 1].Color == gems[x - 1, y].Color))
                        {
                            return true;
                        }
                    }

                    // 4x3
                    if (x < size.x - 2 && y < size.y - 1)
                    {
                        if (gems[x, y].Color == gems[x + 1, y].Color && (
                            gems[x - 1, y - 1].Color == gems[x, y].Color ||
                            gems[x - 1, y + 1].Color == gems[x, y].Color ||
                            gems[x + 2, y - 1].Color == gems[x, y].Color ||
                            gems[x + 2, y + 1].Color == gems[x, y].Color
                            ))
                        {
                            return true;
                        }
                    }

                    // 3x4
                    if (x < size.x - 1 && y < size.y - 2)
                    {
                        if (gems[x, y].Color == gems[x, y + 1].Color && (
                            gems[x - 1, y - 1].Color == gems[x, y].Color ||
                            gems[x + 1, y - 1].Color == gems[x, y].Color ||
                            gems[x - 1, y + 2].Color == gems[x, y].Color ||
                            gems[x + 1, y + 2].Color == gems[x, y].Color
                            ))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    bool HasMatches()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.up, Vector2.right, Vector2.down, Vector2.left
        };

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Gem currentGem = gems[x, y];
                if(currentGem == null)
                {
                    Debug.LogWarning("Check for matches when board has empty cells.");
                    continue;
                }

                foreach(Vector2 direction in directions)
                {
                    var hits = Physics2D.RaycastAll(currentGem.transform.position, direction, cellSize * 2);

                    if(hits.Length > 2)
                    {
                        if (hits[1].transform.GetComponent<Gem>().Color == currentGem.Color &&
                            hits[2].transform.GetComponent<Gem>().Color == currentGem.Color)
                        {
                            return true;
                        }
                    }                    
                }
            }
        }

        return false;
    }

    void RemoveMatches()
    {
        List<Vector2Int> matches = new List<Vector2Int>();
        int gemsNumber = 1;

        for (int y = 0; y < size.y; y++)
        {
            Gem.Colors colorToMach = gems[0, y].Color;
            for (int x = 1; x <= size.x; x++)
            {
                if (x != size.x && colorToMach == gems[x, y].Color)
                {
                    gemsNumber++;
                }
                else
                {
                    if (x != size.x)
                    {
                        colorToMach = gems[x, y].Color;
                    }

                    if (gemsNumber >= 3)
                    {
                        for (int i = 1; i <= gemsNumber; i++)
                        {
                            Vector2Int position = new Vector2Int(x - i, y);

                            if (!matches.Contains(position))
                            {
                                matches.Add(position);
                            }
                        }

                        CalculatePoint(gemsNumber);
                    }

                    gemsNumber = 1;
                }
            }
        }

        gemsNumber = 1;
        for (int x = 0; x < size.x; x++)
        {
            Gem.Colors colorToMach = gems[x, 0].Color;
            for (int y = 1; y <= size.y; y++)
            {
                if (y != size.y && colorToMach == gems[x, y].Color)
                {
                    gemsNumber++;
                }
                else
                {
                    if (y != size.y)
                    {
                        colorToMach = gems[x, y].Color;
                    }

                    if (gemsNumber >= 3)
                    {
                        for (int i = 1; i <= gemsNumber; i++)
                        {
                            Vector2Int position = new Vector2Int(x, y - i);

                            if (!matches.Contains(position))
                            {
                                matches.Add(position);
                            }
                        }

                        CalculatePoint(gemsNumber);
                    }

                    gemsNumber = 1;
                }
            }
        }

        if (matches.Count == 0)
        {
            Debug.LogWarning("No matches found in RemoveMatches method.");
            IsReady = true;
            return;
        }

        Sequence destroySequence = DOTween.Sequence();
        foreach (Vector2Int gemPositon in matches)
        {
            destroySequence.Insert(0, DoDestroyGem(gemPositon.x, gemPositon.y));
        }

        destroySequence.OnComplete(() => FillGaps());
    }

    void CalculatePoint(int number) // better name for parametr
    {
        if(SFXManager.Instance != null)
        {
            SFXManager.Instance.Play(matchAudioClip);
        }

        comboMultiplier++;
        switch (number)
        {
            case 3:
                GameManager.Instance.AddScore(GameManager.Instance.PointsForMatch3 * comboMultiplier);
                break;
            case 4:
                GameManager.Instance.AddScore(GameManager.Instance.PointsForMatch4 * comboMultiplier);
                break;
            case 5:
                GameManager.Instance.AddScore(GameManager.Instance.PointsForMatch5 * comboMultiplier);
                break;
        }
    }

    void FillGaps()
    {
        Sequence moveDownSequence = DOTween.Sequence();

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (gems[x, y] == null)
                {
                    for (int yy = y + 1; yy < size.y; yy++)
                    {
                        if (gems[x, yy] != null)
                        {
                            moveDownSequence.Insert(0, DoSwapCells(x, y, x, yy));
                            break;
                        }
                    }
                }
            }
        }

        moveDownSequence.OnComplete(() => {
            Sequence createNewGemsSequence = DOTween.Sequence();
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (gems[x, y] == null)
                    {
                        createNewGemsSequence.Insert(0, DoCreateGem(x, y));
                    }
                }
            }

            createNewGemsSequence.OnComplete(() =>
            {
                if (HasMatches())
                {
                    RemoveMatches();
                }
                else
                {
                    if(CheckIsMatchPossible())
                    {
                        IsReady = true;
                    }
                    else
                    {
                        Mix();
                    }
                }
            });
        });        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldBehaviour : MonoBehaviour
{
    public EndMove gameEvent;
    protected Vector2[] fieldpos = new Vector2[25];
    protected int width = 5;
    protected int height = 5;
    protected float deltaPos = 1.078F;
    protected int center;
    protected Collider2D col;
    [SerializeField] protected GameObject piecePrefab;
    [SerializeField] protected Transform pieces;
    [SerializeField] protected Vector2 startPos = new Vector2(-2.156F, -2.156F);
    [SerializeField] protected bool debug = false;
    protected Vector3 scale;
    protected Quaternion rotation;
    [SerializeField] protected string gamemodeclass;
    // Start is called before the first frame update
    public void Start()
    {
        scale = transform.parent.localScale;
        int middle = width / 2;
        center = middle * height + middle;
        col = GetComponent<Collider2D>();
        int numCells = width * height;
        fieldpos = new Vector2[numCells];
        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        if (debug) print(startPos);
        for (int i = 0; i < height; i++)
        {
            string pos = "";
            for (int j = 0; j < width; j++)
            {
                int ind = i * width + j;
                fieldpos[i * width + j] = new Vector2(j * deltaPos + startPos.x, i * deltaPos + startPos.y);
                pos += fieldpos[i * width + j];
                pos += ", ";
            }
        }
    }
    public void SetEvent(EndMove parEvent)
    {
        gameEvent = parEvent;
    }
    public void SetScale(Vector3 s)
    {
        scale = s;
    }
    protected Vector2 convertVector(Vector2 pos)
    {
        return (pos * scale) + (Vector2)transform.position;
    }
    // Update is called once per frame
    
    public void SetParameters(int w, int h, float delta, GameObject prefab, bool deb)
    {
        width = w;
        height = h;
        deltaPos = delta;
        piecePrefab = prefab;
        debug = deb;
    }
    public void SetParameters(int w, int h, float delta, bool deb)
    {
        width = w;
        height = h;
        deltaPos = delta;
        debug = deb;
    }

    public void SetRotation(Quaternion r)
    {
        rotation = r;
    }
    public float GetDeltaPos()
    {
        return deltaPos;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    virtual public void Display(int[] field)
    {

    }

    virtual public void Display(int[] field, GameObject moveShow)
    {

    }
    public void clearField()
    {
        for (int i = 0; i < pieces.childCount; i++)
        {
            Destroy(pieces.GetChild(i).gameObject);
        }
    }

    public Vector3 GetPosOnField(int ind)
    {
        Vector2 pos = convertVector(fieldpos[ind]);
        return new Vector3(pos.x, pos.y, 0);
    }

    public float GetLeftSide()
    {
        return startPos.x * scale.x + transform.position.x;
    }

    public float GetBottomSide()
    {
        return startPos.y * scale.y + transform.position.y;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndMove : UnityEvent<int, int>
{
}

public class FieldController : MonoBehaviour
{
    public UnityEvent AttemptMove;
    public EndMove endMoveEvent = new EndMove();
    [SerializeField] protected FieldBehaviour field;
    [SerializeField] protected bool debug = false;
    protected int[] moves = { };
    protected int width = 5;
    protected int height = 5;
    protected float deltaPos = 1.078F;
    protected float leftSide, bottomSide;
    public void SetRotation(Quaternion r)
    {
        field.SetRotation(r);
    }
    virtual public int GetCenter()
    {
        return (width / 2) * height + width / 2;
    }
    virtual public void Start() { }

    virtual public void Setup() { }

    virtual public void Display() { }

    virtual public void Display(int[] f) { }

    virtual public void SetMoves(int[] m)
    {
        moves = (int[])m.Clone();
    }

    virtual public Vector2 GetPos(int coord, Vector2 pos) { return new Vector2(); }

    virtual public Vector2 GetPos(int coord) { return field.GetPosOnField(coord); }

    virtual public Vector2 GetPos(Vector2 pos) { return new Vector2(); }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return width;
    }

    public void SetScale(Vector3 s)
    {
        field.SetScale(s);
    }
}

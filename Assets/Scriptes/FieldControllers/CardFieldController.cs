using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFieldController : FieldController
{
    [SerializeField] GameObject moveObj;
    // Start is called before the first frame update
    override public void Start()
    {
        leftSide = field.GetLeftSide();
        bottomSide = field.GetBottomSide();
    }

    // Update is called once per frame
    public void SetTextures(GameObject move)
    {
        //Set Texture of move show object
        moveObj = move;
    }
    override public Vector2 GetPos(Vector2 pos)
    {
        int ind = Mathf.RoundToInt((pos.x - leftSide) / deltaPos) + Mathf.RoundToInt((pos.y - bottomSide) / deltaPos) * width;
        //print(field.GetPosOnField(ind));
        return field.GetPosOnField(ind);
    }

    override public void SetMoves(int[] m)
    {
        //Set move of card
        moves = (int[])m.Clone();
        Display();
    }

    override public void Setup()
    {
        field.SetParameters(width, height, deltaPos, debug);
        field.Start();
    }

    override public void Display()
    {
        field.clearField();
        field.Display(moves, moveObj);
    }
}

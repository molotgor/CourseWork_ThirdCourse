using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIFieldController : FieldController
{
    public AIGameController par;
    public UnityEvent GameEvent;
    [SerializeField] GameObject piecePrefab;
    int center = (5 / 2 * 5) + (5 / 2);

    override public void Start()
    {
        base.Start();
        leftSide = field.GetLeftSide();
        bottomSide = field.GetBottomSide();
        deltaPos = field.GetDeltaPos();
    }

    
    override public void Display(int[] f)
    {
        field.clearField();
        //print("Display on contr");
        field.Display(f);
    }

    override public void Setup()
    {
        field.SetParameters(width, height, deltaPos, piecePrefab, debug);
        field.Start();
    }

    override public Vector2 GetPos(int coord, Vector2 pos)
    {
        AttemptMove.Invoke();
        int ind = Mathf.RoundToInt((pos.x - leftSide) / (deltaPos * transform.localScale.x)) + Mathf.RoundToInt((pos.y - bottomSide) / (deltaPos * transform.localScale.y)) * width;
        //print(ind);
        //print(coord);
        //print(moves.Length);
        /*
        string m = "";
        for (int i = 0; i < moves.Length; i++)
        {
            m += moves[i];
            m += ", ";

        }
        */
        //print(m);
        //Get possible move for in X coord (max 2 in each direction)
        int indMod5 = coord % 5;
        int[] indFieldX = { ((indMod5 <= 2) ? -indMod5 : -2), ((indMod5 <= 2) ? 2 : 4 - indMod5) };
        //print(new Vector2(indFieldX[0], indFieldX[1]));

        //Get possible move for in Y coord (max 2 in each direction)
        int indDiv5 = coord / 5;
        int[] indFieldY = { ((indDiv5 <= 2) ? -indDiv5 : -2), ((indDiv5 <= 2) ? 2 : 4 - indDiv5) };
        //print(new Vector2(indFieldY[0], indFieldY[1]));
        for (int i = 0; i < moves.Length; i++)
        {
            //print(i);
            int move = moves[i] - center;
            int target = move + coord;
            //Get direction of move
            Vector2 moveDir = new Vector2(moves[i] % 5, moves[i] / 5);
            Vector2 centerDir = new Vector2(center % 5, center / 5);
            moveDir -= centerDir;

            //If move in possible limit and equal to target square than commit move
            if (target > 24 || target < 0)
                continue;
            if (moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1] && moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1] && ind == move + coord && par.GetPosition(coord, move))
            {
                //print("EndMove from " + coord + " to " + ind);
                endMoveEvent.Invoke(coord, ind);
                SetMoves(new int[] { });
                return field.GetPosOnField(ind);
            }
        }
        return field.GetPosOnField(coord);
    }
}

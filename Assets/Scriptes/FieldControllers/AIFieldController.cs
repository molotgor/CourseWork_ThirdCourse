using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFieldController : FieldController
{
    public AIGameController par;
    [SerializeField] GameObject piecePrefab;
    int center = (5 / 2 * 5) + (5 / 2);
    public void Start()
    {
        base.Start();

        leftSide = field.GetLeftSide();
        bottomSide = field.GetBottomSide();
        deltaPos = field.GetDeltaPos();
    }
    public List<int> GetAllPos(int coord, int[] m)
    {
        List<int> res = new List<int>();
        int indMod5 = coord % 5;
        int[] indFieldX = { ((indMod5 <= 2) ? -indMod5 : -2), ((indMod5 <= 2) ? 2 : 4 - indMod5) };
        //print(new Vector2(indFieldX[0], indFieldX[1]));
        int indDiv5 = coord / 5;
        int[] indFieldY = { ((indDiv5 <= 2) ? -indDiv5 : -2), ((indDiv5 <= 2) ? 2 : 4 - indDiv5) };
        //print(new Vector2(indFieldY[0], indFieldY[1]));
        for (int i = 0; i < m.Length; i++)
        {
            int move = m[i] - center;
            int target = move + coord;
            //print(new Vector2(m[i], move));
            //print(target);
            Vector2 moveDir = new Vector2(m[i] % 5, m[i] / 5);
            Vector2 centerDir = new Vector2(center % 5, center / 5);
            moveDir -= centerDir;
            //print(moveDir);

            if (moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1] && moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1] && par.GetPosition(coord, move))
            {
                res.Add(move + coord);
            }
        }
        return res;
    }
    override public void Display(int[] f)
    {
        field.clearField();
        print("Display on contr");
        field.Display(f);
    }

    override public void Setup()
    {
        field.SetParameters(width, height, deltaPos, piecePrefab, debug);
        field.SetGameMode("TwoPlayerFieldController");
        field.Start();
    }

    override public Vector2 GetPos(int coord, Vector2 pos)
    {
        AttemptMove.Invoke();
        int ind = Mathf.RoundToInt((pos.x - leftSide) / (deltaPos * transform.localScale.x)) + Mathf.RoundToInt((pos.y - bottomSide) / (deltaPos * transform.localScale.y)) * width;
        //print(ind);
        //print(coord);
        //print(moves.Length);
        string m = "";
        for (int i = 0; i < moves.Length; i++)
        {
            m += moves[i];
            m += ", ";

        }
        print(m);
        int indMod5 = coord % 5;
        int[] indFieldX = { ((indMod5 <= 2) ? -indMod5 : -2), ((indMod5 <= 2) ? 2 : 4 - indMod5) };
        print(new Vector2(indFieldX[0], indFieldX[1]));
        int indDiv5 = coord / 5;
        int[] indFieldY = { ((indDiv5 <= 2) ? -indDiv5 : -2), ((indDiv5 <= 2) ? 2 : 4 - indDiv5) };
        print(new Vector2(indFieldY[0], indFieldY[1]));
        for (int i = 0; i < moves.Length; i++)
        {
            print(i);
            int move = moves[i] - center;
            int target = move + coord;
            //print(new Vector2(moves[i], move));
            //print(target);
            Vector2 moveDir = new Vector2(moves[i] % 5, moves[i] / 5);
            Vector2 centerDir = new Vector2(center % 5, center / 5);
            moveDir -= centerDir;
            //print(moveDir);
            print(moveDir.x >= indFieldX[0]);
            print(moveDir.x <= indFieldX[1]);
            print(moveDir.y >= indFieldY[0]);
            print(moveDir.y <= indFieldY[1]);

            if (target > 24 || target < 0)
                continue;
            print(par.GetPosition(coord, move));
            print(moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1]);
            print(moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1]);
            print(ind == move + coord);
            print(ind);
            print(move);
            print(coord);

            print(ind == move + coord && par.GetPosition(coord, move));
            if (moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1] && moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1] && ind == move + coord && par.GetPosition(coord, move))
            {
                print("EndMove from " + coord + " to " + ind);
                endMoveEvent.Invoke(coord, ind);
                SetMoves(new int[] { });
                return field.GetPosOnField(ind);
            }
        }
        return field.GetPosOnField(coord);
    }
}

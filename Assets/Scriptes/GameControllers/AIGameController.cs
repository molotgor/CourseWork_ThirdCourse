using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AIGameController : GameController
{
    [SerializeField] [Range(0, 5)] float timeWait = 1;
    public void Start()
    {
        base.Start();
        gameField.AttemptMove.AddListener(GetMove);
        hands[1].DeActivate();
    }
    protected void GetMove()
    {
        print(active);
        int[] moves = hands[active].GetMove();
        printArr(moves);
        gameField.SetMoves(moves);
    }

    override protected void SetupField()
    {
        AIFieldController f = (AIFieldController)gameField;
        f.endMoveEvent.AddListener(MoveEnd);
        f.par = this;
        //f.AttemptMove.AddListener(GetMove);
    }
    override protected void SetActive(int act)
    {
        print("SetActive from " + active + " to " + act);
        active = act;
        //hands[act].Activate();
        //hands[1 - act].DeActivate();
        if (active == 1)
        {
            print(act);
            hands[0].DeActivate();
            StartCoroutine(CommitMove());
        }
        else
        {
            print("Activating Player Hand");
            hands[0].Activate();
        }
        /*
        if (active < 2)
            StartCoroutine(CommitMove());
        */
    }

    protected IEnumerator CommitMove()
    {
        print(active);
        print("AI doing move");
        RandomThinkOnMove();
        /*
        print(1 - active); 
        List<int[]> BlueMoves = new List<int[]>();
        BlueMoves.Add((int[])hands[0].GetMove(0).Clone());
        BlueMoves.Add((int[])hands[0].GetMove(1).Clone());

        List<int[]> RedMoves = new List<int[]>();
        RedMoves.Add((int[])hands[1].GetMove(0).Clone());
        RedMoves.Add((int[])hands[1].GetMove(1).Clone());

        int[] side = (int[])sideCards[0].GetMove().Clone();

        int depth = 5;
        ThinkOnMove(active, BlueMoves, RedMoves, side, field, depth, 0);
        */
        yield return new WaitForSecondsRealtime(timeWait);
    }

    List<int> ThinkOnMove(int act, List<int[]> BlueMoves, List<int[]> RedMoves, int[] side, int[] fieldState, int depth, int cost)
    {
        if (depth < 0)
        {
            List<int> res = new List<int>();
            res.Add(cost);
            return res;
        }
        int teorActive = act;
        List<int[]> teorBlueMoves = BlueMoves;
        List<int[]> teorRedMoves = RedMoves;
        int[] teorSideMove = (int[])side.Clone();
        int[] teorField = (int[])fieldState.Clone();
        List<List<int[]>> moves = new List<List<int[]>>();
        moves.Add(teorBlueMoves);
        moves.Add(teorRedMoves);
        Dictionary<int, Dictionary<int, List<int>>> teorMoves = GetAllMoves(teorActive, teorField, moves[teorActive]);
        List<List<int>> results = new List<List<int>>();
        foreach (int i in teorMoves.Keys)
        {
            foreach (int j in teorMoves[i].Keys)
            {
                for (int z = 0; z < teorMoves[i][j].Count; z++)
                {
                    // randInto = z
                    // randCoord = i
                    // randCard = j
                    if (((gameBoard[z] & PieceCategory.Master) > 0) || ((z == temples[1 - teorActive]) && ((gameBoard[i] & PieceCategory.Master) > 0)))
                    {
                        //ShowWin(teorActive);
                        teorActive = 2;
                        cost += 10 * (teorActive - active);
                    }
                    else
                    {
                        //SetActive(1 - teorActive);
                        if (fieldState[z] > 0)
                        {
                            cost += 1 * (teorActive - active);
                        }
                        fieldState[z] = fieldState[i];
                        fieldState[i] = PieceCategory.None;
                        
                        moves[teorActive][j] = RotateMove(teorSideMove);
                        side = (int[])RotateMove(moves[teorActive][j]).Clone();
                        results.Add(ThinkOnMove(1 - teorActive, moves[0], moves[1], side, fieldState, depth - 1, cost));
                    }
                }
            }
        }

        //Format {From, To, Card, Res}
        return new List<int>();
    }

    void RandomThinkOnMove()
    {
        int teorActive = active;
        if (teorActive < 1)
            return;
        List<int[]> teorBlueMoves = new List<int[]>();
        teorBlueMoves.Add((int[])hands[0].GetMove(0).Clone());
        teorBlueMoves.Add((int[])hands[0].GetMove(1).Clone());

        List<int[]> teorRedMoves = new List<int[]>();
        teorRedMoves.Add((int[])hands[1].GetMove(0).Clone());
        teorRedMoves.Add((int[])hands[1].GetMove(1).Clone());

        List<List<int[]>> moves = new List<List<int[]>>();
        moves.Add(teorBlueMoves);
        moves.Add(teorRedMoves);
        printArr(teorRedMoves[0]);
        printArr(teorRedMoves[1]);
        int[] teorSideMove = (int[])sideCards[0].GetMove().Clone();
        printArr(teorSideMove);

        int[] teorField = (int[])gameBoard.Clone();

        Dictionary<int, Dictionary<int, List<int>>> teorMoves = GetAllMoves(teorActive, teorField, moves[teorActive]);
        string res = "";
        foreach (int i in teorMoves.Keys)
        {
            res += i;
            res += " =>";
            foreach (int j in teorMoves[i].Keys)
            {
                res += j;
                res += " => (";
                for (int z = 0; z < teorMoves[i][j].Count; z++)
                {
                    res += teorMoves[i][j][z];
                    res += ", ";
                }

                res += "); ";
            }
            res += "\n";
        }
        print(res);

        int randCoord = teorMoves.ElementAt(Random.Range(0, teorMoves.Keys.Count)).Key;
        print(randCoord.ToString());
        int randCard = teorMoves[randCoord].ElementAt(Random.Range(0, teorMoves[randCoord].Count)).Key;
        print(randCard.ToString());
        int randInto = teorMoves[randCoord][randCard][Random.Range(0, teorMoves[randCoord][randCard].Count)];
        print(teorMoves[randCoord][randCard].Count);
        print(randInto.ToString());
        int[] tempMove = (int[])hands[active].GetMove(randCard).Clone();
        printArr(tempMove);
        if (((gameBoard[randInto] & PieceCategory.Master) > 0) || ((randInto == temples[1 - teorActive]) && ((gameBoard[randCoord] & PieceCategory.Master) > 0)))
        {
            ShowWin(teorActive);
            active = 2;
        }
        else
        {
            //SetActive(1 - teorActive);
            gameBoard[randInto] = gameBoard[randCoord];
            gameBoard[randCoord] = PieceCategory.None;
            hands[teorActive].SetMove(randCard, RotateMove(teorSideMove));
            blueSideCard.SetMove(RotateMove(tempMove));
            gameField.endMoveEvent.Invoke(randCoord, randInto);
        }
    }

    Dictionary<int, Dictionary<int, List<int>>> GetAllMoves(int act, int[] f, List<int[]> hand)
    {

        for (int i = 0; i < gameField.GetHeight(); i++)
        {
            string line = "";
            for (int j = 0; j < width; j++)
            {
                line += gameBoard[i * width + j];
                line += ", ";
            }
            print(line);
        }
        Dictionary<int, Dictionary<int, List<int>>> moves = new Dictionary<int, Dictionary<int, List<int>>>();
        for (int i = 0; i < gameBoard.Length; i++)
        {
            if ((gameBoard[i] & (act + 1) * 4) < 1)
                continue;
            Dictionary<int, List<int>> moveFromI = new Dictionary<int, List<int>>();
            for (int j = 0; j < hand.Count; j++)
            {
                List<int> move = GetAllPos(i, hand[j], f, act);
                if (move.Count > 0)
                    moveFromI.Add(j, move);
            }
            if (moveFromI.Count > 0)
                moves.Add(i, moveFromI);
        }
        return moves;
    }
    List<int> GetAllPos(int coord, int[] m, int[] f, int act)
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
            //print(new Vector2(m[i], move));
            //print(target);
            Vector2 moveDir = new Vector2(m[i] % 5, m[i] / 5);
            Vector2 centerDir = new Vector2(center % 5, center / 5);
            moveDir -= centerDir;
            //print(moveDir);

            if (moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1] && moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1] && GetPosition(coord, move, f, act))
            {
                res.Add(move + coord);
            }
        }
        return res;
    }

    public bool GetPosition(int from, int to, int[] f, int act)
    {
        //print(from);
        //print(to);
        //print(from + to);
        return (f[from] & ((act + 1) * 4)) > 0 && (f[from + to] & ((act + 1) * 4)) == 0;
    }

    override public void MoveEnd(int from, int to)
    {
        //for (int i = 0; i < hands[active].GetMove().Length; i++)
        //print(hands[active].GetMove()[i]);
        int[] m = (int[])hands[active].GetMove().Clone();
        int opp = 1 - active;
        if (opp == 0)
        {
            //print(active);
            //print(opp);
            SetActive(opp);
            printBoard();
            return;
        }
        hands[active].SetMove((int[])sideCards[active].GetMove().Clone());
        sideCards[active].SetMove((int[])m.Clone());
        //print("RotateMove");
        // for (int i = 0; i < RotateMove(m).Length; i++)
        //print(RotateMove(m)[i]);
        //sideCards[opp].SetMove((int[])RotateMove(m).Clone());
        if (((gameBoard[to] & PieceCategory.Master) > 0) || ((to == temples[opp]) && ((gameBoard[from] & PieceCategory.Master) > 0)))
            ShowWin(active);
        gameBoard[to] = gameBoard[from];
        gameBoard[from] = PieceCategory.None;
        SetActive(opp);
        printBoard();
    }


}

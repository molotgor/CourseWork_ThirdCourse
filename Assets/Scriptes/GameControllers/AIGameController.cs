using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AIGameController : GameController
{
    [SerializeField] [Range(0, 5)] float timeWait = 1;
    
    override public void Start()
    {
        base.Start();
        gameField.AttemptMove.AddListener(GetMove);
        hands[1].DeActivate();
    }
    override protected void GetMove()
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
        //If active player is bot than start his turn

        //print("SetActive from " + active + " to " + act);
        active = act;
        if (active == 1)
        {
            //print(act);
            hands[0].DeActivate();
            StartCoroutine(CommitMove());
        }
        else
        {
            //print("Activating Player Hand");
            hands[0].Activate();
        }
    }

    protected IEnumerator CommitMove()
    {
        //print(active);
        //print("AI doing move");

        //Get moves for both players
        List<int[]> BlueMoves = new List<int[]>();
        BlueMoves.Add((int[])hands[0].GetMove(0).Clone());
        BlueMoves.Add((int[])hands[0].GetMove(1).Clone());

        List<int[]> RedMoves = new List<int[]>();
        RedMoves.Add((int[])hands[1].GetMove(0).Clone());
        RedMoves.Add((int[])hands[1].GetMove(1).Clone());

        //Get side card and gameField
        int[] side = (int[])sideCards[0].GetMove().Clone();
        int[] teorField = (int[])gameBoard.Clone();

        //Init depth and start search for move
        int depth = 2;
        ThinkOnMove(teorField, active, BlueMoves, RedMoves, side, depth, true);
        yield return new WaitForSecondsRealtime(timeWait);
    }

    int Evaluate(int[] teorField, int teorActive)
    {
        //Return value of diffetence of players board states
        int blue = countSide(teorField, PieceCategory.Blue);
        int red = countSide(teorField, PieceCategory.Red);
        int evaluation = blue - red;
        int perspective = (teorActive == 1) ? 1 : -1;
        return evaluation * perspective;
    }

    int countSide(int[] teorField, int teorActive)
    {
        //Count players board state
        int res = 0;
        for (int i = 0; i < teorField.Length; i++)
        {
            if ((teorField[i] & teorActive) > 0)
            {
                res += (teorField[i] & PieceCategory.Student) * 10;
                res += (teorField[i] & PieceCategory.Master) * 1000;
            }
        }
        //print(teorActive / 4);
        return res;
    }

    //Function for printing dictionary of moves
    void printDict(Dictionary<int, Dictionary<int, List<int>>> dict)
    {
        string res = "";
        foreach (int i in dict.Keys)
        {
            res += i;
            res += " =>";
            foreach (int j in dict[i].Keys)
            {
                res += j;
                res += " => (";
                foreach (int z in dict[i][j])
                {
                    res += z;
                    res += ", ";
                }

                res += "); ";
            }
            res += "\n";
        }
        print(res);
    }

    int ThinkOnMove(int[] teorField, int teorActive, List<int[]> teorBlueMoves, List<int[]> teorRedMoves, int[] teorSideMove, int depth, bool start)
    {
        
        //Evaluate cost of field state
        int eval = Evaluate(teorField, teorActive);
        //if (depth == 0)
            //print(eval.ToString() + "\n" + printArr2d(teorField));
        //if cost >= than its win
        if (depth == 0 || Mathf.Abs(eval) >= 1000)
        {
            return eval;
        }
        //if depth is zero than return eval
        
        List<List<int[]>> moves = new List<List<int[]>>();
        moves.Add(teorBlueMoves);
        moves.Add(teorRedMoves);
        int maxEval = int.MinValue;
        string res = "";
        Dictionary<int, Dictionary<int, List<int>>> teorMoves = GetAllMoves(teorActive, teorField, moves[teorActive]);
        Dictionary<int, Dictionary<int, List<int>>> bestMoves = new Dictionary<int, Dictionary<int, List<int>>>();
        foreach (int i in teorMoves.Keys)
        {
            if (start)
                res += i.ToString() + " => ";
            foreach (int j in teorMoves[i].Keys)
            {
                if (start)
                    res += j.ToString() + " => (";
                foreach (int z in teorMoves[i][j])
                {
                    if (start)
                        res += z.ToString() + " = ";
                    //Make move from i to z by j card
                    int[] tempField = (int[])teorField.Clone();
                    List<int[]> tempBlueMoves = teorBlueMoves;
                    List<int[]> tempRedMoves = teorRedMoves;
                    int[] tempSideMove = (int[])teorSideMove.Clone();
                    tempField[z] = tempField[i];
                    tempField[i] = PieceCategory.None;
                    //Swap card with side
                    if (teorActive > 0)
                    {
                        tempRedMoves[j] = tempSideMove;
                        tempSideMove = RotateMove(hands[teorActive].GetMove(j));
                    }
                    else
                    {
                        tempBlueMoves[j] = tempSideMove;
                        tempSideMove = RotateMove(hands[teorActive].GetMove(j));
                    }
                    //Search deeper
                    int evalSearch = -ThinkOnMove(tempField, 1 - teorActive, tempBlueMoves, tempRedMoves, tempSideMove, depth - 1, false);
                    //If EvalCost is more than max than create new dictionary with better moves
                    //if (start)
                        //res += evalSearch.ToString() + "; ";
                    if (maxEval < evalSearch)
                    {
                        maxEval = evalSearch;
                        if (!start)
                            continue;
                        List<int> l = new List<int>();
                        l.Add(z);
                        Dictionary<int, List<int>> d = new Dictionary<int, List<int>>();
                        d.Add(j, l);
                        Dictionary<int, Dictionary<int, List<int>>> dd = new Dictionary<int, Dictionary<int, List<int>>>();
                        dd.Add(i, d);
                        bestMoves = dd;
                        //printDict(bestMoves);
                    }
                    //If EvalCost is same than max than add move to dictionary
                    else if (maxEval == evalSearch)
                    {
                        if (!start)
                            continue;
                        if (bestMoves.ContainsKey(i))
                        {
                            //print("BestMovesContains" + i.ToString());
                            if (bestMoves[i].ContainsKey(j))
                            {
                                //print("BestMovesContains" + j.ToString());
                                bestMoves[i][j].Add(z);
                            }
                            else
                            {
                                //print("BestMovesDontContains" + j.ToString());
                                List<int> l = new List<int>();
                                l.Add(z);
                                bestMoves[i].Add(j, l);
                            }
                        }
                        else
                        {
                            //print("BestMovesDontContains" + i.ToString());
                            List<int> l = new List<int>();
                            l.Add(z);
                            Dictionary<int, List<int>> d = new Dictionary<int, List<int>>();
                            d.Add(j, l);
                            bestMoves.Add(i, d);
                        }
                        //printDict(bestMoves);
                    }
                }
                if (start)
                    res += "); ";
            }
            if (start)
                res += "\n";
        }

        if (!start)
            return maxEval;
        //print(res);
        //If it is start of search than make move with best cost

        //print("Commit AI BEST MOVE");
        print(maxEval);
        printDict(bestMoves);
        int Coord = bestMoves.ElementAt(Random.Range(0, bestMoves.Keys.Count)).Key;
        int Card = bestMoves[Coord].ElementAt(Random.Range(0, bestMoves[Coord].Count)).Key;
        int Into = bestMoves[Coord][Card][Random.Range(0, bestMoves[Coord][Card].Count)];
        int[] tempMove = (int[])hands[teorActive].GetMove(Card).Clone();
        //print(Coord.ToString() + " " + Card.ToString() + " " + Into.ToString());
        if (((gameBoard[Into] & PieceCategory.Master) > 0) || ((Into == temples[1 - teorActive]) && ((gameBoard[Coord] & PieceCategory.Master) > 0)))
        {
            gameBoard[Into] = gameBoard[Coord];
            gameBoard[Coord] = PieceCategory.None;
            hands[teorActive].SetMove(Card, RotateMove(teorSideMove));
            blueSideCard.SetMove(RotateMove(tempMove));
            gameField.endMoveEvent.Invoke(Coord, Into);
            ShowWin(teorActive);
            active = 2;
        }
        else
        {
            //SetActive(1 - teorActive);
            gameBoard[Into] = gameBoard[Coord];
            gameBoard[Coord] = PieceCategory.None;
            hands[teorActive].SetMove(Card, RotateMove(teorSideMove));
            blueSideCard.SetMove(RotateMove(tempMove));
            gameField.endMoveEvent.Invoke(Coord, Into);
        }
        gameField.Display(gameBoard);
        return maxEval;
    }

    Dictionary<int, Dictionary<int, List<int>>> GetAllMoves(int act, int[] f, List<int[]> hand)
    {
        /*
        for (int i = 0; i < gameField.GetHeight(); i++)
        {
            string line = "";
            for (int j = 0; j < width; j++)
            {
                line += gameBoard[i * width + j];
                line += ", ";
            }
            //print(line);
        }
        */
        Dictionary<int, Dictionary<int, List<int>>> moves = new Dictionary<int, Dictionary<int, List<int>>>();
        for (int i = 0; i < gameBoard.Length; i++)
        {
            if ((gameBoard[i] & (act + 1) * 4) < 1)
                continue;
            //For each board space with piece of active player get all possible moves
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
        //Get possible move for in X coord (max 2 in each direction)
        int indMod5 = coord % 5;
        int[] indFieldX = { ((indMod5 <= 2) ? -indMod5 : -2), ((indMod5 <= 2) ? 2 : 4 - indMod5) };
        //print(new Vector2(indFieldX[0], indFieldX[1]));
        
        //Get possible move for in Y coord (max 2 in each direction)
        int indDiv5 = coord / 5;
        int[] indFieldY = { ((indDiv5 <= 2) ? -indDiv5 : -2), ((indDiv5 <= 2) ? 2 : 4 - indDiv5) };
        //print(new Vector2(indFieldY[0], indFieldY[1]));

        for (int i = 0; i < m.Length; i++)
        {
            int move = m[i] - center;
            //print(new Vector2(m[i], move));
            //print(target);

            //Get direction of move
            Vector2 moveDir = new Vector2(m[i] % 5, m[i] / 5);
            Vector2 centerDir = new Vector2(center % 5, center / 5);
           
            moveDir -= centerDir;
            //print(moveDir);
            
            if (moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1] && moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1] && GetPosition(coord, move, f, act))
            {
                //If move in possible limit than add in result array
                res.Add(move + coord);
            }
        }
        return res;
    }

    public bool GetPosition(int from, int to, int[] f, int act)
    {
        //Show that player can or not move to possition
        return (f[from] & ((act + 1) * 4)) > 0 && (f[from + to] & ((act + 1) * 4)) == 0;
    }

    override public void MoveEnd(int from, int to)
    {
        //if bot ended turn than swap active player
        int opp = 1 - active;
        if (opp == 0)
        {
            //print(active);
            //print(opp);
            SetActive(opp);
            printBoard();
            return;
        }
        int[] m = (int[])hands[active].GetMove().Clone();
        //Swap played card with side card if player ended move
        hands[active].SetMove((int[])sideCards[active].GetMove().Clone());
        sideCards[active].SetMove((int[])m.Clone());
        //If player winning than show win screen
        if (((gameBoard[to] & PieceCategory.Master) > 0) || ((to == temples[opp]) && ((gameBoard[from] & PieceCategory.Master) > 0)))
            ShowWin(active);
        //Change board state
        gameBoard[to] = gameBoard[from];
        gameBoard[from] = PieceCategory.None;
        gameField.Display(gameBoard);
        SetActive(opp);
        printBoard();
    }
}

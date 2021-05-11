using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayerGameController : GameController
{

    override protected void SetupField()
    {
        //Connecting gameField signals with gameController functions
        TwoPlayerFieldController field = (TwoPlayerFieldController)gameField;
        field.endMoveEvent.AddListener(MoveEnd);
        field.par = this;
        field.AttemptMove.AddListener(GetMove);
    }

    override public void MoveEnd(int from, int to)
    {
        //Swap played card with side card
        int[] m = (int[])hands[active].GetMove().Clone();
        hands[active].SetMove((int[])sideCards[active].GetMove().Clone());
        int opp = 1 - active;
        sideCards[opp].SetMove((int[])RotateMove(m).Clone());
        sideCards[active].SetMove((int[])m.Clone());

        //If after move current player is winning then show win screen
        if (((gameBoard[to] & PieceCategory.Master) > 0) || ((to == temples[opp]) && ((gameBoard[from] & PieceCategory.Master) > 0)))
            ShowWin(active);

        //If game is still continues then change gameBoard and activePlayer
        gameBoard[to] = gameBoard[from];
        gameBoard[from] = PieceCategory.None;
        SetActive(opp);

        printBoard();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TwoPlayerFieldController : FieldController
{
    public TwoPlayerGameController par;
    [SerializeField] GameObject piecePrefab;
    int center;

    // Start is called before the first frame update
    override public void Start()
    {
        int middle = width / 2;
        center = middle * height + middle;
        leftSide = field.GetLeftSide();
        bottomSide = field.GetBottomSide();
        deltaPos = field.GetDeltaPos();
        SetRotation(transform.rotation);
        //Setup();
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
        //print(m);
        int indMod5 = coord % 5;
        int[] indFieldX = { ((indMod5 <= 2) ? -indMod5 : -2), ((indMod5 <= 2) ? 2 : 4 - indMod5) };
        //print(new Vector2(indFieldX[0], indFieldX[1]));
        int indDiv5 = coord / 5;
        int[] indFieldY = { ((indDiv5 <= 2) ? -indDiv5 : -2), ((indDiv5 <= 2) ? 2 : 4 - indDiv5) };
        //print(new Vector2(indFieldY[0], indFieldY[1]));
        for (int i = 0; i < moves.Length; i++)
        {
            int move = moves[i] - center;
            int target = move + coord;
            print(new Vector2(moves[i], move));
            print(target);
            Vector2 moveDir = new Vector2(moves[i] % 5, moves[i] / 5);
            Vector2 centerDir = new Vector2(center % 5, center / 5);
            moveDir -= centerDir;
            print(moveDir);

            if (moveDir.x >= indFieldX[0] && moveDir.x <= indFieldX[1] && moveDir.y >= indFieldY[0] && moveDir.y <= indFieldY[1] && ind == move + coord && par.GetPosition(coord, move))
            {
                endMoveEvent.Invoke(coord, ind);
                SetMoves(new int[] { });
                return field.GetPosOnField(ind);
            }
        }
        return field.GetPosOnField(coord);
    }

    override public void Setup()
    {
        field.SetParameters(width, height, deltaPos, piecePrefab, debug);
        field.SetGameMode("TwoPlayerFieldController");
        field.Start();
    }

    override public void Display(int[] f)
    {
        field.clearField();
        print("Display on contr");
        field.Display(f);
    }

    
}

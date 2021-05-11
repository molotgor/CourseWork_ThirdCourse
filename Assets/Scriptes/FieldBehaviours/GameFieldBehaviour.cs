using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameFieldBehaviour : FieldBehaviour
{
    // Start is called before the first frame update

    override public void Display(int[] f)
    {
        //print("Display on GameFieldBehaviour");
        int[] field = (int[])f.Clone();
        clearField();
        FieldController par = (FieldController)transform.parent.gameObject.GetComponent("FieldController");
        //print(par);
        int count = 1;
        for (int i = 0; i < height; i++)
        {
            string line = "";
            for (int j = 0; j < width; j++)
            {
                line += field[i * width + j];
                line += ", ";
                int ind = i * width + j;
                if (field[ind] == 0)
                    continue;
                //For each place in field array with piece create gameobject with apropriate parameters
                if (((field[ind] & PieceCategory.Red) > 0)) piecePrefab.transform.rotation = new Quaternion(0, 0, 180, 0);
                else piecePrefab.transform.rotation = new Quaternion(0, 0, 0, 0);
                GameObject tempObj = Instantiate(piecePrefab, pieces, false);
                tempObj.name = "Piece" + (count++);
                tempObj.transform.position = rotation * convertVector(fieldpos[ind]);
                //print(tempObj.transform.position.z);
                //print(tempObj.transform.position.z);
                PieceBehaviour temp = (PieceBehaviour)tempObj.GetComponent("PieceBehaviour");
                if ((field[ind] & PieceCategory.Blue) > 0) temp.SetColor(PieceCategory.Blue);
                if ((field[ind] & PieceCategory.Red) > 0) temp.SetColor(PieceCategory.Red);
                if ((field[ind] & PieceCategory.Student) > 0) temp.SetRank(PieceCategory.Student);
                if ((field[ind] & PieceCategory.Master) > 0) temp.SetRank(PieceCategory.Master);
                temp.field = par;
                temp.SetCoord(ind);
                PieceBehaviour n = (PieceBehaviour)tempObj.GetComponent("PieceBehaviour");
                par.endMoveEvent.AddListener(n.EndMove);
            }
            //print(line);
        }
        pieces.position = new Vector3(pieces.position.x, pieces.position.y, 0);
    }
}

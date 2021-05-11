using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFieldBehaviour : FieldBehaviour
{
    // Start is called before the first frame update
    override public void Display(int[] moves, GameObject moveShow)
    {
        clearField();
        Vector3 parChange = transform.parent.parent.position;
        //For each cell of move init move pointer
        for (int i = 0; i < moves.Length; i++)
        {
            //print(moves[i]);
            Vector2 pos = GetPosOnField(moves[i]);
            SpriteRenderer spr = moveShow.GetComponent<SpriteRenderer>();
            spr.sortingLayerName = "GamePiece";
            //print(pos);
            moveShow.transform.position = Quaternion.Inverse(rotation) * (new Vector3(pos.x, pos.y, 0) - parChange);
            Instantiate(moveShow, pieces, false);
        }
        Destroy(moveShow);
    }
}

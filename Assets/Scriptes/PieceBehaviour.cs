using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PieceBehaviour : MonoBehaviour
{
    int color;
    int rank;
    [SerializeField] Texture2D student;
    [SerializeField] Texture2D master;
    public FieldController field;
    int coord;
    private Vector2 pos;
    bool moveAllowed;
    Collider2D col;


    void Start()
    {   
        //Set sprite to represent type of piece
        col = GetComponent<Collider2D>();
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        switch (rank)
        {
            case 1:
                {
                    spr.sprite = Sprite.Create(student, new Rect(0.00F, 0.00F, 64.00F, 64.00F), new Vector2(0.5F, 0.5F), 64);
                }
                break;
            case 2:
                {
                    spr.sprite = Sprite.Create(master, new Rect(0.00F, 0.00F, 64.00F, 64.00F), new Vector2(0.5F, 0.5F), 64);
                }
                break;
        }

        switch (color)
        {
            case 4:
                {
                    spr.color = Color.blue;
                }
                break;
            case 8:
                {
                    spr.color = Color.red;
                }
                break;
        }
    }
    public void SetRank(int rankPiece)
    {
        rank = rankPiece;
    }

    public void SetColor(int colorPiece)
    {
        color = colorPiece;
    }

    public void SetCoord(int coordField)
    {
        coord = coordField;
    }
    
    void Update()
    {
        //If there is tap on screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            //And this touch collide with piece
            switch (touch.phase)
            {
                //At start we activate move
                case (TouchPhase.Began):
                    {
                        Collider2D touchCol = Physics2D.OverlapPoint(touchPosition);
                        if (col.OverlapPoint(touchPosition))
                        {
                            moveAllowed = true;
                        }
                    }
                    break;
                //If move active position is same as touch
                case (TouchPhase.Moved):
                    {
                        if (moveAllowed)
                        {
                            transform.position = new Vector3(touchPosition.x, touchPosition.y, -1);
                        }
                    }
                    break;
                //At end we try get position on field
                case (TouchPhase.Ended):
                    {
                        if (moveAllowed)
                        {
                            moveAllowed = false;
                            pos = transform.position;
                            //print(pos);
                            Vector3 moveEffect = field.GetPos(coord, pos);
                            transform.position = new Vector3(moveEffect.x, moveEffect.y, -1);
                        }
                    }
                    break;

            }
            
        }
    }

    //Function that being called at the end of move
    public void EndMove(int from, int to)
    {
        //If move start == position of piece than move that piece
        if (from == coord) 
        {
            coord = to;
            Vector3 moveEffect = field.GetPos(coord);
            transform.position = new Vector2(moveEffect.x, moveEffect.y);
            return;
        }

        //If move end == position of piece than destroy that piece
        if (to == coord)
        {
            Destroy(gameObject);
        }
    }
}

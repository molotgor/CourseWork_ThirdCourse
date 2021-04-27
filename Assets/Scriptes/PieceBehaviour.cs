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


    // Start is called before the first frame update
    void Start()
    {   
        col = GetComponent<Collider2D>();
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        switch (rank)
        {
            case 1:
                {
                    //print("Student");
                    spr.sprite = Sprite.Create(student, new Rect(0.00F, 0.00F, 64.00F, 64.00F), new Vector2(0.5F, 0.5F), 64);
                }
                break;
            case 2:
                {
                    //print("Master");
                    spr.sprite = Sprite.Create(master, new Rect(0.00F, 0.00F, 64.00F, 64.00F), new Vector2(0.5F, 0.5F), 64);
                }
                break;
        }

        switch (color)
        {
            case 4:
                {
                    //print("Blue");
                    spr.color = Color.blue;
                }
                break;
            case 8:
                {
                    //print("Red");
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

    public int GetRank()
    {
        return rank;
    }

    public int GetColor()
    {
        return color;
    }

    public int GetCategory()
    {
        return color | rank;
    }
    public void SetCoord(int coordField)
    {
        coord = coordField;
    }

    public int GetCoord()
    {
        return coord;
    }

    public void SetPosition(Vector2 pos)
    {
        //print("BEFORE");
        //print(transform.gameObject.transform.position);
        transform.position = new Vector3(pos.x, pos.y, -1);
        //print("AFTER");
        //print(transform.gameObject.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {
                case (TouchPhase.Began):
                    {
                        Collider2D touchCol = Physics2D.OverlapPoint(touchPosition);
                        //if (col == touchCol && color == field.GetCurrentPlayer())
                        if (col.OverlapPoint(touchPosition))
                        {
                            moveAllowed = true;
                        }
                    }
                    break;
                case (TouchPhase.Moved):
                    {
                        if (moveAllowed)
                        {
                            transform.position = new Vector3(touchPosition.x, touchPosition.y, -1);
                        }
                    }
                    break;
                case (TouchPhase.Ended):
                    {
                        if (moveAllowed)
                        {
                            moveAllowed = false;
                            pos = transform.position;
                            print(pos);
                            Vector3 moveEffect = field.GetPos(coord, pos);
                            transform.position = new Vector3(moveEffect.x, moveEffect.y, -1);
                            //coord = (int)moveEffect.z;
                        }
                    }
                    break;

            }
            
        }
    }

    public void EndMove(int from, int to)
    {
        if (from == coord) 
        {
            coord = to;
            Vector3 moveEffect = field.GetPos(coord);
            transform.position = new Vector2(moveEffect.x, moveEffect.y);
            return;
        }

        if (to == coord)
        {
            Destroy(gameObject);
        }
    }
}

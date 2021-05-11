using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Chosen : UnityEvent<int>
{
}

public class CardBehaviour : MonoBehaviour
{
    public Chosen ImChosen = new Chosen();
    [SerializeField] CardFieldController fieldController;
    [SerializeField] Texture2D moveShow;
    [SerializeField] Texture2D pieceTexture;
    [SerializeField] bool active;
    [SerializeField] int[] moves;
    [SerializeField] SpriteRenderer border;
    BoxCollider2D col;
    bool chosen = false;
    bool reverse = false;
    int num;
    // Start is called before the first frame update
    public void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.edgeRadius = col.edgeRadius * ((transform.localScale.x < transform.localScale.y) ? transform.localScale.x : transform.localScale.y);
        TextureReset();
        fieldController.Setup();
        fieldController.SetRotation(transform.rotation);
        fieldController.Start();
        fieldController.SetMoves(moves);
        fieldController.Display();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Collider2D touchCol = Physics2D.OverlapPoint(touchPosition);
            //If touch collides with card at the start of touch
            if (touch.phase == TouchPhase.Began && col.OverlapPoint(touchPosition) && !chosen)
            {
                if (active)
                {
                    //if it is active player hand than chose this card
                    ImChosen.Invoke(num);
                }
                else
                {
                    //if it is not active player hand than reverse this card
                    reverse = true;
                    transform.RotateAround(transform.position, Vector3.forward, 180);
                }
            }
            //If touch ended and card is reversed than reverse it back
            if (touch.phase == TouchPhase.Ended && reverse)
            {
                reverse = false;
                transform.RotateAround(transform.position, Vector3.forward, 180);
            }
        }
        
    }

    void SetBorderColor(Color c)
    {
        border.color = c;
    }

    public void SetChosen(bool ch)
    {
        chosen = ch;
        if (ch) SetBorderColor(Color.red);
        else SetBorderColor(Color.black);
    }

    public void SetNum(int n)
    {
        num = n;
    }

    public void Activate()
    {
        active = true;
    }
    public void DeActivate()
    {
        SetChosen(false);
        active = false;
    }

    public int[] GetMove()
    {
        return moves;
    }

    public void SetRotation(Quaternion r)
    {
        fieldController.SetRotation(r);
    }

    public void TextureReset()
    {
        GameObject moveObj = new GameObject();
        SpriteRenderer moveComp = moveObj.AddComponent<SpriteRenderer>();
        moveComp.sprite = Sprite.Create(moveShow, new Rect(0.00F, 0.00F, 64.00F, 64.00F), new Vector2(0.5F, 0.5F), 64);
        fieldController.SetTextures(moveObj);
    }

    public void SetMove(int[] m)
    {
        TextureReset();
        moves = (int[])m.Clone();
        fieldController.SetMoves(m);
    }
}

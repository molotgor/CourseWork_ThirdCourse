using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    CardBehaviour[] cardsInHand = new CardBehaviour[2];
    [SerializeField] int colorOfPlayer;
    [SerializeField] bool active = false;
    [SerializeField] int[] cards1;
    [SerializeField] int[] cards2;
    int chosen = -1;

    public void Start()
    {
        //Add cards to the hand
        for(int i = 0; i < cardsInHand.Length; i++)
        {
            cardsInHand[i] = (CardBehaviour)transform.GetChild(i).GetComponent("CardBehaviour");
            cardsInHand[i].SetNum(i);
            cardsInHand[i].ImChosen.AddListener(ChangeChosen);
            cardsInHand[i].SetRotation(transform.rotation);
        }
        SetMove(cards1, cards2);
    }

    public void StartCards()
    {
        cardsInHand[0].Start();
        cardsInHand[1].Start();
    }

    public void ChangeChosen(int num)
    {
        //Make chosen card at num
        cardsInHand[num].SetChosen(true);
        chosen = num;
        //Make not chosen other cards
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            if (i == num) continue;
            cardsInHand[i].SetChosen(false);
        }
    }

    public void Activate()
    {
        active = true;
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            cardsInHand[i].Activate();
        }
    }

    public void DeActivate()
    {
        active = false;
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            cardsInHand[i].DeActivate();
        }
    }

    public int[] GetMove()
    {
        //Get move if any card chosen
        if (chosen > -1) return cardsInHand[chosen].GetMove();
        return new int[]{};
    }

    public int[] GetMove(int i)
    {
        //Get move of i card
        if (i < 2) return cardsInHand[i].GetMove();
        return new int[] { };
    }
    public void SetMove(int[] m)
    {
        //Set move of chosen card
        if (chosen > -1) cardsInHand[chosen].SetMove((int[])m.Clone());
    }
    public void SetMove(int[] m1, int[] m2)
    {
        //Set move of both cards
        cardsInHand[0].SetMove((int[])m1.Clone());
        cardsInHand[1].SetMove((int[])m2.Clone());
    }
    public void SetMove(int i, int[] m)
    {
        //Set move of i card
        cardsInHand[i].SetMove((int[])m.Clone());
    }
}

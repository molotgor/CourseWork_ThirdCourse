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
    // Start is called before the first frame update
    public void Start()
    {
        for(int i = 0; i < cardsInHand.Length; i++)
        {
            cardsInHand[i] = (CardBehaviour)transform.GetChild(i).GetComponent("CardBehaviour");
            cardsInHand[i].SetNum(i);
            cardsInHand[i].ImChosen.AddListener(ChangeChosen);
            cardsInHand[i].SetRotation(transform.rotation);
        }
        SetHand(cards1, cards2);
    }

    public void StartCards()
    {
        cardsInHand[0].Start();
        cardsInHand[1].Start();
    }

    public void ChangeChosen(int num)
    {
        cardsInHand[num].SetChosen(true);
        chosen = num;
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
            print("activate " + i + " card in " + name);
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
        if (chosen > -1) return cardsInHand[chosen].GetMove();
        return new int[]{};
    }

    public int[] GetMove(int i)
    {
        if (i < 2) return cardsInHand[i].GetMove();
        return new int[] { };
    }
    public void SetMove(int[] m)
    {
        if (chosen > -1) cardsInHand[chosen].SetMove((int[])m.Clone());
    }
    public void SetMove(int[] m1, int[] m2)
    {
        cardsInHand[0].SetMove((int[])m1.Clone());
        cardsInHand[1].SetMove((int[])m2.Clone());
    }
    public void SetMove(int i, int[] m)
    {
        cardsInHand[i].SetMove((int[])m.Clone());
    }
    public void SetHand(int[] m1, int[] m2)
    {
        cardsInHand[0].SetMove(m1); 
        cardsInHand[1].SetMove(m2);
    }
}

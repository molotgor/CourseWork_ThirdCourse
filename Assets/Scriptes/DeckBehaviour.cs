using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<int, int[]> deck;
    int[] order;
    public void Start()
    {
        //Initialize deck
        order = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        deck = new Dictionary<int, int[]> {
            { 0, new int[]{ 7, 16, 18 } },
            { 1, new int[]{ 7, 13, 17 } },
            { 2, new int[]{ 11, 13, 17 } },
            { 3, new int[]{ 8, 11, 13, 16 } },
            { 4, new int[]{ 6, 8, 15, 19 } },
            { 5, new int[]{ 6, 8, 17 } },
            { 6, new int[]{ 6, 14, 18 } },
            { 7, new int[]{ 8, 11, 18 } },

            { 8, new int[]{ 10, 14, 17 } },
            { 9, new int[]{ 7, 11, 17 } },
            { 10, new int[]{ 8, 10, 16 } },
            { 11, new int[]{ 6, 8, 16, 18 } },
            { 12, new int[]{ 6, 11, 13, 18 } },
            { 13, new int[]{ 11, 13, 16, 18 } },
            { 14, new int[]{ 7, 22 } },
            { 15, new int[]{ 6, 13, 16 } },

            { 16, new int[]{ 6, 17, 19 } },
            { 17, new int[]{ 8, 14, 16 } },
            { 18, new int[]{ 8, 10, 17 } },
            { 19, new int[]{ 7, 15, 19 } },
            { 20, new int[]{ 8, 15, 17 } },
            { 21, new int[]{ 8, 11, 17 } },
            { 22, new int[]{ 8, 13, 18 } },
            { 23, new int[]{ 2, 21, 23 } },

            { 24, new int[]{ 8, 16, 17 } },
            { 25, new int[]{ 6, 14, 17 } },
            { 26, new int[]{ 6, 13, 17 } },
            { 27, new int[]{ 6, 17, 18 } },
            { 28, new int[]{ 6, 11, 16 } },
            { 29, new int[]{ 6, 10, 18 } },
            { 30, new int[]{ 10, 14, 16, 18 } },
            { 31, new int[]{ 6, 8, 10, 14 } }
        };
    }

    public void Shuffle()
    {
        int n = order.Length;
        while (n > 1)
        {
            //Take random element and swap with last
            n--;
            int k = Random.Range(0, n + 1);
            int val = order[k];
            order[k] = order[n];
            order[n] = val;
        }

        /*
         * print deck
        string res = "";
        for (int i = 0; i < order.Length; i++)
        {
            res += order[i];
            res += ", ";
        }
        print(res);
        */
    }

    public int[] getCard(int key)
    {
        return deck[order[key]];
    }
}

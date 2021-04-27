using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Concede : UnityEvent<int>
{
}


public class ConcedeButtons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ConcedeButton BlueConcede;
    [SerializeField] ConcedeButton RedConcede;
    public Concede concede = new Concede();

    // Update is called once per frame
    public void ChangeActive(int act)
    {
        if (act > 0)
        {
            BlueConcede.gameObject.SetActive(false);
            RedConcede.gameObject.SetActive(true);
        }
        else
        {
            RedConcede.gameObject.SetActive(false);
            BlueConcede.gameObject.SetActive(true);
        }
    }

    public void Concede(int c)
    {
        concede.Invoke(c);
    }
}

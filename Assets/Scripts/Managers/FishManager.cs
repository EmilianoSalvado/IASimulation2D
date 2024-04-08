using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishManager : MonoBehaviour
{
    HashSet<Boid> _fishes = new HashSet<Boid>();

    public static FishManager instance;
    [SerializeField] TMPro.TextMeshProUGUI _number;
    [SerializeField] int _initQ;

    private void Awake()
    {
        instance = this;
    }

    public void Add(Boid b)
    {
        _fishes.Add(b);
        Refresh(_initQ);
    }

    public void Refresh(Slider q)
    {
        int count = 0;

        foreach (Boid b in _fishes)
        {
            if (count < q.value)
            {
                if (!b.gameObject.activeSelf)
                    b.gameObject.SetActive(true);
                count++;
            }
            else
                b.gameObject.SetActive(false);
        }

        _number.text = q.value.ToString();
    }

    public void Refresh(int q)
    {
        int count = 0;

        foreach (Boid b in _fishes)
        {
            if (count < q)
            {
                if (!b.gameObject.activeSelf)
                    b.gameObject.SetActive(true);
                count++;
            }
            else
                b.gameObject.SetActive(false);
        }

        _number.text = q.ToString();
    }
}

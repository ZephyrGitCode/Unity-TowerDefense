using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] float buildTime = 1f;
    [SerializeField] public int cost = 10;

    private void Start() {
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        // Disable all children
        // Enable all children sequentially
        // based on buildTime variable

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTime);
            foreach(Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
}

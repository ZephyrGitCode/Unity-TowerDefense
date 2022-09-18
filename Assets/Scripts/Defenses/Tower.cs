using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField, Tooltip("Time in seconds to build the defence unit.")]
    float buildTime = 1f;

    [SerializeField, Tooltip("This tower's name.")]
    public string myName = "Name";

    [SerializeField, Tooltip("This tower's Type.")]
    public string myType = "Ranged Single Target";

    [SerializeField, Tooltip("Cost to build the tower.")]
    public int myCost = 10;

    [Tooltip("My Prefab, used by BuyDefence.")]
    public GameObject myPrefab;

    public void Activate() {
        StartCoroutine(Build());
    }

    /// <summary>
    /// Disable all children, enable all children sequentially, buildTime determines this.
    /// </summary>
    IEnumerator Build()
    {
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

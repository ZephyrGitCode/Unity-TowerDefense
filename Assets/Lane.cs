using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [Tooltip("Bool for knowing which player is assigned to which lane.")]
    public PlayerSelector myPlayer;

    [Tooltip("Finish line for lane.")]
    public EnemySpawner enemySpawner;

    [Tooltip("Finish line for lane.")]
    public Transform finishLine;
}

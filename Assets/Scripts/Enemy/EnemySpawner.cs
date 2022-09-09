using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles spawning new units. Spawn area is defined by object's collider bounds.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// Instantiate new units within spawn area. Called by user when spawning new units.
    /// </summary>
    /// <param name="unit"></param>
    public bool DeployUnit(GameObject unit, Lane lane)
    {
        GameObject newUnit = GameObject.Instantiate(unit, GetSpawnPosition(), Quaternion.identity);
        //newUnit.GetComponent<Enemy>().myLane = this.transform;
        Enemy enemy = newUnit.transform.GetComponent<Enemy>();
        Debug.Log("Enemy: "+enemy);
        enemy.myBank = lane.myPlayer.myBank;
        UnitMover unitMover = newUnit.transform.GetComponent<UnitMover>();
        unitMover.SetDestination(lane.finishLine);

        return true;
    }
    
    /// <summary>
    /// Return random Vector3 position within box collider bounds. Called by DeployUnit.
    /// </summary>
    public Vector3 GetSpawnPosition()
    {
        Bounds bounds = GetComponent<Collider>().bounds;
        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

        return bounds.center + new Vector3(offsetX, transform.position.y, offsetZ);
    }
}

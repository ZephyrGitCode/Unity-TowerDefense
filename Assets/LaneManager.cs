using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public Lane[] myLanes;

    /// <summary>
    /// Assign a lane to a user, called from PlayerSelector on new player.
    /// </summary>
    /// <param name="player"></param>
    public Lane AssignLane(PlayerSelector player)
    {
        foreach (Lane lane in myLanes)
        {
            if (lane.myPlayer == null)
            {
                lane.myPlayer = player;
                return lane;
            }
        }
        return null;
    }
}

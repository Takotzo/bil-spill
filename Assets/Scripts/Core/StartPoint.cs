using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private static List<StartPoint> startPoints = new List<StartPoint>();
    
    private static int count = 0;
    
    public static Transform GetStartPos()
    {
        Transform point = startPoints[count].transform;            
        count++;
        if (count == startPoints.Count)
            count = 0;
        
        return point;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private void OnEnable()
    {
        startPoints.Add(this);
    }

    private void OnDisable()
    {
        startPoints.Remove(this);
    }

}

using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public static List<StartPoint> startPoints = new List<StartPoint>();
    
    
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

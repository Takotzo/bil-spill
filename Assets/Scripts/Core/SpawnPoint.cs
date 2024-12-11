using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class SpawnPoint : MonoBehaviour
    {
        private static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    
        private static int count = 0;
    
        public static Vector3 GetRandomSpawnPos()
        {
            if (spawnPoints.Count == 0)
            {
                return Vector3.zero;
            } 
            
            Vector3 point = spawnPoints[count].transform.position;            
            count++;

            return point;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }

        private void OnEnable()
        {
            spawnPoints.Add(this);
        }

        private void OnDisable()
        {
            spawnPoints.Remove(this);
        }
    }
}

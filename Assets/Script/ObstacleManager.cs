using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Edge cameraEdge;
    public float spacing;
    public float coverageDistance = 1;
    public int neighborhood, obstacleLimit;
    public List<GameObject> obstacleList = new List<GameObject>();
    public List<GameObject> activeObstacle = new List<GameObject>();

    void  Start()
    {
        SpawnInsideCamera(10);
    }

    void Update()
    {
        SpawnOutsideCamera();
        Despawn();
    }

    private GameObject ObstacleType(int first = 0, int? last = null) {
        if(last == null)
            last = obstacleList.Count - 1;

        int index = Random.Range(first, (int)last);

        return obstacleList[index];
    }

    private Vector3? RandomPosition(string location = "outside")
    {
        Vector3 loc = Vector3.zero;
        float spawnPointX = 0;
        float spawnPointY = 0;

        bool InsideCamera(Vector3 loc) {
            if((loc.x >= cameraEdge.world[3]) || (loc.x <= cameraEdge.world[1]))
                return true;
            else if((loc.y >= cameraEdge.world[2]) || (loc.y <= cameraEdge.world[0]))
                return true;
            else
                return false;
        }

        if(location == "outside") {
            // FIXME: spawnloc should be in front of car

            spawnPointX = Random.Range(cameraEdge.world[3] * coverageDistance, cameraEdge.world[1] * coverageDistance);
            spawnPointY = Random.Range(cameraEdge.world[2] * coverageDistance, cameraEdge.world[0] * coverageDistance);
            loc.Set(spawnPointX, spawnPointY, 0);
            
            if(!InsideCamera(loc))
                return null;
        }
        else if(location == "inside") {
            spawnPointX = Random.Range(cameraEdge.world[3], cameraEdge.world[1]);
            spawnPointY = Random.Range(cameraEdge.world[2], cameraEdge.world[0]);
            loc.Set(spawnPointX, spawnPointY, 0);   
        }

        return loc;
    }

    private Quaternion RandomRotation()
    {
        return Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    private bool Crowded(Vector2 position, int? neighborhoodLimit = null)
    {
        if(neighborhoodLimit == null) neighborhoodLimit = neighborhood;
        Collider2D[] obstacleHit = Physics2D.OverlapCircleAll(position, spacing, LayerMask.GetMask("Obstacle"));
        
        return(obstacleHit.Length > neighborhoodLimit);
    }

    private void SpawnInsideCamera(int limit = 5)
    {
        for(int i = 0; i < limit; i++) {
            Vector3 position = (Vector3)RandomPosition("inside");
            
            if(Crowded(position))
                continue;

            GameObject obstacle = Instantiate(ObstacleType(3), position, RandomRotation(), gameObject.transform);
            activeObstacle.Add(obstacle);
        }
    }

    private void SpawnOutsideCamera()
    {
        Vector3 position = (Vector3)RandomPosition();
        
        if((position == null) || Crowded(position, 0))
            return;

        GameObject obstacle = Instantiate(ObstacleType(), position, RandomRotation(), gameObject.transform);
        activeObstacle.Add(obstacle);
    }

    private void Despawn()
    {
        // FIXME: despawn obstacle behind car
        if(activeObstacle.Count < obstacleLimit) return;

        for(int i = 0; i < 5; i++) {
            Destroy(activeObstacle[i]);
            activeObstacle.RemoveAt(i);
        }
    }
}

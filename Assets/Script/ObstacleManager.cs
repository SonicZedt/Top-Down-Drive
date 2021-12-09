using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Edge cameraEdge;
    
    [Header("Spawn Setting")]
    public float spacing;
    public int neighborhood, obstacleLimit;
    public GameObject spawnArea;
    public GameObject despawnArea;

    [Header("Obstacles")]
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
            float yt = spawnArea.transform.position.y + spawnArea.transform.localScale.y;
            float xr = spawnArea.transform.position.x + spawnArea.transform.localScale.x;
            float xl = spawnArea.transform.position.x - spawnArea.transform.localScale.x;
            float yb = spawnArea.transform.position.y - spawnArea.transform.localScale.y;

            spawnPointX = Random.Range(xl, xr);
            spawnPointY = Random.Range(yt, yb);
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
        if(activeObstacle.Count < obstacleLimit)
            return;

        bool InDespawnArea(GameObject obstacle) {
            Vector3 obstaclePosition = obstacle.transform.position;
            float yt = despawnArea.transform.position.y + despawnArea.transform.localScale.y;
            float xr = despawnArea.transform.position.x + despawnArea.transform.localScale.x;
            float xl = despawnArea.transform.position.x - despawnArea.transform.localScale.x;
            float yb = despawnArea.transform.position.y - despawnArea.transform.localScale.y;

            if((obstaclePosition.x >= xl) && (obstaclePosition.x <= xr))
                if((obstaclePosition.y >= yb) && (obstaclePosition.y <= yt))
                    return true;
                else
                    return false;
            else
                return false;
        }

        for(int i = 0; i < activeObstacle.Count - 1; i++) {
            if(!InDespawnArea(activeObstacle[i]))
                continue;
            
            Destroy(activeObstacle[i]);
            activeObstacle.RemoveAt(i);
        }
    }
}

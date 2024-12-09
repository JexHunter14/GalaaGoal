using UnityEngine;

public class DiamondSpawner : MonoBehaviour
{
    public GameObject diamondPrefab;
    public int numberOfDiamonds = 4;
    private Bounds combinedBounds;
    public float margin = 0.5f;
    public float minDistanceFromEnemies = 1f; 

    void Start()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Walls");

        if (walls.Length == 0)
        {
            Debug.LogError("No objects with tag 'Walls' found.");
            return;
        }

        CalculateCombinedBounds(walls);
        PlaceDiamonds();
    }

    void CalculateCombinedBounds(GameObject[] walls)
    {
        Renderer firstWallRenderer = walls[0].GetComponent<Renderer>();
        combinedBounds = firstWallRenderer.bounds;

        foreach (GameObject wall in walls)
        {
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                combinedBounds.Encapsulate(wallRenderer.bounds);
            }
        }

        combinedBounds.Expand(new Vector3(-margin * 2, -margin * 2, 0));
    }

    void PlaceDiamonds()
    {
        GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");

        if (diamonds.Length < numberOfDiamonds)
        {
            int toInstantiate = numberOfDiamonds - diamonds.Length;
            for (int i = 0; i < toInstantiate; i++)
            {
                Instantiate(diamondPrefab, GetValidRandomPosition(), Quaternion.identity);
            }

            diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        }

        for (int i = 0; i < numberOfDiamonds; i++)
        {
            if (i < diamonds.Length)
            {
                diamonds[i].transform.position = GetValidRandomPosition();
            }
        }
    }

    public void RespawnDiamond(GameObject diamond)
    {
        diamond.GetComponent<Collider2D>().enabled = true;
        diamond.GetComponent<Rigidbody2D>().isKinematic = true;
        diamond.transform.SetParent(null);
        diamond.transform.position = GetValidRandomPosition();
    }

    Vector3 GetValidRandomPosition()
    {
        Vector3 randomPosition;
        bool validPosition = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        do
        {
            randomPosition = new Vector3(
                Random.Range(combinedBounds.min.x, combinedBounds.max.x),
                Random.Range(combinedBounds.min.y, combinedBounds.max.y),
                0f);

            validPosition = true;

            foreach (GameObject enemy in enemies)
            {
                if (Vector3.Distance(randomPosition, enemy.transform.position) < minDistanceFromEnemies)
                {
                    validPosition = false;
                    break;
                }
            }

        } while (!validPosition);

        return randomPosition;
    }
}

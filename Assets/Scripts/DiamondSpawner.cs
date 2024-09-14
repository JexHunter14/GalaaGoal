using UnityEngine;

public class DiamondSpawner : MonoBehaviour
{
    public GameObject diamondPrefab;
    public int numberOfDiamonds = 4;
    private Camera mainCamera;
    private float cameraWidth;
    private float cameraHeight;

    void Start()
    {
        mainCamera = Camera.main;
        CalculateCameraBounds();
        PlaceDiamonds();
    }

    void CalculateCameraBounds()
    {
     
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        
        cameraWidth = screenBounds.x * 2;
        cameraHeight = screenBounds.y * 2;
    }

    void PlaceDiamonds()
    {
       
        GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");

       
        if (diamonds.Length < numberOfDiamonds)
        {
            int toInstantiate = numberOfDiamonds - diamonds.Length;
            for (int i = 0; i < toInstantiate; i++)
            {
                Instantiate(diamondPrefab, GetRandomPosition(), Quaternion.identity);
            }

            diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        }

       
        for (int i = 0; i < numberOfDiamonds; i++)
        {
            if (i < diamonds.Length)
            {
                diamonds[i].transform.position = GetRandomPosition();
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        
        float x = Random.Range(mainCamera.transform.position.x - cameraWidth / 2, mainCamera.transform.position.x + cameraWidth / 2);
        float y = Random.Range(mainCamera.transform.position.y - cameraHeight / 2, mainCamera.transform.position.y + cameraHeight / 2);
        return new Vector3(x, y, 0f); 
    }
}

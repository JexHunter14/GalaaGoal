using UnityEngine;
using UnityEngine.UI;

public class DiamondCollector : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    private GameObject attachedDiamond = null; 
    public Transform followPoint; 
    public float followSpeed = 5f;
    public Vector3 diamondOffset = new Vector3(0, -1f, 0);
    void Start()
    {
        UpdateScoreText();
    }

    void Update()
    {
       
        if (attachedDiamond != null)
        {
           
            Vector3 targetPosition = followPoint.position + diamondOffset;
            attachedDiamond.transform.position = Vector3.Lerp(attachedDiamond.transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Diamond") && attachedDiamond == null)
        {
            AttachDiamond(collision.gameObject);
        }
       
        else if (collision.gameObject.CompareTag("Area") && attachedDiamond != null)
        {
            DropOffDiamond();
        }
    }

    void AttachDiamond(GameObject diamond)
    {
        attachedDiamond = diamond; 
        attachedDiamond.GetComponent<Collider2D>().enabled = false; 
        attachedDiamond.GetComponent<Rigidbody2D>().isKinematic = true; 
        attachedDiamond.transform.SetParent(this.transform); 
    }

    void DropOffDiamond()
    {
   
        score++;
        UpdateScoreText();

        
        Destroy(attachedDiamond);
        attachedDiamond = null; 
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}

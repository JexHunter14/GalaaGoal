using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletOnline : MonoBehaviourPun
{
  public float speed = 5f;
  public float lifeTime = 3f;
  private Rigidbody2D rb2D;
  public GameObject shooter;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
         rb2D.velocity = transform.up * speed;
         rb2D.drag = 0;
         rb2D.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision){
      PhotonView collisionPV = collision.gameObject.GetComponent<PhotonView>();
      if(collisionPV != null){
        if(collisionPV.gameObject.CompareTag("Player")){
          int collisionPlayerID = collisionPV.Owner.ActorNumber;
          PhotonView shooterPv = shooter.GetComponent<PhotonView>();
          if(collisionPlayerID == shooterPv.Owner.ActorNumber){
            //Debug.LogError("you shot yourself");
          }
          else {
             //Debug.LogError("you shot someone else");
             DiamondCollectorOnline dcOnline = collisionPV.GetComponent<DiamondCollectorOnline>();
             updateHealth uphealth = dcOnline.Health.GetComponent<updateHealth>();

             PhotonView healthPv = uphealth.GetComponent<PhotonView>();
             bool beDamaged = uphealth.beDamaged;
             healthPv.RPC("damage", RpcTarget.AllBuffered, beDamaged);
             healthPv.RPC("updateHPdmg", RpcTarget.AllBuffered);
             if (photonView.IsMine)
               {
                   // Local client owns the bullet
                   PhotonNetwork.Destroy(gameObject);
               }
          }

        }
      }


      rb2D.velocity = transform.up * speed;
    }

    [PunRPC]
    private void RequestDestroy(int viewID)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView target = PhotonView.Find(viewID);
            if (target != null)
            {
                PhotonNetwork.Destroy(target.gameObject);
            }
        }
    }
}

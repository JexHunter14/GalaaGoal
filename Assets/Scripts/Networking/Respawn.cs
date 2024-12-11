using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Respawn : MonoBehaviourPun
{
  private float respawnTime = 2f;
  private float blinkInterval = 0.2f;
  private Movement movement;
  private SpriteRenderer spriterenderer;
  //private PolygonCollider2D playerCollider2D;
  private updateHealth upHealth;
  private GameObject Health;
  private int spawnIndex;

  private static Vector3[] playerPositions = {
    new Vector3(-4, 7, 0),
    new Vector3(3, 6, 0),
    new Vector3(-4,-7,0),
    new Vector3(3,-6,0)
  };
    // Start is called before the first frame update
    void Start()
    {

    }
    [PunRPC]
    public void RespawnPlayer(){
        if(photonView.IsMine){
          //Debug.LogError("Starting Respawn");
          DiamondCollectorOnline dcOnline = gameObject.GetComponent<DiamondCollectorOnline>();
          spriterenderer = gameObject.GetComponent<SpriteRenderer>();
          movement = gameObject.GetComponent<Movement>();
          //playerCollider2D = gameObject.GetComponent<PolygonCollider2D>();
          Health = dcOnline.Health;
          upHealth = Health.GetComponent<updateHealth>();
          spawnIndex = dcOnline.spawnIndex;
          StartCoroutine(RespawnRoutine());
      }
    }
    private IEnumerator RespawnRoutine()
    {
      if(movement != null){
        movement.enabled = false;
      }
      if(movement == null){
        //Debug.Log("Movement is null for some reason");
      }
      //playerCollider2D.enabled = false;

      gameObject.transform.position = playerPositions[spawnIndex-1];

      float timeSpent = 0f;
      while(timeSpent < respawnTime){
        spriterenderer.enabled = !spriterenderer.enabled;
        yield return new WaitForSeconds(blinkInterval);
        timeSpent += blinkInterval;
      }
      spriterenderer.enabled = true;
      if(movement != null){
        movement.enabled = true;
      }
      //playerCollider2D.enabled = true;
      upHealth.beDamaged = true;
      upHealth.health = 100;
      PhotonView healthPv = Health.GetComponent<PhotonView>();
      healthPv.RPC("updateHP", RpcTarget.AllBuffered, 100);
      upHealth.respawning = false;
    }
}

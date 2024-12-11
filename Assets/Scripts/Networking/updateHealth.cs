using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class updateHealth : MonoBehaviourPun
{
  public GameObject player;
  public int health = 100;
  public bool beDamaged = true;
  public bool respawning = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(health == 0 && !respawning){
        Respawn respawn = player.GetComponent<Respawn>();
        PhotonView playerPv = player.GetComponent<PhotonView>();
        respawning = true;
        playerPv.RPC("RespawnPlayer", RpcTarget.AllBuffered);
      }
    }
    [PunRPC]
    public void damage(bool beDamaged){
      if(beDamaged){
        health = health - 25;
        if(health == 0){
          beDamaged = false;
        }
    }

    }
    [PunRPC]
    public void updateHP(int hp){
      health = hp;
      Text healthText = gameObject.GetComponent<Text>();
      healthText.text = $"HP:{health}%";
    }
    [PunRPC]
    public void updateHPdmg(){
      Text healthText = gameObject.GetComponent<Text>();
      healthText.text = $"HP:{health}%";
    }
}

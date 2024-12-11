using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class onlineShooting : MonoBehaviourPun
{

  public GameObject projectilePrefab;
  private Transform shootPoint;
  public float shootSpeed;
  private bool coolDowned = true;

    // Update is called once per frame
    void Start(){
      shootPoint = gameObject.transform;
    }

    void Update()
    {
      if(photonView.IsMine && Input.GetKeyDown(KeyCode.Space) && coolDowned){
        Shoot();
      }
    }
    private void Shoot(){
      Vector3 shootPosition = shootPoint.position + shootPoint.up * 1f;
      GameObject Bullet = PhotonNetwork.Instantiate("Bullet", shootPosition, shootPoint.rotation);
      int BulletID = Bullet.GetComponent<PhotonView>().ViewID;
      photonView.RPC("setUpBullet", RpcTarget.AllBuffered, BulletID);
      coolDowned = false;
      StartCoroutine(CoolDown());
    }
    [PunRPC]
    private void setUpBullet(int BulletID){
      PhotonView bulletPv = PhotonView.Find(BulletID);
      GameObject bullet = bulletPv.gameObject;
      bulletOnline bOnline = bullet.GetComponent<bulletOnline>();
      bOnline.shooter = gameObject;
    }
    private IEnumerator CoolDown(){
      yield return new WaitForSeconds(2f);
      coolDowned = true;
    }
}

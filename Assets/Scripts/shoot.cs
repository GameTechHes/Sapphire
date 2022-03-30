using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
 
public class shoot : MonoBehaviour
{
   public GameObject projectile;
   public int ammoCount = 10;
   private bool _canShoot = true;
   public int timeBetweenShots = 1;   
   public float launchVelocity = 700f;
   public Text _ammoText;


   void Start(){
      _ammoText.text = ammoCount.ToString();
   }

   IEnumerator Fire(){
      if (ammoCount > 0){
         yield return null;
      
      ammoCount--;
      _ammoText.text = ammoCount.ToString();
      Debug.Log("A button was pressed");
      GameObject ball = Instantiate(projectile, transform.position, transform.rotation);
      ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity,0));
      yield return new WaitForSeconds(timeBetweenShots);
      _canShoot = true;
      }
   }
 
   void Update(){
      if(_canShoot && Keyboard.current.enterKey.wasPressedThisFrame){
         _canShoot = false;
         StartCoroutine(Fire());
      }
   }
}
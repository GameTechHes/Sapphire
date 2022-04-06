using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StarterAssets;
 
public class shoot : MonoBehaviour
{
   public GameObject projectile;
   public int ammoCount = 10;
   private bool _canShoot = true;
   public int timeBetweenShots = 1;   
   public float launchVelocity = 700f;
   public Text _ammoText;
   GameObject PlayerArmature;
	private StarterAssetsInputs _input;


   void Start(){
      _ammoText.text = ammoCount.ToString();
      PlayerArmature = GameObject.Find("PlayerArmature");
      _input = PlayerArmature.GetComponent<StarterAssetsInputs>();
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
      if(_input.shoot){
         _input.shoot = false;
         if(_canShoot && Keyboard.current.enterKey.wasPressedThisFrame){
            Debug.Log("Player shot.");
            _canShoot = false;
            StartCoroutine(Fire());
         }
      }
   }
}
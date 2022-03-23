using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BarrelLauncher : MonoBehaviour
{
    public Transform barrelPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Transform obj = Instantiate(barrelPrefab, transform.position + transform.forward * 2, transform.rotation);
            var rbody = obj.GetComponent<Rigidbody>();
            rbody.velocity = transform.forward * 2;
            Destroy(obj.gameObject, 8);
        }
    }
}

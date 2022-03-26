using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levitation : MonoBehaviour
{
    public float frequency = 1f;
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
    float timeEllapsed;
    bool levitate = false;
    public float amplitude = 0.5f; 
    // Use this for initialization
    void Start () {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        StartCoroutine(setLevitation());
    }
     
    // Update is called once per frame
    void Update () {
        // Spin object around Y-Axis
        // transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        if(levitate == true){
            tempPos = posOffset;
            tempPos.y += Mathf.Sin (timeEllapsed * Mathf.PI * frequency) * amplitude;
            
            transform.position = tempPos;
            timeEllapsed += Time.deltaTime;
        }
        // Float up/down with a Sin()

    }
    IEnumerator setLevitation(){
        float wait_time = Random.Range (1f, 2f);
        yield return new WaitForSeconds (wait_time);
        levitate = true;
        timeEllapsed = 0;
    }
}

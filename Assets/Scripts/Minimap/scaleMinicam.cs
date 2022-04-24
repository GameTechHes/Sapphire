using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleMinicam : MonoBehaviour
{
    float distanceX = 0;
    float distanceZ = 0;
    Camera minicam;
    public GameObject camUI;
    RectTransform rectTransform;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        minicam = GetComponent<Camera>();
        originalRotation = minicam.transform.rotation;

        if(camUI){
            Debug.Log("CamUI found");
        }
        rectTransform = camUI.GetComponent<RectTransform>();
        float minX = 10000f;
        float minZ = 10000f;
        float maxX = -10000f;
        float maxZ = -10000f;
        foreach(GameObject refPoint in GameObject.FindGameObjectsWithTag("ReferencePoint")){
             Debug.Log("ReferencePoint");
             minX = refPoint.transform.position.x < minX ?  refPoint.transform.position.x : minX;
             minZ = refPoint.transform.position.z < minZ ?  refPoint.transform.position.z : minZ;
             maxX = refPoint.transform.position.x > maxX ?  refPoint.transform.position.x : maxX;
             maxZ = refPoint.transform.position.z > maxZ ?  refPoint.transform.position.z : maxZ;
        }
        Debug.Log("Distance X" + (maxX - minX).ToString());
        Debug.Log("Distance Z" + (maxZ - minZ).ToString());
        distanceZ = maxZ - minZ;
        distanceX = maxX - minX;

        if(distanceX < distanceZ){
            minicam.orthographicSize = 0.5f * distanceZ;
        } else{
            float unitsPerPixel = distanceX / rectTransform.rect.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * rectTransform.rect.height;
            Debug.Log(desiredHalfHeight.ToString());
            minicam.orthographicSize = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        minicam.transform.rotation = originalRotation;
        
    }
}

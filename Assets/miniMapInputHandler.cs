using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class miniMapInputHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler
 {
    public Camera minicam;
    public GameObject myPrefab;
     public void OnPointerDown( PointerEventData eventData )
     {
     }
 
     public void OnPointerUp( PointerEventData eventData )
     {
     }

     public void OnScroll(PointerEventData eventData){
        Debug.Log(eventData.scrollDelta);
        float minSize = 5;
        float maxSize = 200;
        float scrollingUnit = 20;
        float scroll = eventData.scrollDelta.y;
        if(scroll > 0 && minicam.orthographicSize - scrollingUnit > minSize){
            scrollingUnit = -20;
        }
        else if(scroll < 0 && minicam.orthographicSize + scrollingUnit < maxSize){
            scrollingUnit = 20;
        }
        else{
            scrollingUnit = 0;
        }
        minicam.orthographicSize += scrollingUnit;
     }

 
     public void OnPointerClick(PointerEventData eventData){
        
        Vector2 curosr = new Vector2(0, 0);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform,
            eventData.pressPosition, eventData.pressEventCamera, out curosr))
        {

            Texture texture = GetComponent<RawImage>().texture;
            Rect rect = GetComponent<RawImage>().rectTransform.rect;

            float coordX = Mathf.Clamp(0, (((curosr.x - rect.x) * texture.width) / rect.width), texture.width);
            float coordY = Mathf.Clamp(0, (((curosr.y - rect.y) * texture.height) / rect.height), texture.height);

            float calX = coordX / texture.width;
            float calY = coordY / texture.height;

       
            curosr = new Vector2(calX, calY);
            
            CastRayToWorld(curosr);
        }
        
        
    }
    
    private void CastRayToWorld(Vector2 vec)
    {
        Ray MapRay = minicam.ScreenPointToRay(new Vector2(vec.x * minicam.pixelWidth,
            vec.y * minicam.pixelHeight));

        RaycastHit miniMapHit;

        if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
        {
            Debug.Log("miniMapHit: " + miniMapHit.collider);
            Debug.DrawRay(MapRay.origin, MapRay.direction*1000, Color.green, 5);

            if(miniMapHit.collider.GetComponent<SpawnArea>() != null){
                Debug.Log("Can Spawn a mob here!");
                Instantiate(myPrefab, miniMapHit.collider.transform.position, Quaternion.identity);
            }
        }

        
    }
 }

using System;
using Sapphire;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minimap
{
    public class MiniMapInputHandler : MonoBehaviour, IPointerClickHandler, IScrollHandler
    {
        public Camera minicam;
        // public GameObject myPrefab;
        private NetworkManager nm;
        private const float ScrollMultiplier = 0.2f;

        private void Update()
        {
            if (minicam == null && Player.Local != null)
            {
                minicam = Player.Local.GetMinimapCamera();
            }
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            nm = FindObjectOfType<NetworkManager>();
        }
        public void OnScroll(PointerEventData eventData)
        {
            if (minicam != null)
            {
                var minSize = 5.0f;
                var maxSize = 200.0f;
                var scrollingUnit = ScrollMultiplier;
                var scroll = eventData.scrollDelta.y;
                
                if (scroll > 0 ) //&& minicam.orthographicSize - scrollingUnit > minSize)
                {
                    // Zoom out
                    scrollingUnit = ScrollMultiplier * minicam.orthographicSize;
                }
                else if (scroll < 0 ) // && minicam.orthographicSize + scrollingUnit < maxSize)
                {
                    // Zoom in
                    scrollingUnit = -ScrollMultiplier * (minicam.orthographicSize - minicam.orthographicSize * ScrollMultiplier);
                }
                else
                {
                    scrollingUnit = 0;
                }

                minicam.orthographicSize += scrollingUnit;
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform,
                    eventData.pressPosition, eventData.pressEventCamera, out var cursor))
            {
                var texture = GetComponent<RawImage>().texture;
                var rect = GetComponent<RawImage>().rectTransform.rect;

                var coordX = Mathf.Clamp(0, (((cursor.x - rect.x) * texture.width) / rect.width), texture.width);
                var coordY = Mathf.Clamp(0, (((cursor.y - rect.y) * texture.height) / rect.height), texture.height);

                var calX = coordX / texture.width;
                var calY = coordY / texture.height;


                cursor = new Vector2(calX, calY);

                CastRayToWorld(cursor);
            }
        }

        private void CastRayToWorld(Vector2 vec)
        {
            if (minicam)
            {
                var mapRay = minicam.ScreenPointToRay(new Vector2(vec.x * minicam.pixelWidth,
                    vec.y * minicam.pixelHeight));

                LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast")); // ignore collisions with layerX
                if (Physics.Raycast(mapRay, out var miniMapHit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(mapRay.origin, mapRay.direction * 1000, Color.green, 5);
                    Debug.Log(miniMapHit.collider.name);

                    if (miniMapHit.collider.GetComponent<SpawnArea>() != null)
                    {
                        nm.SpawnABotPlease(miniMapHit.collider.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
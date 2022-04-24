using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minimap
{
    public class MiniMapInputHandler : MonoBehaviour, IPointerClickHandler, IScrollHandler
    {
        public Camera minicam;
        public GameObject myPrefab;

        public void OnScroll(PointerEventData eventData)
        {
            var minSize = 5.0f;
            var maxSize = 200.0f;
            var scrollingUnit = 20.0f;
            var scroll = eventData.scrollDelta.y;
            if (scroll > 0 && minicam.orthographicSize - scrollingUnit > minSize)
            {
                scrollingUnit = -20;
            }
            else if (scroll < 0 && minicam.orthographicSize + scrollingUnit < maxSize)
            {
                scrollingUnit = 20;
            }
            else
            {
                scrollingUnit = 0;
            }
            minicam.orthographicSize += scrollingUnit;
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
            var mapRay = minicam.ScreenPointToRay(new Vector2(vec.x * minicam.pixelWidth,
                vec.y * minicam.pixelHeight));

            if (Physics.Raycast(mapRay, out var miniMapHit, Mathf.Infinity))
            {
                Debug.DrawRay(mapRay.origin, mapRay.direction * 1000, Color.green, 5);

                if (miniMapHit.collider.GetComponent<SpawnArea>() != null)
                {
                    Instantiate(myPrefab, miniMapHit.collider.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
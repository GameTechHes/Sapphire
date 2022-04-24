using UnityEngine;

namespace Minimap
{
    /**
     * TODO: To remove if not used anymore
     */
    public class ScaleMinicam : MonoBehaviour
    {
        public GameObject camUI;

        private float _distanceX;
        private float _distanceZ;
        private Camera _minicam;
        private RectTransform _rectTransform;
        private Quaternion _originalRotation;

        void Start()
        {
            _minicam = GetComponent<Camera>();
            _originalRotation = _minicam.transform.rotation;
            _rectTransform = camUI.GetComponent<RectTransform>();
            
            var minX = 10000f;
            var minZ = 10000f;
            var maxX = -10000f;
            var maxZ = -10000f;
            foreach (var refPoint in GameObject.FindGameObjectsWithTag("ReferencePoint"))
            {
                minX = refPoint.transform.position.x < minX ? refPoint.transform.position.x : minX;
                minZ = refPoint.transform.position.z < minZ ? refPoint.transform.position.z : minZ;
                maxX = refPoint.transform.position.x > maxX ? refPoint.transform.position.x : maxX;
                maxZ = refPoint.transform.position.z > maxZ ? refPoint.transform.position.z : maxZ;
            }

            _distanceZ = maxZ - minZ;
            _distanceX = maxX - minX;

            if (_distanceX < _distanceZ)
            {
                _minicam.orthographicSize = 0.5f * _distanceZ;
            }
            else
            {
                var unitsPerPixel = _distanceX / _rectTransform.rect.width;
                var desiredHalfHeight = 0.5f * unitsPerPixel * _rectTransform.rect.height;
                _minicam.orthographicSize = 10;
            }
        }

        void Update()
        {
            _minicam.transform.rotation = _originalRotation;
        }
    }
}
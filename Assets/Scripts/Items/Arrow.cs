using UnityEngine;

namespace Items
{
    public class Arrow : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

namespace Items
{
    /**
     * TODO: Delete this class if player armature not used
     */
    public class PowerUpBase : MonoBehaviour
    {
        private GameObject _playerArmature;
        
        void Start()
        {
            _playerArmature = GameObject.Find("PlayerArmature");
        }
    }
}

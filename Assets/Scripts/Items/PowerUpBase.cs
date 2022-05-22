using System;
using Fusion;
using Sapphire;
using UnityEngine;

namespace Items
{
    public abstract class PowerUpBase : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.gameObject.GetComponent<Player>().PlayerType == PlayerType.KNIGHT)
            {
                var player = other.GetComponent<Player>();
                ApplyEffects(player);
            }
        }

        protected abstract void ApplyEffects(Player player);
    }
}
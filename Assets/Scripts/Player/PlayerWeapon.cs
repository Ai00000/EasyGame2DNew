using System;
using UnityEngine;

namespace Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                print("击中敌人了");
            }
        }
    }
}
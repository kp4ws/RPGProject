using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        
        public void TakeDamage(float damage)
        {
            //health = Mathf.Clamp(health - damage, 0, damage); This works too, but less efficient in this case :P
            health = Mathf.Max(health - damage, 0);
            Debug.Log(health);
        }
    }
}
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float healthPoints = 100f;
        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            //health = Mathf.Clamp(health - damage, 0, damage); This works too, but less efficient in this case :P
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            
            if(healthPoints == 0) //We can say equal to 0 because our previous line clamps the value
            {
                Die();
            }
        }

        private void Die()
        {
            if(isDead)
                return;
        
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
        }
    }
}
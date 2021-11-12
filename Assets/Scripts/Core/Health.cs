using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
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
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if(healthPoints == 0)
            {
                Die();
            }
            
            //TODO challenge:
            //
            //Save cinematics so they only play once
            //
            //
        }
    }
}
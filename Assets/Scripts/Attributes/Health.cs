using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils; //Used for safe initialization in Start
using UnityEngine.Events;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> //Cannot use this directly so we needed to create a subclass
        {
        }

        private LazyValue<float> healthPoints;
        private bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            //health = Mathf.Clamp(health - damage, 0, damage); This works too, but less efficient in this case :P
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if(healthPoints.value == 0) //We can say equal to 0 because our previous line clamps the value
            {
                onDie.Invoke(); //Look into audio randomizer for soundFX
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();//divide by original health we set
        }

        public float GetFraction ()
        {
            //Use InverseLerp for this?
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if(isDead)
                return;
        
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null)
            {
                return;
            }

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenhealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenhealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if(healthPoints.value == 0)
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
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Attributes;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 6f;

        private NavMeshAgent navMeshAgent;
        private Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); //Prevents collision detection when entity is dead
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            //Velocity has a direction which is why we use Vector3 instead of float
            Vector3 globalVelocity = navMeshAgent.velocity;

            //We need this as local velocity so the animator knows the velocity relative to the player
            Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        [System.Serializable]
        private struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            //Dictionary<string, object> data = new Dictionary<string, object>();
            //data["position"] = new SerializableVector3(transform.position);
            //data["rotation"] = new SerializableVector3(transform.eulerAngles); //vector representation of the angle
            return data;
        }

        public void RestoreState(object state)
        {
            //Dictionary<string, object> data = (Dictionary<string, object>)state;
            //GetComponent<NavMeshAgent>().enabled = false;
            //transform.position = position.ToVector();
            //GetComponent<NavMeshAgent>().enabled = true; 

            //TODO use .Warp() instead?

            //GetComponent<NavMeshAgent>().Warp(((SerializableVector3)data["position"]).ToVector());
            //transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();

            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().Warp(data.position.ToVector());
            transform.eulerAngles = data.rotation.ToVector();
        }
    }
}
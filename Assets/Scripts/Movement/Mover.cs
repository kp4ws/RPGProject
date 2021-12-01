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
        [SerializeField] float maxNavPathLength = 40f; //Max distance you can select for the player to move to

        private NavMeshAgent navMeshAgent;
        private Health health;

        private void Awake()
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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false; //If there's no path to a specific nav mesh (example: on top of a building) don't allow path
            if (GetPathLength(path) > maxNavPathLength) return false;
            
            return true;
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

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
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
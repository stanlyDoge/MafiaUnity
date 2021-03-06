using UnityEngine;

namespace MafiaUnity
{
    // Spawns player to pre-defined point. Make sure you set game path to use it.
    // This is a helper script and should be only used in editor for dev purposes!
    class PlayerSpawner : MonoBehaviour
    {
        bool isInitialized = false;
        void Update() 
        {
            if (isInitialized == true)
                return;

            if (!GameManager.instance.GetInitialized())
                return;

            var go = new GameObject("playerController");
            var tommy = GameManager.instance.modelGenerator.LoadObject("models/Tommy.4ds");
            var player = tommy.AddComponent<ModelAnimationPlayer>();
            tommy.transform.parent = transform;
            tommy.transform.localPosition = Vector3.zero;

            var playerController = gameObject.AddComponent<PlayerController>();
            playerController.playerCamera = GameObject.Find("Main Camera");
            playerController.playerPawn = tommy;

            var rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            var collider = gameObject.AddComponent<CapsuleCollider>();
            collider.center = new Vector3(0, 1f, 0);
            collider.height = 2f;

            isInitialized = true;
        }
    }
}
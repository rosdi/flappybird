using UnityEngine;

namespace Assets.Scripts {
    public class CameraTracksPlayer : MonoBehaviour {
        private Transform player;
        private float cameraOffsetFromPlayer;

        // Use this for initialization
        void Start () {

            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");

            if (playerGameObject == null) {
                Debug.LogError("Can't find player game object.");
                return;
            }

            player = playerGameObject.transform;

            cameraOffsetFromPlayer = transform.position.x - player.position.x;
        }
	
        // Update is called once per frame
        void Update () {
            if (player == null) {   //player died probably
                return;
            }

            Vector3 cameraPosition = transform.position;
            cameraPosition.x = player.position.x + cameraOffsetFromPlayer;
            transform.position = cameraPosition;
        }
    }
}

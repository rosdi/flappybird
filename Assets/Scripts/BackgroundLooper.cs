using UnityEngine;

namespace Assets.Scripts {
    public class BackgroundLooper : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D obj) {
            //string objectName = obj.name;

            float objWidth = ((BoxCollider2D) obj).size.x;
            Vector3 objPos = obj.transform.position;
            objPos.x += (objWidth*GameManager.NUM_OF_BACKGROUND_PANELS);

            if (obj.tag.Equals("Pipe")) {   //randomize pipe up and down position
                objPos.y = Random.Range(GameManager.MIN_Y, GameManager.MAX_Y);
                obj.transform.FindChild("ScoreArea").GetComponent<BoxCollider2D>().enabled = true;  //reenable score detection
            }

            obj.transform.position = objPos;

        }
    }
}

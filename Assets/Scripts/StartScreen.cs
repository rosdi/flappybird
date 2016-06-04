using UnityEngine;

namespace Assets.Scripts {

	public class StartScreen : MonoBehaviour {
		//private GameObject flappyTextGo;
		//private GameObject birdTextGo;
		private const float FLAPPY_TEXT_WIDTH = 0.56f;
		private const float BIRD_TEXT_WIDTH = 0.38f;
		private const float WORD_GAP = 0.01f;

		// Use this for initialization
		void Start () {
			//GameObject[] childObjects = GetComponentsInChildren<GameObject>();
			//flappyTextGo = GameObject.Find("FlappyText");
			//birdTextGo = GameObject.Find("BirdText");

			//center title to the screen
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height * 0.75f, Camera.main.nearClipPlane));

			transform.position = pos;

			//pos.x -= (FLAPPY_TEXT_WIDTH / 2f) - WORD_GAP;
			//flappyTextGo.transform.position = pos;

			//pos.x = (pos.x + (FLAPPY_TEXT_WIDTH / 2f) + (BIRD_TEXT_WIDTH / 2f)) + WORD_GAP;
			//birdTextGo.transform.position = pos;
		}

	
		// Update is called once per frame
		void Update()
		{
			//this is the only way to detect mouse click on child objects
			//http://gamedev.stackexchange.com/a/82947
			if (Input.GetMouseButtonUp(0))	
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
				if (hit && "StartButton".Equals(hit.collider.gameObject.name))
				{
					Application.LoadLevel("ReadyScene");
				}
			}
		}
	}
}

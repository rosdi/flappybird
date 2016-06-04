using UnityEngine;

namespace Assets.Scripts {
	public class BackgroundMover : MonoBehaviour {

		public float Speed = 2f;

		// Use this for initialization
		void Start () {
	
		}
	
		// Update is called once per frame
		void FixedUpdate () {
			Vector3 pos = transform.position;
			pos.x += Speed*Time.deltaTime;
			transform.position = pos;
		}
	}
}

using UnityEngine;

namespace Assets.Scripts {

	public class DummyPlayer : MonoBehaviour {

		public float FlapStrength = 0.6755f;    //this is just sweet in Unity 5.1 to make the bird float in the middle
		public int FlapFrequency = 10;

		private Animator _animator;
		private bool _flapNow = true;
		private const float FLIP_DOWN_DELAY = 2.7f;

		// Use this for initialization
		void Start () {
			_animator = GetComponent<Animator>();            
		}
	
		// Update is called once per frame
		// Do graphics and input update here
		void Update () {
			if (FlapFrequency-- * Time.deltaTime < 0) {
				FlapFrequency = 30;
				_flapNow = true;
			}
		}

		// Do physics engine updates here
		void FixedUpdate()
		{
			if (_flapNow) {
				FlapUp();
				_flapNow = false;
			}


			float angle;
			if (GetComponent<Rigidbody2D>().velocity.y + FLIP_DOWN_DELAY > 0)
			{
				angle = Mathf.Lerp(GetComponent<Rigidbody2D>().transform.rotation.z, 90,
								   (GetComponent<Rigidbody2D>().velocity.y + FLIP_DOWN_DELAY) / 60f);
			}
			else
			{
				angle = Mathf.Lerp(GetComponent<Rigidbody2D>().transform.rotation.z, -90,
								   -(GetComponent<Rigidbody2D>().velocity.y + FLIP_DOWN_DELAY - 0.7f) / 2f);
			}

			GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, angle);

		}

		private void FlapUp() {
			_animator.SetTrigger("DoFlap");

			//reset velocity before flapping up to prevent it flapping too high
			Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
			velocity.y = 0;
			GetComponent<Rigidbody2D>().velocity = velocity;

			GetComponent<Rigidbody2D>().AddForce(Vector2.up*FlapStrength); //flap up
		}

	}
}

using UnityEngine;

namespace Assets.Scripts {

	public class Player : MonoBehaviour {

		public float FlapSpeed = 80f;
		public float ForwardSpeed = 0f;
		public bool GodMode = false;

		public static bool Dead { get; set; }
		public static bool HitGround { get; set; }

		private Animator _animator;
		private bool IsFlapping { get; set; }

		private const float FLIP_DOWN_DELAY = 2.7f;

		// Use this for initialization
		void Start () {
			_animator = GetComponent<Animator>();
			
			IsFlapping = true;
			Dead = false;
			HitGround = false;
		}
	
		// Update is called once per frame
		// Do graphics and input update here
		void Update () {
			if (!Dead && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
				IsFlapping = true;
			}

			if (Dead && !HitGround) {
				transform.Rotate(0f, 0f, -((transform.rotation.z + 90) * Time.deltaTime));
			}
		}

		// Do physics engine updates here
		void FixedUpdate()
		{
			if (Dead)
			{                
				return;
			}

			FlyForward();

			if (IsFlapping)
			{
				FlapUp();
			}


			float angle;
			if (GetComponent<Rigidbody2D>().velocity.y + FLIP_DOWN_DELAY > 0)
			{
				angle = Mathf.Lerp(GetComponent<Rigidbody2D>().transform.rotation.z, 90,
								   (GetComponent<Rigidbody2D>().velocity.y + FLIP_DOWN_DELAY) / 15f);
			}
			else
			{
				angle = Mathf.Lerp(GetComponent<Rigidbody2D>().transform.rotation.z, -90,
								   -(GetComponent<Rigidbody2D>().velocity.y + FLIP_DOWN_DELAY - 0.7f) / 2f);
				//-0.7f just because the turning feels right

			}

			GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, angle);

		}

		private void FlapUp() {
			_animator.SetTrigger("DoFlap");

			//reset velocity before flapping up to prevent it flapping too high
			Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
			velocity.y = 0;
			GetComponent<Rigidbody2D>().velocity = velocity;

			GetComponent<Rigidbody2D>().AddForce(Vector2.up*FlapSpeed); //flap up
			IsFlapping = false;
		}

		private void FlyForward() {
			Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
			//reset velocity before flapping up to prevent it flapping too fast                
			velocity.x = 0;
			GetComponent<Rigidbody2D>().velocity = velocity;
			GetComponent<Rigidbody2D>().AddForce(Vector2.right*ForwardSpeed); //keep flying forward
		}

		void OnTriggerEnter2D(Collider2D colliderObj) {			
			
			if (colliderObj.tag.Equals("Ground")) {   //stop bird from falling off the screen
				StabBirdToTheGround((BoxCollider2D) colliderObj);
			}

			if (colliderObj.tag.Equals("Ceiling"))
			{
				StopBirdFromFlyingOverCeiling((BoxCollider2D) colliderObj);
				return;
			}

			if (Dead) {
				return;
			}

			if (colliderObj.tag.Equals("ScoreArea")) {
				colliderObj.enabled = false;    //prevent multiple scoring
				GameManager.AddScore();
				return;
			}

			if (!GodMode) {
				PlayerKilled();
			}
		}

		private void StopBirdFromFlyingOverCeiling(BoxCollider2D ceiling)
		{
			transform.position = new Vector3(
				transform.position.x,
				ceiling.transform.position.y - 0.47f,	//reposition the bird just enough so that it does not go over the ceiling
				transform.position.z);
		}

		private void PlayerKilled() {

			Debug.Log("Player killed.");

			GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			gameManager.PlayerDie();
			_animator.SetTrigger("Die");
			
			if (!HitGround) {
				GetComponent<Rigidbody2D>().gravityScale = 0.45f; //make bird fall slowly
			}          

			Debug.Log("Player is dead, on ground: " + Dead + ", " + HitGround);
		}

		private void StabBirdToTheGround(BoxCollider2D ground) {
			
			GodMode = false;    //bird only invincible to the pipes.
			HitGround = true;

			//flip bird face downward
			GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, -90);

			//stop bird once hit ground
			GetComponent<Rigidbody2D>().gravityScale = 0f;
			Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
			velocity.y = 0f;
			velocity.x = 0f;
			GetComponent<Rigidbody2D>().velocity = velocity;

			//stab bird's beak to the ground
			transform.position = new Vector3(
				transform.position.x,
				ground.transform.position.y + ((ground).size.y * 0.61f),	//position the bird just enough so the beak is stabbed to the ground
				transform.position.z);
		}
	}
}

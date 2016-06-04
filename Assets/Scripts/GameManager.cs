using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
	public class GameManager : MonoBehaviour {

		public const int NUM_OF_BACKGROUND_PANELS = 10;
		public const float MIN_Y = 0.80f;
		public const float MAX_Y = 1.37f;

		private static int _score;
		private static int _highestScore;
		private static ScoreManager _scoreManager;

		public static bool GamePaused { get; set; }


		void Start() {
			Cursor.visible = false;
			GamePaused = false;

			_score = 0;
			_highestScore = PlayerPrefs.GetInt("highestScore", 0);
			
			_scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
			_scoreManager.UpdateScore(_score);

			GeneratePipes();

		}

		void OnDestroy() {
			PlayerPrefs.SetInt("highestScore", _highestScore);
			Debug.Log("Game Manager destroyed, highest newScore was: " + _highestScore);
		}

		public static void AddScore() {
			_score++;
			if (_score > _highestScore) {
				_highestScore = _score;
			}

			_scoreManager.UpdateScore(_score);
		}

		public void PlayerDie() {

			GUITexture hurtFlash = GameObject.Find("HurtFlash").GetComponent<GUITexture>();
			hurtFlash.pixelInset = new Rect(0, 0, Screen.width, Screen.height);    //maximize texture to fill screen

			Player.Dead = true;

			StartCoroutine(FlashWhenHurt(hurtFlash));
		}

		void GeneratePipes() {
			GameObject pipe = GameObject.FindGameObjectWithTag("Pipe");
			Vector3 pipePosition = pipe.transform.position;
			float pipeWidth = ((BoxCollider2D) pipe.GetComponent<Collider2D>()).size.x;

			//generate 9 pipes + 1 existing pipe so total 10 pipes
			for (int i = 1; i < 10; i++) {
				pipePosition.x += pipeWidth;
				pipePosition.y = Random.Range(MIN_Y, MAX_Y);    //make non uniform pipe positions
				Instantiate(pipe, pipePosition, Quaternion.identity);
			}

			//randomize the first pipe too
			pipe.transform.position = new Vector3(pipe.transform.position.x, Random.Range(MIN_Y, MAX_Y), pipe.transform.position.z); 
		}

	   public IEnumerator FadeScreen(float start, float end, float length, GUITexture textureGameObject) {
			//if (!textureGameObject.flashTexture.color.a.Equals(start)) {
			//    return;
			//}

			for (float i = 0.0f; i < 1.0; i += Time.deltaTime*(1/length)) {
				Color color = textureGameObject.color;
				
				color.a = Mathf.Lerp(start, end, i);
				textureGameObject.color = color;

				yield return null;

				color.a = end;
				textureGameObject.color = color; // ensure the fade is completely finished (because lerp doesn't always end on an exact value)
			}
		}

		public IEnumerator FlashWhenHurt(GUITexture flashTexture){
			StartCoroutine(FadeScreen(0f, 0.7f, 0.03f, flashTexture));
			yield return new WaitForSeconds(0.03f);
			StartCoroutine(FadeScreen(0.7f, 0f, 0.03f, flashTexture));
		}
	}
}

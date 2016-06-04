using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
	public class ScoreManager : MonoBehaviour {

		private const float SPRITE_WIDTH = 0.07f;
		private const string SPRITE_TAG = "SCORE_SPRITE";   //http://answers.unity3d.com/questions/33597/is-it-possible-to-create-a-tag-programmatically.html

		void Start() {

		}
	
		public void UpdateScore (int score) {

			//remove previous scores
			GameObject[] oldScoreDigits = GameObject.FindGameObjectsWithTag(SPRITE_TAG);
			foreach (var oldScoreDigit in oldScoreDigits) {
				Destroy(oldScoreDigit);
			}

			string scoreStr = score.ToString();

			for (int i = 0; i < scoreStr.Length; i++)
			{
				GameObject digit = (GameObject)Instantiate(GameObject.Find(scoreStr.Substring(i,1)));

				digit.tag = SPRITE_TAG;   //tag this object for easy removal
				digit.transform.parent = this.transform;
				digit.transform.localPosition = new Vector3(i * 0.08f, 0, digit.transform.localPosition.z);    
			}
			
			//center score to top screen
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2f, Screen.height * 0.96f, Camera.main.nearClipPlane));
			pos.x -= scoreStr.Length*SPRITE_WIDTH/2f; 
			transform.position = pos;
		}
	}
}

using System.Collections;
using UnityEngine;

namespace Scrapper {
	public class ColorBlink : MonoBehaviour {

		public Color RegularColor = Color.white;
		public Color TargetColor = Color.red;
		public float Duration = 0.2f;
		public SpriteRenderer SpriteRenderer;
		private bool _blinking;
		
		public void Blink() {
			
			if(_blinking) return;
			
			StartCoroutine(DoBlink());
		}

		private IEnumerator DoBlink() {

			_blinking = true;

			SpriteRenderer.color = RegularColor;
			
			float time = 0.0f;
			while (time <= Duration / 2.0f) {
				
				float blend = time / (Duration / 2.0f);
				SpriteRenderer.color = Color.Lerp(RegularColor, TargetColor, blend);
				
				time += Time.deltaTime;
				yield return 0;
			}

			while (time <= Duration) {

				float blend = (time - Duration / 2.0f) / (Duration / 2.0f);
				SpriteRenderer.color = Color.Lerp(TargetColor, RegularColor, blend);

				time += Time.deltaTime;
				yield return 0;
			}

			SpriteRenderer.color = RegularColor;
			
			_blinking = false;
		}
	}
}
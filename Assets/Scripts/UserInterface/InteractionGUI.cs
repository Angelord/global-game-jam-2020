using UnityEngine;

namespace UserInterface {
	public class InteractionGUI : MonoBehaviour {

		public Senses Player;

		private void OnDrawGizmos() {
			if(Player == null) return;

			ScrapBehaviour repair = Player.GetRepairTarget();
			if (repair != null) {
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(repair.transform.position, 0.2f);
			}

			ScrapBehaviour salvage = Player.GetSalvageTarget();
			if (salvage != null) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(salvage.transform.position, 0.18f);
			}
		}
	}
}
using UnityEngine;

public abstract class ScrapBehaviour : MonoBehaviour {

	public virtual bool Usable { get { return false; } }

	public virtual bool Salvageable { get { return false; } }

	public virtual bool Repairable { get { return false; } }

	public virtual float RepairCost { get { return 0.0f; } }

	public virtual float Salvage(float amount) { return 0.0f; }

	public virtual void Use() { }

	public virtual void Repair() { }
}
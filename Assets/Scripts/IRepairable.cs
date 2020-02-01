public interface IRepairable {

	bool NeedsRepair { get; }

	float RepairCost { get; }

	void Repair();
}
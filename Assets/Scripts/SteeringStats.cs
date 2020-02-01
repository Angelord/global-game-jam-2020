using UnityEngine;

[CreateAssetMenu(fileName = "SteeringStats", menuName = "Scrapper/Steering Stats", order = 1)]
public class SteeringStats : ScriptableObject {

	[Range(0.0f, 10.0f)] public float Cohesion;
	[Range(0.0f, 10.0f)] public float Separation;
	[Range(0.0f, 10.0f)] public float Follow;
	public float FollowDistance;
}
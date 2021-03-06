﻿using UnityEngine;

[CreateAssetMenu(fileName = "SteeringStats", menuName = "Scrapper/Steering Stats", order = 1)]
public class SteeringStats : ScriptableObject {

	[Range(0.0f, 10.0f)] public float ObstacleAvoidance = 1.2f;
	[Range(0.0f, 10.0f)] public float Cohesion;
	[Range(0.0f, 10.0f)] public float Separation;
	[Range(0.0f, 10.0f)] public float Follow;
	[Range(0.0f, 10.0f)] public float Attack;
	[Range(0.0f, 10.0f)] public float RecallBonus = 0.5f;
	public float ObstacleAvoidanceRange = 2.0f;
	public LayerMask ObstacleLayers;
	public float MinFillowDistance = 0.5f;
	public float FollowDecceleration;
	public float AttackDecceleration = 0.2f;
}
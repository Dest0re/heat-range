using Godot;
using System;

namespace HeatRange
{
	public sealed class Ship : RigidBody2D
	{
		public override void _IntegrateForces(Physics2DDirectBodyState state)
		{
			var dir = Input.GetAxis("left", "right");
			var res = state.AngularVelocity + dir * Def.Player.RotationSpeed / state.Step;
			res = Math.Max(Math.Min(res, Def.Player.MaxRotationSpeed), -Def.Player.MaxRotationSpeed);
			state.AngularVelocity = res;

			state.LinearVelocity += new Vector2(Input.GetActionStrength("move") * Def.Player.MoveSpeed / state.Step, 0f).Rotated(Rotation);
			state.LinearVelocity = state.LinearVelocity.Clamped(Def.Player.MaxMoveSpeed);
		}
	}
}

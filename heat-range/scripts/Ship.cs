using Godot;
using System;
using System.Linq;

namespace HeatRange
{
	public sealed class Ship : RigidBody2D
	{
		public RigidBody2D? TargetedBody { get; set; } = null;

		public override void _IntegrateForces(Physics2DDirectBodyState state)
		{
			var dir = Input.GetAxis("left", "right");
			var res = state.AngularVelocity + dir * Def.Player.RotationSpeed / state.Step;
			res = Math.Max(Math.Min(res, Def.Player.MaxRotationSpeed), -Def.Player.MaxRotationSpeed);
			state.AngularVelocity = res;

			state.LinearVelocity += new Vector2(Input.GetActionStrength("move") * Def.Player.MoveSpeed / state.Step, 0f).Rotated(Rotation);
			state.LinearVelocity = state.LinearVelocity.Clamped(Def.Player.MaxMoveSpeed);
		}

		public override void _PhysicsProcess(float delta)
		{
			TargetedBody = AsteroidSystem.Asteroids
				.Select(x => (x, x.Position.DistanceSquaredTo(GetGlobalMousePosition())))
				.Where(x => x.Item2 < Math.Pow(Def.GameProcess.AimDistance, 2))
				.OrderBy(x => x.Item2)
				.Select(x => x.Item1)
				.FirstOrDefault();
			Update();
		}

		// REMOVE!
		/*public override void _Ready()
		{
			var t = Def.FP.Asteroid.Prefab<RigidBody2D>();
			t.Position = new(300, 300);
			t.LinearVelocity = new(3, 0);
			t.AngularVelocity = 0.1f;
			AsteroidSystem.Asteroids.Add(t);
			GetTree().Root.CallDeferred("add_child", t);

			t = Def.FP.Asteroid.Prefab<RigidBody2D>();
			t.Position = new(400, 300);
			t.LinearVelocity = new(3, 0);
			t.AngularVelocity = 0.1f;
			AsteroidSystem.Asteroids.Add(t);
			GetTree().Root.CallDeferred("add_child", t);
		}*/

		public override void _Draw()
		{
			if (TargetedBody != null)
			{
				DrawLine(Transform.AffineInverse() * GetGlobalMousePosition(), Transform.AffineInverse() * TargetedBody.Position, new(1, 1, 1, .3f), 2, true);
				DrawCircle(Transform.AffineInverse() * TargetedBody.Position, Def.GameProcess.AimDistance, new(1, 1, 1, .1f));
				DrawCircle(Transform.AffineInverse() * Position, Def.Player.ShootDistance, new(1, 1, 1, .05f));
			}
		}
	}
}

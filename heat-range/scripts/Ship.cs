using Godot;
using System;
using System.Linq;

namespace HeatRange
{
	public sealed class Ship : RigidBody2D
	{
		public RigidBody2D TargetedBody { get; set; } = null;

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
		public override void _Ready()
		{
			Input.SetMouseMode(Input.MouseMode.Hidden);

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
		}

		public override void _Draw()
		{
			if (TargetedBody != null)
			{
				for (int i = 0; i < 3; i++)
				{
					float a = (i / 3f + DateTime.Now.Millisecond / 1000f) * Mathf.Pi * 2f;
					float b = ((i + 1) % 3 / 3f + DateTime.Now.Millisecond / 1000f) * Mathf.Pi * 2f;
					Vector2 v1 = new(Mathf.Cos(a) * 40f, Mathf.Sin(a) * 40f);
					Vector2 v2 = new(Mathf.Cos(b) * 40f, Mathf.Sin(b) * 40f);
					DrawLine(Transform.AffineInverse() * (TargetedBody.Position + v2), Transform.AffineInverse() * (TargetedBody.Position + v1), new(1, 1, 1, .3f), 2, true);
				}
				for (int i = 0; i < 3; i++)
				{
					float a = (i / 3f + DateTime.Now.Millisecond / 500f) * Mathf.Pi * 2f;
					float b = ((i + 1) % 3 / 3f + DateTime.Now.Millisecond / 500f) * Mathf.Pi * 2f;
					Vector2 v1 = new(Mathf.Cos(a) * 15f, Mathf.Sin(a) * 15f);
					Vector2 v2 = new(Mathf.Cos(b) * 15f, Mathf.Sin(b) * 15f);
					DrawLine(Transform.AffineInverse() * (GetGlobalMousePosition() + v2), Transform.AffineInverse() * (GetGlobalMousePosition() + v1), new(1, 1, 1, .6f), 2, true);
				}
				DrawLine(Transform.AffineInverse() * GetGlobalMousePosition(), Transform.AffineInverse() * TargetedBody.Position, new(1, 1, 1, .05f), 2, true);
				DrawCircle(Transform.AffineInverse() * TargetedBody.Position, Def.GameProcess.AimDistance, new(1, 1, 1, .021f));
				DrawCircle(Transform.AffineInverse() * Position, Def.Player.ShootDistance, new(1, 1, 1, .02f));
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					float a = (i / 3f + DateTime.Now.Millisecond / 1000f) * Mathf.Pi * 2f;
					float b = ((i + 1) % 3 / 3f + DateTime.Now.Millisecond / 1000f) * Mathf.Pi * 2f;
					Vector2 v1 = new(Mathf.Cos(a) * 10f, Mathf.Sin(a) * 10f);
					Vector2 v2 = new(Mathf.Cos(b) * 10f, Mathf.Sin(b) * 10f);
					DrawLine(Transform.AffineInverse() * (GetGlobalMousePosition() + v2), Transform.AffineInverse() * (GetGlobalMousePosition() + v1), new(1, 1, 1, .3f), 2, true);
				}
			}
		}
	}
}

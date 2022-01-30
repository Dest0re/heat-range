using Godot;
using System;

public class Reactor : StaticBody2D
{
	private int _minimalScreenBorder = Math.Min(
		(int)ProjectSettings.GetSetting("display/window/size/width"), 
		(int)ProjectSettings.GetSetting("display/window/size/height"));

	[Export(PropertyHint.Range, "0,1")]
	public float Fullness
	{
		get => _Fulliness;
		
		set
		{
			_Fulliness = value;

			OuterRingSize = _Fulliness;

			if (_Fulliness > CriticalMass)
			{
				var cropSize = _Fulliness - CriticalMass;

				InnerRingSize = (1 / (1 - CriticalMass)) * cropSize;
			}
			else
			{
				InnerRingSize = 0;
			}
		}
	}
	
	[Export(PropertyHint.Range, "0,0.2")]
	public float StartInnerRingSize { get; set; }
	
	[Export(PropertyHint.Range, "0,0.5")]
	public float StartOuterRingSize { get; set; }
	
	[Export(PropertyHint.Range, "0,1")]
	public float CriticalMass { get; set; }

	private float _InnerRingSize;
	private float _OuterRingSize;
	private float _Fulliness;
	
	[Export()]
	private float _RealInnerRingSize;
	[Export()]
	private float _RealOuterRingSize;

	public float InnerRingSize
	{
		get => _InnerRingSize;

		set
		{
			_InnerRingSize = value;
		}
	}

	public float OuterRingSize
	{
		get => _OuterRingSize;

		set
		{
			_OuterRingSize = value;
		}
	}

	public override void _Ready()
	{
		_RealInnerRingSize = StartInnerRingSize;
		_RealOuterRingSize = StartOuterRingSize;
	}

	public override void _PhysicsProcess(float delta)
	{
		_RealInnerRingSize += Mathf.Pow( _InnerRingSize - _RealInnerRingSize, 2.0f) * Math.Sign(_InnerRingSize - _RealInnerRingSize);
		_RealOuterRingSize += Mathf.Pow(_OuterRingSize - _RealOuterRingSize, 2.0f) * Math.Sign(_OuterRingSize - _RealOuterRingSize);

		Update();
	}

	public override void _Draw()
	{
		var radius = _minimalScreenBorder * _RealOuterRingSize / 2;
		
		DrawCircle(new Vector2(0, 0), radius, Colors.Blue);
		DrawCircle(new Vector2(0, 0), radius-2, Color.Color8(0, 0, 0, 255));
		DrawCircle(new Vector2(0, 0), _minimalScreenBorder * _RealInnerRingSize, Colors.Red);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Godot;
using System;

public class Reactor : StaticBody2D
{
	private int _minimalScreenBorder = Math.Min(
		(int)ProjectSettings.GetSetting("display/window/size/width"), 
		(int)ProjectSettings.GetSetting("display/window/size/height"));

	private int _maximalScreenBorder = Math.Max(
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


	[Export] 
	private NodePath _InnerRingPath;

	[Export] 
	private NodePath _OuterRingPath;

	private ShaderMaterial _InnerRingShader;
	private ShaderMaterial _OuterRingShader;

	private float _InnerRingSize;
	private float _OuterRingSize;
	private float _Fulliness;
	
	private float _RealInnerRingSize;
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

		_InnerRingShader = GetNode<CanvasItem>(_InnerRingPath).Material as ShaderMaterial;
		_OuterRingShader = GetNode<CanvasItem>(_OuterRingPath).Material as ShaderMaterial;
	}

	public override void _PhysicsProcess(float delta)
	{
		_RealInnerRingSize = Mathf.Lerp(_InnerRingSize, _RealInnerRingSize, .95f);
		_RealOuterRingSize = Mathf.Lerp(_OuterRingSize, _RealOuterRingSize, .95f);

		_InnerRingShader.SetShaderParam("radius", _minimalScreenBorder * _RealInnerRingSize / _maximalScreenBorder);
		_OuterRingShader.SetShaderParam("radius", _minimalScreenBorder * _RealOuterRingSize / 2 / _maximalScreenBorder);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
using UnityEngine;

//Just gives the animals a little movement as an example
public class EntityMovementSystem : ComponentSystem 
{
	[Inject] private Data data;
	
	protected override void OnUpdate()
	{
		var dt = Time.deltaTime;

		for (var index = 0; index < data.Length; ++index)
		{
			var position = data.Position[index].Value;
			var heading = data.Heading[index].Value;

			float wobbleX = Mathf.PerlinNoise(position.x, position.y) - 0.5f;
			float wobbleY = Mathf.PerlinNoise(position.y, position.x) - 0.5f;
			position += dt * new float2(wobbleX, wobbleY);
			
			data.Position[index] = new Position2D {Value = position};
			data.Heading[index] = new Heading2D {Value = heading};
		}
	}

	public struct Data
	{
		public int Length;
		public ComponentDataArray<Position2D> Position;
		public ComponentDataArray<Heading2D> Heading;
	}
}

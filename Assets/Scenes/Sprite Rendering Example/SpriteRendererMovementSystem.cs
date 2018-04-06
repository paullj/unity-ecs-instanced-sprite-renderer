using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
using UnityEngine;
using Unity.Jobs;

namespace toinfiniityandbeyond.Examples
{
	//Just gives the animals a little movement as an example
	public class EntityMovementSystem : JobComponentSystem
	{
		[ComputeJobOptimization]
		struct PositionJob : IJobProcessComponentData<Position2D>
		{
			public float dt;

			public void Execute(ref Position2D position)
			{
				float wobbleX = Mathf.PerlinNoise(position.Value.x, position.Value.y) - 0.5f;
				float wobbleY = Mathf.PerlinNoise(position.Value.y, position.Value.x) - 0.5f;
				position.Value += dt * new float2(wobbleX, wobbleY);
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var job = new PositionJob() {dt = Time.deltaTime};
			return job.Schedule(this, 64, inputDeps);
		}
	}
}
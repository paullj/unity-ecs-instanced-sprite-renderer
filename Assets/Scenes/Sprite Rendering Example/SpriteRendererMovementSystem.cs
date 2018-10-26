using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace toinfiniityandbeyond.Examples
{
	//Just gives the animals a little movement as an example
	public class EntityMovementSystem : JobComponentSystem
	{
		[BurstCompile]
		struct PositionJob : IJobProcessComponentData<Position>
		{
			public float dt;

			public void Execute(ref Position position)
			{
				float wobbleX = Mathf.PerlinNoise(position.Value.x, position.Value.z) - 0.5f;
				float wobbleY = Mathf.PerlinNoise(position.Value.z, position.Value.x) - 0.5f;
				position.Value += dt * new float3(wobbleX, 0, wobbleY);
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var job = new PositionJob() {dt = Time.deltaTime};
			return job.Schedule(this, inputDeps);
		}
	}
}
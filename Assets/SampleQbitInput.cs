using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class SampleQbitInput : ComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<NetworkIdComponent>();
        RequireSingletonForUpdate<EnableQueuebitGhostReceiveSystemComponent>();
    }

    protected override void OnUpdate()
    {
        var localInput = GetSingleton<CommandTargetComponent>().targetEntity;
        if (localInput == Entity.Null)
        {
            var localPlayerId = GetSingleton<NetworkIdComponent>().Value;
            Entities.WithNone<QbitInput>().ForEach((Entity ent, ref QbitDataComponent qbit) =>
            {
                if (qbit.PlayerId == localPlayerId)
                {
                    PostUpdateCommands.AddBuffer<QbitInput>(ent);
                    PostUpdateCommands.SetComponent(GetSingletonEntity<CommandTargetComponent>(), new CommandTargetComponent {targetEntity = ent});
                }
            });
            return;
        }
        var input = default(QbitInput);
        input.tick = World.GetExistingSystem<ClientSimulationSystemGroup>().ServerTick;
        if (Input.GetKey("a"))
            input.horizontal -= 1;
        if (Input.GetKey("d"))
            input.horizontal += 1;
        if (Input.GetKey("s"))
            input.vertical -= 1;
        if (Input.GetKey("w"))
            input.vertical += 1;
        if (Input.GetKey("space"))
            input.spacebarSpecial = 1;
        var inputBuffer = EntityManager.GetBuffer<QbitInput>(localInput);
        inputBuffer.AddCommandData(input);
    }
}
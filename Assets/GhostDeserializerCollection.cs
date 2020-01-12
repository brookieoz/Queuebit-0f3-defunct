using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;

public struct QueuebitGhostDeserializerCollection : IGhostDeserializerCollection
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public string[] CreateSerializerNameList()
    {
        var arr = new string[]
        {
            "QbitGhostSerializer",
        };
        return arr;
    }

    public int Length => 1;
#endif
    public void Initialize(World world)
    {
        var curQbitGhostSpawnSystem = world.GetOrCreateSystem<QbitGhostSpawnSystem>();
        m_QbitSnapshotDataNewGhostIds = curQbitGhostSpawnSystem.NewGhostIds;
        m_QbitSnapshotDataNewGhosts = curQbitGhostSpawnSystem.NewGhosts;
        curQbitGhostSpawnSystem.GhostType = 0;
    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_QbitSnapshotDataFromEntity = system.GetBufferFromEntity<QbitSnapshotData>();
    }
    public bool Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                return GhostReceiveSystem<QueuebitGhostDeserializerCollection>.InvokeDeserialize(m_QbitSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, reader, ref ctx, compressionModel);
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    public void Spawn(int serializer, int ghostId, uint snapshot, DataStreamReader reader,
        ref DataStreamReader.Context ctx, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                m_QbitSnapshotDataNewGhostIds.Add(ghostId);
                m_QbitSnapshotDataNewGhosts.Add(GhostReceiveSystem<QueuebitGhostDeserializerCollection>.InvokeSpawn<QbitSnapshotData>(snapshot, reader, ref ctx, compressionModel));
                break;
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }

    private BufferFromEntity<QbitSnapshotData> m_QbitSnapshotDataFromEntity;
    private NativeList<int> m_QbitSnapshotDataNewGhostIds;
    private NativeList<QbitSnapshotData> m_QbitSnapshotDataNewGhosts;
}
public struct EnableQueuebitGhostReceiveSystemComponent : IComponentData
{}
public class QueuebitGhostReceiveSystem : GhostReceiveSystem<QueuebitGhostDeserializerCollection>
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<EnableQueuebitGhostReceiveSystemComponent>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.NetCode;
using Unity.Entities;
using Unity.Networking.Transport;

public struct QbitInput : ICommandData<QbitInput>
{
    public uint Tick => tick;
    public uint tick;
    public int horizontal;
    public int vertical;
    public int spacebarSpecial;

    public void Deserialize(uint tick, DataStreamReader reader, ref DataStreamReader.Context ctx)
    {
        this.tick = tick;
        horizontal = reader.ReadInt(ref ctx);
        vertical = reader.ReadInt(ref ctx);
        spacebarSpecial = reader.ReadInt(ref ctx);
    }

    public void Serialize(DataStreamWriter writer)
    {
        writer.Write(horizontal);
        writer.Write(vertical);
        writer.Write(spacebarSpecial);
    }

    public void Deserialize(uint tick, DataStreamReader reader, ref DataStreamReader.Context ctx, QbitInput baseline,
        NetworkCompressionModel compressionModel)
    {
        Deserialize(tick, reader, ref ctx);
    }

    public void Serialize(DataStreamWriter writer, QbitInput baseline, NetworkCompressionModel compressionModel)
    {
        Serialize(writer);
    }
}

public class NetQueuebitSendCommandSystem : CommandSendSystem<QbitInput>
{
}
public class NetQueuebitReceiveCommandSystem : CommandReceiveSystem<QbitInput>
{
}


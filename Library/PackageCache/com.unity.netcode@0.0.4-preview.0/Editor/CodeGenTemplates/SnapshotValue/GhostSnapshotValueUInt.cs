public struct GhostSnapshotData
{
    #region __GHOST_FIELD__
    private uint __GHOST_FIELD_NAME__;
    #endregion

    #region __GHOST_FIELD_GET_SET__
    public __GHOST_FIELD_TYPE_NAME__ Get__GHOST_FIELD_NAME__(GhostDeserializerState deserializerState)
    {
        return (__GHOST_FIELD_TYPE_NAME__)__GHOST_FIELD_NAME__;
    }
    public __GHOST_FIELD_TYPE_NAME__ Get__GHOST_FIELD_NAME__()
    {
        return (__GHOST_FIELD_TYPE_NAME__)__GHOST_FIELD_NAME__;
    }
    public void Set__GHOST_FIELD_NAME__(__GHOST_FIELD_TYPE_NAME__ val, GhostSerializerState serializerState)
    {
        __GHOST_FIELD_NAME__ = (uint)val;
    }
    public void Set__GHOST_FIELD_NAME__(__GHOST_FIELD_TYPE_NAME__ val)
    {
        __GHOST_FIELD_NAME__ = (uint)val;
    }
    #endregion

    public void PredictDelta(uint tick, ref GhostSnapshotData baseline1, ref GhostSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        #region __GHOST_PREDICT__
        __GHOST_FIELD_NAME__ = (uint)predictor.PredictInt((int)__GHOST_FIELD_NAME__, (int)baseline1.__GHOST_FIELD_NAME__, (int)baseline2.__GHOST_FIELD_NAME__);
        #endregion
    }

    public void Serialize(int networkId, ref GhostSnapshotData baseline, DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        #region __GHOST_CALCULATE_CHANGE_MASK_ZERO__
        changeMask__GHOST_MASK_BATCH__ = (__GHOST_FIELD_NAME__ != baseline.__GHOST_FIELD_NAME__) ? 1u : 0;
        #endregion
        #region __GHOST_CALCULATE_CHANGE_MASK__
        changeMask__GHOST_MASK_BATCH__ |= (__GHOST_FIELD_NAME__ != baseline.__GHOST_FIELD_NAME__) ? (1u<<__GHOST_MASK_INDEX__) : 0;
        #endregion
        #region __GHOST_WRITE__
        if ((changeMask__GHOST_MASK_BATCH__ & (1 << __GHOST_MASK_INDEX__)) != 0)
            writer.WritePackedUIntDelta(__GHOST_FIELD_NAME__, baseline.__GHOST_FIELD_NAME__, compressionModel);
        #endregion
    }

    public void Deserialize(uint tick, ref GhostSnapshotData baseline, DataStreamReader reader, ref DataStreamReader.Context ctx,
        NetworkCompressionModel compressionModel)
    {
        #region __GHOST_READ__
        if ((changeMask__GHOST_MASK_BATCH__ & (1 << __GHOST_MASK_INDEX__)) != 0)
            __GHOST_FIELD_NAME__ = reader.ReadPackedUIntDelta(ref ctx, baseline.__GHOST_FIELD_NAME__, compressionModel);
        else
            __GHOST_FIELD_NAME__ = baseline.__GHOST_FIELD_NAME__;
        #endregion
    }
    public void Interpolate(ref GhostSnapshotData target, float factor)
    {
        #region __GHOST_INTERPOLATE__
        #endregion
    }
}

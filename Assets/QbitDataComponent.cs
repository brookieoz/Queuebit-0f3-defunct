using Unity.Entities;
using Unity.NetCode;
[GenerateAuthoringComponent]
public struct QbitDataComponent : IComponentData
{
    [GhostDefaultField]
    public int PlayerId;
    public bool IsActive;
    public int Life;
    public int Speedclass;
    public int DashingLevel;
    public char PreviousInput; //udlr for directions, s for space
}

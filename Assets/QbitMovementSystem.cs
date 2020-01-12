using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Transforms;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class QbitMovementSystem : ComponentSystem
{
    private int teleportDistance;
    protected override void OnUpdate()
    {
        var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
        var tick = group.PredictingTick;
        //var deltaTime = Time.DeltaTime;
        Entities.ForEach((DynamicBuffer<QbitInput> inputBuffer, ref Translation trans, ref PredictedGhostComponent prediction, ref QbitDataComponent qbitData) =>
        {
            if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                return;
            QbitInput input;
            inputBuffer.GetDataAtTick(tick, out input);
            //if (qbitData.IsActive == true) {
                if (input.spacebarSpecial != 0) {
                    //Implement special moves here
                    if (qbitData.Speedclass == 3) { //speeder teleport
                        if (qbitData.DashingLevel >= 8) {
                            //long teleport
                            teleportDistance = 12;
                        } 
                        else if (qbitData.DashingLevel >= 5) {
                            //medium teleport
                            teleportDistance = 6;
                        }
                        else if (qbitData.DashingLevel >= 2) {
                            teleportDistance = 2;
                        }
                        else teleportDistance = 0;
                        switch (qbitData.PreviousInput) {
                            case 'l': trans.Value.x -= teleportDistance; break;
                            case 'r': trans.Value.x += teleportDistance; break;
                            case 'u': trans.Value.y += teleportDistance; break;
                            case 'd': trans.Value.y -= teleportDistance; break;
                            default: break;
                        }
                    }
                    qbitData.PreviousInput = 's';
                    qbitData.IsActive = false;
                }
                else { //spacebar has priority to prevent multiple inputs
                    if (input.horizontal > 0) {
                        trans.Value.x += 1; //right
                        qbitData.IsActive = false;
                        if (qbitData.PreviousInput == 'r') {
                            qbitData.DashingLevel += 1;
                        }
                        else {
                            qbitData.PreviousInput = 'r';
                            qbitData.DashingLevel = 1;
                        }
                    }
                    else if (input.horizontal < 0) {
                        trans.Value.x -= 1; //left
                        qbitData.IsActive = false;
                        if (qbitData.PreviousInput == 'l') {
                            qbitData.DashingLevel += 1;
                        }
                        else {
                            qbitData.PreviousInput = 'l';
                            qbitData.DashingLevel = 1;
                        }
                    }
                    if (input.vertical > 0) {
                        trans.Value.z += 1; //up
                        qbitData.IsActive = false;
                        if (qbitData.PreviousInput == 'u') {
                            qbitData.DashingLevel += 1;
                        }
                        else {
                            qbitData.PreviousInput = 'u';
                            qbitData.DashingLevel = 1;
                        }
                    }
                    else if (input.vertical < 0) {
                        trans.Value.z -= 1; //down
                        qbitData.IsActive = false;
                        if (qbitData.PreviousInput == 'd') {
                            qbitData.DashingLevel += 1;
                        }
                        else {
                            qbitData.PreviousInput = 'd';
                            qbitData.DashingLevel = 1;
                        }
                    }
                }
            //}
        });
    }
}


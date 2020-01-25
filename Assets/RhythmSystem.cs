using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.Diagnostics;
using Unity.NetCode;
[UpdateInGroup(typeof(ClientAndServerSimulationSystemGroup))]
public class RhythmSystem : ComponentSystem
{
    public Stopwatch localGameStopwatch;

    public long bpm;
    public long nextWholebeatTime;
    public long wholebeatDuration;
    public long nextSubbeatTime;
    public long subbeatDuration;
    public int subbeatCounter;
    public bool stopwatchStarted; //temporary flag


    protected override void OnCreate() {
        localGameStopwatch = new Stopwatch();
        bpm = 60L; //TODO : Set this to song bpm later. Focusing on single bpm now.
        wholebeatDuration = (60000L / bpm); // in ms
        subbeatDuration = wholebeatDuration / 12;
        nextWholebeatTime = 5 * wholebeatDuration; //set for 5 beats in the future - may need to be changed.
        nextSubbeatTime = nextWholebeatTime; //sync up all 3 playmodes. May need to change hard code.
        subbeatCounter = 11;
    }

    protected override void OnUpdate() {
        //Start upon receiving sync signal
        //(after all players have loaded in)
        if (stopwatchStarted == false) {
            localGameStopwatch.Start(); //temporary flag
            //TODO : Start audio playback from audio source
        }

        if (localGameStopwatch.ElapsedMilliseconds > nextSubbeatTime) {
            subbeatCounter++;
            nextSubbeatTime += subbeatDuration;
            if (subbeatCounter > 11) {
                subbeatCounter = 0;
            }
            //UnityEngine.Debug.Log(subbeatCounter); //
            switch (subbeatCounter) {   //Refresh every Qbit's active flag between every applicable beat
                case 0: //Main beat on 0, duple on 0, 6, triple on 0, 4, 8
                    //play sfx
                    break;
                case 2:
                    //reset 3 between triplets 0 and 4
                    Entities.ForEach((ref QbitDataComponent dataComponent) => {
                        if (dataComponent.Speedclass == 3) {
                            dataComponent.IsActive = true;
                        }
                    });
                    break;
                case 3:
                    //reset 2 between duple 0 and 6
                    Entities.ForEach((ref QbitDataComponent dataComponent) => {
                        if (dataComponent.Speedclass == 2) {
                            dataComponent.IsActive = true;
                        }
                    });
                    break;
                case 6:
                    //reset 1 between 0 and 12, 3 between 4 and 8
                    Entities.ForEach((ref QbitDataComponent dataComponent) => {
                        if (dataComponent.Speedclass == 1 | dataComponent.Speedclass == 3) {
                            dataComponent.IsActive = true;
                        }
                     });
                     break;
                case 9:
                    //reset 2 between duple 6 and 12
                    Entities.ForEach((ref QbitDataComponent dataComponent) => {
                        if (dataComponent.Speedclass == 2) {
                            dataComponent.IsActive = true;
                        }
                    });
                    break;
                case 10:
                    //reset 3 between triplet 8 and 12
                    Entities.ForEach((ref QbitDataComponent dataComponent) => {
                        if (dataComponent.Speedclass == 3) {
                            dataComponent.IsActive = true;
                        }
                    });
                    break;
                default:
                    break;
                
            }
        }

        //UnityEngine.Debug.Log(localGameStopwatch.ElapsedMilliseconds);
        //TODO: Life and Damage systems based off of timing windows
    }
}

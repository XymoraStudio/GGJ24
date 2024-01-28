using System;
using System.Collections.Generic;
using UnityEngine;

public class Cubicle : MonoBehaviour {
    [SerializeField] List<Worker> cubicleWorkers;

    public static event Action<Cubicle> OnCubicleSlapped;

    private void Start() {
        Worker.OnSlapped += WorkerSlapped;
    }
    private void OnDestroy() {
        Worker.OnSlapped -= WorkerSlapped;
    }

    void WorkerSlapped(Worker worker) {
        if(cubicleWorkers.Contains(worker)) {
            OnCubicleSlapped?.Invoke(this);
        }
    }

    #region Laughing
    public void Laugh() {
        foreach(Worker worker in cubicleWorkers) {
            worker.Laugh();
        }
    }
    public void StopLaughing() {
        foreach(Worker worker in cubicleWorkers) {
            worker.StopLaughing();
        }
    }
    #endregion
}

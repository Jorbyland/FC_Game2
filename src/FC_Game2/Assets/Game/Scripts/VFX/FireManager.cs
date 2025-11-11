using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;

public class FireManager : MonoBehaviour
{
    public VisualEffect fireVFX;
    public int maxFireCount = 1000;
    public bool test = false;

    struct FireData
    {
        public Vector3 position;
        public float size;
    };

    GraphicsBuffer fireBuffer;
    List<FireData> activeFires = new();
    int lastCount;

    void Start()
    {
        fireBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxFireCount, sizeof(float) * 4);
        fireVFX.SetGraphicsBuffer("FireBuffer", fireBuffer);
        fireVFX.SetInt("FireCount", 0);
        // for (int i = 0; i < maxFireCount; i++)
        // {
        //     AddFire(new Vector3(Random.Range(-50f, 50f), 2f, Random.Range(-50f, 50f)), 1f);
        // }
    }

    public void AddFire(Vector3 pos, float size)
    {
        if (activeFires.Count >= maxFireCount) return;
        activeFires.Add(new FireData { position = pos, size = size });
    }

    void LateUpdate()
    {
        int count = activeFires.Count;
        if (count != lastCount)
        {
            fireBuffer.SetData(activeFires);
            fireVFX.SetInt("FireCount", count);
            lastCount = count;
        }
    }

    void Update()
    {
        if (test)
        {
            AddFire(new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f)), 1f);
            test = false;
        }
    }

    void OnDestroy()
    {
        fireBuffer?.Release();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{

	public static CameraShaker Instance { get; private set; }
	private CinemachineVirtualCamera cam;
	private float shakeTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
		{
			shakeTimer -= Time.deltaTime;
			if (shakeTimer <= 0f)
			{
				CinemachineBasicMultiChannelPerlin cinbasicPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
				cinbasicPerlin.m_AmplitudeGain = 0f;
			}
		}
    }

	public void ShakeCam(float intens, float time)
	{
		CinemachineBasicMultiChannelPerlin cinbasicPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		cinbasicPerlin.m_AmplitudeGain = intens;
		shakeTimer = time;
	}

	void Awake()
	{
		Instance = this;
		cam = GetComponent<CinemachineVirtualCamera>();
	}
}

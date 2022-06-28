using GGJ.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	PlayerController pl;
	GameStats stats;

	public LineRenderer lineRenderer;
	public LayerMask checkLayers;
	public Transform laserSpawn;
	private float canFire = 1f;
	private Vector3 mousePos;
	private bool isShooting = false;
	GunData gunData = new GunData();

    // Start is called before the first frame update
    void Start()
    {
		if (lineRenderer == null) Debug.Log("lineRenderer not set");
		pl = GetComponent<PlayerController>();;
		stats = Game.GetModel<GameStats>();
		//gunStatus = GameStateEnum.Mater;
		findGunData(gunData.type);
		canFire = 0;
		updateColor();
	}

	void findGunData(GameStateEnum type)
	{
		foreach (GunData item in stats.guns)
		{
			if (item.type == type)
			{
				gunData = item;
				return;
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public bool shoot(Vector2 mousepos)
	{
		mousePos = mousepos;
		if (canFire <= 0)
		{
			canFire = gunData.fireRate;
			doShooting();
			return isShooting;
		}
		canFire -= Time.deltaTime;
		return false;
	}

	public bool swap()
	{
		if (gunData.type == GameStateEnum.Mater)
		{
			findGunData(GameStateEnum.Antimater);
			pl.IsGreen = false;
		}
		else
		{
			findGunData(GameStateEnum.Mater);
			pl.IsGreen = true;
		}

		updateColor();
		HealthBar.ChangeState(gunData.type);
		canFire = 0;   ///reset gun shooting
		return true;
	}

	void updateColor()
	{
		Color newCol;

		if (ColorUtility.TryParseHtmlString(gunData.color, out newCol))
		{
			lineRenderer.startColor = newCol;
			lineRenderer.endColor = newCol;
		}
	}

	void doShooting()
	{
		switch (gunData.type)
		{
			case GameStateEnum.Mater:
				StartCoroutine(raycastShooting());
				break;
			case GameStateEnum.Antimater:
				StartCoroutine(raycastShooting());
				break;
			default:
				break;
		}
		
	}

	IEnumerator raycastShooting()
	{
		//RaycastHit2D hit = Physics2D.Raycast(transform.position, this.transform.forward, checkLayers);
		RaycastHit2D hit = Physics2D.Raycast(pl.transform.position, pl.rbTurret.up, 150f, checkLayers);

		if (hit.collider != null)
		{

			// Calculate the distance from the surface and the "error" relative
			// to the floating height.
			//float distance = Mathf.Abs(hit.point.y - transform.position.y);
			//float heightError = floatHeight - distance;
			int layer = hit.collider.gameObject.layer;
			if (layer == 7 || layer == 8)
			{
				EnemyController en = hit.transform.GetComponent<EnemyController>();
				if (en != null)
				{
					if ((layer == 7 && pl.IsGreen) || (layer == 8 && !pl.IsGreen))
						en.Damage(gunData.damage);

					lineRenderer.SetPosition(0, laserSpawn.position);
					lineRenderer.SetPosition(1, hit.point);
				}
				else
				{
					Debug.Log("no enemy controller, wrong layer");
					yield break;
				}
			}
			else
			{
				lineRenderer.SetPosition(0, laserSpawn.position);
				lineRenderer.SetPosition(1, hit.point);
			}

		}
		else
		{
			doEmptyShot();
		}

		lineRenderer.enabled = true;
		isShooting = true;

		yield return new WaitForSeconds(.1f);

		lineRenderer.enabled = false;
		isShooting = false;

	}

	void doEmptyShot()
	{
		Vector3 playerDirection = (pl.rbTurret.up);

		Vector3 spawnPos =  playerDirection * 150f;
		//Vector3 offset = pl.rbTurret.rotation * laserSpawn.localPosition;
		
		lineRenderer.SetPosition(0, laserSpawn.position);
		lineRenderer.SetPosition(1, spawnPos);
		//lineRenderer.transform.position = laserSpawn.position;
	}
}

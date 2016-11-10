using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MovingUnit {

    public void initialize(Cart startPos, int health, int damage, float speed)
    {
        base.movingUnitInitialize(startPos, health, damage, speed);
    }

    private Vector3 touchPosition;
    private bool isTouch;
	override protected void Update()
    {
        Camera camera = Camera.main;

        GameManager.instance.mapManager.litMap(position);
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            isTouch = true;
        }
        else if (!Input.GetMouseButton(0) && isTouch)
        {
            if (Input.mousePosition == touchPosition)
            {
                Cart dest = Utils.toCartesian(camera.ScreenToWorldPoint(Input.mousePosition));
                computePath(dest);
                GameManager.instance.turnLeft = path.Count;
            }
            isTouch = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.enemySkipMove();
            animator.Play("mallowSeduction");
            Debug.Log("Stop Enemies");
        }
        base.Update();
	}

	private IEnumerator loot(GameObject item)
	{
		while (moving)
			yield return null;
		GameManager.instance.itemManager.loot(item);
	}

    override public bool attemptMove()
    {
		GameObject unit, item;
        bool canMove = move(out unit, out item);
        if (!canMove && unit != null)
        {
            if (unit.transform.tag == "Enemy")
            {
                path.Clear();
                GameManager.instance.turnLeft = 0;
            }
            if (unit.transform.tag == "Finish")
            {
                GameManager.instance.win();
            }
        }
		if (canMove && item != null)
		{
			GameManager.instance.mapManager.map[nextPos.x][nextPos.y].item = null;
			StartCoroutine(loot(item));
		}
        return canMove;
    }
}

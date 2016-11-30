using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Player : MovingUnit {

    public void initialize(Cart startPos, int health, int damage, float speed)
    {
        base.movingUnitInitialize(startPos, health, damage, speed);
        moveDone();
    }

	public string   anim = null;

	public void playAnim(string s)
	{
		anim = s;
	}

    private Vector3 touchPosition;
    private bool isTouch;

	override protected void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
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
                    Camera camera = Camera.main;

                    Cart dest = Utils.toCartesian(camera.ScreenToWorldPoint(Input.mousePosition));
                    computePath(dest);
                    GameManager.instance.turnLeft = path.Count;
                }
                isTouch = false;
            }
        }
		else if (anim != null)
		{
			animator.Play(anim);
			if (anim == "mallowSeduction") {
				GameManager.instance.enemySkipMove ();
			}
			anim = null;
		}

		base.Update();
	}

    protected override void moveDone()
    {
        GameManager.instance.mapManager.litMap(position);

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            bool behind = false;
            bool active = false;
            foreach (Transform child in building.transform)
            {
                if (child.gameObject.layer != LayerMask.NameToLayer("BlockingLayer"))
                {
                    active = child.gameObject.activeInHierarchy;
                    if (Mathf.Abs(this.transform.position.x - child.position.x) < Utils.tileSizeInUnits.x &&
                        Mathf.Abs(this.transform.position.y - child.position.y) < Utils.tileSizeInUnits.y)
                    {
                        behind = true;
                        foreach (Transform c in building.transform)
                        {
                            if (c.gameObject.layer != LayerMask.NameToLayer("BlockingLayer"))
                                c.gameObject.SetActive(false);
                        }
                        if (behind)
                            break;
                    }
                }
            }
            if (!behind && !active)
            {
                foreach (Transform c in building.transform)
                {
                    if (c.gameObject.layer != LayerMask.NameToLayer("BlockingLayer"))
                        c.gameObject.SetActive(true);
                }
            }
        }
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

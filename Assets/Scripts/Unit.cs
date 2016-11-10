using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour {

    protected Animator animator;

    public float zIndex;
    public int _health;
    public int _damage;

    private Cart pos;
    public Cart position { get { return pos; } set { pos = value; transform.position = pos.toIsometric(); } }

	void Start()
	{
		animator = GetComponent<Animator>();
	}

    protected void unitInitialize (Cart startPos, int health, int damage)
    {
        GameManager.instance.mapManager.map[startPos.x][startPos.y].unit = gameObject;

		position = startPos;

        _health = health;
        _damage = damage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalStuff;

public class RoombaScript : BasicFurnitureScript
{

	public Sprite upSprite, downSprite, leftSprite, rightSprite;
	private SpriteRenderer sRenderer;

	public Direction roombaDirection;
	private GridManager gManager;
	private float moveAnimSpeed;

	public override void Start() {
		base.Start();
		sRenderer = GetComponent<SpriteRenderer>();
		gManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
		moveAnimSpeed = TimeManagerScript.instance.moveAnimationSpeed;
	}

	public void MoveRoomba(int xMax, int yMax) {

		if(roombaDirection == Direction.up || roombaDirection == Direction.down) {
			MoveUD(yMax);
		} else {
			MoveLR(xMax);
		}

	}

	public void MoveLR(int max) {

		int nx, ny;
		if (roombaDirection == Direction.left) {
			nx = itemPos.x - 1;
			ny = itemPos.y;
		} else {
			nx = itemPos.x + 1;
			ny = itemPos.y;
		}

		//check if stuck
		if (nx > max - 1) { //hit right edge
			if (!gManager.TileIsOccupied(new Vector2Int(itemPos.x - 1, ny))) {
				FlipDirection();//move and flip
				MoveInDirection(roombaDirection);
			} else {
				FlipDirection();//just flip
			}
		} else if (nx < 0) { //hit left edge
			if (!gManager.TileIsOccupied(new Vector2Int(itemPos.x + 1, ny))) {
				FlipDirection();//move and flip
				MoveInDirection(roombaDirection);
			} else {
				FlipDirection();//just flip
			}
		} else if (itemPos.x == 0 || itemPos.x == max - 1) {
			if (gManager.TileIsOccupied(new Vector2Int(nx, ny))) {
				FlipDirection();
			} else {
				MoveInDirection(roombaDirection); //just move forward
			}
		} else if (gManager.TileIsOccupied(new Vector2Int(itemPos.x + 1, ny)) &&
			gManager.TileIsOccupied(new Vector2Int(itemPos.x - 1, ny))) { //stuck in between
			FlipDirection();//just flip
		} else { //check if bump on an object, not stuck
			if (gManager.TileIsOccupied(new Vector2Int(nx, ny))) {
				FlipDirection();//move and flip
				MoveInDirection(roombaDirection);
			} else {
				MoveInDirection(roombaDirection); //just move forward
			}
		}


	}

	public void MoveUD(int max) {
		
		int nx, ny;
		if (roombaDirection == Direction.up) {
			nx = itemPos.x;
			ny = itemPos.y - 1;
		} else {
			nx = itemPos.x;
			ny = itemPos.y + 1;
		}
		//check if stuck
		if (ny > max - 1) { //hit bottom
			if(!gManager.TileIsOccupied(new Vector2Int(nx, itemPos.y - 1))) {
				FlipDirection();//move and flip
				MoveInDirection(roombaDirection);
			} else {
				FlipDirection();//just flip
			}
		} else if (ny < 0) { //hit top
			if (!gManager.TileIsOccupied(new Vector2Int(nx, itemPos.y + 1))) {
				FlipDirection();//move and flip
				MoveInDirection(roombaDirection);
			} else {
				
				FlipDirection();//just flip
			}
		} else if(itemPos.y == max - 1) {
			if (gManager.TileIsOccupied(new Vector2Int(nx, ny))) {
				FlipDirection();
			} else {
				MoveInDirection(roombaDirection); //just move forward
			}
		}  else if(itemPos.y == 0) {
			if (gManager.TileIsOccupied(new Vector2Int(nx, ny))) {
				FlipDirection();
			} else {
				MoveInDirection(roombaDirection); //just move forward
			}
		} else if (gManager.TileIsOccupied(new Vector2Int(nx, itemPos.y + 1)) &&
			gManager.TileIsOccupied(new Vector2Int(nx, itemPos.y - 1))) { //stuck in between
			FlipDirection();//just flip
		} else { //check if bump on an object, not stuck
			if(gManager.TileIsOccupied(new Vector2Int(nx, ny))) {
				FlipDirection();//move and flip
				MoveInDirection(roombaDirection);
			} else {
				MoveInDirection(roombaDirection); //just move forward
			}
		}
	}

	public void MoveInDirection(Direction d) {
		gItem.MoveOneUnitBySeconds(d, moveAnimSpeed);
	}

	public void FlipDirection() {
		if (roombaDirection == Direction.up) {
			roombaDirection = Direction.down;
			sRenderer.sprite = downSprite;
		} else if (roombaDirection == Direction.down) {
			roombaDirection = Direction.up;
			sRenderer.sprite = upSprite;
		}
		else if (roombaDirection == Direction.left) {
			roombaDirection = Direction.right;
			sRenderer.sprite = rightSprite;
		}
		else {
			roombaDirection = Direction.left;
			sRenderer.sprite = leftSprite;

		}
	}

    
}

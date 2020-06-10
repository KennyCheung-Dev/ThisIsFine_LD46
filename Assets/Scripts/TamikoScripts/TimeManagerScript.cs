using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalStuff;

//deals with time for now
//minimum map size: 5x5
//TODO: delay/disable wait button 
//TODO: lose if coffee or dog burns

public class TimeManagerScript : MonoBehaviour
{

	#region SINGLETON PATTERN

	private static TimeManagerScript _instance;
	public static TimeManagerScript instance {
		get {
			if (_instance == null) {
				_instance = (TimeManagerScript)FindObjectOfType(typeof(TimeManagerScript));
				if (_instance == null) {
					Debug.LogError("An instance of " + typeof(TimeManagerScript) +
						" is needed in the scene, but there is none.");
				}
			}

			return _instance;
		}
	}
	#endregion

	//increases each time a unit of time passes
	public int timeUnitsElapsed = 0;
	public int par = 0;
	public int furnitureNeededToBurn;
	public int burningFurniture;

	public GameObject[] allItemsOnGrid;
	public GameObject[] absolutelyAllItems;
	public BasicFurnitureScript[] allFurnitureToBurn;
	private GameObject itemsToManage;

	private FireManagerScript fmScript;
	private MoveManager mmScript;

	private GameObject[] fireIndicators;

	private GridManager gManager;
	private FineOMeterScript fMeterScript;
	private WaitButtonScript wbScript;
	private MovesText mText;
	private Text movesTextWin;
	private GameObject winScreen;


	public Vector2Int gridSize;
	public bool levelIsWin = false;
	public bool levelIsLose = false;

	//speed of movement animation in seconds
	public float moveAnimationSpeed = 0.15f;
	private DogScript dog;



	//public GameObject desk;

	void Awake() {
		absolutelyAllItems = FindAllDirectChildren(GameObject.FindGameObjectWithTag("ItemsToManage"));
		List<BasicFurnitureScript> fntb = new List<BasicFurnitureScript>();
		for (int i = 0; i < absolutelyAllItems.Length; i++) {

			if(absolutelyAllItems[i].GetComponent<BasicFurnitureScript>() && absolutelyAllItems[i].GetComponent<BasicFurnitureScript>().isFlammable) {
				fntb.Add(absolutelyAllItems[i].GetComponent<BasicFurnitureScript>());
				furnitureNeededToBurn++;
			}
		}
		allFurnitureToBurn = fntb.ToArray();
		winScreen = GameObject.FindGameObjectWithTag("WinScreen");
		wbScript = GameObject.FindGameObjectWithTag("WaitButton").GetComponent<WaitButtonScript>();
		fMeterScript = GameObject.FindGameObjectWithTag("FineOMeter").GetComponent<FineOMeterScript>();
		dog = GameObject.FindGameObjectWithTag("Dog").GetComponent<DogScript>();
		fmScript = GetComponent<FireManagerScript>();
		mmScript = GetComponent<MoveManager>();
		gManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
		mText = GameObject.FindGameObjectWithTag("MovesText").GetComponent<MovesText>();
		movesTextWin = FindGameObjectInChildWithTag(winScreen, "MovesTextWin").GetComponent<Text>();
		winScreen.GetComponent<RectTransform>().localPosition = new Vector3(-119, 52, 0.02f);
	}

	void Start() {
		
		winScreen.SetActive(false);
		mText.Updatetext(timeUnitsElapsed, par);
		fMeterScript.maxValue = furnitureNeededToBurn;
		UpdateMeter();
		fMeterScript.ChangeMaxValue(furnitureNeededToBurn);
		gridSize = new Vector2Int((int)gManager.GetGridSize().x, (int)gManager.GetGridSize().y);
		allItemsOnGrid = gManager.GetGameObjectsOnGrid();
		timeUnitsElapsed = 0;
	}

	//call this method for time to move forward one step
	//TIME MOVES FORWARD LIKE SO:
	// - move everything needed to move 1 unit, update positions
	// - set fire to objects needed to set fire
	// - move away any fires needed to move away
	// - update the slider
	// - animate move everthing needed to animate

	public void StepTimeForward() {
		wbScript.DisableButtonForATime(moveAnimationSpeed);
		allItemsOnGrid = gManager.GetGameObjectsOnGrid();

		// - move everything needed to move 1 unit, update positions
		mmScript.MoveAllNeeded();

		// - set fire to objects needed to set fire
		fmScript.SpreadFire();


		fmScript.MoveAwayAllFiresIfNeeded(timeUnitsElapsed);

		UpdateMeter();

		timeUnitsElapsed++; //tick time by 1
		

		//only freeze things on fire
		gManager.FreezeObjectsOnFire();

		CheckIfWin();

	}

	//win if dog has coffee, dog isn't on fire and all furniture burnd
	public void CheckIfWin() {
		mText.Updatetext(timeUnitsElapsed, par);
		if (dog.isOnFire) {
			levelIsWin = false;
			levelIsLose = true;
			wbScript.LoseButton();
		}

		if (burningFurniture >= furnitureNeededToBurn &&
			dog.gotCoffee && !dog.isOnFire) {
			levelIsWin = true;
			wbScript.WinButton();
			Invoke("SetWinScreen", 0.5f);
		}

	}

	void SetWinScreen() {
		print("FASDdasd");
		winScreen.SetActive(true);
		if(par > 0) {
			movesTextWin.text = "Moves: " + timeUnitsElapsed + "  Par: " + par;
		} else {
			movesTextWin.text = "Moves: " + timeUnitsElapsed;
		}
        //HighScore.Instance.UpdateScore(timeUnitsElapsed);
	}

	public void UpdateMeter() {
		
		int burning = 0;
		for(int i = 0; i < allFurnitureToBurn.Length; i++) {
			if (allFurnitureToBurn[i].isOnFire) burning++;
		}
		burningFurniture = burning;
		fMeterScript.ChangeValue(burning);
	}



	//for undo maybe???
	/*public void stepTimeBackward() {


	}*/

}

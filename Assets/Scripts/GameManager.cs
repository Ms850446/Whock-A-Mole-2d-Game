using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
	[SerializeField] private List<MoleShowHide> moles;
	[Header("UI Objects")]
	// plyButton is ref we can hide it when we are playing
	[SerializeField] private GameObject PlayButton;
	[SerializeField] private GameObject gameUi;
	[SerializeField] private GameObject outoftimetext;
	[SerializeField] private GameObject bombtext;
	[SerializeField] private TMPro.TextMeshProUGUI timetext;
	[SerializeField] private TMPro.TextMeshProUGUI scoretext;
	private float startingTime = 30f; // determine how much time we have in the game (30 seconds)
	// Global Variables
	private float timeRemaining; // determine which moles are animating
	private HashSet<MoleShowHide> currentMoles = new HashSet<MoleShowHide>();
	public static int score;
	private bool playing = false;
	// this is public so the play button can see it
	public void Start(){
		StartGame();
	}
	public void StartGame()
	{// hide/show the UI element we do/don't want to see
		Debug.Log("entered start game");
		PlayButton.SetActive(false);
		outoftimetext.SetActive(false);
		bombtext.SetActive(false);
		gameUi.SetActive(true);
		// hides all the visible moles
		for(int i = 0; i< moles.Count; i++)
		{
			moles[i].Hide();
			moles[i].SetIndex(i);
		}
		// remove any old game state
		currentMoles.Clear();
		// start with 30 sec
		timeRemaining = startingTime;
		score = 0;
		scoretext.text = "0";
		playing = true;
	}
	public void GameOver(int type)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
		// show the message
		// if(type ==0)
		// {
		// 	outoftimetext.SetActive(true);
		// }
		// else
		// {
		// 	bombtext.SetActive(true);
		// }
		// foreach (MoleShowHide mole in moles)
		// {
		// 	mole.StopGame();
		// }
		// // stop the game and show the start UI
		// playing = false;
		// PlayButton.SetActive(true);
	}
	public void AddScore(int moleIndex)
	{ // add and update score
	score += 1;
	scoretext.text = $"{score}";
	// increase time by a little bit
	timeRemaining += 1;
	 // remove from activate moles
	 currentMoles.Remove(moles[moleIndex]);

	}
	public void Missed(int moleIndex, bool isMole)
	{
		if(isMole)
		{
			// decrase time by a little bit
			timeRemaining -= 2;

		}
		// remove from active moles
		currentMoles.Remove(moles[moleIndex]);
	}
	void Update()
	{
		if(playing)
		{
			timeRemaining -= Time.deltaTime;
			if (timeRemaining <= 0)
			{
				timeRemaining = 0;
				GameOver(0);
			}
			timetext.text = $"{(int)timeRemaining/60}:{(int)timeRemaining %60:D2}";
			// check if we need to start any more moles
			// every 10 moles hit there will be more moles appearing
			if (currentMoles.Count <= (score/10))
			{
				// choose a random moles
				int index = Random.Range(0, moles.Count);
				// we check if that's in our current moles hash set which would indicate it's active
				if(!currentMoles.Contains(moles[index]))
				{
					currentMoles.Add(moles[index]);
					moles[index].Activate(score/10);
				}
			}
		}
	}

}

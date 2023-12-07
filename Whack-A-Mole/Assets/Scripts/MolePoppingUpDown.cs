using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    //Offset for hiding the mole
    private Vector2 startPosition = new Vector2(0, -2.56f); //vector to because it's 2D, 2.56 because we make it 256 pixels
    private Vector2 endPosition = new Vector2(0,0); // to centered the mole when it popping up
    private float showMole = 0.5f; // show/hide the mole for 0.5 second
    private float duration = 1f; // how long mole stays visible
    private IEnumerator ShowHide(Vector2 start, Vector2 end) {
        /*----------------------SHOW THE MOLE-----------------------------*/

        transform.localPosition = start; //to make sure that we are in the start position
        float elapsed = 0f; // track time that has elapsed
        while (elapsed < showMole) { //keep going until we reach our duration
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showMole); //lerp function take start, end, interpolation factor(0:1)
            elapsed += Time.deltaTime; // Update at max framerate.
            yield return null; //immediately return and loops again
        }
        transform.localPosition = end; //to make sure that we are in the end position
        yield return new WaitForSeconds(duration); // wait for the shown state duration
        /*----------------------HIDE THE MOLE-----------------------------*/

        elapsed = 0f;
        while (elapsed < showMole) {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showMole); //same code just revers the start and end
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Make sure we're exactly back at the start position.
        transform.localPosition = start;
    }
    private void Start(){
        SetLevel(0);
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }
    /*---------------------------CLICK ON THE MOUSE--------------------------*/

    //references to sprites to change reaction and shape of the mole when we tap on it
    [Header("Graphics")]
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite moleHardHat;
    [SerializeField] private Sprite moleHatBroken;
    [SerializeField] private Sprite moleHit;
    [SerializeField] private Sprite moleHatHit;
    private SpriteRenderer spriteRenderer; // to render the sprites
    private bool hittable = true; //check if i hit the mole or not
    private Animator animator; //for bombs
    private void Awake(){
        // Get references to the components we'll need.
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void OnMouseDown(){
         //when we tap on the mole its reaction change & stays in its position for a while then hide it     
         if (hittable) {
            switch (moleType) {
                case MoleType.Standard:
                spriteRenderer.sprite = moleHit;
                StopAllCoroutines(); // Stop the animation
                StartCoroutine(QuickHide());
                // Turn off hittable so that we can't keep tapping for score.
                hittable = false;
                break;
                case MoleType.HardHat:
                // If lives == 2 reduce, and change sprite.
                if (lives == 2) {
                  spriteRenderer.sprite = moleHatBroken;
                  lives--;
                } 
                else {
                  spriteRenderer.sprite = moleHatHit;
                  // Stop the animation
                  StopAllCoroutines();
                  StartCoroutine(QuickHide());
                  // Turn off hittable so that we can't keep tapping for score.
                  hittable = false;
                }
                break;
                case MoleType.Bomb:
                break;
                default:
                break;
            }
        }
    }
    private IEnumerator QuickHide(){
         //wait quarter of second
         yield return new WaitForSeconds(0.25f);
         // Whilst we were waiting we may have spawned again here, so just
         // check that hasn't happened before hiding it. This will stop it
         // flickering in that case.
         if (!hittable) {
           Hide();
         }
    }
    public void Hide(){
        transform.localPosition = startPosition; //set the location to the starting position
    }

    /*---------------------------CHANGE MOLE TYPE--------------------------- */

    public enum MoleType { Standard, HardHat, Bomb };
    private MoleType moleType;
    private float hardRate = 0.25f;
    private int lives;
    private float bombRate = 0f;
    public void CreateNext(){
        //create random num between 0 & 1
        // if the random num less than HardHatRate -> set the type to Hard hat, set lives to 2
        float random = Random.Range(0f, 1f);
        if (random < bombRate) {
          // Make a bomb.
          moleType = MoleType.Bomb;
          // The animator handles setting the sprite.
          animator.enabled = true;
        } 
        else {
            animator.enabled = false; //set the animator off before set the sprite
            random = Random.Range(0f, 1f); 
            if (random < hardRate) {
                // Create a hard one.
                moleType = MoleType.HardHat;
                spriteRenderer.sprite = moleHardHat;
                lives = 2;
            }
            else {
                // Create a standard one.
                moleType = MoleType.Standard;
                spriteRenderer.sprite = mole;
                lives = 1;
            }  
        }
      // Mark as hittable so we can register an onclick event.
      hittable = true; // to reset each time everytime we create a mole
    }
    /*----------------------------DIFFICULTY LEVEL----------------------*/
    private void SetLevel(int level){
        //first we need no bombs at the start of the game
        // As level increases increse the bomb rate to 0.25 at level 10.
        //so we will increase bomb rate by 2.5% every level
        bombRate = Mathf.Min(level * 0.025f, 0.25f);
        //Similarly HardHat
        // Increase the amounts of HardHats until 100% at level 40.
        hardRate = Mathf.Min(level * 0.025f, 1f);
        // Duration bounds get quicker as we progress. No cap on insanity.
        float durationMin = Mathf.Clamp(1 - level * 0.1f, 0.01f, 1f);
        float durationMax = Mathf.Clamp(2 - level * 0.1f, 0.01f, 2f);
        duration = Random.Range(durationMin, durationMax);


    }

}

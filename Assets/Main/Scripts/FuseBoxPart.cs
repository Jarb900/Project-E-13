using UnityEngine;
using UnityEngine.Events;

public class FuseBoxPart : MonoBehaviour
{
    [Header("Settings")]
    public Animator masterAnimator; // Drag the FuseBox_Master here
    public string triggerName;      // "TrigOpen" for door, "TrigSwitch" for switch

    [Header("Audio Settings")]
    public AudioClip interactSound; // Drag Door_Open.mp3 or Click.mp3 here
    public AudioClip generatorStrart;
    public AudioSource generatorSource; // Drag the Generator's AudioSource here
    public float blendTime = 0.5f;

    [Header("Game Logic")]
    public bool requiresDoorOpen = false; // Check this for the SWITCH only
    public bool isDoorOpen = false; // Global memory of the door state

    public bool SwitchHasBeenUsed = false;
    private AudioSource myAudio; // The speaker on THIS object

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // This function is called by your PlayerInteraction script
    public void Interact()
    {
        // If I am the door, and I am already open, STOP here.
        if (triggerName == "TrigOpen" && isDoorOpen) return;

        // 1. If this is the Switch, stop if the door is closed
        if (requiresDoorOpen && !isDoorOpen) return;
        if (requiresDoorOpen && SwitchHasBeenUsed) return;

        // 2. Play the Animation
        if (masterAnimator != null)
        {
            masterAnimator.SetTrigger(triggerName);
        }

        // 3. Play Sound (The "Click" or "Creak")
        if (myAudio && interactSound)
        {
            myAudio.PlayOneShot(interactSound); //
        }

        // 4. Update Game Logic
        if (triggerName == "TrigOpen")
        {
            // If we just opened the door, tell the Switch it's allowed to move now
   
            UnlockTheSwitch();

        }
        else if (triggerName == "TrigSwitch")
        {
            // If we just flipped the switch, turn on the power!
            SwitchHasBeenUsed = true;
            TurnOnGenerator();
        }
    }
    private void UnlockTheSwitch()
    {
        isDoorOpen = true;
        if (masterAnimator != null)
        {
            var allParts = masterAnimator.GetComponentsInChildren<FuseBoxPart>();
            foreach (var part in allParts)
            {
                part.isDoorOpen = true;
            }
        }
    }
    private void TurnOnGenerator()
    {
        if (generatorSource != null && generatorStrart != null)
        {
            // 1. Make the GENERATOR play the start sound (so it sounds 3D)
            generatorSource.PlayOneShot(generatorStrart);

            // 2. Schedule the "Loop" to start early
            if (generatorSource != null && generatorStrart != null)
            {
                // Calculate when to start: Total Length minus the overlap time
                float delay = generatorStrart.length - blendTime;

                // Safety check: Prevent negative numbers if the sound is too short
                if (delay < 0) delay = 0;

                generatorSource.PlayDelayed(delay);
            }
        }
    }
}
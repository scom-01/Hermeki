using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InfiniteRunnerEngine;
using MoreMountains.Tools;
using UnityEngine.UIElements;

public class HermekiInfiniteRunLevelManager : LevelManager
{
    [Header("Backgrounds")]
    [Tooltip("랜덤 배경 오브젝트")]
    public List<GameObject> Backgrounds;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RandomizeBackground();
        CurrentPlayableCharacters[0].GetComponent<Rigidbody2D>().isKinematic = true;
    }

    protected override void PrepareStart()
    {
        base.PrepareStart();
    }
    protected virtual void RandomizeBackground()
    {
        if (Backgrounds.Count != 0)
        {
            for (int i = 0; i < Backgrounds.Count;)
            {
                if (Backgrounds[i] == null)
                {
                    Backgrounds.RemoveAt(i);
                    continue;
                }
                Backgrounds[i].SetActive(false);
                i++;
            }

            int randomBg = UnityEngine.Random.Range(0, Backgrounds.Count);
            Backgrounds[randomBg].SetActive(true);
        }
    }

    /// <summary>
    /// Handles the start of the level : starts the autoincrementation of the score, sets the proper status and triggers the corresponding event.
    /// </summary>
    public override void LevelStart()
    {
        base.LevelStart();
        CurrentPlayableCharacters[0].GetComponent<Rigidbody2D>().isKinematic = false;
    }

    /// <summary>
    /// Instantiates all the playable characters and feeds them to the gameManager
    /// </summary>
    protected override void InstantiateCharacters()
    {
        base.InstantiateCharacters();
    }

    /// <summary>
    /// Resets the level : repops dead characters, sets everything up for a new game
    /// </summary>
    public override void ResetLevel()
    {
        base.ResetLevel();
    }

    /// <summary>
    /// Turns buttons on or off depending on the chosen mobile control scheme
    /// </summary>
    protected override void ManageControlScheme()
    {
        base.ManageControlScheme();
    }

    /// <summary>
    /// Every frame
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// What happens when all characters are dead (or when the character is dead if you only have one)
    /// </summary>
    protected override void AllCharactersAreDead()
    {
        // we test the status to avoid triggering it twice.
        if ((GameManager.Instance.Status == GameManager.GameStatus.GameOver) || (GameManager.Instance.Status == GameManager.GameStatus.LifeLost))
        {
            return;
        }

        // if we've specified an effect for when a life is lost, we instantiate it at the camera's position
        if (LifeLostExplosion != null)
        {
            GameObject explosion = Instantiate(LifeLostExplosion);
            explosion.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        }


        GUIManager.Instance.SetGameOverScreen(true);
        GameManager.Instance.SetStatus(GameManager.GameStatus.GameOver);
        MMEventManager.TriggerEvent(new MMGameEvent("GameOver"));

    }
}

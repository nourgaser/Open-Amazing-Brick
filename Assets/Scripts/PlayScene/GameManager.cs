using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private struct Level
    {
        public GameObject level;
        public int number;
    }

    private LinkedList<Level> levels = new LinkedList<Level>();

    public GameObject levelPrefab;

    private Transform player;

    public static int score = 0;

    private float levelHeightExtent;

    public float spaceBetweenLevels;

    private LinkedListNode<Level> currentLevel;

    public static event UnityAction levelPassed;
    public static event UnityAction gameEnded;

    private static Color[] colors = new Color[] { new Color(0xFF / 255f, 0x5D / 255f, 0x5D / 255f), new Color(0xFF / 255f, 0xD4 / 255f, 0x5E / 255f), new Color(0x74 / 255f, 0xDE / 255f, 0xDB / 255f), new Color(0x6A / 255f, 0x5E / 255f, 0xFF / 255f), new Color(0xFF / 255f, 0x5E / 255f, 0x76 / 255f) };

    // Start is called before the first frame update
    void Start()
    {
        Transform passage = levelPrefab.transform.Find("passage").Find("left");
        levelHeightExtent = passage.position.y + passage.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y;

        levels.AddFirst(generateLevel(0, 3));
        currentLevel = levels.First;

        while (getLevelTopY(levels.Last.Value.level) <= Boundaries.screenBounds.y)
            levels.AddLast(generateLevel(0, getLevelTopY(levels.Last.Value.level) + spaceBetweenLevels));

        PlayerController.playerPastHalfScreen.AddListener(moveLevelsDown);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        PlayerController.playerCollided += endGame1;
        PlayerController.playerFellOutOfScreen += endGame2;
    }

    void advanceLevel()
    {
        score++;
        currentLevel = currentLevel.Next;
        levelPassed.Invoke();
    }

    Level generateLevel(float x = 0, float y = 0, float z = 0)
    {
        GameObject level = Object.Instantiate(levelPrefab);
        level.transform.position = new Vector3(x, y, z);

        Transform passage = level.gameObject.transform.Find("passage");
        Transform bottomObstacle = level.gameObject.transform.Find("bottomObstacle");
        Transform topObstacle = level.gameObject.transform.Find("topObstacle");

        float passageOffset = Random.Range(-1f, 1f);
        float bottomObstacleOffset = passageOffset * Random.Range(0f, 2f);
        float topObstacleOffset = passageOffset * Random.Range(0f, 2f);

        passage.position = new Vector3(passage.position.x + passageOffset, passage.position.y, 0);
        bottomObstacle.position = new Vector3(bottomObstacle.position.x + bottomObstacleOffset, bottomObstacle.position.y, 0);
        topObstacle.position = new Vector3(topObstacle.position.x + topObstacleOffset, topObstacle.position.y, 0);

        Level newLevel = new Level();
        newLevel.level = level;
        if (levels.Count > 0)
            newLevel.number = levels.Last.Value.number + 1;
        else
            newLevel.number = 0; //this should happen ONLY for the first level.

        setLevelColor(newLevel);

        return newLevel;
    }

    void setLevelColor(Level level)
    {
        Color color = colors[(level.number / colors.Length) % colors.Length];

        SpriteRenderer[] children = level.level.transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer child in children)
        {
            child.color = color;
        }

    }

    void moveLevelsDown(float y)
    {
        // Move all levels down.
        foreach (Level level in levels)
            level.level.transform.position = new Vector3(level.level.transform.position.x, level.level.transform.position.y - y, level.level.transform.position.z);


        // Delete bottom level if it goes out of screen.
        if (levels.Count > 0 && getLevelTopY(levels.First.Value.level) < -Boundaries.screenBounds.y)
        {
            GameObject.Destroy(levels.First.Value.level);
            levels.RemoveFirst();
        }

        // Spawn new level if top level's highest point is on the screen.
        float lastLevelY = getLevelTopY(levels.Last.Value.level);
        if (lastLevelY <= Boundaries.screenBounds.y) levels.AddLast(generateLevel(0, lastLevelY + spaceBetweenLevels));

        // Adcance level if player's new position is above the current level's highest point.
        if (player.transform.position.y >= getLevelTopY(currentLevel.Value.level)) advanceLevel();
    }

    float getLevelTopY(GameObject level)
    {
        return level.transform.position.y + levelHeightExtent;
    }

    void endGame1()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        PlayerController.playerCollided -= endGame1;
        
        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerController>().controlsEnabled = false;

        PlayerPrefs.SetInt("lastscore", score);

        if (PlayerPrefs.HasKey("highscore") && PlayerPrefs.GetInt("highscore") < score)
            PlayerPrefs.SetInt("highscore", score);

        score = 0;

        player.GetComponent<Animator>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animation>().Play();
        Time.timeScale = 1.5f;
    }

    void endGame2() {
        PlayerController.playerFellOutOfScreen -= endGame2;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(0);
        Time.timeScale = 1.5f;
        gameEnded.Invoke();
        SceneManager.LoadScene("MainMenu");
    }

}

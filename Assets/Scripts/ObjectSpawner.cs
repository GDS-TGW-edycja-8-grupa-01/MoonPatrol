using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using CsvHelper;
using System.Linq;

public class ObjectSpawner : MonoBehaviour
{
    private class Feed
    {
        public float time;
        public string prefabName;
        public string extendedProperties;
    }


    private float lastCreated;
    private List<Feed> feed;

    public GameObject rock;
    public GameObject sectorMarker;
    public GameObject mine;
    public GameObject enemyTier1;
    public GameObject enemyTier2;
    private Sprite[] spriteSheet;

    [Range(0.5f, 10.0f)]
    public float enemySpawnDelay;

    private string[] letters = { "mp_gui_marker_a", "mp_gui_marker_b", "mp_gui_marker_c", "mp_gui_marker_d", "mp_gui_marker_e"
            , "mp_gui_marker_f", "mp_gui_marker_g", "mp_gui_marker_h", "mp_gui_marker_i", "mp_gui_marker_j", "mp_gui_marker_k"
            , "mp_gui_marker_l", "mp_gui_marker_m", "mp_gui_marker_n", "mp_gui_marker_o", "mp_gui_marker_p" };
    private int currentSector = 0;

    void Start()
    {
        var conf = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);

        conf.Delimiter = ",";
        conf.MemberTypes = CsvHelper.Configuration.MemberTypes.Fields;

        using (var reader = new StreamReader(".\\Misc\\Feed.csv"))
        using (var csv = new CsvReader(reader, conf))
        {
            feed = csv.GetRecords<Feed>().ToList();
        }

        spriteSheet = Resources.LoadAll<Sprite>("Sprites/GUI/mp_gui_markers_all");

        return;
    }

    void FixedUpdate()
    {
        Feed current;
        string prefabName;

        try
        {
            current = feed.First<Feed>(f => Time.unscaledTime >= f.time);
        }
        catch (System.InvalidOperationException e)
        {
            return;
        }

        feed.Remove(current);
        prefabName = current.prefabName.ToLower();

        if (prefabName == "sectormarker" && currentSector <= letters.Length)
        {
            InstantiateSectorMarker();
            currentSector++;
        }
        
        if (prefabName == "rock")
        {
            InstantiateRock();
        }

        if (prefabName == "mine")
        {
            InstantiateMine();
        }

        if (prefabName == "enemywave")
        {
            InstantiateEnemyWave(current.extendedProperties);
        }

    }

    private void InstantiateSectorMarker()
    {
        GameObject go = Instantiate(sectorMarker) as GameObject;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        
        sr.sprite = spriteSheet.ToList<Sprite>().First(s => s.name == letters[currentSector]);
    }

    private void InstantiateRock()
    {
        GameObject go = Instantiate(rock) as GameObject;
    }

    private void InstantiateMine()
    {
        GameObject go = Instantiate(mine) as GameObject;
    }

    private void InstantiateEnemyWave(string tier)
    {
        GameObject go = Instantiate(enemyTier1) as GameObject;
        StartCoroutine(StartCoroutine());
        
    }

    private IEnumerator StartCoroutine()
    {
        yield return new WaitForSeconds(5);

        GameObject go = Instantiate(enemyTier1) as GameObject;
    }
}
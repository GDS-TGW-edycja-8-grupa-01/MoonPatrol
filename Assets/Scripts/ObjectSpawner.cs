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
    private Sprite[] spriteSheet;

    private string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
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

        spriteSheet = Resources.LoadAll<Sprite>("Sprites/letters");

        return;
    }

    void FixedUpdate()
    {
        Feed current;

        try
        {
            current = feed.First<Feed>(f => Time.unscaledTime >= f.time);
        }
        catch (System.InvalidOperationException e)
        {
            return;
        }

        feed.Remove(current);

        if (current.prefabName == "SectorMarker" && currentSector <= letters.Length)
        {
            InstantiateSectorMarker();
            currentSector++;
        }

        if (current.prefabName == "Rock")
        {
            InstantiateRock();
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
}
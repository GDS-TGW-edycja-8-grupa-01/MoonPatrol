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

    public GameObject sectorMarker;

    void Start()
    {
        var conf = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);

        conf.Delimiter = ";";
        conf.MemberTypes = CsvHelper.Configuration.MemberTypes.Fields;

        using (var reader = new StreamReader(".\\Misc\\Feed.csv"))
        using (var csv = new CsvReader(reader, conf))
        {
            feed = csv.GetRecords<Feed>().ToList();
        }
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
        Instantiate(sectorMarker);
        sectorMarker.SendMessage("AssignLetter", current.extendedProperties);
    }
}
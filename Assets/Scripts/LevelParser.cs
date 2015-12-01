using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class LevelParser : MonoBehaviour
{
    public string relativeFilePath;

    public LevelData LoadLevelFromFile()
    {
        FileStream stream = new FileStream(Application.dataPath + relativeFilePath, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
        LevelData container = serializer.Deserialize(stream) as LevelData;
        stream.Close();

        return container;

    }
}

public class BlockData
{
    public int bgIndex;
    public bool ground;
    public int sideIndex;

    [XmlArray("Dogs")]
    [XmlArrayItem("Dog")]
    public List<EnemyDogParam> dogs = new List<EnemyDogParam>();

    [XmlArray("Planes")]
    [XmlArrayItem("Plane")]
    public List<AeroPlaneParam> planeParams = new List<AeroPlaneParam>();

    [XmlArray("Powerups")]
    [XmlArrayItem("Powerup")]
    public List<PowerUpParam> powerUpParams = new List<PowerUpParam>();

    [XmlArray("Birds")]
    [XmlArrayItem("Bird")]
    public List<BirdParam> birdParams = new List<BirdParam>();

    [XmlArray("Ufos")]
    [XmlArrayItem("Ufo")]
    public List<UfoParam> ufoParams = new List<UfoParam>();

    [XmlArray("Miscs")]
    [XmlArrayItem("Misc")]
    public List<MiscParam> miscParams = new List<MiscParam>();
}

[XmlRoot("LevelData")]
public class LevelData
{
    [XmlArray("Levels")]
    [XmlArrayItem("Level")]
    public List<BlockData> blocks = new List<BlockData>();
}

public class MiscParam
{
    public int _type;

    public float _posX;

    public float _posY;

    public float _size;
}
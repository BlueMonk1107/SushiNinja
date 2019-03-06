using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SceneData
{
    private List<string> _name;
    private List<Vector3> _position;
    private List<Vector3> _rotation;
    private List<Vector3> _scale;
    private List<int> _layer;
    private List<string> _tag;
    private GameObjectData _gameObjectData;
    private List<bool> _has_Collider; 
    private List<Vector3> _collider_Center;
    private List<Vector3> _collider_Size; 
    /// <summary>
    /// 数据个数
    /// </summary>
    public int Count
    {
        get { return _name.Count; }
    }
    public SceneData()
    {
        _name = new List<string>();
        _position = new List<Vector3>();
        _rotation = new List<Vector3>();
        _scale = new List<Vector3>();
        _layer = new List<int>();
        _tag = new List<string>();
        _has_Collider = new List<bool>();
        _collider_Center = new List<Vector3>();
        _collider_Size = new List<Vector3>();
    }
    public void SaveData(string name, Vector3 pos, Vector3 rot, Vector3 sca, int layer, string tag)
    {
        _name.Add(name);
        _position.Add(pos);
        _rotation.Add(rot);
        _scale.Add(sca);
        _layer.Add(layer);
        _tag.Add(tag);
        _has_Collider.Add(false);
    }

    public void SaveData(string name, Vector3 pos, Vector3 rot, Vector3 sca, int layer, string tag,Vector3 collider_Center, Vector3 collider_Size)
    {
        _name.Add(name);
        _position.Add(pos);
        _rotation.Add(rot);
        _scale.Add(sca);
        _layer.Add(layer);
        _tag.Add(tag);
        _has_Collider.Add(true);
        _collider_Center.Add(collider_Center);
        _collider_Size.Add(collider_Size);
    }

    public GameObjectData GetData(int i)
    {
        if (i >= Count)
        {
            Debug.Log("超出数据大小");
            return default(GameObjectData);
        }
        else
        {
            if (_has_Collider[i])
            {
                _gameObjectData = new GameObjectData(_name[i], _position[i], _rotation[i], _scale[i], _layer[i], _tag[i],_collider_Center[i],_collider_Size[i]);
            }
            else
            {
                _gameObjectData = new GameObjectData(_name[i], _position[i], _rotation[i], _scale[i], _layer[i], _tag[i]);
            }
            
            return _gameObjectData;
        }
    }
}

public struct GameObjectData
{
    public string _name { get; private set; }
    public Vector3 _position { get; private set; }
    public Vector3 _rotation { get; private set; }
    public Vector3 _scale { get; private set; }
    public int _layer { get; private set; }
    public string _tag { get; private set; }
    public  Vector3 _collider_Center { get; private set; }
    public Vector3 _collider_Size { get; private set; }

    public GameObjectData(string name, Vector3 pos, Vector3 rot, Vector3 sca, int layer, string tag):this()
    {
        _name = name;
        _position = pos;
        _rotation = rot;
        _scale = sca;
        _layer = layer;
        _tag = tag;
    }
    public GameObjectData(string name, Vector3 pos, Vector3 rot, Vector3 sca, int layer, string tag, Vector3 collider_Center, Vector3 collider_Size) : this()
    {
        _name = name;
        _position = pos;
        _rotation = rot;
        _scale = sca;
        _layer = layer;
        _tag = tag;
        _collider_Center = collider_Center;
        _collider_Size = collider_Size;
    }
}

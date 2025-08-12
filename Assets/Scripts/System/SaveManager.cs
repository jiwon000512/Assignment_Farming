using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class FieldObjectDataSet
{
    public List<FieldObjectData> datas = new List<FieldObjectData>();
}

/// <summary>
/// Newtonsoft���� vector3�� ������ �������� �ʱ� ������
/// ��ü�� Ŭ������ �����߽��ϴ�.
/// </summary>
[Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public SerializableVector3(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

/// <summary>
/// �����͸� ����,�ε� ���ִ� Ŭ����.
/// Newtonsoft ��Ű���� ����߽��ϴ�.
/// </summary>
public class SaveManager
{

    public static void Save<T>(T data) where T : class
    {
        string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

        string savePath = GetPath(typeof(T));
        File.WriteAllText(savePath, json);
    }

    public static T Load<T>() where T : class, new()
    {
        string savePath = GetPath(typeof(T));
        if (!File.Exists(savePath))
            return null;

        string json = File.ReadAllText(savePath);
        return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
    }

    public static string GetPath(System.Type type)
    {
        if (type == typeof(FieldObjectDataSet))
        {
            return Path.Combine(Application.persistentDataPath, "fieldObjectData.json");
        }

        return "";
    }
}

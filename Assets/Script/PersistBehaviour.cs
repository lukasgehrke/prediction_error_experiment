using System;
using System.IO;
using System.Reflection;
using UnityEngine;

/**
 * Allows saving and loading of values even when in play mode
 */

public class PersistBehaviour : MonoBehaviour
{
    public void SaveValues()
    {
        // save field values
        const BindingFlags flags = /*BindingFlags.NonPublic | */BindingFlags.Public | BindingFlags.Instance;
        FieldInfo[] fields = GetType().GetFields(flags);
        string dirname = "Temp/SerializedObjects/";
        string filename = dirname + name + "_" + GetInstanceID() + ".txt";

        //create directory if it doesn't exist
        Directory.CreateDirectory(dirname);

        using (StreamWriter sw = new StreamWriter(filename))
        {
            //write info to file
            foreach (FieldInfo fieldInfo in fields)
            {
                sw.WriteLine(fieldInfo.Name + ":" + fieldInfo.GetValue(this));
            }
        }
        Debug.Log("Saved Field Values to: " + filename);
    }

    public void LoadValues()
    {
        // load from file
        string filename = "Temp/SerializedObjects/" + name + "_" + GetInstanceID() + ".txt";

        if (!File.Exists(filename))
        {
            Debug.Log(filename + " not found.");
            return;
        }

        const BindingFlags flags = /*BindingFlags.NonPublic | */BindingFlags.Public | BindingFlags.Instance;

        using (StreamReader sr = new StreamReader(filename))
        {
            string line;
            string fieldName;
            string fieldValue;
            while ((line = sr.ReadLine()) != null)
            {
                fieldName = line.Split(':')[0];
                fieldValue = line.Split(':')[1];
                FieldInfo field = GetType().GetField(fieldName, flags);
                Type fieldType = field.FieldType;
                // set field value
                if (fieldType.IsEnum)
                {
                    field.SetValue(this, Enum.Parse(field.FieldType, fieldValue));
                }
                // is saved with single digit precision
                else if (fieldType.IsInstanceOfType(Vector3.one))
                {
                    float x, y, z;
                    string[] parts = fieldValue.Substring(1, fieldValue.Length - 2).Split(',');
                    x = float.Parse(parts[0]);
                    y = float.Parse(parts[1]);
                    z = float.Parse(parts[2]);
                    field.SetValue(this, new Vector3(x, y, z));
                }
                else
                {
                    try
                    {
                        field.SetValue(this, Convert.ChangeType(fieldValue, fieldType));
                    }
                    catch (Exception e)
                    {
                        // ignore
                        Debug.Log(e.Message);
                    }
                }
            }
        }

        Debug.Log("Loaded Field Values from: " + filename);
    }
}
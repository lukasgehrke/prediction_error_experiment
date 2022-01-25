using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class LoadXMLData : MonoBehaviour // the Class
{
   // public TextAsset GameAsset;

    public static List<Dictionary<string, string>> info = new List<Dictionary<string, string>>();
    public  Dictionary<string, string> obj;

    void Start()
    { //Timeline of the Level creator
        GetData();
    }

    public void GetData()
    {
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        FileStream fs = new FileStream("data.xml", FileMode.Open, FileAccess.Read);
       
        xmlDoc.Load(fs); // load the file.
        if(xmlDoc==null) Debug.Log("can not read");
        XmlNodeList parameterList = xmlDoc.GetElementsByTagName("parameter"); // array of the level nodes.

        foreach (XmlNode pInfo in parameterList)
        {
            XmlNodeList pcontent = pInfo.ChildNodes;
            obj = new Dictionary<string, string>(); // Create a object(Dictionary) to colect the both nodes inside the level node and then put into levels[] array.

            foreach (XmlNode pItens in pcontent) // levels itens nodes.
            {
                if (pItens.Name == "trial")
                {
                    obj.Add("trial", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "block")
                {
                    obj.Add("block", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "fixation")
                {
                    obj.Add("fixation", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "onset")
                {
                    obj.Add("onset", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "height")
                {
                    obj.Add("height", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "table_height")
                {
                    obj.Add("table_height", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "table_distance")
                {
                    obj.Add("table_distance", pItens.InnerText); // put this in the dictionary.
                }
                
                // TU Berlin new params
                if (pItens.Name == "volatility")
                {
                    obj.Add("volatility", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "volatility_level")
                {
                    obj.Add("volatility_level", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "vibroIntensity")
                {
                    obj.Add("vibroIntensity", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "vibroFeedbackCondition")
                {
                    obj.Add("vibroFeedbackCondition", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "emsFeedbackCondition")
                {
                    obj.Add("emsFeedbackCondition", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "vibro_feedback_duration")
                {
                    obj.Add("vibro_feedback_duration", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "width")
                {
                    obj.Add("width", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "current")
                {
                    obj.Add("current", pItens.InnerText); // put this in the dictionary.
                }
                if (pItens.Name == "pulseCount")
                {
                    obj.Add("pulseCount", pItens.InnerText); // put this in the dictionary.
                }
}
            info.Add(obj); // add whole obj dictionary in the levels[].
        }
    }
}
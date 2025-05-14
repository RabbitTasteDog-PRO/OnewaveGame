using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Newtonsoft.Json.Linq;

public class DataReader : MonoBehaviour
{
    public struct STDataReadInfo
    {
        public string dataPath;
        public Action<string> ACTION_READ_LINE;
    }

    public void ReadTextData(string dataName, Action<string> _ACTION_READ_LINE)
    {
        TextAsset targetFile = Resources.Load<TextAsset>(string.Format("Json/{0}", dataName));
        /// 패스 설정 
        string jsonFile = targetFile.text;
        if (string.IsNullOrEmpty(jsonFile))
        {
            return;
        }

        _ACTION_READ_LINE(jsonFile);
    }


    public void ReadCvsData(string dataName, Action<string[]> actionReadLine)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(Path.Combine("DataFile", $"{dataName}"));
        string[] lines = textAsset.text.Split('\n');

        actionReadLine(lines);
    }
}
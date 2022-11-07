using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

namespace GameManagement.Data
{
    public class JsonSaver
    {
        private static readonly string _filename = "saveData1.sav";

        public static string GetSaveFilename()
        {
            return Application.persistentDataPath + "/" + _filename;
        }

        public void Save(SaveData data)
        {
            data.hashValue = "";

            // generate the json data
            string json = JsonUtility.ToJson(data);

            // compute the hash and save it into the data structure
            string hashString = GetSHA256(json);
            data.hashValue = hashString;
            
            // generate the json containing both the data and the hash value
            json = JsonUtility.ToJson(data);

            string saveFilename = GetSaveFilename();

            Debug.Log("SAVING TO :"+saveFilename);
            FileStream filestream = new FileStream(saveFilename, FileMode.Create);

            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(json);
            }
        }

        public bool Load(SaveData data)
        {
            string loadFilename = GetSaveFilename();
            string json;

            FileStream filestream = new FileStream(loadFilename, FileMode.Open);

            if (File.Exists(loadFilename))
            {
                using (StreamReader reader = new StreamReader(filestream))
                {
                    json = reader.ReadToEnd();

                    if (CheckData(json))
                    {
                        Debug.Log("DATA CHECK OK!");
                        JsonUtility.FromJsonOverwrite(json, data);
                    }
                    else
                    {
                        Debug.Log("DATA HAVE BEEN TEMPERED WITH");
                        data = new SaveData();
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Delete()
        {
            File.Delete(GetSaveFilename());
        }

        private string GetSHA256(string text)
        {
            byte[] textToBytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed mySHA256 = new SHA256Managed();
            byte[] hashValue = mySHA256.ComputeHash(textToBytes);

            return GetTextStringFromHash(hashValue);
        }

        private bool CheckData(string json)
        {
            SaveData tempSaveData = new SaveData();
            Debug.Log("JSON IN "+json);
            
            JsonUtility.FromJsonOverwrite(json, tempSaveData);

            string oldHash = tempSaveData.hashValue;
            tempSaveData.hashValue = string.Empty;

            string tempJson = JsonUtility.ToJson(tempSaveData);
            string newHash = GetSHA256(tempJson);

            if (oldHash == newHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public string GetTextStringFromHash(byte[] hash)
        {
            string hexString = String.Empty;

            for (int i = 0; i < hash.Length; i++)
                hexString += hash[i].ToString("x2");

            return hexString;
        }
    }
}

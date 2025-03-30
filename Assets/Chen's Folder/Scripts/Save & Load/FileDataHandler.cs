using System;
using System.Collections.Generic;
using System.IO;
using Chen_s_Folder.Scripts.Save___Load.Data;
using UnityEngine;

namespace Chen_s_Folder.Scripts.Save___Load
{
    public class FileDataHandler
    {
        string _dataDirPath = "";
        string _dataFileName = "";

        private bool _useEncryption;
        private readonly string _encryptionCodeWord = "word";
        private readonly string _backupExtension = " .bak";

        public Dictionary<string, GameData> LoadAllProfiles()
        {
            Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;
                string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("This doesn't belong here");
                    continue;
                }
                GameData profileData = Load(profileId);
                if (profileData != null)
                {
                    profileDictionary.Add(profileId, profileData);
                }
                else
                {
                    Debug.Log("something feels odd about this file..");
                }
            }
            return profileDictionary;
        }
        //constructor
        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this._useEncryption = useEncryption;
            this._dataDirPath = dataDirPath;
            this._dataFileName = dataFileName;
        }

        public GameData Load(string profileId, bool allowRestoreFromBackup = true)
        {
            if (profileId == null)
            {
                return null;
            }
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    if (_useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }
                    //deserialize
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Failed to load data file. Trying rollback backup.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                    //if we hit this else block, it means the backup file is also corrupted
                    else
                    {
                        Debug.LogError("Error occured when trying to load file at path: " + fullPath + "and backup did not work.\n" + e);
                    }
                }
            }
            return loadedData;
        }

        public void Save(GameData data, string profileId)
        {
            if (profileId == null) { return; }
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            string backupFilePath = fullPath + _backupExtension;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
                //verify the newly saved file can be loaded and isn't corrupted
                GameData verifiedGameData = Load(profileId);
                //if it's verified - back it up
                if (verifiedGameData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file couldn't be verified and backup couldn't make it");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to the file: " + fullPath + "\n" + e);
            }
        }

        public string GetMostRecentProfileId()
        {
            string mostRecentProfileId = null;

            Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
            foreach (KeyValuePair<string, GameData> pair in profilesGameData)
            {
                string profileId = pair.Key;
                GameData gameData = pair.Value;

                //defensive check if we've encountered a null game data
                if (gameData == null)
                {
                    continue;
                }

                if (mostRecentProfileId == null)
                {
                    mostRecentProfileId = profileId;
                }
                else
                {
                    DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                    DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                    //the greater date time - the most recent
                    if (newDateTime > mostRecentDateTime)
                    {
                        mostRecentProfileId = profileId;
                    }
                }
            }

            return mostRecentProfileId;
        }

        public void Delete(string profileId)
        {
            if (profileId == null)
            {
                return;
            }
            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            try
            {
                if (File.Exists(fullPath))
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else
                {
                    Debug.LogWarning("Tried to delete the data, but couldn't find any data here: " + fullPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to delete profile data for this profile: " + profileId + "at path: " + fullPath + "\n" + e);
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }
            return modifiedData;
        }

        private bool AttemptRollback(string fullPath)
        {
            bool success = false;
            string backupFilePath = fullPath + _backupExtension;
            try
            {
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                }
                else
                {
                    throw new Exception("Tried to roll back but there's no backup file to rollback to.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to roll back to backup file at: " + backupFilePath + "\n" + e);
            }

            return success;
        }
    }
}

using Chen_s_Folder.Scripts.Save___Load.Data;

namespace Chen_s_Folder.Scripts.Save___Load
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void SaveData(GameData data);

    }
}

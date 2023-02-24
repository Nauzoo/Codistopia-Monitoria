[System.Serializable]
public class GameSave
{      
    public bool hasSave = false;

    public string[] happendEvents;

    public float[] playerPosition = new float[] { 0f, 0f };

    public float[] vickyPosition = new float[] { 0f, 0f };

    public int SavedScene = 2;

}

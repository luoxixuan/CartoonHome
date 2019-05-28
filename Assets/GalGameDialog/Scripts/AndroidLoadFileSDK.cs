using UnityEngine;

public class AndroidLoadFileSDK
{
    public static byte[] LoadFile(string path)
    {

        AndroidJavaClass m_AndroidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject m_AndroidJavaObject = null;
        if (m_AndroidJavaClass != null)
        {
            m_AndroidJavaObject = m_AndroidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        Debug.Log("DialogManage::LoadDialogs File path " + path);
        byte[] s = m_AndroidJavaObject.Call<byte[]>("LoadFile", path);
        return s;
    }

    public static string LoadTextFile(string path)
    {
        byte[] bytes = LoadFile(path);
        if (bytes == null)
            return "Error bytes=null";

        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    public static AssetBundle LoadAssetBundle(string path)
    {
        byte[] bytes = LoadFile(path);
        if (bytes == null)
            return null;

        return AssetBundle.LoadFromMemory(bytes);
    }
}

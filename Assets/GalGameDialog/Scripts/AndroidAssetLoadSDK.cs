using UnityEngine;

public class AndroidAssetLoadSDK
{
    public static byte[] LoadFile(string path)
    {
        Debug.Log("DialogManage::LoadDialogs File path " + path);
        AndroidJavaClass m_AndroidJavaClass = new AndroidJavaClass("com.ihaiu.assetloadsdk.AssetLoad");
        return m_AndroidJavaClass.CallStatic<byte[]>("loadFile", path);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePath
{
    static bool BasePathChecked = false;

    static string basePath()
    {
        return 
            System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ShopDemo"
             );
    }

    static string GetBasePath()
    {
        string result = basePath();
        if(!BasePathChecked && !System.IO.Directory.Exists(result))
        {
            try {
                System.IO.Directory.CreateDirectory(result);
                BasePathChecked = true;
            } catch(Exception e)
            {
                Debug.Log(e.Message);
            }
            
        }
        return result;
    }

    public static string Shop(string name)
    {
        return System.IO.Path.Combine(GetBasePath(), name + ".json");
    }

    public static string Inventory => System.IO.Path.Combine(GetBasePath(), "Inventory.json"); 
}

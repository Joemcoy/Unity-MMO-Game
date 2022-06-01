using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
using Devdog.InventoryPro;

public static class GenerateBundleInventory
{
    [MenuItem("Tools/Convert ItemDatabase to bundle protocol")]
    static void Convert()
    {
        var path = AssetDatabase.GetAssetPath(ItemManager.database);
        var generated = path.Replace(Path.GetFileNameWithoutExtension(path), "GeneratedItemDatabase");

        if (File.Exists(generated))
            File.Delete(generated);

        if (AssetDatabase.CopyAsset(path, generated))
        {
            var database = AssetDatabase.LoadAssetAtPath<ItemDatabase>(generated);
            SetBundlePath(database.items, "items");
            SetBundlePath(database.statDefinitions, "stats");
            SetBundlePath(database.categories, "categories");
            SetBundlePath(database.currencies, "currencies");
            SetBundlePath(database.equipmentTypes, "equiptypes");

            database.items = new InventoryItemBase[0];
            database.categories = new ItemCategory[0];
            database.currencies = new CurrencyDefinition[0];
            database.equipmentTypes = new EquipmentType[0];
            AssetDatabase.RemoveUnusedAssetBundleNames();
            
            ItemManager.instance.sceneItemDatabase = database;
        }
        else
            Debug.LogError("Failed to generate a copy of asset!");
    }

    static void SetBundlePath<TObjects>(TObjects[] values, string partial) where TObjects : Object
    {
        foreach (var value in values)
        {
            var itempath = AssetDatabase.GetAssetPath(value);
            var name = value.name;
            var type = value.GetType();
            var member = type.GetMember("name");

            if (member.Length > 0)
            {
                if (member[0].MemberType == System.Reflection.MemberTypes.Field)
                    name = type.GetField("name").GetValue(value) as string;
                else if(member[0].MemberType == System.Reflection.MemberTypes.Property)
                    name = type.GetProperty("name").GetValue(value, null) as string;
            }
            name = name.ToLower().Replace(' ', '_');
            AssetImporter.GetAtPath(itempath).assetBundleName = string.Format("inventory/{0}/{1}", partial, name);
        }
    }
}
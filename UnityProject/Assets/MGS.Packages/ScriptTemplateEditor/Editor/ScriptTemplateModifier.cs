/*************************************************************************
 *  Copyright © 2021 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ScriptTemplateModifier.cs
 *  Description  :  Modifier for script template.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  2/12/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace MGS.ScriptTemplate.Editors
{
    public class ScriptTemplateModifier : UnityEditor.AssetModificationProcessor
    {
        #region Field and Property
        private const string SCRIPT_EXTENSIONS = ".cs$|.js$|.boo$|.shader$|.compute$";
        private const string META_EXTENSION = ".meta";

        private const string COPYRIGHT_YEAR = "#COPYRIGHTYEAR#";
        private const string CREATE_DATE = "#CREATEDATE#";
        #endregion

        #region Private Method
        private static void OnWillCreateAsset(string metaPath)
        {
            var assetPath = metaPath.Replace(META_EXTENSION, string.Empty);
            if (Regex.IsMatch(Path.GetExtension(assetPath), SCRIPT_EXTENSIONS))
            {
                try
                {
                    var content = File.ReadAllText(assetPath);
                    content = content.Replace(COPYRIGHT_YEAR, DateTime.Now.Year.ToString());
                    content = content.Replace(CREATE_DATE, DateTime.Now.ToShortDateString());
                    File.WriteAllText(assetPath, content);
                    AssetDatabase.Refresh();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tabler.Logic.Helper;

namespace tabler.Logic.Classes
{
    public class TranslationManager
    {
        public const string COLUMN_MODNAME = "Mod";
        public const string COLUMN_IDNAME = "ID";
        public const string STRINGTABLE_NAME = "stringtable.xml";

        private readonly FileInfo _fiExcelFile;
        public TranslationComponents TranslationComponents;

        public TranslationManager()
        {
        }

        public TranslationManager(FileInfo fiExcelFile)
        {
            _fiExcelFile = fiExcelFile;
        }


        private List<string> PrepareHeaders(List<string> headers, bool insertMod)
        {
            headers = headers.OrderBy(l => l).ToList();

            if (headers.Any(x => x.ToLowerInvariant() == "english"))
            {
                headers.Remove("English");
                headers.Insert(0, "English");
            }

            headers.Insert(0, COLUMN_IDNAME);
            if (insertMod)
            {
                headers.Insert(0, COLUMN_MODNAME);
            }


            //remove duplicates
            headers = headers.Distinct().ToList();

            return headers;
        }


        private TranslationComponents GetTranslationComponents(DirectoryInfo lastPathToDataFiles, bool insertMod)
        {
            var allStringtableFiles = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories).ToList();

            if (allStringtableFiles.Any() == false)
            {
                return null;
            }

            var xh = new XmlHelper();
            var transComp = xh.ParseXmlFiles(allStringtableFiles);

            transComp.Headers = PrepareHeaders(transComp.Headers, insertMod);

            TranslationComponents = transComp;
            return TranslationComponents;
        }


        private static bool SaveModInfosToXml(DirectoryInfo lastPathToDataFiles, List<ModInfoContainer> lstModInfos)
        {
            //if going through mods instead of files, 
            // we could create files
            // too tired :D ->  TODO
            var filesByNameInDirectory = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories);

            var xh = new XmlHelper();
            xh.UpdateXmlFiles(filesByNameInDirectory, lstModInfos);

            return true;
        }


        public TranslationComponents GetGridData(DirectoryInfo lastPathToDataFiles)
        {
            return GetTranslationComponents(lastPathToDataFiles, false);
        }

        public bool SaveGridData(DirectoryInfo lastPathToDataFiles, List<ModInfoContainer> lstModInfos)
        {
            return SaveModInfosToXml(lastPathToDataFiles, lstModInfos);
        }
    }
}
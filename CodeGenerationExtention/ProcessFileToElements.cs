using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GeoCybernetica.CodeGenerationExtention
{
    /// <summary>
    /// Добывает из файла открытого нужную инфраструктуру.
    /// </summary>
    public class ProcessFileToElements
    {
        #region Public Constructors

        public ProcessFileToElements(string file)
        {
            Classes = new List<CodeClass>();
            Structs = new List<CodeStruct>();
            Interfaces = new List<CodeInterface>();
            Enums = new List<CodeEnum>();

            ParseClassFile(file);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<CodeClass> Classes { get; private set; }

        public List<CodeEnum> Enums { get; private set; }

        public List<CodeInterface> Interfaces { get; private set; }

        public List<CodeStruct> Structs { get; private set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Служебный метод, просто для практики.
        /// </summary>
        /// <param name="element"></param>
        [Conditional("DEBUG")]
        private void DebugFindClass(CodeClass element)
        {
            if (element != null)
            {
                foreach (var property in element.Members.Cast<CodeElement>())
                {
                    var prop = property as CodeProperty;
                    if (prop != null)
                    {
                        //   prop.Name.Split('.').LastOrDefault();
                    }
                }
            }
        }

        private void DetectElementsInFile(CodeElements elements)
        {
            foreach (CodeElement element in elements)
            {
                if (element is CodeClass)
                {
                    Classes.Add(element as CodeClass);
                }

                if (element is CodeEnum)
                {
                    Enums.Add(element as CodeEnum);
                }

                if (element is CodeStruct)
                {
                    Structs.Add(element as CodeStruct);
                }

                if (element is CodeInterface)
                {
                    Interfaces.Add(element as CodeInterface);
                }

                DetectElementsInFile(element.Children);
            }
        }
        private void ParseClassFile(string file)
        {
            var dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            ProjectItem projectItem = dte.Solution.FindProjectItem(file);
            //if (!VsShellUtilities.IsDocumentOpen(ServiceProvider.GlobalProvider, filePath,
            //VSConstants.LOGVIEWID_Primary, out hierarchy, out itemId, out frame))
            //{
            //    VsShellUtilities.OpenDocument(ServiceProvider.GlobalProvider, filePath,
            //        VSConstants.LOGVIEWID_Primary, out hierarchy, out itemId, out frame);
            //}
 
            FileCodeModel codeModel = projectItem.FileCodeModel;
            DetectElementsInFile(codeModel.CodeElements);
        }

        #endregion Private Methods
    }
}
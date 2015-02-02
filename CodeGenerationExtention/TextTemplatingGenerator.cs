using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace GeoCybernetica.CodeGenerationExtention
{
    /// <summary>
    /// Bridge between t4 script and code file (source for parametrs).
    /// </summary>
    public class TextTemplatingGenerator : ITextTemplatingCallback
    {

        #region Private Fields

        private readonly StringBuilder _outputError = new StringBuilder();

        #endregion Private Fields

        #region Public Properties

        public string OutputError { get { return _outputError.ToString(); } }

        #endregion Public Properties

        #region Public Methods

        public void ErrorCallback(bool warning, string message, int line, int column)
        {
            _outputError.AppendLine(string.Format("{3}: {0}. Line {1}, column {2}", message, line, column, warning ? "Warning" : "Error"));
        }

        public void ProcessFileWithTemplate(string name, string fullPathToScript)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(fullPathToScript)) 
                return;

            try
            {
                ITextTemplating textTemplatingService = ServiceProvider.GlobalProvider.GetService(typeof(STextTemplating)) as ITextTemplating;
                
                if (textTemplatingService != null)
                {

                    var templatingComponents = (ITextTemplatingComponents)textTemplatingService;
                    DefineSessionParametrs(name, textTemplatingService);
                    var output = textTemplatingService.ProcessTemplate(fullPathToScript, File.ReadAllText(fullPathToScript), this, templatingComponents.Hierarchy);
                    Clipboard.SetText(output);
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("Can't ProcessFileWithTemplate( {0} , {1} )", name, fullPathToScript);
            }
        }

        public void SetFileExtension(string extension)
        {

        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private static void DefineSessionParametrs(string name,
            ITextTemplating textTemplatingService)
        {
            var fileParser = new ProcessFileToElements(name);
            var host = textTemplatingService as ITextTemplatingSessionHost;
            if (host != null)
            {
                host.Session = host.CreateSession();

                if (fileParser.Classes.Any())
                {
                    host.Session["codeClass"] = fileParser.Classes.First();
                    host.Session["codeClasses"] = fileParser.Classes;
                }
                if (fileParser.Interfaces.Any())
                {
                    host.Session["codeInterface"] = fileParser.Interfaces.First();
                    host.Session["codeInterfaces"] = fileParser.Interfaces;
                }

                if (fileParser.Structs.Any())
                {
                    host.Session["codeStruct"] = fileParser.Structs.First();
                    host.Session["codeStructs"] = fileParser.Structs;
                }

                if (fileParser.Enums.Any())
                {
                    host.Session["codeEnum"] = fileParser.Enums.First();
                    host.Session["codeEnums"] = fileParser.Enums;
                }
            }
            else
            {
                Debug.WriteLine("Can't get ITextTemplatingSessionHost");
            }
        }

        #endregion Private Methods
    }
}

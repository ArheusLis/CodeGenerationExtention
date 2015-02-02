using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using GeoCybernetica.CodeGenerationExtention.Files;
using Microsoft.VisualStudio.Shell;

namespace GeoCybernetica.CodeGenerationExtention
{
   /// <summary>
    /// This class implements a very specific type of command: this command will count the
    /// number of times the user has clicked on it and will change its text to show this count.
    /// </summary>
    internal class DynamicScriptCommand : OleMenuCommand
    {
        private string _fileName;

        /// <summary>
        /// This is the function that is called when the user clicks on the menu command.
        /// It will check that the selected object is actually an instance of this class and
        /// increment its click counter.
        /// </summary>
        private static void ClickCallback(string fileName, Func<string> getClass, Action<string> putMessage)
        {
            try
            {
                string className = getClass.Invoke();
                if (className == string.Empty)
                {
                    putMessage("File is empty. Script can't run.");
                    return;
                }

                if (className.EndsWith("*"))
                    className = className.Remove(className.Length - 2, 1);

                var generator = new TextTemplatingGenerator();
                generator.ProcessFileWithTemplate(className, Path.Combine(PackageEnvironment.ScriptDirectoryFullPath, fileName));

                putMessage(generator.OutputError == string.Empty
                    ? "Code generation complite. Check Clipboard."
                    : generator.OutputError);
            }
            catch (Exception exception)
            {
                putMessage(exception.GetType().ToString());
                Debug.WriteLine("Не удалось сгенерировать.");
            }
      
        }
        
        public DynamicScriptCommand(CommandID id, string fullFileName, Func<string> getClass, Action<string> putMessage) :
            base((_, __) => ClickCallback(fullFileName, getClass, putMessage), id, fullFileName)
        {
            _fileName = new FileInfo(fullFileName).Name;
        }

        /// <summary>
        /// If a command is defined with the TEXTCHANGES flag in the VSCT file and this package is
        /// loaded, then Visual Studio will call this property to get the text to display.
        /// </summary>
        public override string Text
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
    }
}

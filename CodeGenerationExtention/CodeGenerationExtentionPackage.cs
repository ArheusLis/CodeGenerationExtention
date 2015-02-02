using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using GeoCybernetica.CodeGenerationExtention.Files;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GeoCybernetica.CodeGenerationExtention
{
    /// <summary>
    /// This is the class that implements the package. This is the class that Visual Studio will create
    /// when one of the commands will be selected by the user, and so it can be considered the main
    /// entry point for the integration with the IDE.
    /// Notice that this implementation derives from Microsoft.VisualStudio.Shell.Package that is the
    /// basic implementation of a package provided by the Managed Package Framework (MPF).
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidMenuAndCommandsPkg_string)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ComVisible(true)]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    public sealed class MenuCommandsPackage : Package
    {
        #region Private Fields

        private readonly List<int> _slots = new List<int>()
        {
            PkgCmdIDList.cmdCommand1,
            PkgCmdIDList.cmdCommand2,
            PkgCmdIDList.cmdCommand3,
            PkgCmdIDList.cmdCommand4,
            PkgCmdIDList.cmdCommand5,
            PkgCmdIDList.cmdCommand6,
            PkgCmdIDList.cmdCommand7,
            PkgCmdIDList.cmdCommand8,
            PkgCmdIDList.cmdCommand9,
            PkgCmdIDList.cmdCommand10
        };

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default constructor of the package. This is the constructor that will be used by VS
        /// to create an instance of your package. Inside the constructor you should do only the
        /// more basic initializazion like setting the initial value for some member variable. But
        /// you should never try to use any VS service because this object is not part of VS
        /// environment yet; you should wait and perform this kind of initialization inside the
        /// Initialize method.
        /// </summary>
        public MenuCommandsPackage()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public string GetCurrentClassFileName()
        {
            try
            {
                // Show a Message Box to prove we were here
                var uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                IVsWindowFrame frame;
                string data;
                object point;
                uiShell.GetCurrentBFNavigationItem(out frame, out data, out point);
                if (frame == null)
                {
                    OutputCommandString("Please, open file");
                    return string.Empty;
                }

                object obj;
                frame.GetProperty((int)__VSFPROPID.VSFPROPID_Caption, out obj);
                object fullpath;
                frame.GetProperty((int)__VSFPROPID.VSFPROPID_pszMkDocument, out fullpath);

                var caption = obj.ToString();
                if (caption.EndsWith("*"))
                {
                    OutputCommandString("Please, save file before start.");
                }
                return fullpath.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Initialization of the package; this is the place where you can put all the initialization
        /// code that relies on services provided by Visual Studio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            
            // Now get the OleCommandService object provided by the MPF; this object is the one
            // responsible for handling the collection of commands implemented by the package.
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CreateList(mcs);
                CreateOpenScriptFolderCommand(mcs);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void CreateList(OleMenuCommandService mcs)
        {
            var files = Directory.GetFiles(PackageEnvironment.ScriptDirectoryFullPath, "*.tt");
            for (int index = 0; index < files.Length; index++)
            {
                string file = files[index];
                try
                {
                    var id = new CommandID(GuidList.guidMenuAndCommandsCmdSet, _slots[index]);
                    var command = new DynamicScriptCommand(id, file, GetCurrentClassFileName, OutputCommandString) { Visible = true };
                    mcs.AddCommand(command);
                }
                catch (Exception exception)
                {
                    OutputCommandString(string.Format("Can't add t4 file {0}. Exception: {1}", file, exception.GetType()));
                }
            }
        }

        private void CreateOpenScriptFolderCommand(OleMenuCommandService mcs)
        {
            var id = new CommandID(GuidList.guidMenuAndCommandsCmdSet, PkgCmdIDList.cmdidOpenFolder);
            var command = new OleMenuCommand(OpenStriptFolder, id);
            mcs.AddCommand(command);
        }

        private void OpenStriptFolder(object sender, EventArgs e)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = {UseShellExecute = true, FileName = @"explorer", Arguments = PackageEnvironment.ScriptDirectoryFullPath}
                };

                process.Start();
            }
            catch (Exception ex)
            {
                OutputCommandString(ex.GetType().ToString());
            }
        }

        /// <summary>
        /// This function prints text on the debug ouput and on the generic pane of the 
        /// Output window.
        /// </summary>
        /// <param name="text"></param>
        private void OutputCommandString(string text)
        {
            // Build the string to write on the debugger and Output window.
            var outputText = new StringBuilder();
            outputText.AppendFormat("CodeGenerator: {0}\n", text);

            var windowPane = (IVsOutputWindowPane)GetService(typeof(SVsGeneralOutputWindowPane));
            if (null == windowPane)
            {
                Debug.WriteLine("Failed to get a reference to the Output window General pane");
                return;
            }
            if (ErrorHandler.Failed(windowPane.OutputString(outputText.ToString())))
            {
                Debug.WriteLine("Failed to write on the Output window");
            }
        }

        #endregion Private Methods
    }
}

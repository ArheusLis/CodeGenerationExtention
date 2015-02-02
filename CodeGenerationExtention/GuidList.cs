using System;

namespace GeoCybernetica.CodeGenerationExtention
{
    /// <summary>
    /// This class is used only to expose the list of Guids used by this package.
    /// This list of guids must match the set of Guids used inside the VSCT file.
    /// </summary>
    public static class GuidList
    {
        public const string guidCodeGenerationExtentionPkgString = "449a16d6-7fe1-4032-a9c0-5ff300ca9e07";
        public const string guidCodeGenerationExtentionCmdSetString = "29e0b056-4d8d-4fba-a767-e7b7d3de13b7";
        public const string guidToolWindowPersistanceString = "5e83dab3-6040-46d4-8f86-5f67fc56060e";

        public const string guidCodeGenerationExtentionCmdShowMenuString = "6d83dab3-6040-45d4-8fa6-5f67fc56020e";

        public static readonly Guid guidCodeGenerationExtentionCmdSet = new Guid(guidCodeGenerationExtentionCmdSetString);

        public static readonly Guid guidCodeGenerationExtentionCmdShowMenu = new Guid(guidCodeGenerationExtentionCmdShowMenuString);

               // Now define the list of guids as public static members.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Guid guidMenuAndCommandsPkg = new Guid("{3C7C5ABE-82AC-4A37-B077-0FF60E8B1FD3}");
        public const string guidMenuAndCommandsPkg_string = "3C7C5ABE-82AC-4A37-B077-0FF60E8B1FD3";

        public static readonly Guid guidMenuAndCommandsCmdSet = new Guid("{b2252b1a-25b3-4ec3-8826-d67cd5526653}");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Guid guidGenericCmdBmp = new Guid("{0A4C51BD-3239-4370-8869-16E0AE8C0A46}");
    }
}
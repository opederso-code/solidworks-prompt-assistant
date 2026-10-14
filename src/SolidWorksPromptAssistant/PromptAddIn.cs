using System.Runtime.InteropServices;
using CADBooster.SolidDna;

namespace SolidWorksPromptAssistant
{
    [Guid("8015517F-4519-4C5A-9D23-49229723C56C")]
    [ComVisible(true)]
    public sealed class PromptAddIn : SolidAddIn
    {
        public override void PreConnectToSolidWorks() { }

        public override void PreLoadPlugIns()
        {
            PlugInIntegration.AddPlugInToLoad<PromptPlugin>();
        }

        public override void ApplicationStartup() { }
    }
}

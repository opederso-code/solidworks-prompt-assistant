using CADBooster.SolidDna;
using System;
using System.Windows.Forms;

namespace SolidWorksPromptAssistant
{
    public sealed class PromptPlugin : SolidPlugIn
    {
        private PromptWindow? _window;

        public override string AddInTitle => "SOLIDWORKS Prompt Assistant";
        public override string AddInDescription => "Preview and run typed commands in SOLIDWORKS";

        public override void ConnectedToSolidWorks()
        {
            _window = new PromptWindow();
            _window.Show();
        }

        public override void DisconnectedFromSolidWorks()
        {
            if (_window != null && !_window.IsDisposed)
                _window.Close();
            _window = null;
        }
    }
}

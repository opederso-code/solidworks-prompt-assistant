using System;
using System.Drawing;
using System.Windows.Forms;
using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;

namespace SolidWorksPromptAssistant
{
    internal sealed class PromptWindow : Form
    {
        private readonly TextBox _input = new TextBox { Dock = DockStyle.Top };
        private readonly TextBox _output = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
        private readonly Button _run = new Button { Text = "Preview / run", Dock = DockStyle.Bottom, Height = 36 };

        public PromptWindow()
        {
            Text = "SOLIDWORKS Prompt Assistant";
            Size = new Size(480, 300);
            Controls.Add(_output);
            Controls.Add(_run);
            Controls.Add(_input);
            _output.Text = "Try: help, active document, or set property Project = Prototype";
            _run.Click += (_, __) => RunPrompt();
            _input.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; RunPrompt(); } };
        }

        private void RunPrompt()
        {
            var command = CommandParser.Parse(_input.Text);
            if (command.Kind == CommandKind.Invalid)
            {
                _output.Text = command.Description;
                return;
            }
            if (command.Kind == CommandKind.Help)
            {
                _output.Text = "Commands:\r\n  active document\r\n  set property <name> = <value>";
                return;
            }
            try
            {
                // SolidDNA owns the connection; use its underlying SOLIDWORKS object.
                var application = SolidWorksEnvironment.IApplication?.UnsafeObject;
                var document = application?.ActiveDoc as ModelDoc2;
                if (document == null)
                {
                    _output.Text = "Open a SOLIDWORKS document first.";
                    return;
                }
                if (command.Kind == CommandKind.ActiveDocument)
                {
                    _output.Text = $"Active document: {document.GetTitle()}";
                    return;
                }
                var preview = $"Set custom property '{command.Name}' to '{command.Value}' on '{document.GetTitle()}'?";
                _output.Text = preview;
                if (MessageBox.Show(this, preview, "Confirm change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                var properties = document.Extension.CustomPropertyManager[""];
                properties.Add3(command.Name, (int)SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText,
                    command.Value, (int)SolidWorks.Interop.swconst.swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
                _output.Text = $"Updated '{command.Name}'. Save the document in SOLIDWORKS to keep the change.";
            }
            catch (Exception ex)
            {
                _output.Text = $"Command failed: {ex.Message}";
            }
        }
    }
}

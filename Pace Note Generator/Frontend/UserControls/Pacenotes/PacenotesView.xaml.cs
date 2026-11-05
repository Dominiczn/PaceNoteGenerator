using Pace_Note_Generator.Backend;
using System.Windows.Controls;

namespace Pace_Note_Generator.Frontend.UserControls.Pacenotes
{
    public partial class PacenotesView : UserControl
    {
        List<Pacenote> pacenotes = new List<Pacenote>();

        public PacenotesView(List<Pacenote> pacenotes)
        {
            InitializeComponent();
            this.pacenotes = pacenotes;

        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
    }
}

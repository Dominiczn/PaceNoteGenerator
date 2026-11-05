using Pace_Note_Generator.Backend;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pace_Note_Generator.Frontend.UserControls.Pacenotes
{
    public partial class PacenotesView : UserControl
    {
        private List<Pacenote> pacenotes = new List<Pacenote>();

        private readonly Dictionary<Key, string> hazardKeys = new Dictionary<Key, string>()
        {
            {Key.D1, "Blind" },
            {Key.D2, "Crest" },
            {Key.D3, "Jump" },
            {Key.D4, "Narrows" },
            {Key.D5, "Junction" },
            {Key.D6, "Off Camber" },
            {Key.D7, "Gravel" },
            {Key.D8, "Tarmac" },
            {Key.D9, "Cut" },
            {Key.D0, "Dont Cut" },
            {Key.Back, "Unconfirmed" },
        };

        public PacenotesView(List<Pacenote> pacenotes)
        {
            InitializeComponent();
            this.pacenotes = pacenotes;
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {

            }

            else if (e.Key == Key.Left)
            {

            }
        }
    }
}

using Pace_Note_Generator.Backend;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pace_Note_Generator.Frontend.UserControls.Pacenotes
{
   
    public partial class PacenoteCard : UserControl
    {
        public PacenoteCard()
        {
            InitializeComponent();
        }

        public void DisplayPacenote(Pacenote pacenote)
        {
            if (pacenote.Direction == Direction.Straight)
            {
                ImgCorner.Visibility = Visibility.Hidden;
                TxtStraight.Visibility = Visibility.Visible;
                ImgStraightBorder.Visibility = Visibility.Visible;
                TxtStraight.Text = $"{pacenote.StraightLength}";
            }
            else
            {
                ImgStraightBorder.Visibility = Visibility.Hidden;
                TxtStraight.Visibility = Visibility.Hidden;
                ImgCorner.Visibility = Visibility.Visible;
                ImgCorner.Source = new BitmapImage(new Uri($"/Frontend/UserControls/Images/{pacenote.CornerSeverity}{pacenote.Direction}.png", UriKind.Relative));
            }
        }
    }
}

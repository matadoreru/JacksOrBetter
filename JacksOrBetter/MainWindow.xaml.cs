using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JacksOrBetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();
        Baralla baralla = new Baralla();
        Ma ma = new Ma();
        Carta cartaRandom;
        bool cartesMostrades = false;
        int nRondaExtra = 0;
        bool cartesMostrades2Ronda = false;
        bool haGuanyat = false;
        bool segondaRonda = false;

        int ganancies;

        public MainWindow()
        {
            InitializeComponent();
            btnDuplicar.Visibility = Visibility.Collapsed;
            btnSemiDuplicar.Visibility = Visibility.Collapsed;
            tbExtra.Visibility = Visibility.Collapsed;
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            int indexCarta = gridMa.Children.IndexOf(img);
            Carta carta = ma[indexCarta];
            if(segondaRonda)
            {
                carta.CaraAmunt = true;
                ActualitzarGridImage(gridMa);
                CheckHaGuanyat2n(carta);
                tbCredito.Text = (Convert.ToInt32(tbCredito.Text) + ganancies).ToString();
                nRondaExtra++;
                if (ganancies <= 0 || nRondaExtra == 4)
                {
                    ResetGame(gridMa);
                    segondaRonda = false;
                    haGuanyat = false;
                    nRondaExtra = 0;
                    tbApostar.Text = "0";
                }
            }
            else
            {
                if (carta.CaraAmunt)
                {
                    img.Opacity = 0.5;
                    carta.CaraAmunt = false;
                }
                else
                {
                    carta.CaraAmunt = true;
                    img.Opacity = 1;
                }
            }
        }

        private void btnRepartir_Click(object sender, RoutedEventArgs e)
        {
            // Mira si el jugador no ha apostat diners
            if(Convert.ToInt32(tbApostar.Text) == 0)
            {

            } 
            else
            {
                // Si encara no s'han mostrat les cartes
                if (!cartesMostrades)
                {
                    baralla = new Baralla();
                    baralla.Barrejar();
                    ma = new Ma();
                    AfegirCartesMa(true);
                    ActualitzarGridImage(gridMa);
                    cartesMostrades = true;
                }
                else
                {
                    #region Posar noves cartes
                    int nElim = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        Carta carta = ma[i];
                        if (!carta.CaraAmunt)
                        {
                            Carta cartaNova = baralla.Roba();
                            cartaNova.CaraAmunt = true;
                            ma[i] = cartaNova;
                        }
                    }
                    ActualitzarGridImage(gridMa);
                    #endregion

                    ganancies = CheckHaGuanyat1r(ma);
                    if (haGuanyat)
                    {
                        tbExtra.Visibility = Visibility.Visible;
                        btnDuplicar.Visibility = Visibility.Visible;
                        btnSemiDuplicar.Visibility = Visibility.Visible;
                    }
                    cartesMostrades = false;
                    tbApostar.Text = "0";
                }
            }       
        }

        /// <summary>
        /// Mira si ha guanyat el jugador i mostra el resultat
        /// </summary>
        /// <param name="ma">Pasar la ma del jugador</param>
        private int CheckHaGuanyat1r(Ma ma)
        {
            int ganancies = 0;
            if (ma.HiHaEscalaReialDeColor)
            {
                if (Convert.ToInt32(tbApostar.Text) == 5)
                    ganancies = 4000;
                else
                    ganancies = 250 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaEscalaDeColor)
            {
                ganancies = 1 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaPoker)
            {
                ganancies = 25 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaFull)
            {
                ganancies = 9 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaEscalaDeColor)
            {
                ganancies = 1 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaEscala)
            {
                ganancies = 4 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaTrio)
            {
                ganancies = 3 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            else if (ma.HiHaDobleParella)
            {
                ganancies = 2 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;

            }
            else if (ma.HiHaParellaMinima(Valor.Jota))
            {
                ganancies = 1 * Convert.ToInt32(tbApostar.Text);
                haGuanyat = true;
            }
            if (haGuanyat)
            {
                MessageBox.Show("Has guanyat! Cantidad --> " + ganancies + "\nTotal: " + tbCredito.Text);
            }
            else
            {
                MessageBox.Show("Has perdut! Cantidad --> " + tbApostar.Text + "\nTotal: " + tbCredito.Text);
            }
            gridMa.Children.Clear();
            return ganancies;
        }

        /// <summary>
        /// Mira si ha guanyat la segona ronda extra
        /// </summary>
        /// <param name="cartaTriada">Carta que ha triat el jugador</param>
        /// <returns></returns>
        private int CheckHaGuanyat2n(Carta cartaTriada)
        {
            int compare = cartaTriada.CompareTo(cartaRandom);
            if (compare == 0)
            {
                ganancies = 0;
                MessageBox.Show("Has empetat, ronda cancelada, ni guanyes, ni perds diners");
            }
            else if(compare > 0)
            {
                ganancies = Convert.ToInt32(tbApostar.Text) * 2;
                MessageBox.Show("Has guanyat!, dupliques els teus diners guanyats!");
            }
            else
            {
                ganancies = -Convert.ToInt32(tbApostar.Text);
                MessageBox.Show("Has perdut. Perds els teus diners apostats i es cancela la ronda extra.");
            }
            return ganancies;
        }

        /// <summary>
        /// Afegeix 5 cartes a la ma
        /// </summary>
        /// <param name="caraAmunt"></param>
        private void AfegirCartesMa(bool caraAmunt)
        {
            if(caraAmunt)
            {
                for (int i = 0; i < 5; i++)
                {
                    ma.Afegeix(baralla.Roba());
                    ma[i].CaraAmunt = true;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    ma.Afegeix(baralla.Roba());
                }
            }
            
        }

        /// <summary>
        /// Actualitza les imatges del grid amb les noves cartes de la ma
        /// </summary>
        /// <param name="gridMa"></param>
        private void ActualitzarGridImage(Grid gridMa)
        {
            gridMa.Children.Clear();
            for (int i = 0; i < 5; i++)
            {
                Image img = new Image();
                if (ma[i].CaraAmunt)
                    img.Source = (ImageSource)FindResource(ma[i].ClauImatge);
                else
                    img.Source = (ImageSource)FindResource("Joker2");
                img.Margin = new Thickness(5);
                img.MouseDown += Img_MouseDown;
                gridMa.Children.Add(img);
                Grid.SetColumn(img, i);
            }
        }

        /// <summary>
        /// Resetea baralla, ma y grid
        /// </summary>
        /// <param name="gridMa"></param>
        private void ResetGame(Grid gridMa)
        {
            baralla = new Baralla();
            baralla.Barrejar();
            ma = new Ma();
            gridMa.Children.Clear();
            btnRepartir.IsEnabled = true;
            btnDuplicar.IsEnabled = true;
            btnSemiDuplicar.IsEnabled = true;
            tbExtra.Visibility = Visibility.Collapsed;
            btnDuplicar.Visibility = Visibility.Collapsed;
            btnSemiDuplicar.Visibility = Visibility.Collapsed;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            tbExtra.Visibility = Visibility.Collapsed;
            btnDuplicar.Visibility = Visibility.Collapsed;
            btnSemiDuplicar.Visibility = Visibility.Collapsed;
            if(segondaRonda)
            {
                tbApostar.Text = "0";
                ResetGame(gridMa);
                segondaRonda = false;
            }
            if(haGuanyat)
            {
                tbCredito.Text = (Convert.ToInt32(tbCredito.Text) + ganancies).ToString();
                haGuanyat = false;
            }
            else
            {
                if (Convert.ToInt32(tbApostar.Text) + Convert.ToInt32(btn.Tag) == 5)
                {
                    tbCredito.Text = Convert.ToString(Convert.ToInt32(tbCredito.Text) - Convert.ToInt32(btn.Tag));
                    tbApostar.Text = Convert.ToString(Convert.ToInt32(btn.Tag) + Convert.ToInt32(tbApostar.Text));
                    btnRepartir_Click(sender, e);
                }
                else if (Convert.ToInt32(tbApostar.Text) + Convert.ToInt32(btn.Tag) < 5)
                {
                    tbCredito.Text = Convert.ToString(Convert.ToInt32(tbCredito.Text) - Convert.ToInt32(btn.Tag));
                    tbApostar.Text = Convert.ToString(Convert.ToInt32(btn.Tag) + Convert.ToInt32(tbApostar.Text));
                }
            }   
        }

        #region DragAndDrop
        private void tbApostar_Drop(object sender, DragEventArgs e)
        {     
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                TextBlock tb = (TextBlock)sender;
                String cadena = e.Data.GetData(DataFormats.Text).ToString();
                if (Convert.ToInt32(tbApostar.Text) + Convert.ToInt32(cadena) == 5)
                {
                    tbCredito.Text = Convert.ToString(Convert.ToInt32(tbCredito.Text) - Convert.ToInt32(cadena));
                    tbApostar.Text = Convert.ToString(Convert.ToInt32(cadena) + Convert.ToInt32(tbApostar.Text));
                    btnRepartir_Click(sender, e);
                }
                else if (Convert.ToInt32(tbApostar.Text) + Convert.ToInt32(cadena) < 5)
                {
                    tbCredito.Text = Convert.ToString(Convert.ToInt32(tbCredito.Text) - Convert.ToInt32(cadena));
                    tbApostar.Text = Convert.ToString(Convert.ToInt32(cadena) + Convert.ToInt32(tbApostar.Text));
                }
            }
        } 

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop((Image)sender, ((Image)sender).Tag, DragDropEffects.Move);
            }
        }

        #endregion

        private void btnSemiDuplicar_Click(object sender, RoutedEventArgs e)
        {
            segondaRonda = true;
            ResetGame(gridMa);
            tbApostar.Text = (ganancies / 2).ToString();
            RondaExtra();
        }

        private void btnDuplicar_Click(object sender, RoutedEventArgs e)
        {
            segondaRonda = true;
            tbApostar.Text = ganancies.ToString();
            ResetGame(gridMa);
            RondaExtra();
        }

        private void RondaExtra()
        {
            btnDuplicar.IsEnabled = false;
            btnSemiDuplicar.IsEnabled = false;
            btnRepartir.IsEnabled = false;
            AfegirCartesMa(false);
            cartaRandom = ma[random.Next(0, 5)];
            cartaRandom.CaraAmunt = true;
            ActualitzarGridImage(gridMa);
        }
    }
}

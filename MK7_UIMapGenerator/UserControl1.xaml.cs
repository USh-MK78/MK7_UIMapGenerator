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
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace MK7_UIMap_Generator
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            MainViewPort.ModelUpDirection = new Vector3D(0, 1, 0);
            GL3D.Normal = new Vector3D(0, 1, 0);
            PCam.UpDirection = new Vector3D(0, 1, 0);

            S_Light.Altitude = 200;
        }
    }
}

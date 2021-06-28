using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections;

namespace MK7_UIMap_Generator
{
    public partial class Form1 : Form
    {
        //UserControl1.xamlの初期化
        //ここは作成時の名前にも影響されるので必ず確認すること
        public UserControl1 render = new UserControl1();

        public Dictionary<string, ArrayList> MV3D_Dictionary = new Dictionary<string, ArrayList>();

        public Form1()
        {
            InitializeComponent();
            render.MouseLeftButtonDown += Render_MouseLeftButtonDown;

            //Disable 3D<->2D Mode
            d2DToolStripMenuItem.Enabled = false;

            visibilityToolStripMenuItem.Enabled = false;
        }

        HitTestResult HTR = null;
        ModelVisual3D FindMV3D = null;
        
        string ModelName = "";
        private void Render_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Get mouse coordinates of MainViewport
            var fd = e.GetPosition(this.render);

            if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
            {
                //Run hit test from mouse coordinates
                HitTestResult HTRs = VisualTreeHelper.HitTest(render.MainViewPort, fd);
                HTR = HTRs as RayMeshGeometry3DHitTestResult;
                if (HTR != null)
                {
                    //ダウンキャスト
                    //ModelVisual3Dを検索
                    FindMV3D = (ModelVisual3D)HTR.VisualHit;
                    string GetName = HTR.VisualHit.GetName();
                    ModelName = GetName;
                    label2.Text = ModelName;
                }
                if (HTR == null)
                {
                    label2.Text = "None";
                }
            }
        }

        private void Open_OBJ_TSM_Click(object sender, EventArgs e)
        {
            elementHost1.Child = render;

            OpenFileDialog Open_OBJ = new OpenFileDialog()
            {
                Title = "Open Model",
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "obj file|*.obj"
            };

            if (Open_OBJ.ShowDialog() != DialogResult.OK) return;

            Model3DGroup M3D_Group = null;
            ObjReader OBJ_Reader = new ObjReader();
            M3D_Group = OBJ_Reader.Read(Open_OBJ.FileName);

            for (int MDLChildCount = 0; MDLChildCount < M3D_Group.Children.Count; MDLChildCount++)
            {
                Model3D NewM3D = M3D_Group.Children[MDLChildCount];
                ModelVisual3D MV3D = new ModelVisual3D
                {
                    Content = NewM3D
                };

                #region Scale
                Matrix3D M = MV3D.Content.Transform.Value;
                M.M11 = M.M11 / 100;
                M.M22 = M.M22 / 100;
                M.M33 = M.M33 / 100;
                MV3D.Transform = new MatrixTransform3D(M);
                #endregion

                GeometryModel3D GM3D = (GeometryModel3D)M3D_Group.Children[MDLChildCount];
                string MatName = GM3D.Material.GetName();

                //Give a name to ModelVisual3D
                MV3D.SetName(MatName);

                ArrayList MV3D_ArrayList = new ArrayList();
                MV3D_ArrayList.Add(false);
                MV3D_ArrayList.Add(MV3D);

                try
                {
                    MV3D_Dictionary.Add(MatName, MV3D_ArrayList);
                }
                catch (System.ArgumentException)
                {
                    //マテリアルの名前が同じだった場合
                    MV3D_Dictionary.Add(MatName + MDLChildCount, MV3D_ArrayList);
                }

                //表示
                render.MainViewPort.Children.Add(MV3D);

                //Enable 3D<->2D Mode
                d2DToolStripMenuItem.Enabled = true;

                visibilityToolStripMenuItem.Enabled = true;
            }
        }

        private void Export_Image(object sender, EventArgs e)
        {
            SaveFileDialog SaveImage = new SaveFileDialog()
            {
                Title = "Save UIMapPos_Image",
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "png file|*.png"
            };

            if (SaveImage.ShowDialog() != DialogResult.OK) return;

            //Save image
            Viewport3DHelper.Export(render.MainViewPort.Viewport, SaveImage.FileName);
        }

        private void ClearViewport(object sender, EventArgs e)
        {
            //Remove the model from the Viewport
            foreach(string Key in MV3D_Dictionary.Keys)
            {
                var ModelVisual3D = (ModelVisual3D)MV3D_Dictionary[Key][1];
                render.MainViewPort.Children.Remove(ModelVisual3D);
            }

            MV3D_Dictionary.Clear();

            d2DToolStripMenuItem.Checked = false;
            render.MainViewPort.Orthographic = false;
            render.MainViewPort.CameraController.IsTouchZoomEnabled = true;
            render.MainViewPort.CameraController.IsRotationEnabled = true;

            //Disable 3D<->2D Mode
            d2DToolStripMenuItem.Enabled = false;

            visibilityToolStripMenuItem.Enabled = false;
        }

        private void VP3D_2D_ChangeTSM_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem Viewport_Type_3D = (ToolStripMenuItem)sender;
            Viewport_Type_3D.Checked = !Viewport_Type_3D.Checked;

            if (Viewport_Type_3D.Checked == true)
            {
                //Projection mode
                render.MainViewPort.Orthographic = true;
                render.MainViewPort.CameraController.IsTouchZoomEnabled = false;

                //Disable the ability to rotate the camera by dragging the Viewport
                render.MainViewPort.CameraController.IsRotationEnabled = false;
            }
            if (Viewport_Type_3D.Checked == false)
            {
                render.MainViewPort.Orthographic = false;
                render.MainViewPort.CameraController.IsTouchZoomEnabled = true;
                render.MainViewPort.CameraController.IsRotationEnabled = true;
            }
        }

        private void visibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Search for already open forms
            FormCollection formCollection = Application.OpenForms;

            bool CheckFrm = new bool();

            foreach(Form FindForm in formCollection)
            {
                if(FindForm.Name == "ModelVisibilityForm")
                {
                    //Set to false if it already exists.
                    CheckFrm = false;
                }
                else
                {
                    CheckFrm = true;
                }
            }

            if(CheckFrm == true)
            {
                ModelVisibilityForm modelVisibilityForm = new ModelVisibilityForm();
                modelVisibilityForm.Show();
            }

            //ModelVisibilityForm modelVisibilityForm = new ModelVisibilityForm();
            //modelVisibilityForm.Show();
        }

        private void Help_TSM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("[ 2D Mode ]\r\nQ, A, D, Z Key : Move\r\n\r\n[ Viewport ]\r\nCTRL + Left Click : Get the name of the model you clicked on.");
        }
    }
}

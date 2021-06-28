using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Collections;

namespace MK7_UIMap_Generator
{
    class ViewPortObjVisibleSetting
    {
        /// <summary>
        /// 3Dオブジェクトの表示、非表示の切り替えに使用する
        /// </summary>
        /// <param name="TSMI">ToolStripMenuItemの指定</param>
        /// <param name="UserCtrl">UserControl(UserControl1.xaml)</param>
        /// <param name="MV3D">ModelVisual3D List</param>
        public void ViewportObj_Visibility(bool Visible, UserControl1 UserCtrl, List<ModelVisual3D> MV3D)
        {
            //非表示にする
            if (Visible == true)
            {
                //Visual3Dに変換
                var MV3DList_Del = MV3D.ToArray<Visual3D>();

                //foreachで全て削除
                foreach (var d in MV3DList_Del)
                {
                    UserCtrl.MainViewPort.Children.Remove(d);
                }
            }
            //表示する
            if (Visible == false)
            {
                //ModelVisual3Dに変換
                var MV3DList_Add = MV3D.ToArray<ModelVisual3D>();

                //foreachで全て追加
                foreach (var d in MV3DList_Add)
                {
                    UserCtrl.MainViewPort.Children.Add(d);
                }
            }
        }

        /// <summary>
        /// 3Dオブジェクトの表示、非表示
        /// </summary>
        /// <param name="Visible">bool</param>
        /// <param name="UserCtrl">UserControl(UserControl1.xaml)</param>
        /// <param name="MV3D">ModelVisual3D List</param>
        public void ViewportObj_Visibility(bool Visible, UserControl1 UserCtrl, ModelVisual3D MV3D)
        {
            //非表示にする
            if (Visible == true)
            {
                var M3D_Del = (Visual3D)MV3D;
                UserCtrl.MainViewPort.Children.Remove(M3D_Del);
            }
            //表示する
            if (Visible == false)
            {
                try
                {
                    var M3D_Del = (Visual3D)MV3D;
                    UserCtrl.MainViewPort.Children.Add(M3D_Del);
                }
                catch (System.ArgumentException)
                {
                    //Nothing
                }
            }
        }
    }
}

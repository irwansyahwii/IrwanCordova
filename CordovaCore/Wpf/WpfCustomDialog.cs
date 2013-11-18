using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CordovaCore.Wpf
{
  public class WpfCustomDialog : IDynamicDialog
  {
    Window window = new Window();
    WpfDynamicDialog dlg;
    #region IDynamicDialog Members

    public void Initialize()
    {
      dlg = new WpfDynamicDialog();
      window.Content = dlg;
    }

    protected int buttonIndex;
    public void AddButton( string caption )
    {
      Button btn = new Button { Content = caption };
      btn.Tag = buttonIndex;
      btn.Click += btn_Click;
      dlg.PanelButtons.Children.Add( btn);
      buttonIndex++;
    }

    void btn_Click( object sender, RoutedEventArgs e )
    {
      Button btn = (Button)sender;
      window.Close();
      ButtonChosen = btn.Tag.ToString();
    }

    public string ButtonChosen
    {
      get;
      set;
    }

    public string ShowDialog()
    {
      window.MinWidth = 150;
      window.MinHeight = 150;
      window.SizeToContent = SizeToContent.WidthAndHeight;
      //window.Height = 150;
      //window.Width = 150;
      window.ShowDialog();

      return ButtonChosen;
    }

    #endregion
  }
}

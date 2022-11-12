using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DoAn1_WPF.ViewModel
{
    public class DialogViewModel : BaseViewModel
    {
        public ICommand CancelCommand { get; set; }
        public DialogViewModel()
        {            
            CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }
    }
}

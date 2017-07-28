using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace com.uftarturo
{
    class Dialog
    {
        private static AlertDialog alertMessage;
        private static ProgressDialog progress;
        public static void Show(string title, string message, Activity a)
        {
            alertMessage = new AlertDialog.Builder(a).Create();
            alertMessage.SetCanceledOnTouchOutside(true);
            // Editar actual componente y mostrar
            alertMessage.SetTitle(title);
            alertMessage.SetMessage(message);
            alertMessage.Show();
        }
        public static void Loading(string message, Activity a)
        {
            progress = new ProgressDialog(a);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage(message);
            progress.SetCancelable(false);
            progress.Show();
        }
        public static void Stop()
        {
            try
            {
                progress.Dismiss();
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace com.uftarturo
{
    // Iniciar al prender equipo
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.LowPriority)]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Intent serviceStart = new Intent(context, typeof(MainActivity));
            serviceStart.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(serviceStart);
        }
    }
    // Iniciar aplicacion
    [Activity(Label = "@string/LoginName", MainLauncher = true, Icon = "@drawable/logo")]
    public class MainActivity : Activity
    {
        private Database db;
        private Activity t;
        private void Init()
        {
            // Base de datos
            db = new Database("arturou");
            if(db.GetRecord().Length > 10)
            {
                this.Change();
            }
            // Activdad
            this.t = this;
        }
        private void Change()
        {
            var intent = new Intent(this, typeof(Login));
            StartActivity(intent);
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            // Iniciar
            this.Init();
            // Escuchar click
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate {
                // Procedimiento
                Dialog.Loading("Conectando al servidor UFT.\n Por favor espere.", this);
                // Validacion de datos
                string user = (string)FindViewById<EditText>(Resource.Id.user).Text, pass = (string)FindViewById<EditText>(Resource.Id.password).Text;
                if (user.Length == 0 || pass.Length == 0)
                {
                    Dialog.Show("Datos invalidos", "El usuario o contrasena no pueden estar en blanco",this);
                    Dialog.Stop();
                    return;
                }
                this.Exec(user, pass);
            };
        }
        
        private void Exec(string user, string pass)
        {
            t.RunOnUiThread(async () =>
            {
                try
                {
                    // WebClient wc = new WebClient();
                    string url = "http://nofeed.xyz/uft/?user=" + user + "&pass=" + pass;
                    //string result = wc.DownloadString(url);
                    string result = await UFT.GetData(url);
                    JsonValue o = JsonValue.Parse(result);
                    string error = o["error"];
                    if (error == "loginFail")
                    {
                        Dialog.Stop();
                        Dialog.Show("Error", "Usuario y/o contraseña incorrectos", this);
                        return;
                    }
                    db.DeleteRecord();
                    db.AddRecord(user, pass, result);
                    // Abrir nuevo layout
                    this.Change();
                }
                catch (WebException e)
                {
                    Dialog.Show("Sin conexion", "Verifica tu conexion a internet y vuelve a intentarlo", this);
                    Dialog.Stop();
                }
            });
        }
    }
}


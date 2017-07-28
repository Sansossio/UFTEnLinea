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
using System.Json;
using Android.Graphics;
using System.Net;
using System.Timers;
using Org.Json;

namespace com.uftarturo
{
    [Activity(Label = "@string/login", Icon = "@drawable/logo")]
    public class Login : Activity
    {
        private Database db;
        public Activity t;
        private Timer timer;
        private string user, pass;
        private Button[] tasks = new Button[100];
        private LinearLayout layout;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
   
            // Escuchar click
            Button button = FindViewById<Button>(Resource.Id.exit);
            button.Click += delegate
            {
                this.timer.Enabled = false;
                db.DeleteRecord();
                this.Change();
            };
            Button update = FindViewById<Button>(Resource.Id.update);
            update.Click += delegate
            {
                this.Update();
            };
            t = this;
            // Layour
            this.layout = (LinearLayout)FindViewById<LinearLayout>(Resource.Id.layout);
            // Timer
            this.timer = new Timer(2*3600*1000);
            this.timer.Interval = (2*3600*1000);
            this.timer.Elapsed += OnTimedEvent;
            this.timer.Enabled = true;
            // Iniciar
            this.Init();
        }
        private void Change()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }
        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.RunOnUiThread(() =>
            {
                this.Update();
            }); 
        }
        private void Init()
        {
            // Cancelar carga
            Dialog.Stop();       
            // imagen de perfil
            ImageView profile = (ImageView)FindViewById<ImageView>(Resource.Id.profile);
            profile.SetBackgroundResource(Resource.Drawable.loading);
            // Base de datos
            db = new Database("arturou");
            // Verificar existencia de datos
            if(db.GetRecord().Length < 10)
            {
                Dialog.Show("Error Interno", "No se encontro ninguna cuenta activa", this);
                // Abrir login page
                db.DeleteRecord();
                this.Change();
                return;
            }
            try
            {
                string[] data = db.GetRecord().Split('|');
                this.user = data[0];
                this.pass = data[1];
                JsonValue o = JsonValue.Parse(data[2]);
                TextView name = (TextView)FindViewById<TextView>(Resource.Id.name);
                name.Text = o["name"];
                new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                    this.edit(this.GetImageBitmapFromUrl(o["picture"]));
                })).Start();
                JSONArray c = new JSONObject(data[2]).GetJSONArray("courses");
                int size = c.Length();
                this.layout.RemoveAllViews();
                for(int i = 0; i < size; i++)
                {
                    try
                    {
                        string d = c.GetJSONObject(i).ToString();
                        JsonValue cs = JsonValue.Parse(d);
                        string[] dat = cs["tarea"].ToString().Split(new string[] { "XX" }, StringSplitOptions.None);
                        tasks[i] = new Button(this);
                        dat[2] = dat[2].Replace('"', ' ');
                        dat[1] = dat[1].Replace('"', ' ');
                        dat[0] = dat[0].Replace('"', ' ');
                        tasks[i].Text = dat[0];
                        tasks[i].Id = i;
                        
                        tasks[i].Click += delegate {
                            Dialog.Show(dat[0], "Tipo de asignacion: " + dat[2] + "\n\n" + dat[1], this);
                        };
                        this.layout.AddView(tasks[i]);
                    }catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }  
                }
                TextView tp = (TextView)FindViewById<TextView>(Resource.Id.task);
                tp.Text = size == 0 ? "Sin tareas pendientes!" : "Tienes " + size + " tareas pendientes";
                if(size > 0)
                {
                    Notifications.Show("UFT En Linea","Tienes " + size + " tareas sin entregar", this);
                }
            }
            catch(Exception e)
            {
                Dialog.Show("Error Interno", e.ToString(), this);
                Console.WriteLine(e);
            }
            
        }
       private void edit(Bitmap e)
        {
            t.RunOnUiThread(() =>
            {
                ImageView profile = (ImageView)FindViewById<ImageView>(Resource.Id.profile);
                profile.SetImageBitmap(e);
            });
        }
        private async void Update()
        {
            if (this.user == null || this.pass == null)
            {
                this.Change();
            }
            try
            {
                string url = "http://nofeed.xyz/uft/?user=" + this.user + "&pass=" + this.pass;
                Dialog.Loading("Conectando al servidor UFT.\nPor favor espere.", this);
                string result = await UFT.GetData(url);
                JsonValue o = JsonValue.Parse(result);
                string error = o["error"];
                if (error == "loginFail")
                {
                    Dialog.Stop();
                    Dialog.Show("Error", "Usuario y/o contraseña incorrectos", this);
                    this.Change();
                    return;
                }
                db.DeleteRecord();
                db.AddRecord(user, pass, result);
                this.Init();
                Dialog.Stop();
            }
            catch(Exception e)
            {
                Dialog.Stop();
                Console.WriteLine(e);
            }
            

        }
    }
}
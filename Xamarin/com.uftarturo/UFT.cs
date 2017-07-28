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
using System.Threading.Tasks;
using System.Net;

namespace com.uftarturo
{
    public class UFT
    {
        public static async Task<string> GetData(string url)
        {
            using (var client = new WebClient())
            {
                return await client.DownloadStringTaskAsync(url);
            }
        }
    }
}
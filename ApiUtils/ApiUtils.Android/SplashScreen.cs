using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ApiUtils.Droid
{
    [Activity(Label = "ApiUtils", Icon = "@mipmap/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task.Run(async () => { await SimulateStartup(); });
        }

        // Simulates background work that happens behind the splash screen
        async Task SimulateStartup()
        {
            await Task.Delay(100);// Simulate a bit of startup work.
            StartActivity(typeof(MainActivity));
        }
    }
}
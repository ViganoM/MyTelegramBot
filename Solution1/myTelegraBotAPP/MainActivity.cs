
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using myTelegramBot;

namespace myTelegraBotAPP {
    [Activity(Label = "myTelegraBotAPP", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            FindViewById<Switch>(Resource.Id.switch1).CheckedChange += RunSwitch;
        }

        private void RunSwitch(object sender, CompoundButton.CheckedChangeEventArgs e) {
            Switch runSwitch = sender as Switch;
            if ( runSwitch.Checked )
                Program.Updater();
            else
                Program.run = false;
        }
    }
}



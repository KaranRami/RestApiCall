using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApiUtils.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel(ContentPage view)
        {
            m_View = view;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }
        public async Task SingleRun(Func<Task> operation)
        {
            object _lock = new object();
            lock (_lock)
            {
                if (CommandInitiated)
                    return;

                CommandInitiated = true;
            }

            try
            {
                await operation();
            }
            catch (Exception)
            {

            }
            CommandInitiated = false;
        }

        public void WriteExceptionToConsole(Exception e, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string callerfile = null, [CallerMemberName] string caller = null)
        {
            Console.WriteLine("Exception thrown in: " + callerfile + " at position- Line No:" + lineNumber + ", Caller Member:" + caller + ", Message:" + e.Message);
        }

        #region Property
        public ContentPage m_View { get; set; }
        public bool CommandInitiated { get; set; }
        #endregion
    }
}

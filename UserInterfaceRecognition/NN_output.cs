using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Forms;
using NN_lib;
using System.ComponentModel;
using System.Threading.Tasks;

namespace UserInterfaceRecognition
{
    public class NN_output: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public Commands StartC { get; set; }

        public Commands CancelC { get; set; }

        public ObservableCollection<Tuple<string, int, float>> ClassObserv { get; set; }

        public ObservableCollection<NN_model> Observ { get; set; }

        public ObservableCollection<NN_model> SelectedClassObserv { get; set; }

        public My_Lib ml { get; set; }

        public Dispatcher Dispatcherr { get; set; }

        public NN_output()
        {
            this.Observ = new ObservableCollection<NN_model>();
            this.ClassObserv = new ObservableCollection<Tuple<string, int, float>>();
            this.SelectedClassObserv = new ObservableCollection<NN_model>();
            this.ml = new My_Lib();
            this.ml.Notify += OnPredictionCome;
            this.Dispatcherr = Dispatcher.CurrentDispatcher;

            this.StartC = new Commands(Start);
            this.CancelC = new Commands(Cancel);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private Tuple<string, int, float> selectedClass;
        public  Tuple<string, int, float> SelectedClass
        {
            get
            {
                return selectedClass;
            }
            set 
            { 
                selectedClass = value;
                if (value != null)
                {
                    this.SelectedClassObserv = new ObservableCollection<NN_model>(Observ.Where(p => p.ClassLabel == selectedClass.Item1));
                    OnPropertyChanged("SelectedClassObserv");
                }
                
            }
        }

        private void OnPredictionCome(Info i, EventArgs e)
        {

            Dispatcherr.BeginInvoke(DispatcherPriority.Background,new Action(() =>
            {
                Observ.Add(new NN_model(i.path, i.kind, i.probability));
                int index = -1;
                foreach (var tmp in ClassObserv)
                {
                    if (tmp.Item1.Equals(i.kind))
                    {
                        index = ClassObserv.IndexOf(tmp);
                        break;
                    }
                }
                if (index != -1)
                    ClassObserv[index] = new Tuple<string, int, float>(ClassObserv[index].Item1, ClassObserv[index].Item2 + 1, ClassObserv[index].Item3);
                else
                    ClassObserv.Add(new Tuple<string, int, float>(i.kind, 1, i.probability));
            }));
            
        }

        private void Start(object sender)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClassObserv.Clear();
                Observ.Clear();
                SelectedClassObserv.Clear();
                Recognition(fbd.SelectedPath); 
                
            }
        }

        private void Recognition(string path)
        {
            Task.Run(() =>
                {
                    My_Lib.cts = new CancellationTokenSource();
                    ml.ParallelProcess(path);
                });
        }

        private void Cancel(object sender)
        {
            this.ml.Stop();
        }
     
    }
}

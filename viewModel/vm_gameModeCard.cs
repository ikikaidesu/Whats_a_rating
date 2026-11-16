using movieRateGuess.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace movieRateGuess.viewModel
{
    public class vm_gameModeCard : viewModelMain
    {
        private string _popUpText;
        public string PopUpText
        {
            get => _popUpText;
            set
            {
                _popUpText = value;
                OnPropertyChanged(nameof(PopUpText));
            }
        }
        private int _popUpWidth;
        public int PopUpWidth
        {
            get => _popUpWidth;
            set
            {
                _popUpWidth = value;
                OnPropertyChanged(nameof(PopUpWidth));
            }
        }
        private int _popUpHeight;
        public int PopUpHeight
        {
            get => _popUpHeight;
            set
            {
                _popUpHeight = value;
                OnPropertyChanged(nameof(PopUpHeight));
            }
        }
        private int _toolTipHorizontalOffset;
        public int PopUpHorizontalOffset
        {
            get => _toolTipHorizontalOffset;
            set
            {
                _toolTipHorizontalOffset = value;
                OnPropertyChanged(nameof(PopUpHorizontalOffset));
            }
        }

        private int _toolTipVerticalOffset;
        public int PopUpVerticalOffset
        {
            get => _toolTipVerticalOffset;
            set
            {
                _toolTipVerticalOffset = value;
                OnPropertyChanged(nameof(PopUpVerticalOffset));
            }
        }

        public ICommand goToSoloModeCommand { get; set; }

        public vm_gameModeCard(ICommand goToSoloModeCommand)
        {
            this.goToSoloModeCommand = goToSoloModeCommand;
            initializeData();
        }


        private void initializeData()
        {
            PopUpText = "В данном режиме вашей задачей является отгадывание оценки случайного популярного фильма.\r\nОтгадывание будет округляемых целых чисел.  К примеру:\r\n7.0-7.49 → 7\r\n7.5-7.99 → 8";
            PopUpWidth = 450;
            PopUpHeight = 275;
            PopUpHorizontalOffset = -185;
            PopUpVerticalOffset = -375;
        }
    }
}



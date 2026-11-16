using movieRateGuess.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace movieRateGuess.viewModel
{
    public class vm_gameModeFriendCard : viewModelMain
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

        public ICommand goToFriendModeCommand { get; set; }
        

        public vm_gameModeFriendCard(ICommand goToFriendModeCommand)
        {
            this.goToFriendModeCommand = goToFriendModeCommand;
            initializeData();
        }


        private void initializeData()
        {
            PopUpText = "В данном режиме вашей задачей является отгадывание оценки вашего друга на фильм. Вставить нужно ссылку на профиль друга. К примеру:\r\nhttps://www.kinopoisk.ru/user/90928130/\r\nОтгадывание будет  целых чисел.";
            PopUpWidth = 450;
            PopUpHeight = 310;
            PopUpHorizontalOffset = -185;
            PopUpVerticalOffset = -410;
        }
    }
}

using movieRateGuess.models;
using movieRateGuess.utilities;
using movieRateGuess.utilities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace movieRateGuess.viewModel
{
    public class vm_menu : viewModelMain
    {
        private m_menu model = new m_menu();
        private vm_navigation _currentView;
        private vm_gameModeCard _gameModeCard;
        public vm_gameModeCard GameModeCard
        {
            get => _gameModeCard;
            set
            {
                _gameModeCard = value;
                OnPropertyChanged(nameof(GameModeCard));
            }
        }

        private vm_gameModeFriendCard _gameModeFirendCard;
        public vm_gameModeFriendCard GameModeFriendCard
        {
            get => _gameModeFirendCard;
            set
            {
                _gameModeFirendCard = value;
                OnPropertyChanged(nameof(_gameModeFirendCard));
            }
        }

        private bool _isHelpWindowOpen;
        public bool IsHelpWindowOpen
        {
            get => _isHelpWindowOpen;
            set
            {
                _isHelpWindowOpen = value;
                OnPropertyChanged(nameof(IsHelpWindowOpen));
            }
        }

        private string _helpTextInvalidLink;
        public string HelpTextInvalidLink
        {
            get => _helpTextInvalidLink;
            set
            {
                _helpTextInvalidLink = value;
                OnPropertyChanged(nameof(HelpTextInvalidLink));
            }
        }

        public ICommand goToSoloModeCommand { get; set; }
        public ICommand goToFriendModeCommand { get; set; }

        public ICommand closeHelpWindowCommand { get; set; }

        public vm_menu(object _currentView)
        {
            initializeData(_currentView);
        }

        private void initializeData(object _currentView)
        {
            this._currentView = (vm_navigation)_currentView;
            goToSoloModeCommand = new RelayCommand(goToSoloMode);
            goToFriendModeCommand = new RelayCommand(goToFriendMode);
            IsHelpWindowOpen = false;
            HelpTextInvalidLink = "Введена некорректная ссылка. Пожалуйста, проверьте правильность ссылки и попробуйте снова.";

            GameModeCard = new vm_gameModeCard(goToSoloModeCommand);
            GameModeFriendCard = new vm_gameModeFriendCard(goToFriendModeCommand);
            closeHelpWindowCommand = new RelayCommand(closeHelpWindow);

        }

        private void goToSoloMode(object obj)
        {
            _currentView.navigateTo(enum_pages.game);
        }

        private void goToFriendMode(object obj)
        {
            string link = "";
            string friendId = "";
            if (obj.GetType() == typeof(string))
            {
                link = (string)obj;
                friendId = model.checkIfLinkIsValid(link);
            }
            if (string.IsNullOrEmpty(friendId) == false)
            {
                _currentView.navigateTo(enum_pages.game, friendId);
            }
            else
            {
                IsHelpWindowOpen = true;
            }
        }

        private void closeHelpWindow(object obj)
        {
            IsHelpWindowOpen = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lb2_prog_2
{
    internal class WordGameVM : INotifyPropertyChanged
    {
        private WordGame game;
        public WordGame Game => game;
        public ReadOnlyObservableCollection<WordGame.Letter> Letters { get { return game.Letters; } }

        public WordGameVM()
        {
            game = new WordGame();
            game.StartNewGame(7);
        }

        private Command startNewGameCommand;
        public Command StartNewGameCommand
        {
            get {
                return startNewGameCommand ??
                    (startNewGameCommand = new Command(obj =>
                    {
                        if (int.TryParse((string)obj, out int num))
                        {
                            if (!game.StartNewGame(num))
                                MessageBox.Show("Минимальное количество букв: 4", "Неверный ввод");

                        }
                        else MessageBox.Show("Введите числовое значение", "Неверный ввод");
                    })); }
        }

        private Command clearCommand;
        public Command ClearCommand
        {
            get
            {
                return clearCommand ??
                    (clearCommand = new Command(obj =>
                    {
                        game.ClearWord();
                    }));
            }
        }

        private Command checkCommand;
        public Command CheckCommand
        {
            get
            {
                return checkCommand ??
                    (checkCommand = new Command(async obj =>
                    {
                        await game.CheckWord(CheckWordTextGears.CheckWord, "b1D2VhZMMzRyD4bF");
                    }));
            }
        }

        private Command letterCommand;
        public Command LetterCommand
        {
            get
            {
                return letterCommand ??
                    (letterCommand = new Command(obj =>
                    {
                        if (int.TryParse((string)obj, out int id))
                        {
                            game.PressLetter(id);
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

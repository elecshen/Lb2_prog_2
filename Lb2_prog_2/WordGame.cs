using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lb2_prog_2
{
    internal class WordGame : INotifyPropertyChanged
    {
        public struct Letter
        {
            // Структура для более удобного хранения информации о содержимом кнопки
            public Letter(int i, string l, int p)
            {
                id = i;
                letter = l;
                points = p;
                isNotPressed = true;
            }
            public int id { get; set; }
            public string letter { get; set; }
            public int points { get; set; }
            public bool isNotPressed { get; set; }
        }

        readonly string ABC = "АЕЁИОУЫЭЮЯБВГДЖЗЙКЛМНПРСТФХЦЧШЩЪЬ";

        // Баллы для каждого символа алфавита
        // Необходимо для присвоения одинаковым буквам одинаковых баллов
        private int[] letterPoints;
        // Список символов достыхных для набора слова
        private ObservableCollection<Letter> letters;
        public ReadOnlyObservableCollection<Letter> Letters { get; }
        // Список введённых слов, которые прошли проверку
        private ObservableCollection<string> wordsHistory;
        public ReadOnlyObservableCollection<string> WordsHistory { get; }
        // Текущее набираемое слово
        private string word;
        // Сумма очков которые можно получить, если слово пройдёт проверку
        private int potentialPoints;
        // Очки за все корректно введёные слова
        private int score;

        public int Score
        {
            get { return score; }
            private set
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        public string Word
        {
            get { return word; }
            private set
            {
                word = value;
                OnPropertyChanged("Word");
            }
        }

        public int PotentialPoints
        {
            get { return potentialPoints; }
            private set { 
                potentialPoints = value;
                OnPropertyChanged("PotentialPoints");
            }
        }

        public WordGame()
        {
            letterPoints = new int[ABC.Length];
            word = "";
            potentialPoints = 0;
            score = 0;
            letters = new ObservableCollection<Letter>();
            Letters = new ReadOnlyObservableCollection<Letter>(this.letters);
            wordsHistory = new ObservableCollection<string>();
            WordsHistory = new ReadOnlyObservableCollection<string>(this.wordsHistory);
        }

        public bool StartNewGame(int num)
        {
            if (num < 4) return false; 
            ClearWord();
            Score = 0;
            wordsHistory.Clear();

            SetRandomLetters(num);
            return true;
        }

        // Установка баллов за символ и списка доступных для ввода символов
        private void SetRandomLettersV1(int num)
        {
            letters.Clear();
            Random random = new Random();
            for (int i = 0; i < letterPoints.Length; i++)
            {
                letterPoints[i] = random.Next(1, 10);
            }
            int c;
            for (int i = 0; i < num; i++)
            {
                c = random.Next(ABC.Length);
                letters.Add(new Letter(i, ((char)('А' + c)).ToString(), letterPoints[c]));
            }
        }

        private void SetRandomLetters(int num)
        {
            letters.Clear();
            Random random = new Random();
            for (int i = 0; i < letterPoints.Length; i++)
            {
                letterPoints[i] = random.Next(1, 10);
            }
            int c;
            for (int i = 0; i < num; i++)
            {
                c = (c = random.Next(ABC.Length+10)) > 19 ? c - 10 : c / 2;
                letters.Add(new Letter(i, ABC[c].ToString(), letterPoints[c]));
            }
        }

        public bool PressLetter(int id)
        {
            if (id >= 0 && id < letters.Count)
            {
                var l = letters[id];
                Word += l.letter;
                PotentialPoints += l.points;
                l.isNotPressed = false;
                letters[id] = l;
                return true;
            }
            return false;
        }

        private void SaveWordAndGetPoints()
        {
            Score += PotentialPoints;
            wordsHistory.Add(Word);
            ClearWord();
        }

        public async Task<bool> CheckWord(Func<string, string, Task<bool>> checkFunc, string apiKey)
        {
            if(!wordsHistory.Contains(word) && await checkFunc(Word, apiKey))
            {
                SaveWordAndGetPoints();
                return true;
            }
            ClearWord();
            return false;
        }

        public void ClearWord()
        {
            PotentialPoints = 0;
            Word = "";
            for (int i = 0; i < letters.Count; i++)
            {
                Letter letter = letters[i];
                letter.isNotPressed = true;
                letters[i] = letter;
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

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lb2_prog_2
{
    internal class WordGame : INotifyPropertyChanged
    {
        private ObservableCollection<char> allowedLetters;
        public ReadOnlyObservableCollection<char> AllowedLetters;
        private ObservableCollection<int> letterPoints;
        public ReadOnlyObservableCollection<int> LetterPoints;
        private ObservableCollection<string> wordsHistory;
        public ReadOnlyObservableCollection<string> WordsHistory;
        private string word;
        private int potentialPoints;
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

        public WordGame(ObservableCollection<char> allowedLetters = null, ObservableCollection<int> letterPoints = null)
        {
            if (allowedLetters == null)
                this.allowedLetters = new ObservableCollection<char>();
            else
                this.allowedLetters = allowedLetters;
            if (letterPoints == null)
                this.letterPoints = new ObservableCollection<int>();
            else
                this.letterPoints = letterPoints;
            this.word = "";
            this.potentialPoints = 0;
            this.wordsHistory = new ObservableCollection<string>();
            this.score = 0;
            AllowedLetters = new ReadOnlyObservableCollection<char>(this.allowedLetters);
            LetterPoints = new ReadOnlyObservableCollection<int>(this.letterPoints);
            WordsHistory = new ReadOnlyObservableCollection<string>(this.wordsHistory);
        }

        public void startNewGame(int num)
        {
            clearWord();
            Score = 0;
            wordsHistory.Clear();

            setRandomLetters(num);
        }

        private void setRandomLetters(int num)
        {
            Random random = new Random();
            for(int i = 0; i < num; i++)
            {
                allowedLetters.Add((char)random.Next(0x0410, 0x042F));
                letterPoints.Add(random.Next(1, 5));
            }
        }

        public bool enterLetter(char letter)
        {
            if(allowedLetters.Contains(letter))
            {
                Word += letter;
                PotentialPoints += letterPoints[allowedLetters.IndexOf(letter)];
                return true;
            }
            return false;
        }

        private void saveWordAndGetPoints()
        {
            Score += PotentialPoints;
            wordsHistory.Add(Word);
            clearWord();
        }

        public async Task<bool> checkWord(Func<string, string, Task<bool>> checkFunc, string apiKey)
        {
            if(await checkFunc(Word, apiKey))
            {
                saveWordAndGetPoints();
                return true;
            }
            clearWord();
            return false;
        }

        public void clearWord()
        {
            PotentialPoints = 0;
            Word = "";
            // TODO сделать функцию посчета очков при изменении слова
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

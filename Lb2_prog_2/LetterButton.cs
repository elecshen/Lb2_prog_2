using System.Windows;
using System.Windows.Controls;

namespace Lb2_prog_2
{
    internal class LetterButton : Button
    {
        // Параметр для хранения второстепенного контента(количество баллов за символ)
        public string SecondaryContent
        {
            get { return (string)GetValue(SecondaryContentProperty); }
            set { SetValue(SecondaryContentProperty, value); }
        }
        // Регистрация параметра
        public static readonly DependencyProperty SecondaryContentProperty = DependencyProperty.Register(
            nameof(SecondaryContent), typeof(string), typeof(LetterButton), new PropertyMetadata(""));

        // Параметр идентификатора символа для распознавания нажатой кнопки
        public string IdData
        {
            get { return (string)GetValue(IdDataProperty); }
            set { SetValue(IdDataProperty, value); }
        }
        public static readonly DependencyProperty IdDataProperty = DependencyProperty.Register(
            nameof(IdData), typeof(string), typeof(LetterButton), new PropertyMetadata(""));

        public LetterButton()
        {
            DefaultStyleKey = typeof(LetterButton);
        }
    }
}

﻿using System.Windows;
using System.Windows.Controls;

namespace Lb2_prog_2
{
    internal class LetterButton : Button
    {
        public string SecondaryContent
        {
            get { return (string)GetValue(SecondaryContentProperty); }
            set { SetValue(SecondaryContentProperty, value); }
        }
        public static readonly DependencyProperty SecondaryContentProperty = DependencyProperty.Register(
            nameof(SecondaryContent), typeof(string), typeof(LetterButton), new PropertyMetadata(""));

        public LetterButton()
        {
            DefaultStyleKey = typeof(LetterButton);
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Yahtzee
{
    public static class GameManager
    {
        public static int tryCounter = 0;
        public static int scoreASector = 0;
        public static int scoreBSector = 0;
        public static bool gotBonus = false;
    }

    public class Dice
    {
        public int diceValue;

        Random random = new Random();

        public Dice() { SetDefault(); }

        public void SetDefault() { diceValue = 0; }

        public void SetRandom() { diceValue = random.Next(1, 7); }
    }

    public partial class MainWindow : Window
    {
        public bool[] leaveState = new bool[5] { false, false, false, false, false };
        public Dice[] diceList = new Dice[5];

        string path = "/Assets/";

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 5; ++i) diceList[i] = new Dice();
        }

        public void ResetDices()
        {
            for (int i = 0; i < 4; ++i) diceList[i].SetDefault();
            Dice0.Source = new BitmapImage(new Uri(path + "blank.png", UriKind.Relative));
            Dice1.Source = new BitmapImage(new Uri(path + "blank.png", UriKind.Relative));
            Dice2.Source = new BitmapImage(new Uri(path + "blank.png", UriKind.Relative));
            Dice3.Source = new BitmapImage(new Uri(path + "blank.png", UriKind.Relative));
            Dice4.Source = new BitmapImage(new Uri(path + "blank.png", UriKind.Relative));
        }

        public void ResetLeave()
        {
            Leave0.Background = Brushes.White;
            Leave1.Background = Brushes.White;
            Leave2.Background = Brushes.White;
            Leave3.Background = Brushes.White;
            Leave4.Background = Brushes.White;

            Leave0.IsEnabled = false;
            Leave1.IsEnabled = false;
            Leave2.IsEnabled = false;
            Leave3.IsEnabled = false;
            Leave4.IsEnabled = false;

            for (int i = 0; i < 5; ++i) leaveState[i] = false;

            GameManager.tryCounter = 0;
            Try_Counter.Text = "0/3 Tries";
        }

        public void CheckBonus()
        {
            if (!GameManager.gotBonus && GameManager.scoreASector >= 63)
            {
                GameManager.scoreASector += 35;
                Button_Bonus.Content = "35";
                GameManager.gotBonus = true;
            }
            TotalA.Text = GameManager.scoreASector.ToString();
            TotalB.Text = GameManager.scoreBSector.ToString();
            TotalScore.Text = (GameManager.scoreASector + GameManager.scoreBSector).ToString();
        }

        public void RollSet(object sender, RoutedEventArgs e)
        {
            string aux = "_oldal.png";
            for (int i = 0; i < 5; ++i)
            {
                if (!leaveState[i])
                {
                    diceList[i].SetRandom();
                    int value = diceList[i].diceValue;
                    switch (i)
                    {
                        case 0:
                            Dice0.Source = new BitmapImage(new Uri(path + value.ToString() + aux, UriKind.Relative));
                            break;
                        case 1:
                            Dice1.Source = new BitmapImage(new Uri(path + value.ToString() + aux, UriKind.Relative));
                            break;
                        case 2:
                            Dice2.Source = new BitmapImage(new Uri(path + value.ToString() + aux, UriKind.Relative));
                            break;
                        case 3:
                            Dice3.Source = new BitmapImage(new Uri(path + value.ToString() + aux, UriKind.Relative));
                            break;
                        case 4:
                            Dice4.Source = new BitmapImage(new Uri(path + value.ToString() + aux, UriKind.Relative));
                            break;
                    }
                }
            }
            ++GameManager.tryCounter;
            Try_Counter.Text = GameManager.tryCounter.ToString() + "/3 Tries";
            if (GameManager.tryCounter >= 3) Roll_Button.IsEnabled = false;
            else if (GameManager.tryCounter == 1)
            {
                Leave0.IsEnabled = true;
                Leave1.IsEnabled = true;
                Leave2.IsEnabled = true;
                Leave3.IsEnabled = true;
                Leave4.IsEnabled = true;
            }
        }

        public void FinishGame(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Congratulations for obtaining " +
                (GameManager.scoreASector + GameManager.scoreBSector).ToString() + 
                " points! Do you want to retry?", "Yahtzee!", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetDices();
                    ResetLeave();
                    GameManager.scoreASector = 0;
                    GameManager.scoreBSector = 0;
                    CheckBonus();
                    ResetScoreButtons();
                    GameManager.gotBonus = false;
                    break;
                case MessageBoxResult.No:
                    Close();
                    break;
            }
        }

        public void ResetScoreButtons()
        {
            One_Button.Content = "";
            Two_Button.Content = "";
            Three_Button.Content = "";
            Four_Button.Content = "";
            Five_Button.Content = "";
            Six_Button.Content = "";
            Button_Bonus.Content = "";
            ThrKind_Button.Content = "";
            FourKind_Button.Content = "";
            FHouse_Button.Content = "";
            SmallStr_Button.Content = "";
            LargeStr_Button.Content = "";
            Yahtzee_Button.Content = "";
            Chance_Button.Content = "";

            One_Button.IsEnabled = true;
            Two_Button.IsEnabled = true;
            Three_Button.IsEnabled = true;
            Four_Button.IsEnabled = true;
            Five_Button.IsEnabled = true;
            Six_Button.IsEnabled = true;
            ThrKind_Button.IsEnabled = true;
            FourKind_Button.IsEnabled = true;
            FHouse_Button.IsEnabled = true;
            SmallStr_Button.IsEnabled = true;
            LargeStr_Button.IsEnabled = true;
            Yahtzee_Button.IsEnabled = true;
            Chance_Button.IsEnabled = true;
        }

        #region SectionA

        public void CountOnes(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (diceList[i].diceValue == 1) x += diceList[i].diceValue;
            }
            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreASector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CountTwos(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (diceList[i].diceValue == 2) x += diceList[i].diceValue;
            }
            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreASector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CountThrees(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (diceList[i].diceValue == 3) x += diceList[i].diceValue;
            }
            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreASector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CountFours(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (diceList[i].diceValue == 4) x += diceList[i].diceValue;
            }
            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreASector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CountFives(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (diceList[i].diceValue == 5) x += diceList[i].diceValue;
            }
            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreASector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CountSixes(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (diceList[i].diceValue == 6) x += diceList[i].diceValue;
            }
            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreASector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        #endregion

        #region SectionB

        public void CheckThree(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            int[] frec = new int[6] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 5; ++i)
            {
                x += diceList[i].diceValue;
                ++frec[diceList[i].diceValue - 1];
            }

            bool isThree = false;
            for (int i = 0; i < 6; ++i)
            {
                if (frec[i] >= 3) isThree = true;
            }
            if (!isThree) x = 0;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreBSector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CheckFour(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            int[] frec = new int[6] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 5; ++i)
            {
                ++frec[diceList[i].diceValue - 1];
                x += diceList[i].diceValue;
            }
            
            bool isFour = false;
            for (int i = 0; i < 6; ++i)
            {
                if (frec[i] >= 4) isFour = true;
            }
            if (!isFour) x = 0;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreBSector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CheckFullHouse(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            int[] frec = new int[6] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 5; ++i)
                ++frec[diceList[i].diceValue - 1];

            bool isThree = false, isTwo = false;
            for (int i = 0; i < 6; ++i)
            {
                if (frec[i] == 3) isThree = true;
                if (frec[i] == 2) isTwo = true;
            }
            if (!isTwo || !isThree) x = 0;
            else x = 25;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreBSector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CheckSmall(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            int[] frec = new int[6] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 5; ++i)
                ++frec[diceList[i].diceValue - 1];

            int temp = 0, aux_max = 0;
            for (int i = 0; i < 6; ++i)
            {
                if (frec[i] >= 1) ++temp;
                else
                {
                    if (aux_max < temp) aux_max = temp;
                    temp = 0;
                }
            }
            if (aux_max == 0) aux_max = temp;
            if (aux_max >= 4) x = 30;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreBSector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CheckLarge(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            int[] frec = new int[6] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 5; ++i)
                ++frec[diceList[i].diceValue - 1];

            int temp = 0, aux_max = 0;
            for (int i = 0; i < 6; ++i)
            {
                if (frec[i] >= 1) ++temp;
                else
                {
                    if (aux_max < temp) aux_max = temp;
                    temp = 0;
                }
            }
            if (aux_max == 0) aux_max = temp;
            if (aux_max == 5) x = 40;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreBSector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CheckYahtzee(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            int[] frec = new int[6] { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 5; ++i)
                ++frec[diceList[i].diceValue - 1];

            for (int i = 0; i < 6; ++i)
                if (frec[i] == 5)
                    x = 50;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            GameManager.scoreBSector += x;
            CheckBonus();
            Roll_Button.IsEnabled = true;
            ResetDices();
            ResetLeave();
        }

        public void CountAll(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            int x = 0;
            for (int i = 0; i < 5; ++i) x += diceList[i].diceValue;

            thisButton.Content = x.ToString();
            thisButton.IsEnabled = false;
            Roll_Button.IsEnabled = true;
            GameManager.scoreBSector += x;
            CheckBonus();
            ResetDices();
            ResetLeave();
        }

        #endregion

        #region LeaveButtons

        public void LeaveStateChange0(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if (leaveState[0])
            {
                thisButton.Background = Brushes.White;
                leaveState[0] = false;
            }
            else
            {
                thisButton.Background = Brushes.Green;
                leaveState[0] = true;
            }
        }

        public void LeaveStateChange1(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if (leaveState[1])
            {
                thisButton.Background = Brushes.White;
                leaveState[1] = false;
            }
            else
            {
                thisButton.Background = Brushes.Green;
                leaveState[1] = true;
            }
        }

        public void LeaveStateChange2(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if (leaveState[2])
            {
                thisButton.Background = Brushes.White;
                leaveState[2] = false;
            }
            else
            {
                thisButton.Background = Brushes.Green;
                leaveState[2] = true;
            }
        }

        public void LeaveStateChange3(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if (leaveState[3])
            {
                thisButton.Background = Brushes.White;
                leaveState[3] = false;
            }
            else
            {
                thisButton.Background = Brushes.Green;
                leaveState[3] = true;
            }
        }

        public void LeaveStateChange4(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if (leaveState[4])
            {
                thisButton.Background = Brushes.White;
                leaveState[4] = false;
            }
            else
            {
                thisButton.Background = Brushes.Green;
                leaveState[4] = true;
            }
        }

        #endregion
    }
}

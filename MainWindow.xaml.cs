using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_2_optimization_calc
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AsyncChangesTB();
        }


        class EnumMessage
        {
            public static string MSGAfterSizeMatrixTrue = "Введіть свої значення у матрицю або ж продовжіть роботу з тими, що вже є";

            public static string MSGAddUserValues = "Ви ввели свої значення можете почати обрахунок";

            public static string MSGEnterMatrixA1 = "Введіть значення матриці А1 (або використайте ті, що є), для продовження нажміть кнопку \"Продовжити обчислення\" ";

            public static string MSGEnterMatrixA2 = "Введіть значення матриці А2 (або використайте ті, що є), для продовження нажміть кнопку \"Продовжити обчислення\" ";

            public static string MSGEnterMatrixB2 = "Введіть значення матриці B2 (або використайте ті, що є), для продовження нажміть кнопку \"Продовжити обчислення\" ";

            public static string MSGEnteredValuesMatrixA = "Матриця А створена";

            public static string MSGEnteredValuesMatrixA1 = "Матриця А1 створена";

            public static string MSGEnteredValuesMatrixA2 = "Матриця А2 створена";

            public static string MSGEnteredValuesMatrixB2 = "Матриця B2 створена";

            public static string MSGEnteredRandomValuesMatrixA = "Випадкові значення для Матриці А створено";

            public static string MSGEnteredRandomValuesMatrixA1 = "Випадкові значення для Матриці А1 створено";

            public static string MSGEnteredRandomValuesMatrixA2 = "Випадкові значення для Матриці А2 створено";

            public static string MSGEnteredRandomValuesMatrixB2 = "Випадкові значення для Матриці B2 створено";

            public static string MSGEndC2 = "Матриця С2 знайдена";

            public static string MSGEndY3 = "Матриця Y3 знайдена";

            public static string MSGTransposeIsEnd = "Транспонування векторів y1 та y2 завершено";


            public static string MSGMultiplyMatrixB2C2 = "Множення матриць B2 * C2 завершено ";

            public static string MSGMultiplyMatrixC2Mul2 = "Множення матриці C2 * 2 завершено ";

            public static string MSGMultiplyMatrixDiffB2C2 = "Різниця матриць B2 - результат C2*2 завершено ";

            #region Final Calc Message

            public static string Final_vecy2_mul_vecyT = "Результат y2 * y' знайдено";

            public static string Final_vecy2T_mul_vecy2 = "Результат y2' * y2 знайдено";

            public static string Final_vecy2T_mul_vecy2RESULT_mul_Y3 = "Результат (y2' * y2) * Y3 знайдено";

            public static string Final_vectorY3_POW_2 = "Результат Y3 ^ 2 знайдено";

            public static string Final_vecyT_MUL_Y3_POW_2 = "Результат y1' * Y3 ^ 2 знайдено";

            public static string Final_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2 = "Результат (y1' * Y3 ^ 2) * y2 знайдено";

            public static string Final_Y3_mul_y1 = "Результат Y3 * y1 знайдено";

            public static string Final_Y3_mul_y1T = "Результат Y3 * y1' знайдено";

            public static string Final__result_Y3_mul_y1_MUL_result_Y3_mul_y1T = "Результат Y3 * y1 * Y3 * y1' знайдено";

            public static string Final_AddPart1_2 = "1 та 2 частини додані";

            public static string Final_AddPart3_4 = "3 та 4 частини додані";

            public static string Final_AddPart3_4_1_2 = "Результат додавання частин 1 + 2 + 3 + 4 отримано";

            public static string Final_Result = "Отримано результуючу матрицю";

            #endregion
        };

        string textSizeMatrix = "", textForLostFocusTBMainMatrix, textChangeEventTb = "";
        Thread mainFonThread;

        int mainIntSizeMatrixN;

        Dictionary<string, double> vector_y = new Dictionary<string, double>();
        Dictionary<string, double> vector_y2 = new Dictionary<string, double>();

        Dictionary<string, double> vectorTransposed_y = new Dictionary<string, double>();
        Dictionary<string, double> vectorTransposed_y2 = new Dictionary<string, double>();

        
        Dictionary<string, TextBox> dictTBMatrixA = new Dictionary<string, TextBox>(),
                                    dictTBMatrixA1 = new Dictionary<string, TextBox>(),
                                    dictTBMatrixA2 = new Dictionary<string, TextBox>(),
                                    dictTBMatrixB2 = new Dictionary<string, TextBox>();

        Dictionary<string, double> dictMatrixA = new Dictionary<string, double>(),
                                   dictMatrixA1 = new Dictionary<string, double>(),
                                   dictMatrixA2 = new Dictionary<string, double>(),
                                   dictMatrixB2 = new Dictionary<string, double>(),
                                   dictMatrixY3 = new Dictionary<string, double>(),
                                   dictMatrixC2 = new Dictionary<string, double>(),
                                   finalResult = new Dictionary<string, double>();
        #region WorkWithTB
        private async void AsyncChangesTB()
        {
            await Task.Run(() => ChangesTB());
        }

        private void ProgressToDefault()
        {
            PGB.Value = 0;

            TBLPGB.Text = "0%";

            GRDProgress.Visibility = Visibility.Hidden;
        }

        private void ChangesTB()
        {
            while (GRDSizeMatrix.Visibility != Visibility.Hidden)
            {
                Dispatcher.Invoke((ThreadStart)delegate
                {
                    if (TBSizeMatrixN.Text != "")
                    {
                        BTNAcceptSizeMatrix.Visibility = Visibility.Visible;
                    }

                    else
                    {
                        BTNAcceptSizeMatrix.Visibility = Visibility.Hidden;
                    }
                });
            }
        }
        private string TBChangesSizeMatrix(TextBox tb, string text)
        {
            Regex reg = new Regex(@"^[1-9]+[0]*$");

            if (tb.Text == "")
            {
                text = "";
            }

            else
            {
                if (!reg.IsMatch(tb.Text))
                {
                    tb.Text = text;
                    tb.SelectionStart = tb.Text.Length;
                }
                else
                {
                    text = tb.Text;
                }
            }

            return text;
        }
        private string TBChangesMainMatrix(TextBox tb, string text)
        {
            Regex reg = new Regex(@"^[0-9\s]+[,]?\d*$");

            if (tb.Text == "")
            {
                text = "";
            }

            else
            {
                if (!reg.IsMatch(tb.Text))
                {
                    tb.Text = text;
                    tb.SelectionStart = tb.Text.Length;
                }
                else
                {
                    text = tb.Text;
                }
            }

            return text;
        }

        private void TBMatrixSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            textSizeMatrix = TBChangesSizeMatrix(TBSizeMatrixN, textSizeMatrix);
            if (TBSizeMatrixN.Text != "")
            {
                mainIntSizeMatrixN = int.Parse(textSizeMatrix);
            }
        }

        private bool VerifForTBGotFocus(TextBox tb)
        {
            Regex reg = new Regex(@"^[0-9]+[,]?\d*$");

            if (!reg.IsMatch(tb.Text))
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        private void tbMainMatrix_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.BorderBrush != Brushes.LimeGreen)
            {
                textForLostFocusTBMainMatrix = tb.Text;

                tb.Text = "";
            }

            textChangeEventTb = tb.Text;
        }
        private void tbMainMatrix_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Text == "" || tb.Text == ",")
            {
                tb.Text = textForLostFocusTBMainMatrix;
            }
        }

        private void TBMainMatrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (VerifForTBGotFocus(tb) == true)
            {
                tb.BorderBrush = Brushes.LimeGreen;
            }

            else
            {
                tb.BorderBrush = Brushes.Red;
            }
            textChangeEventTb = TBChangesMainMatrix(tb, textChangeEventTb);
        }

        private void WorkWithRandomTB()
        {
            TBRandValueMatrixMin.TextChanged += TBMainMatrix_TextChanged;

            TBRandValueMatrixMin.BorderThickness = new Thickness(1);

            TBRandValueMatrixMin.BorderBrush = Brushes.Orange;

            TBRandValueMatrixMin.GotFocus += tbMainMatrix_GotFocus;

            TBRandValueMatrixMin.LostFocus += tbMainMatrix_LostFocus;


            TBRandValueMatrixMax.TextChanged += TBMainMatrix_TextChanged;

            TBRandValueMatrixMax.BorderThickness = new Thickness(1);

            TBRandValueMatrixMax.BorderBrush = Brushes.Orange;

            TBRandValueMatrixMax.GotFocus += tbMainMatrix_GotFocus;

            TBRandValueMatrixMax.LostFocus += tbMainMatrix_LostFocus;
        }
        
        #endregion

        #region All Matrix

        private void CreateTBForMatrixA()
        {
            try
            {
                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        ColumnDefinition c = new ColumnDefinition();

                        c.Width = new GridLength(100, GridUnitType.Pixel);

                        RowDefinition r = new RowDefinition();

                        r.Height = new GridLength(30, GridUnitType.Pixel);

                        GRDSetMatrixTBA.ColumnDefinitions.Add(c);

                        GRDSetMatrixTBA.RowDefinitions.Add(r);

                    });
                }

                int one_percent = mainIntSizeMatrixN * mainIntSizeMatrixN / 100;

                int x = 0, counter = 0;

                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    for (int j = 0; j < mainIntSizeMatrixN; j++)
                    {

                        Dispatcher.Invoke((ThreadStart)delegate
                        {
                            TextBox tb = new TextBox();

                            tb.VerticalContentAlignment = VerticalAlignment.Center;

                            tb.HorizontalContentAlignment = HorizontalAlignment.Center;

                            tb.FontSize = 15;

                            tb.GotFocus += tbMainMatrix_GotFocus;

                            tb.LostFocus += tbMainMatrix_LostFocus;

                            tb.BorderThickness = new Thickness(1);

                            tb.BorderBrush = Brushes.Orange;

                            tb.Margin = new Thickness(3);

                            tb.Text = i + "," + j;

                            tb.TextChanged += TBMainMatrix_TextChanged;


                            string keyMatrixValue = i.ToString() + ";" + j.ToString();


                            dictTBMatrixA.Add(keyMatrixValue, tb);


                            Grid.SetRow(tb, i);

                            Grid.SetColumn(tb, j);

                            GRDSetMatrixTBA.Children.Add(tb);
                        });

                        if (one_percent > 0)
                        {
                            counter++;

                            if (counter == one_percent)
                            {
                                if (!(x >= 100))
                                {
                                    x += 1;
                                }

                                else
                                {
                                    x = 100;
                                }
                                counter = 0;

                                Dispatcher.Invoke((ThreadStart)delegate
                                {
                                    TBLPGB.Text = "Матриця A: " + x.ToString() + "%";

                                    PGB.Value += 1;

                                });
                            }
                        }

                        else
                        {
                            if (!(x >= 100))
                            {
                                x += 100;
                            }

                            Dispatcher.Invoke((ThreadStart)delegate
                            {
                                TBLPGB.Text = "Матриця A: " + x.ToString() + "%";

                                PGB.Value += 100;

                            });
                        }

                    }
                }

                dictMatrixA = GetForAllMatrixValues(dictTBMatrixA);

                Dispatcher.Invoke(new Action(delegate
                {
                    ProgressToDefault();

                    GRDSetMatrixA.Visibility = Visibility.Visible;

                    BTNRandomMatrixA.Visibility = Visibility.Visible;

                    LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredValuesMatrixA;
                    
                }));

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateTBForMatrixA1()
        {
            try
            {
                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        ColumnDefinition c = new ColumnDefinition();

                        c.Width = new GridLength(100, GridUnitType.Pixel);

                        RowDefinition r = new RowDefinition();

                        r.Height = new GridLength(30, GridUnitType.Pixel);

                        GRDSetMatrixTBA1.ColumnDefinitions.Add(c);

                        GRDSetMatrixTBA1.RowDefinitions.Add(r);

                    });
                }

                int one_percent = mainIntSizeMatrixN * mainIntSizeMatrixN / 100;

                int x = 0, counter = 0;

                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    for (int j = 0; j < mainIntSizeMatrixN; j++)
                    {

                        Dispatcher.Invoke((ThreadStart)delegate
                        {
                            TextBox tb = new TextBox();

                            tb.VerticalContentAlignment = VerticalAlignment.Center;

                            tb.HorizontalContentAlignment = HorizontalAlignment.Center;

                            tb.FontSize = 15;

                            tb.GotFocus += tbMainMatrix_GotFocus;

                            tb.LostFocus += tbMainMatrix_LostFocus;

                            tb.BorderThickness = new Thickness(1);

                            tb.BorderBrush = Brushes.Orange;

                            tb.Margin = new Thickness(3);

                            tb.Text = i + "," + j;

                            tb.TextChanged += TBMainMatrix_TextChanged;


                            string keyMatrixValue = i.ToString() + ";" + j.ToString();


                            dictTBMatrixA1.Add(keyMatrixValue, tb);


                            Grid.SetRow(tb, i);

                            Grid.SetColumn(tb, j);

                            GRDSetMatrixTBA1.Children.Add(tb);
                        });

                        if (one_percent > 0)
                        {
                            counter++;

                            if (counter == one_percent)
                            {
                                if (!(x >= 100))
                                {
                                    x += 1;
                                }

                                else
                                {
                                    x = 100;
                                }
                                counter = 0;

                                Dispatcher.Invoke((ThreadStart)delegate
                                {
                                    TBLPGB.Text = "Матриця A1: " + x.ToString() + "%";

                                    PGB.Value += 1;

                                });
                            }
                        }

                        else
                        {
                            if (!(x >= 100))
                            {
                                x += 100;
                            }

                            Dispatcher.Invoke((ThreadStart)delegate
                            {
                                TBLPGB.Text = "Матриця A1: " + x.ToString() + "%";

                                PGB.Value += 100;

                            });
                        }

                    }
                }

                dictMatrixA1 = GetForAllMatrixValues(dictTBMatrixA1);

                Dispatcher.Invoke(new Action(delegate
                {
                    ProgressToDefault();

                    GRDSetMatrixA1.Visibility = Visibility.Visible;

                    BTNRandomMatrixA1.Visibility = Visibility.Visible;

                    LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredValuesMatrixA1;
                    
                }));

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateTBForMatrixA2()
        {
            try
            {
                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        ColumnDefinition c = new ColumnDefinition();

                        c.Width = new GridLength(100, GridUnitType.Pixel);

                        RowDefinition r = new RowDefinition();

                        r.Height = new GridLength(30, GridUnitType.Pixel);

                        GRDSetMatrixTBA2.ColumnDefinitions.Add(c);

                        GRDSetMatrixTBA2.RowDefinitions.Add(r);

                    });
                }

                int one_percent = mainIntSizeMatrixN * mainIntSizeMatrixN / 100;

                int x = 0, counter = 0;

                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    for (int j = 0; j < mainIntSizeMatrixN; j++)
                    {

                        Dispatcher.Invoke((ThreadStart)delegate
                        {
                            TextBox tb = new TextBox();

                            tb.VerticalContentAlignment = VerticalAlignment.Center;

                            tb.HorizontalContentAlignment = HorizontalAlignment.Center;

                            tb.FontSize = 15;

                            tb.GotFocus += tbMainMatrix_GotFocus;

                            tb.LostFocus += tbMainMatrix_LostFocus;

                            tb.BorderThickness = new Thickness(1);

                            tb.BorderBrush = Brushes.Orange;

                            tb.Margin = new Thickness(3);

                            tb.Text = i + "," + j;

                            tb.TextChanged += TBMainMatrix_TextChanged;


                            string keyMatrixValue = i.ToString() + ";" + j.ToString();


                            dictTBMatrixA2.Add(keyMatrixValue, tb);


                            Grid.SetRow(tb, i);

                            Grid.SetColumn(tb, j);

                            GRDSetMatrixTBA2.Children.Add(tb);
                        });

                        if (one_percent > 0)
                        {
                            counter++;

                            if (counter == one_percent)
                            {
                                if (!(x >= 100))
                                {
                                    x += 1;
                                }

                                else
                                {
                                    x = 100;
                                }
                                counter = 0;

                                Dispatcher.Invoke((ThreadStart)delegate
                                {
                                    TBLPGB.Text = "Матриця A2: " + x.ToString() + "%";

                                    PGB.Value += 1;

                                });
                            }
                        }

                        else
                        {
                            if (!(x >= 100))
                            {
                                x += 100;
                            }

                            Dispatcher.Invoke((ThreadStart)delegate
                            {
                                TBLPGB.Text = "Матриця A2: " + x.ToString() + "%";

                                PGB.Value += 100;

                            });
                        }

                    }
                }

                dictMatrixA2 = GetForAllMatrixValues(dictTBMatrixA2);

                Dispatcher.Invoke(new Action(delegate
                {
                    ProgressToDefault();

                    GRDSetMatrixA2.Visibility = Visibility.Visible;

                    BTNRandomMatrixA2.Visibility = Visibility.Visible;

                    LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredValuesMatrixA2;
                    
                }));

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateTBForMatrixB2()
        {
            try
            {
                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        ColumnDefinition c = new ColumnDefinition();

                        c.Width = new GridLength(100, GridUnitType.Pixel);

                        RowDefinition r = new RowDefinition();

                        r.Height = new GridLength(30, GridUnitType.Pixel);

                        GRDSetMatrixTBB2.ColumnDefinitions.Add(c);

                        GRDSetMatrixTBB2.RowDefinitions.Add(r);

                    });
                }

                int one_percent = mainIntSizeMatrixN * mainIntSizeMatrixN / 100;

                int x = 0, counter = 0;

                for (int i = 0; i < mainIntSizeMatrixN; i++)
                {
                    for (int j = 0; j < mainIntSizeMatrixN; j++)
                    {

                        Dispatcher.Invoke((ThreadStart)delegate
                        {
                            TextBox tb = new TextBox();

                            tb.VerticalContentAlignment = VerticalAlignment.Center;

                            tb.HorizontalContentAlignment = HorizontalAlignment.Center;

                            tb.FontSize = 15;

                            tb.GotFocus += tbMainMatrix_GotFocus;

                            tb.LostFocus += tbMainMatrix_LostFocus;

                            tb.BorderThickness = new Thickness(1);

                            tb.BorderBrush = Brushes.Orange;

                            tb.Margin = new Thickness(3);

                            tb.Text = i + "," + j;

                            tb.TextChanged += TBMainMatrix_TextChanged;


                            string keyMatrixValue = i.ToString() + ";" + j.ToString();


                            dictTBMatrixB2.Add(keyMatrixValue, tb);


                            Grid.SetRow(tb, i);

                            Grid.SetColumn(tb, j);

                            GRDSetMatrixTBB2.Children.Add(tb);
                        });

                        if (one_percent > 0)
                        {
                            counter++;

                            if (counter == one_percent)
                            {
                                if (!(x >= 100))
                                {
                                    x += 1;
                                }

                                else
                                {
                                    x = 100;
                                }
                                counter = 0;

                                Dispatcher.Invoke((ThreadStart)delegate
                                {
                                    TBLPGB.Text = "Матриця B2: " + x.ToString() + "%";

                                    PGB.Value += 1;

                                });
                            }
                        }

                        else
                        {
                            if (!(x >= 100))
                            {
                                x += 100;
                            }

                            Dispatcher.Invoke((ThreadStart)delegate
                            {
                                TBLPGB.Text = "Матриця B2: " + x.ToString() + "%";

                                PGB.Value += 100;

                            });
                        }

                    }
                }

                dictMatrixB2 = GetForAllMatrixValues(dictTBMatrixB2);

                Dispatcher.Invoke(new Action(delegate
                {
                    ProgressToDefault();

                    GRDSetMatrixB2.Visibility = Visibility.Visible;

                    BTNRandomMatrixB2.Visibility = Visibility.Visible;

                    LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredValuesMatrixB2;
                    
                }));

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StartThreadForCreateTBAllMatrix()
        {
            Thread tCreateMatrixA = new Thread(CreateTBForMatrixA);

            tCreateMatrixA.IsBackground = true;

            tCreateMatrixA.Start();

            tCreateMatrixA.Join();
            
            Dispatcher.Invoke((ThreadStart)delegate
            {
                GRDProgress.Visibility = Visibility.Visible;
            });



            Thread tCreateMatrixA1 = new Thread(CreateTBForMatrixA1);

            tCreateMatrixA1.IsBackground = true;

            tCreateMatrixA1.Start();

            tCreateMatrixA1.Join();

            Dispatcher.Invoke((ThreadStart)delegate
            {
                GRDProgress.Visibility = Visibility.Visible;
            });


            Thread tCreateMatrixA2 = new Thread(CreateTBForMatrixA2);

            tCreateMatrixA2.IsBackground = true;

            tCreateMatrixA2.Start();

            tCreateMatrixA2.Join();

            Dispatcher.Invoke((ThreadStart)delegate
            {
                GRDProgress.Visibility = Visibility.Visible;
            });


            Thread tCreateMatrixB2 = new Thread(CreateTBForMatrixB2);

            tCreateMatrixB2.IsBackground = true;

            tCreateMatrixB2.Start();

            tCreateMatrixB2.Join();

            Dispatcher.Invoke((ThreadStart)delegate
            {
                GRDRandomMatrix.Visibility = Visibility.Visible;

                GRDCalculation.Visibility = Visibility.Visible;

                WorkWithRandomTB();
            });
        }

        private async void AsyncCreationTBAllMatrix()
        {
            await Task.Run(() => StartThreadForCreateTBAllMatrix());
        }

        private Dictionary<string, double> GetForAllMatrixValues(Dictionary<string, TextBox> dict)
        {
            Dictionary<string, double> dictRes = new Dictionary<string, double>();

            double value = 0;

            for (int i = 0; i < dict.Count; i++)
            {
                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        var item = dict.ElementAt(i);

                        value = double.Parse(item.Value.Text);

                        dictRes.Add(item.Key, value);
                    });
                
            }

            //foreach (var i in dictRes)
            //{
            //    MessageBox.Show("Value: " + i.Value);
            //}

            return dictRes;
        }

        private void RandomValuesToAllMatrix(Dictionary<string, TextBox> dict, string nameMatrix)
        {
            int one_percent = dict.Count / 100, randomMin = 0, randomMax = 0;

            double x = 0, counter = 0;

            Dispatcher.Invoke((ThreadStart)delegate
            {
                randomMin = int.Parse(TBRandValueMatrixMin.Text);

                randomMax = int.Parse(TBRandValueMatrixMax.Text);
            });

            Random rand = new Random();

            KeyValuePair<string, TextBox> item;

            string randDigit;

            for (int i = 0; i < dict.Count; i++)
            {
                randDigit = (rand.Next(randomMin, randomMax)).ToString();

                Dispatcher.Invoke((ThreadStart)delegate {
                    item = dict.ElementAt(i);

                    item.Value.Text = randDigit;
                });

                if (one_percent > 0)
                {
                    counter++;

                    if (counter == one_percent)
                    {
                        if (!(x >= 100))
                        {
                            x += 1;
                        }

                        else
                        {
                            x = 100;
                        }
                        counter = 0;

                        Dispatcher.Invoke((ThreadStart)delegate
                        {
                            TBLPGB.Text = "Матриця " + nameMatrix + ": " + x.ToString() + "%";

                            PGB.Value += 1;

                        });
                    }
                }

                else
                {
                    if (!(x >= 100))
                    {
                        x += 100;
                    }

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        TBLPGB.Text = "Матриця " + nameMatrix + ": " + x.ToString() + "%";

                        PGB.Value += 100;

                    });
                }

            }


            switch (nameMatrix)
            {
                case "A":
                    { dictMatrixA = GetForAllMatrixValues(dict); }
                    break;

                case "A1":
                    { dictMatrixA1 = GetForAllMatrixValues(dict); }
                    break;

                case "A2":
                    { dictMatrixA2 = GetForAllMatrixValues(dict); }
                    break;

                case "B2":
                    { dictMatrixB2 = GetForAllMatrixValues(dict); }
                    break;
            }


            Dispatcher.Invoke(new Action(delegate
            {
                ProgressToDefault();
              
                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                if (nameMatrix == "A")
                {
                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredRandomValuesMatrixA;
                }

                if (nameMatrix == "A1")
                {
                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredRandomValuesMatrixA1;
                }

                if (nameMatrix == "A2")
                {
                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredRandomValuesMatrixA2;
                }

                if (nameMatrix == "B2")
                {
                    LBLWhichChapterCalculation.Content = EnumMessage.MSGEnteredRandomValuesMatrixB2;
                }
                
            }));        
        }

        private void ClickBTNRandomA(object sender, RoutedEventArgs e)
        {
            GRDProgress.Visibility = Visibility.Visible;

            Thread tMatrixA = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixA, "A"));

            tMatrixA.IsBackground = true;

            tMatrixA.Start();
        }

        private void ClickBTNRandomMatrixA1(object sender, RoutedEventArgs e)
        {
            GRDProgress.Visibility = Visibility.Visible;

            Thread tMatrixA1 = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixA1, "A1"));

            tMatrixA1.IsBackground = true;

            tMatrixA1.Start();
        }

        private void ClickBTNRandomMatrixA2(object sender, RoutedEventArgs e)
        {
            GRDProgress.Visibility = Visibility.Visible;

            Thread tMatrixA2 = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixA2, "A2"));

            tMatrixA2.IsBackground = true;

            tMatrixA2.Start();
        }

        private void ClickBTNRandomMatrixB2(object sender, RoutedEventArgs e)
        {
            GRDProgress.Visibility = Visibility.Visible;

            Thread tMatrixB2 = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixB2, "B2"));

            tMatrixB2.IsBackground = true;

            tMatrixB2.Start();
        }

        private void RandomValuesForAll()
        {
            Thread tMatrixA = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixA, "A"));

            tMatrixA.IsBackground = true;

            tMatrixA.Start();

            tMatrixA.Join();

            Dispatcher.Invoke((ThreadStart)delegate {
                GRDProgress.Visibility = Visibility.Visible;
            });


            Thread tMatrixA1 = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixA1, "A1"));

            tMatrixA1.IsBackground = true;

            tMatrixA1.Start();

            tMatrixA1.Join();

            Dispatcher.Invoke((ThreadStart)delegate {
                GRDProgress.Visibility = Visibility.Visible;
            });


            Thread tMatrixA2 = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixA2, "A2"));

            tMatrixA2.IsBackground = true;

            tMatrixA2.Start();

            tMatrixA2.Join();

            Dispatcher.Invoke((ThreadStart)delegate {
                GRDProgress.Visibility = Visibility.Visible;
            });

            Thread tMatrixB2 = new Thread(() => RandomValuesToAllMatrix(dictTBMatrixB2, "B2"));

            tMatrixB2.IsBackground = true;

            tMatrixB2.Start();

            tMatrixB2.Join();
        }

        private async void AsyncRandomValuesForAll()
        {
            await Task.Run(() => RandomValuesForAll());
        }
        
        private void BTNSizeMarix(object sender, RoutedEventArgs e)
        {
            GRDSizeMatrix.Visibility = Visibility.Hidden;

            GRDProgress.Visibility = Visibility.Visible;

            SVAllMatrix.Visibility = Visibility.Visible;

            AsyncCreationTBAllMatrix();           
        }

        private void RandomValuesToMatrix()
        {
            AsyncRandomValuesForAll();
        }

        private void BTNClickRandomValuesOfMatrix(object sender, RoutedEventArgs e)
        {
            GRDProgress.Visibility = Visibility.Visible;

            mainFonThread = new Thread(RandomValuesToMatrix);

            mainFonThread.SetApartmentState(ApartmentState.STA);

            mainFonThread.Priority = ThreadPriority.Highest;

            mainFonThread.IsBackground = true;

            mainFonThread.Start();
        }

        private void MITryAgain(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();

            mw.Show();

            this.Close();
        }

        private void MIClickTema(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Використання функціональної декомпозиції для розв'язку обчислювальних задач.", "Тема лабораторної");
        }

        #endregion
        
        #region Main Calculation
        private void Calc_y1()
        {
            int one_percent = (mainIntSizeMatrixN * mainIntSizeMatrixN) / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            List<double> vector_b = new List<double>();

            for (int i = 1; i < mainIntSizeMatrixN + 1; i++)
            {
                if ((i - 1) % 2 == 0)
                {
                    vector_b.Add((double)11 * (i * i));
                }
                else
                {
                    vector_b.Add((double)11 / i);
                }
            }

            string key = "";

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                double vectorElemRes = 0;
                
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    vectorElemRes += dictMatrixA[i + ";" + j] * vector_b[j];

                    ProgressForCalculation("y1", additionalCount, one_percent, x, counter, out x, out counter);
                }
                key = i + ";" + 0;

                vector_y.Add(key, vectorElemRes);
            }

            //foreach (var i in vector_y)
            //{
            //    MessageBox.Show("Key: " + i.Key + " Value: " + i.Value);
            //}

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    if (j != 0)
                    {
                        key = i + ";" + j;

                        vector_y.Add(key, 0);
                    }
                }

            }

            //MessageBox.Show(vector_y[0 + ";" + 0]  + "  " + vector_y[0 + ";" + 1] + "  " + vector_y[0 + ";" + 2] + "\n"
            //                + vector_y[1 + ";" + 0] + "  " + vector_y[1 + ";" + 1] + "  " + vector_y[1 + ";" + 2] + "\n"
            //                + vector_y[2 + ";" + 0] + "  " + vector_y[2 + ";" + 1] + "  " + vector_y[2 + ";" + 2]);



            Dispatcher.Invoke((ThreadStart)delegate
            {
                TBLCalc_y1.Text = "Завершено";

                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                LBLWhichChapterCalculation.Content = EnumMessage.MSGEnterMatrixA1;

            });
            
        }

        private void Calc_y2()
        {
            int one_percent = (mainIntSizeMatrixN * mainIntSizeMatrixN) / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            List<double> vector_b1 = new List<double>();

            List<double> vector_c1 = new List<double>();

            //Random rand = new Random();

            //for (int i = 1; i < mainIntSizeMatrixN + 1; i++)
            //{
            //    vector_b1.Add(rand.Next(1, 10));

            //    vector_c1.Add(rand.Next(1, 10));
            //}

            vector_b1.Add(5);
            vector_b1.Add(9);
            vector_b1.Add(4);


            vector_c1.Add(12);
            vector_c1.Add(7);
            vector_c1.Add(2);

            string key = "";

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                double vectorElemRes = 0;

                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    vectorElemRes += dictMatrixA1[i + ";" + j] * (vector_b1[j] - 2 * vector_c1[j]);

                    ProgressForCalculation("y2", additionalCount, one_percent, x, counter, out x, out counter);
                }

                key = i + ";" + 0;

                vector_y2.Add(key, vectorElemRes);
            }

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    if (j != 0)
                    {
                        key = i + ";" + j;

                        vector_y2.Add(key, 0);
                    }
                }

            }

            //MessageBox.Show(vector_y2[0 + ";" + 0] + "  " + vector_y2[0 + ";" + 1] + "  " + vector_y2[0 + ";" + 2] + "\n"
            //               + vector_y2[1 + ";" + 0] + "  " + vector_y2[1 + ";" + 1] + "  " + vector_y2[1 + ";" + 2] + "\n"
            //               + vector_y2[2 + ";" + 0] + "  " + vector_y2[2 + ";" + 1] + "  " + vector_y2[2 + ";" + 2]);


            Dispatcher.Invoke((ThreadStart)delegate
            {
                TBLCalc_y2.Text = "Завершено";

                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                LBLWhichChapterCalculation.Content = EnumMessage.MSGEnterMatrixA2;
            });
        }

        private void ProgressForCalculation(string partName, double additionalCount, int one_percent, double x, double counter, out double X, out double Counter)
        {
            X = x;

            Counter = 0;

            Thread.Sleep(10);

            if (one_percent > 0)
            {
                counter++;

                if (counter == one_percent)
                {
                    if (!(x >= 100))
                    {
                        x += 1;

                        X = x;
                    }

                    else
                    {
                        x = 100;

                        X = x;
                    }

                    counter = 0;

                    Dispatcher.Invoke((ThreadStart)delegate
                    {
                        switch (partName)
                        {
                            case "y1":
                                {
                                    TBLCalc_y1.Text = x.ToString() + "%";

                                    PGBCalc_y1.Value += 1;
                                } break;

                            case "y2":
                                {
                                    TBLCalc_y2.Text = x.ToString() + "%";

                                    PGBCalc_y2.Value += 1;
                                }
                                break;

                            case "C2":
                                {
                                    TBLCalc_C2.Text = x.ToString() + "%";

                                    PGBCalc_C2.Value += 1;
                                }
                                break;


                            case "Y3":
                                {
                                    TBLCalc_Y3.Text = x.ToString() + "%";

                                    PGBCalc_Y3.Value += 1;
                                }
                                break;

                            case "TV":
                                {
                                    TBLCalc_Transpose_y.Text = x.ToString() + "%";

                                    PGBCalc_Transpose_y.Value += 1;
                                }
                                break;
                        }
                    });
                }

                Counter = counter;
            }

            else
            {
                if (!(x >= 100 - additionalCount))
                {
                    x += additionalCount;

                    X = x;
                }

                else
                {
                    x = 100;

                    X = x;
                }

                Dispatcher.Invoke((ThreadStart)delegate
                {

                    switch (partName)
                    {
                        case "y1":
                            {
                                TBLCalc_y1.Text = x.ToString() + "%";

                                PGBCalc_y1.Value += additionalCount;
                            }
                            break;

                        case "y2":
                            {
                                TBLCalc_y2.Text = x.ToString() + "%";

                                PGBCalc_y2.Value += additionalCount;
                            }
                            break;

                        case "C2":
                            {
                                TBLCalc_C2.Text = x.ToString() + "%";

                                PGBCalc_C2.Value += additionalCount;
                            }
                            break;

                        case "Y3":
                            {
                                TBLCalc_Y3.Text = x.ToString() + "%";

                                PGBCalc_Y3.Value += additionalCount;
                            }
                            break;

                        case "TV":
                            {
                                TBLCalc_Transpose_y.Text = x.ToString() + "%";

                                PGBCalc_Transpose_y.Value += additionalCount;
                            }
                            break;
                    }

                });


            }
        }

        private void Calc_MatrixC2()
        {
            int one_percent = (mainIntSizeMatrixN * mainIntSizeMatrixN) / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            string key = "";

            double value = 0;

            int newI = 0;

            int newJ = 0;

            for (int i = 1; i <= mainIntSizeMatrixN; i++)
            {
                for (int j = 1; j <= mainIntSizeMatrixN; j++)
                {
                    newI = i - 1;

                    newJ = j - 1;

                    key = newI + ";" + newJ;
                    
                    value = (double)1 / (i*i + j);

                   // MessageBox.Show("Value C2: " + value);

                    dictMatrixC2.Add(key, value);

                    ProgressForCalculation("C2", additionalCount, one_percent, x, counter, out x, out counter);
                }
            }

            //foreach (var i in dictMatrixC2)
            //{
            //    MessageBox.Show("Value C2: " + i.Value);
            //}

            Dispatcher.Invoke((ThreadStart)delegate
            {
                TBLCalc_C2.Text = "Завершено";

                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                LBLWhichChapterCalculation.Content = EnumMessage.MSGEndC2;

            });
        }

        private void Calc_MatrixY3()
        {
            Dictionary<string, double> dictProductC2_2 = new Dictionary<string, double>();

            Dictionary<string, double> dictDifferenceB2C22 = new Dictionary<string, double>();

            dictProductC2_2 = MulMatrixOnDigit(dictMatrixC2, 2, EnumMessage.MSGMultiplyMatrixC2Mul2);

            dictDifferenceB2C22 = DifferenceMarix(dictMatrixB2, dictProductC2_2, EnumMessage.MSGMultiplyMatrixDiffB2C2);

            //foreach (var i in dictDifferenceB2C22)
            //{
            //    MessageBox.Show("Key: " + i.Key + " Value: " + i.Value);
            //}


            dictMatrixY3 = MultiplyMatrix(dictMatrixA2, dictDifferenceB2C22, EnumMessage.MSGEndY3);

            //foreach (var i in dictMatrixY3)
            //{
            //    MessageBox.Show("Key: " + i.Key + " Value: " + i.Value);
            //}

            Dispatcher.Invoke((ThreadStart)delegate
            {
                TBLCalc_Y3.Text = "Завершено";

                PGBCalc_Y3.Value = 100;

                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                LBLWhichChapterCalculation.Content = EnumMessage.MSGEndY3;
            });
        }

        private void Transpose_y1_and_y2()
        {
            int one_percent = mainIntSizeMatrixN / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            string key = "", yek = "";

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    key = i + ";" + j;

                    yek = j + ";" + i;

                    vectorTransposed_y[key] = vector_y[yek];

                    vectorTransposed_y2[key] = vector_y2[yek];

                    ProgressForCalculation("TV", additionalCount, one_percent, x, counter, out x, out counter);
                }
            }

            //MessageBox.Show(vectorTransposed_y[0 + ";" + 0] + "  " + vectorTransposed_y[0 + ";" + 1] + "  " + vectorTransposed_y[0 + ";" + 2] + "\n"
            //               + vectorTransposed_y[1 + ";" + 0] + "  " + vectorTransposed_y[1 + ";" + 1] + "  " + vectorTransposed_y[1 + ";" + 2] + "\n"
            //               + vectorTransposed_y[2 + ";" + 0] + "  " + vectorTransposed_y[2 + ";" + 1] + "  " + vectorTransposed_y[2 + ";" + 2]);


            //MessageBox.Show(vectorTransposed_y2[0 + ";" + 0] + "  " + vectorTransposed_y2[0 + ";" + 1] + "  " + vectorTransposed_y2[0 + ";" + 2] + "\n"
            //              + vectorTransposed_y2[1 + ";" + 0] + "  " + vectorTransposed_y2[1 + ";" + 1] + "  " + vectorTransposed_y2[1 + ";" + 2] + "\n"
            //              + vectorTransposed_y2[2 + ";" + 0] + "  " + vectorTransposed_y2[2 + ";" + 1] + "  " + vectorTransposed_y2[2 + ";" + 2]);


            Dispatcher.Invoke((ThreadStart)delegate
            {
                PGBCalc_Transpose_y.Value = 100;

                TBLCalc_Transpose_y.Text = "Завершено";

                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

                LBLWhichChapterCalculation.Content = EnumMessage.MSGTransposeIsEnd;
            });
        }
        
       
        private Dictionary<string, double> MultiplyMatrix(Dictionary<string, double> dict1, Dictionary<string, double> dict2, string namesMatrix)
        {
            Dictionary<string, double> dictRes = new Dictionary<string, double>();

            double value = 0;

            int one_percent = (mainIntSizeMatrixN * mainIntSizeMatrixN) / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            string key = "";

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    key = i + ";" + j;

                    value = 0;
                    for (int k = 0;k < mainIntSizeMatrixN;k++)
                    {
                        value += dict1[i + ";" + k] * dict2[k + ";" + j];
                    }
                    
                    dictRes.Add(key, value);

                    ProgressForCalculation("Y3", additionalCount, one_percent, x, counter, out x, out counter);
                }
            }

            Dispatcher.Invoke((ThreadStart)delegate
            {
                TBLCalc_Y3.Text = "0%";

                PGBCalc_Y3.Value = 0;

                LBLWhichChapterCalculation.Foreground = Brushes.Orange;

                LBLWhichChapterCalculation.Content = namesMatrix;
                
            });

            return dictRes;
        }

        private Dictionary<string, double> MulMatrixOnDigit(Dictionary<string, double> dict1, double digit, string message)
        {
            Dictionary<string, double> dictRes = new Dictionary<string, double>();

            int one_percent = (mainIntSizeMatrixN * mainIntSizeMatrixN) / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    dictRes.Add(i + ";" + j, dict1[i + ";" + j] * digit);

                    ProgressForCalculation("Y3", additionalCount, one_percent, x, counter, out x, out counter);
                }
            }

            Dispatcher.Invoke((ThreadStart)delegate
            {
                LBLWhichChapterCalculation.Foreground = Brushes.Orange;

                LBLWhichChapterCalculation.Content = message;
            });

            return dictRes;
        }

        private Dictionary<string, double> DifferenceMarix(Dictionary<string, double> dict1, Dictionary<string, double> dict2, string message)
        {
            Dictionary<string, double> dictRes = new Dictionary<string, double>();

            int one_percent = (mainIntSizeMatrixN * mainIntSizeMatrixN) / 100;

            double x = 0, counter = 0;

            double additionalCount = 100 / mainIntSizeMatrixN;

            for (int i = 0;i < mainIntSizeMatrixN;i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    dictRes[i + ";" + j] = dict1[i + ";" + j] - dict2[i + ";" + j];

                    ProgressForCalculation("Y3", additionalCount, one_percent, x, counter, out x, out counter);
                }
            }

            Dispatcher.Invoke((ThreadStart)delegate
            {
                TBLCalc_Y3.Text = "0%";

                PGBCalc_Y3.Value = 0;

                LBLWhichChapterCalculation.Foreground = Brushes.Orange;

                LBLWhichChapterCalculation.Content = message;
            });

            return dictRes;
        }

        private Dictionary<string, double> AdditionalMarix(Dictionary<string, double> dict1, Dictionary<string, double> dict2, string message)
        {
            Dictionary<string, double> dictRes = new Dictionary<string, double>();
            
            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    dictRes[i + ";" + j] = dict1[i + ";" + j] + dict2[i + ";" + j];

                    Thread.Sleep(10);

                }
            }

            Dispatcher.Invoke((ThreadStart)delegate
            {
                LBLWhichChapterCalculation.Foreground = Brushes.Orange;

                LBLWhichChapterCalculation.Content = message;
            });

            return dictRes;
        }
        
        private Dictionary<string, double> MultiplyMatrixFinal(Dictionary<string, double> dict1, Dictionary<string, double> dict2, string namesMatrix)
        {
            Dictionary<string, double> dictRes = new Dictionary<string, double>();
            
            double value = 0;

           
            string key = "";

            for (int i = 0; i < mainIntSizeMatrixN; i++)
            {
                for (int j = 0; j < mainIntSizeMatrixN; j++)
                {
                    key = i + ";" + j;

                    value = 0;

                    for (int k = 0; k < mainIntSizeMatrixN; k++)
                    {
                        value += dict1[i + ";" + k] * dict2[k + ";" + j];
                    }

                    dictRes.Add(key, value);
                }
            }

            Dispatcher.Invoke((ThreadStart)delegate
            {
                LBLWhichChapterCalculation.Foreground = Brushes.Orange;

                LBLWhichChapterCalculation.Content = namesMatrix;

            });

            return dictRes;
        }
        
        private void FinalCalculation()
        {
            double K1 = 0, K2 = 0;

            Dispatcher.Invoke((ThreadStart) delegate {
                GRDFinalCalc.Visibility = Visibility.Visible;

                K1 = double.Parse(TBK1.Text);

                K2 = double.Parse(TBK2.Text);
            });

            Dictionary<string, double> resultMatrix = new Dictionary<string, double>();


            Dictionary<string, double> result_vecy2_mul_vecyT = new Dictionary<string, double>();


            Dictionary<string, double> result_vecy2T_mul_vecy2 = new Dictionary<string, double>();
            Dictionary<string, double> result_vecy2T_mul_vecy2RESULT_mul_Y3 = new Dictionary<string, double>();


            Dictionary<string, double> result_matrixY3_POW_2 = new Dictionary<string, double>();
            Dictionary<string, double> result_vecyT_MUL_Y3_POW_2 = new Dictionary<string, double>();
            Dictionary<string, double> result_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2 = new Dictionary<string, double>();


            Dictionary<string, double> result_Y3_mul_y1 = new Dictionary<string, double>();
            Dictionary<string, double> result_Y3_mul_y1T = new Dictionary<string, double>();
            Dictionary<string, double> result__result_Y3_mul_y1_MUL_result_Y3_mul_y1T = new Dictionary<string, double>();


            Dictionary<string, double> result_AddPart1_2 = new Dictionary<string, double>();
            Dictionary<string, double> result_AddPart3_4 = new Dictionary<string, double>();
            Dictionary<string, double> result_AddPart1_2_3_4 = new Dictionary<string, double>();

            finalResult = new Dictionary<string, double>();

            #region
            //1 part

            Thread t = new Thread(new ThreadStart(delegate
            {
                result_vecy2_mul_vecyT = MultiplyMatrixFinal(vector_y2, vectorTransposed_y, EnumMessage.Final_vecy2_mul_vecyT);
                
                result_vecy2_mul_vecyT = MulMatrixOnDigit(result_vecy2_mul_vecyT, K1, EnumMessage.Final_vecy2_mul_vecyT);
                
            }));

            t.Start();

            //2 part

            Thread t2 = new Thread(new ThreadStart(delegate
            {
                result_vecy2T_mul_vecy2 = MultiplyMatrixFinal(vector_y2, vectorTransposed_y2, EnumMessage.Final_vecy2T_mul_vecy2);
               
                result_vecy2T_mul_vecy2RESULT_mul_Y3 = MultiplyMatrixFinal(dictMatrixY3, result_vecy2T_mul_vecy2, EnumMessage.Final_vecy2T_mul_vecy2RESULT_mul_Y3);
                
                result_vecy2T_mul_vecy2RESULT_mul_Y3 = MulMatrixOnDigit(result_vecy2T_mul_vecy2RESULT_mul_Y3, K1, EnumMessage.Final_vecy2T_mul_vecy2);
                
            }));

            t2.Start();


            //3 part

            Thread t3 = new Thread(new ThreadStart(delegate
            {
                result_matrixY3_POW_2 = MultiplyMatrixFinal(dictMatrixY3, dictMatrixY3, EnumMessage.Final_vectorY3_POW_2);
                
                result_vecyT_MUL_Y3_POW_2 = MultiplyMatrixFinal(result_matrixY3_POW_2, vectorTransposed_y, EnumMessage.Final_vecyT_MUL_Y3_POW_2);
                
                result_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2 = MultiplyMatrixFinal(vector_y2, result_vecyT_MUL_Y3_POW_2, EnumMessage.Final_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2);
                
                result_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2 = MulMatrixOnDigit(result_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2, K2, EnumMessage.Final_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2);
                
            }));

            t3.Start();



            //5 part

            Thread t5 = new Thread(new ThreadStart(delegate
            {
                result_Y3_mul_y1 = MultiplyMatrixFinal(vector_y, dictMatrixY3, EnumMessage.Final_Y3_mul_y1);
                
                result_Y3_mul_y1T = MultiplyMatrixFinal(dictMatrixY3, vectorTransposed_y, EnumMessage.Final_Y3_mul_y1T);
                
                result__result_Y3_mul_y1_MUL_result_Y3_mul_y1T = MultiplyMatrixFinal(result_Y3_mul_y1, result_Y3_mul_y1T, EnumMessage.Final__result_Y3_mul_y1_MUL_result_Y3_mul_y1T);
                
                result__result_Y3_mul_y1_MUL_result_Y3_mul_y1T = MulMatrixOnDigit(result__result_Y3_mul_y1_MUL_result_Y3_mul_y1T, K2, EnumMessage.Final__result_Y3_mul_y1_MUL_result_Y3_mul_y1T);
                
            }));

            t5.Start();



            t.Join(); t2.Join(); t3.Join(); t5.Join();

            //6 part

            Thread t6 = new Thread(new ThreadStart(delegate
            {
                result_AddPart1_2 = AdditionalMarix(result_vecy2_mul_vecyT, result_vecy2T_mul_vecy2RESULT_mul_Y3, EnumMessage.Final_AddPart1_2);
                
                result_AddPart3_4 = AdditionalMarix(result_vecy2T_MUL_result_vecyT_MUL_Y3_POW_2, result__result_Y3_mul_y1_MUL_result_Y3_mul_y1T, EnumMessage.Final_AddPart3_4);
                
                result_AddPart1_2_3_4 = AdditionalMarix(result_AddPart1_2, result_AddPart3_4, EnumMessage.Final_AddPart3_4_1_2);
                
                finalResult = AdditionalMarix(result_AddPart1_2_3_4, dictMatrixY3, EnumMessage.Final_Result);
                
            }));

            t6.Start();

            t6.Join();

            #endregion

            Dispatcher.Invoke((ThreadStart)delegate
            {

                TBLCalcFinal.Text = "Завершено";

                PGBCalcFinal.IsIndeterminate = false;

                PGBCalcFinal.Value = 100;

                LBLWhichChapterCalculation.Foreground = Brushes.LimeGreen;

            });

        }

        private void GetAllValues()
        {
            dictMatrixA = GetForAllMatrixValues(dictTBMatrixA);

            dictMatrixA1 = GetForAllMatrixValues(dictTBMatrixA1);

            dictMatrixA2 = GetForAllMatrixValues(dictTBMatrixA2);

            dictMatrixB2 = GetForAllMatrixValues(dictTBMatrixB2);
        }

        private void SetAllValues()
        {
            dictTBMatrixA[0 + ";" + 0].Text = "4";
            dictTBMatrixA[0 + ";" + 1].Text = "7";
            dictTBMatrixA[0 + ";" + 2].Text = "7";
            dictTBMatrixA[1 + ";" + 0].Text = "27";
            dictTBMatrixA[1 + ";" + 1].Text = "28";
            dictTBMatrixA[1 + ";" + 2].Text = "22";
            dictTBMatrixA[2 + ";" + 0].Text = "13";
            dictTBMatrixA[2 + ";" + 1].Text = "9";
            dictTBMatrixA[2 + ";" + 2].Text = "20";



            dictTBMatrixA1[0 + ";" + 0].Text = "18";
            dictTBMatrixA1[0 + ";" + 1].Text = "9";
            dictTBMatrixA1[0 + ";" + 2].Text = "18";
            dictTBMatrixA1[1 + ";" + 0].Text = "9";
            dictTBMatrixA1[1 + ";" + 1].Text = "1";
            dictTBMatrixA1[1 + ";" + 2].Text = "21";
            dictTBMatrixA1[2 + ";" + 0].Text = "5";
            dictTBMatrixA1[2 + ";" + 1].Text = "6";
            dictTBMatrixA1[2 + ";" + 2].Text = "6";



            dictTBMatrixA2[0 + ";" + 0].Text = "20";
            dictTBMatrixA2[0 + ";" + 1].Text = "27";
            dictTBMatrixA2[0 + ";" + 2].Text = "16";
            dictTBMatrixA2[1 + ";" + 0].Text = "26";
            dictTBMatrixA2[1 + ";" + 1].Text = "7";
            dictTBMatrixA2[1 + ";" + 2].Text = "21";
            dictTBMatrixA2[2 + ";" + 0].Text = "9";
            dictTBMatrixA2[2 + ";" + 1].Text = "10";
            dictTBMatrixA2[2 + ";" + 2].Text = "15";



            dictTBMatrixB2[0 + ";" + 0].Text = "24";
            dictTBMatrixB2[0 + ";" + 1].Text = "6";
            dictTBMatrixB2[0 + ";" + 2].Text = "17";
            dictTBMatrixB2[1 + ";" + 0].Text = "23";
            dictTBMatrixB2[1 + ";" + 1].Text = "4";
            dictTBMatrixB2[1 + ";" + 2].Text = "28";
            dictTBMatrixB2[2 + ";" + 0].Text = "4";
            dictTBMatrixB2[2 + ";" + 1].Text = "4";
            dictTBMatrixB2[2 + ";" + 2].Text = "20";
        }

        private void MainCalculationNotChecked()
        {
            Dispatcher.Invoke((ThreadStart)delegate { BTNStartCalculation.IsEnabled = false; BTNRandomMatrixA.IsEnabled = false;
                BTNRandomMatrixA1.IsEnabled = false;BTNRandomMatrixA2.IsEnabled = false; BTNRandomMatrixB2.IsEnabled = false;
                MainMenu.IsEnabled = false;BTNAllRandom.IsEnabled = false;
                SetAllValues();
                GetAllValues();
            });

            Thread tCalc_y1 = new Thread(Calc_y1);
            Thread tCalc_y2 = new Thread(Calc_y2);
            Thread tCalc_C2 = new Thread(Calc_MatrixC2);
            Thread tCalc_Y3 = new Thread(Calc_MatrixY3);
            Thread tTransposeVector = new Thread(Transpose_y1_and_y2);
            Thread tFinalCalculation = new Thread(FinalCalculation);

            tCalc_y1.IsBackground = true;

            tCalc_y1.Start();


            tCalc_y2.IsBackground = true;

            tCalc_y2.Start();


            tCalc_C2.IsBackground = true;

            tCalc_C2.Start();

            tCalc_C2.Join();


            tCalc_Y3.IsBackground = true;

            tCalc_Y3.Start();


            tCalc_y1.Join();

            tCalc_y2.Join();

            tTransposeVector.IsBackground = true;

            tTransposeVector.Start();


            tTransposeVector.Join();

            tCalc_y1.Join();

            tCalc_y2.Join();

            tCalc_Y3.Join();

            tFinalCalculation.IsBackground = true;

            tFinalCalculation.Start();

            tFinalCalculation.Join();


            Dispatcher.Invoke((ThreadStart)delegate {
                MainMenu.IsEnabled = true;
            });
        }

        private void MainCalculation()
        {
            MainCalculationNotChecked();     
        }
        
        private async void AsyncMainCalculation()
        {
            await Task.Run(() => MainCalculation());
        }

        private void BTNClickCalculationAll(object sender, RoutedEventArgs e)
        {
            AsyncMainCalculation();       
        }

        private void MIResult(object sender, RoutedEventArgs e)
        {
            Results r = new Results(dictMatrixA, dictMatrixA1, dictMatrixA2, dictMatrixB2, dictMatrixY3, dictMatrixC2, vector_y, vector_y2, vectorTransposed_y, vectorTransposed_y2, finalResult, mainIntSizeMatrixN);

            r.Show();
        }

        #endregion



    }
}

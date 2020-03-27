using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Генерация_функций : Form
    {
        Random rnd = new Random();
        /// <summary>
        /// Краткий список переменных (в нижнем регистре)
        /// </summary>
        char[] letters_short = "abcdefghijklmnopqrstuwxyz".ToCharArray();
        /// <summary>
        /// Краткий список символов операций
        /// </summary>
        char[] operations_short = "∧∨→≡ⴲ↓|".ToCharArray();
        /// <summary>
        /// Символы нуля и единицы
        /// </summary>
        char[] null_and_one = "01".ToCharArray();

        /// <summary>
        /// Массив возможных переменных в формуле (включая 0 и 1)
        /// </summary>
        static char[] letters = "01AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuWwXxYyZz".ToCharArray();
        /// <summary>
        /// Массив возможных символов операций
        /// </summary>
        static char[] operations = "·&∧v∨V→⇒≡⇔↔=~ⴲ⊕↓|".ToCharArray();
        ///<summary>
        ///На какое максимальное количество переменных (разных) в формуле рассчитана программа
        ///</summary>
        static int max_num_of_var = 10;

        bool CorrectnessOfConditions()
        {
            int num_of_variable = -1;
            string table_of_truth = "";

            try
            {
                if (textBox3.Text.Replace(" ", string.Empty) != string.Empty)
                {
                    num_of_variable = Convert.ToInt32(textBox3.Text);
                }
                if (textBox2.Text.Replace(" ", string.Empty) != string.Empty)
                {
                    table_of_truth = textBox2.Text.Replace(" ", string.Empty);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            if (table_of_truth != "" && num_of_variable != -1) //оба параметр пользователь задал
            {
                if (table_of_truth.Length != Math.Pow(2, num_of_variable))
                {
                    MessageBox.Show("Вы задали условия, противоречащие друг другу", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        delegate void Del(string text, TextBox textBox);

        bool Generation()
        {
            // textBox1.Text = string.Empty; //Это не работает, если Generation выполняется в фоновом потоке
            // Здесь и далее закоментированные фрагменты - фрагменты, не работающие с многопоточностью 
            // (в них происходит обращения к контролу основного потока)

            //Универсальные анонимные методы (подходят для изменения текста любого textBox'а)
            Del change_textbox_delegate = new Del((s, textBox) => textBox.Text = s); //Лямбда-выражения представляют упрощенную запись анонимных методов
            Del add_textbox_delegate = new Del((s, textBox) => textBox.AppendText(s));

            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(change_textbox_delegate, string.Empty, textBox1);
            }

            string truth_table_param = textBox2.Text;
            int num_of_var;
            if (textBox2.Text == "") //ТИ не задана
            {
                if (textBox3.Text != "")//кол-во переменных не задано
                {
                    num_of_var = Convert.ToInt32(textBox3.Text);
                }
                else
                {
                    num_of_var = rnd.Next(1, 10);
                }
            }
            else //ТИ задана
            {
                num_of_var = (int)Math.Log(textBox2.Text.Length, 2);
            }

            while (true)
            {
                string generated_function = string.Empty;
                for (int i = 0, k = 0; k < num_of_var; i++)
                {
                    if (i % 2 == 0)
                    {
                        int randomize1 = rnd.Next(0, 2);
                        if (randomize1 == 0)
                        {
                            generated_function += "¬";
                        }

                        int randomize = rnd.Next(0, 6);
                        if (randomize < 5)
                        {
                            generated_function += letters_short[rnd.Next(letters_short.Length)]; k++;
                        }
                        if (randomize == 5)
                            generated_function += null_and_one[rnd.Next(null_and_one.Length)];
                    }
                    else
                    {
                        generated_function += operations_short[rnd.Next(operations_short.Length)];
                    }
                }

                if (truth_table_param != string.Empty)
                {
                    if (truth_table_param == new string(TruthTableBuilding(generated_function)))
                    {
                        if (textBox1.InvokeRequired)
                        {
                            // textBox1.Text = generated_function; //не работает с многопоточностью
                            // добавление функции в окно результата
                            textBox1.Invoke(change_textbox_delegate, generated_function, textBox1);
                            // добавление функции в окно "Перебор вариантов"
                            textBox4.Invoke(add_textbox_delegate, generated_function + Environment.NewLine, textBox4);

                        }
                        else
                        {
                            textBox1.Text = generated_function;
                        }
                        return true;
                    }
                    if (textBox4.InvokeRequired)
                    {
                        // textBox4.Text += generated_function + Environment.NewLine;
                        // добавление функции в окно "Перебор вариантов"
                        textBox4.Invoke(add_textbox_delegate, generated_function + Environment.NewLine, textBox4);
                    }
                    else
                    {
                        textBox4.Text += generated_function + Environment.NewLine;
                    }
                }
                else
                {
                    if (textBox1.InvokeRequired)
                    {
                        //textBox1.Text = generated_function;
                        // добавление функции в окно результата
                        textBox1.Invoke(change_textbox_delegate, generated_function, textBox1);
                        // добавление функции в окно "Перебор вариантов"
                        textBox4.Invoke(add_textbox_delegate, generated_function + Environment.NewLine, textBox4);

                    }
                    return true;
                }
            }
        }

        public Генерация_функций()
        {

            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Укажите желаемые параметры, введя или отметив их на панели. Вы можете оставить любой из параметров неотмеченным (например, если вы захотите построить СКНФ и оставите параметр 'таблица истинности' незаполненным, программа автоматически его сгенерирует. Использование генератора может занять значительное количество времени", "Справка по использованию генератора функций");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty)
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены? Генерация может занять значительное время", "Внимание!", MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    if (CorrectnessOfConditions())
                    {
                        button7.Enabled = true;
                        backgroundWorker1.RunWorkerAsync();
                    }
                }
            }
            else
            {
                if (CorrectnessOfConditions())
                {
                    button7.Enabled = true;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        ///  Построение таблицы истинности для заданной булевой формулы.
        /// </summary>
        /// <param name="example_function">булева формула, данная в виде string</param>
        /// <returns>Возвращает таблицу истинности в виде одномерного массива char[]</returns>
        static char[] TruthTableBuilding(string example_function)
        {

            if (NumOfVar(example_function) > max_num_of_var)
                Console.WriteLine("программа не рассчитана на количество переменных, большее {0}", max_num_of_var);

            int num_of_var = NumOfVar(example_function);

            char[,] table_of_var = new char[(int)System.Math.Pow(2, num_of_var), num_of_var];

            int n = (int)System.Math.Pow(2, num_of_var); //колво строк
            int m = num_of_var; //колво столбцов

            char[] truth_table = new char[n]; //таблица истинности
            for (int i = 0; i < n; i++)
            {
                truth_table[i] = '*';
            }

            for (int j = 0; j < m; j++)
            {
                for (int b = 0; b < n; b += (int)Math.Pow(2, m - j)) // n/Math.Pow(2, j + 1); - количество проходов 
                //Math.Pow(2, m - j)) - на сколько будет сдвинута каретка после одной итерации
                {
                    for (int i = b; i < b + Math.Pow(2, m - 1 - j); i++)
                    {
                        table_of_var[i, j] = '0';

                    }
                    for (int i = b + (int)Math.Pow(2, m - 1 - j); i < b + Math.Pow(2, m - j); i++)
                    {
                        table_of_var[i, j] = '1';
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                char[] values = new char[max_num_of_var];

                for (int j = 0; j < m; j++)
                {
                    values[j] = table_of_var[i, j];
                }

                truth_table[i] = BoolToChar(Computation(example_function, values));
            }

            //for (int i = 0; i < n; i++) //
            //{
            //    for (int j = 0; j < m - 1; j++)
            //    {
            //        Console.Write("{0} ", table_of_var[i, j]);
            //    }
            //    for (int j = m - 1; j < m; j++)
            //    {
            //        Console.Write("{0} \n", table_of_var[i, j]);
            //    }
            //}

            return truth_table;

        }

        static int NumOfVar(string param)
        {
            int count = 0;
            //          char[] bufflet = letters; //ТАК НЕЛЬЗЯ! КОПИРУЕТСЯ ТОЛЬКО ССЫЛКА НА ПЕРВЫЙ ЭЛЕМЕНТ МАССИВА

            char[] bufflet = new char[letters.Length];
            Array.Copy(letters, bufflet, letters.Length);

            char[] buffparam = param.ToCharArray();

            for (int i = 0; i < param.Length; i++)
            {
                for (int j = 2; j < bufflet.Length; j += 2)//первые два символа - '0' и '1' - они не считаются переменными
                {
                    if (bufflet[j] == buffparam[i] || bufflet[j + 1] == buffparam[i])
                    {
                        count++;
                        bufflet[j] = '$';
                        bufflet[j + 1] = '$';
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Вычисление булевой формулы с заданными значениями переменных
        /// </summary>
        /// <param name="param">булева формула</param>
        /// <param name="values">набор значений переменных</param>
        /// <returns>результат (истина или ложь) данной функции на данном наборе значений переменных</returns>
        static bool Computation(string param, char[] values)
        {
            //Console.Write("\nСейчас обрабатываю ");
            //Console.WriteLine(values); //ТОЛЬКО ТАК! другие варианты использования WriteLine приводят к некорректному отображению

            int num_of_var = NumOfVar(param);

            int k = 0;
            while (k < num_of_var)
            {
                for (int i = 2; i < letters.Length; i += 2)
                {
                    if (param.IndexOf(letters[i]) != -1 || param.IndexOf(letters[i + 1]) != -1)
                    {
                        param = param.Replace(letters[i], values[k]);
                        param = param.Replace(letters[i + 1], values[k]);
                        k++;
                        break;
                    }

                }
            }



            //Console.Write("Подставляю значения переменных: ");
            //Console.WriteLine(param);

            /* Идея программы:
             * 1. Вычленить одну скобку
             * 2. Скобку проанализировать (провести все операции по порядку (приоритету))
             * 3. Заменить скобку №1 результатом пункта 2
             * 
             * 
             * */
            string buff = param;
            //Console.WriteLine(buff);

            for (int i = 0; i < (int)((double)NumberOfBrackets(param) / 2); ) //проходим от начала до последней открывающей скобки 
            {
                string buffbegin = buff; //
                int l = 0;
                for (int j = IndexOfInternalBracket(buff) + 1; j < buff.Length; j++) //дохожу от момента, где начинается последняя "(" 
                //до момента, где находится соответствующая ей ")" (т.е. следующая ")" за данной "(")
                {
                    if (buff.IndexOf(")", j) == j)
                        break;
                    l++;
                }

                buff = buff.Substring(IndexOfInternalBracket(buff) + 1, l); //обрезаю строку buff так, чтобы осталось только содержимое скобки
                //Console.WriteLine("\nСкобка: {0}", buff);
                //ДЕЙСТВИЯ В СКОБКАХ

                int index = 0;
                while ((index = buffbegin.IndexOf("(" + buff + ")", index) + 1) != 0)
                    i++;
                buff = buffbegin.Replace("(" + buff + ")", ComputationInBracket(buff)); /*заменяю  в текущей строке (сохранена в начале итерации в
                                                                                                           *исполнения строки ("(" и ")" удаляю тоже */
                //Console.Write("Новая формула (после проведения операций в скобке): ");
                //Console.WriteLine(buff);
            }
            if (NumberOfBrackets(buff) == 0)
            {
                //Console.WriteLine(Convert.ToChar(ComputationInBracket(buff)));
                return CharToBool(Convert.ToChar(ComputationInBracket(buff)));
            }
            else
                return CharToBool(Convert.ToChar(buff));
        }

        /// <summary>
        /// Вычисление булевой формулы, заключенной в скобке. Гарантируется, что в входной строке param не будет ни одного '(' или ')'
        /// </summary>
        /// <param name="param">булева формула БЕЗ СКОБОК</param>
        /// <returns>результат вычисления ("0" или "1")</returns>
        /// <summary>
        /// Вычисление булевой формулы, заключенной в скобке. Гарантируется, что в входной строке param не будет ни одного '(' или ')'
        /// </summary>
        /// <param name="param">булева формула БЕЗ СКОБОК</param>
        /// <returns>результат вычисления ("0" или "1")</returns>
        static string ComputationInBracket(string param)
        {
            /*Последовательно (по порядку в соответствии с приоритетом операций) провожу 
             отдельно в циклах for ВСЕ действия сначала с НЕ, потом с И, потом  с  ИЛИ и т.д.*/

            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("¬") == -1 && param.IndexOf("-") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf("¬", i) == i || param.IndexOf("-", i) == i)
                {
                    buffparam[i + 1] = BoolToChar(Negation(buffparam[i + 1]));
                    buffparam[i] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                }
            }
            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("·") == -1 && param.IndexOf("&") == -1 && param.IndexOf("∧") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('·', i) == i || param.IndexOf('&', i) == i || param.IndexOf('∧', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(Conjunction(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                    continue;
                }
            }
            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("V") == -1 && param.IndexOf("v") == -1 && param.IndexOf("∨") == -1 && param.IndexOf("V") == -1 && param.IndexOf("∨") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('V', i) == i || param.IndexOf('v', i) == i || param.IndexOf('∨', i) == i || param.IndexOf('V', i) == i || param.IndexOf('∨', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(Disjunction(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    //param = buffparam.ToString(); 
                    /*НЕЛЬЗЯ так делать, потому что вызов ToString к простому массиву просто вернет "T[]"     
                     * где T - тип массива (в данном случае "System.Char[]"). Для char[] нет никакого 
                     * особого обработчика. Поэтому воспользуюсь другим способом:
                     * */
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                }
            }
            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("ⴲ") == -1 && param.IndexOf("⊕") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('ⴲ', i) == i || param.IndexOf('⊕', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(XOR(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                    continue;
                }
            }
            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("→") == -1 && param.IndexOf("⇒") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('→', i) == i || param.IndexOf('⇒', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(Implication(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                    continue;
                }
            }
            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("~") == -1 && param.IndexOf("≡") == -1 && param.IndexOf("⇔") == -1 && param.IndexOf(" ") == -1 && param.IndexOf("=") == -1)
                    break;

                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('~', i) == i || param.IndexOf('≡', i) == i || param.IndexOf('⇔', i) == i || param.IndexOf(' ', i) == i || param.IndexOf('=', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(Equivalence(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                    continue;
                }
            } 

            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("↓") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('↓', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(NOR(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                    continue;
                }
            }
            for (int i = 0; i < param.Length; i++)
            {
                if (param.IndexOf("|") == -1)
                    break;
                char[] buffparam = param.ToCharArray();
                if (param.IndexOf('|', i) == i)
                {
                    buffparam[i - 1] = BoolToChar(NAND(buffparam[i - 1], buffparam[i + 1]));
                    buffparam[i] = '$'; buffparam[i + 1] = '$';
                    param = new string(buffparam);
                    param = param.Replace("$", string.Empty);
                    i = 0;
                    continue;
                }
            }
            return param;
        }
        /// <summary>
        /// Подсчёт количества скобок в данной булевой функции
        /// </summary>
        /// <param name="param">булева формула</param>
        /// <returns>количество скобок </returns>
        static int NumberOfBrackets(string param)
        {
            int n = 0;

            for (int index = 0; index < param.Length; index++)
            {
                if (param.IndexOf("(", index) == index || param.IndexOf(")", index) == index)
                {
                    n++;
                }
            }

            return n;
        }

        /// <summary>
        /// Возвращает индекс самой внутренней открывающей скобки ("(") данной булевой формулы
        /// </summary>
        /// <param name="param">булева формула</param>
        /// <returns>индекс самой внутренней "("</returns>
        static int IndexOfInternalBracket(string param)
        {
            string buff = param;

            int max = -1, max_i = -1, n = 0;

            for (int index = 0; index < buff.Length; index++)//узнаю индекс самой внутренней скобки
            {
                if (buff.IndexOf("(", index) == index)
                {
                    n++;
                }
                if (buff.IndexOf(")", index) == index)
                {
                    n--;
                }
                if (n > max)
                {
                    max = n;
                    max_i = index;
                }
            }

            return max_i;
        }

        /// <summary>
        /// Конвертация переменной bool в char
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        static char BoolToChar(bool a)
        {
            if (a == false)
                return '0';
            else
                return '1';
        }

        /// <summary>
        /// Конвертация char в bool
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        static bool CharToBool(char a) //не предусмотрены ошибка для случая, если a не равен 0 или 1
        {
            if (a == '0')
                return false;
            else
                return true;
        }

        /// <summary>
        /// Отрицание (булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        static bool Negation(bool a)
        {
            return !a;
        }

        /// <summary>
        /// Отрицание (булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        static bool Negation(char a)
        {
            return Negation(CharToBool(a));
        }

        /// <summary>
        /// Конъюнкция (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Conjunction(bool a, bool b)
        {
            return a && b;
        }

        /// <summary>
        /// Конъюнкция (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Conjunction(char a, char b)
        {
            return Conjunction(CharToBool(a), CharToBool(b));
        }

        /// <summary>
        /// Дизъюнкция (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Disjunction(bool a, bool b)
        {
            return a || b;
        }

        /// <summary>
        /// Дизъюнкция (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Disjunction(char a, char b)
        {
            return Disjunction(CharToBool(a), CharToBool(b));
        }

        /// <summary>
        /// Импликация (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Implication(bool a, bool b)
        {
            return !a || b;
        }

        /// <summary>
        /// Импликация (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Implication(char a, char b)
        {
            return Implication(CharToBool(a), CharToBool(b));
        }

        /// <summary>
        /// Эквивалентность (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Equivalence(bool a, bool b)
        {
            return a && b || !a && !b;
        }

        /// <summary>
        /// Эквивалентность (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool Equivalence(char a, char b)
        {
            return Equivalence(CharToBool(a), CharToBool(b));
        }

        /// <summary>
        /// Сложение по модулю два (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool XOR(bool a, bool b)
        {
            return !a && b || a && !b;
        }

        /// <summary>
        /// Сложение по модулю два (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool XOR(char a, char b)
        {
            return XOR(CharToBool(a), CharToBool(b));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;

            if (textBox3.Text == "")
            {
                double rand = Math.Pow(2, rnd.Next(1, 10));
                for (int i = 0; i < rand; i++)
                    textBox2.Text += "0";
            }
            else
            {
                for (int i = 0; i < Math.Pow(2, Convert.ToInt32(textBox3.Text)); i++)
                    textBox2.Text += "0";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;

            string[] arr = { "0", "1" };

            if (textBox3.Text == "")
            {
                double rand = Math.Pow(2, rnd.Next(1, 10));
                for (int i = 0; i < rand; i++)
                    textBox2.Text += arr[rnd.Next(0, 2)];
            }
            else
            {
                for (int i = 0; i < Math.Pow(2, Convert.ToInt32(textBox3.Text)); i++)
                    textBox2.Text += arr[rnd.Next(0, 2)];
            }

            string temp1 = textBox2.Text;
            int temp = rnd.Next(0, textBox2.Text.Length);
            //Обеспечиваю выполнимость
            temp1 = temp1.Remove(temp, 1);
            temp1 = temp1.Insert(temp, "1");

            textBox2.Text = temp1;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;

            if (textBox3.Text == "")
            {
                double rand = Math.Pow(2, rnd.Next(1, 10));
                for (int i = 0; i < rand; i++)
                    textBox2.Text += "1";
            }
            else
            {
                for (int i = 0; i < Math.Pow(2, Convert.ToInt32(textBox3.Text)); i++)
                    textBox2.Text += "1";
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CorrectnessOfConditions())
            {
                if (textBox2.Text == string.Empty) //таблица истинности не задана пользователем
                {
                    if (textBox3.Text == string.Empty) //количество переменных не задано пользователем
                    {
                        //если пользователь не задал ни таблицы истинности, ни кол-ва переменных, 
                        //количество переменных генерируется автоматически, таблица истинности генерируется 
                        //случайным образом нужной длины (для сгенерированного количества переменных)
                        int num_of_var = rnd.Next(1, 10);
                        textBox1.Text = ConstructionOfDisjunctiveNormalForm(GenRandomString("01", (int)Math.Pow(2, num_of_var)));
                    }
                    else
                    {
                        // таблица истинности генерируется случайным образом нужной длины (для заданного количества переменных)
                        textBox1.Text = ConstructionOfDisjunctiveNormalForm(GenRandomString("01", Convert.ToInt32(textBox3.Text)));
                    }
                }
                else
                {
                    textBox1.Text = ConstructionOfDisjunctiveNormalForm(textBox2.Text);
                }
            }
        }

        /// <summary>
        /// Генерация случайной строки
        /// </summary>
        /// <param name="Alphabet">Символы, которые будут встречаться в сгенерированной строке</param>
        /// <param name="Length">Длина строки</param>
        /// <returns>Сгенерированная строка</returns>
        string GenRandomString(string Alphabet, int Length)
        {
            //Внутри функции.

            //создаем объект Random, генерирующий случайные числа
            Random rnd = new Random();
            //объект StringBuilder с заранее заданным размером буфера под результирующую строку
            StringBuilder sb = new StringBuilder(Length - 1);
            //переменную для хранения случайной позиции символа из строки Alphabet
            int Position = 0;

            //Далее в цикле генерируем случайную строку:
            for (int i = 0; i < Length; i++)
            {
                //получаем случайное число от 0 до последнего
                //символа в строке Alphabet
                Position = rnd.Next(0, Alphabet.Length);
                //добавляем выбранный символ в объект
                //StringBuilder
                sb.Append(Alphabet[Position]);
            }

            //Возвращаем сгенерированную строку:
            return sb.ToString();
        }

        /// <summary>
        /// Построение СДНФ по таблице истинности
        /// </summary>
        /// <param name="truth_table">таблица истинности</param>
        /// <returns></returns>
        public string ConstructionOfDisjunctiveNormalForm(string truth_table)
        {
            string result = "";
            bool is_begin = true;

            if (truth_table.IndexOf("0") == -1)
                return "1";
            if (truth_table.IndexOf("1") == -1)
                return "0";

            int num_of_var = (int)Math.Log(truth_table.Length, 2);
            char[] list_of_var = new char[num_of_var];
            for (int k = 0; k < num_of_var; k++)
            {
                while (true)
                {
                    int next_index = rnd.Next(2, letters.Length);
                    if (new string(list_of_var).IndexOf(letters[next_index]) == -1 && new string(list_of_var).ToUpper().IndexOf(letters[next_index]) == -1)
                    {
                        list_of_var[k] = letters[next_index];
                        break;
                    }
                }
            }

            for (int i = 0; i < truth_table.Length; i++)
            {
                if (truth_table[i] == '1')
                {
                    if (is_begin)
                        is_begin = false;
                    else
                        result += "∨";

                    string binary_buff = Convert.ToString(i, 2);

                    while (binary_buff.Length != list_of_var.Length)
                    {
                        binary_buff = "0" + binary_buff;
                    }

                    char[] binary_buff_char = binary_buff.ToCharArray(); // бинарное представление конкретного i


                    for (int j = 0; j < binary_buff_char.Length - 1; j++)
                    {
                        if (binary_buff_char[j] == '1')
                            result += list_of_var[j] + "·";
                        if (binary_buff_char[j] == '0')
                            result += "¬" + list_of_var[j] + "·";
                    }
                    for (int j = binary_buff_char.Length - 1; j < binary_buff_char.Length; j++)
                    {
                        if (binary_buff_char[j] == '1')
                            result += list_of_var[j];
                        if (binary_buff_char[j] == '0')
                            result += "¬" + list_of_var[j];
                    }

                }

            }

            return result;
        }

        /// <summary>
        /// Построение СКНФ по таблице истинности
        /// </summary>
        /// <param name="truth_table">таблица истинности</param>
        /// <returns>СКНФ</returns>
        public string ConstructionOfConjunctiveNormalForm(string truth_table)
        {
            string result = "";

            bool is_begin = true;

            if (truth_table.IndexOf("0") == -1)
                return "1";
            if (truth_table.IndexOf("1") == -1)
                return "0";

            int num_of_var = (int)Math.Log(truth_table.Length, 2);
            char[] list_of_var = new char[num_of_var];
            for (int k = 0; k < num_of_var; k++)
            {
                while (true)
                {
                    int next_index = rnd.Next(2, letters.Length);
                    if (new string(list_of_var).IndexOf(letters[next_index]) == -1 && new string(list_of_var).ToUpper().IndexOf(letters[next_index]) == -1)
                    {
                        list_of_var[k] = letters[next_index];
                        break;
                    }
                }
            }


            for (int i = 0; i < truth_table.Length; i++)
            {
                if (truth_table[i] == '0')
                {
                    if (is_begin)
                    {
                        result += "(";
                        is_begin = false;
                    }
                    else
                        result += "∧(";

                    string binary_buff = Convert.ToString(i, 2);

                    while (binary_buff.Length != list_of_var.Length)
                    {
                        binary_buff = "0" + binary_buff;
                    }

                    char[] binary_buff_char = binary_buff.ToCharArray(); // бинарное представление конкретного i


                    for (int j = 0; j < binary_buff_char.Length - 1; j++)
                    {
                        if (binary_buff_char[j] == '0')
                            result += list_of_var[j] + "∨";
                        if (binary_buff_char[j] == '1')
                            result += "¬" + list_of_var[j] + "∨";
                    }
                    for (int j = binary_buff_char.Length - 1; j < binary_buff_char.Length; j++)
                    {
                        if (binary_buff_char[j] == '0')
                            result += list_of_var[j];
                        if (binary_buff_char[j] == '1')
                            result += "¬" + list_of_var[j];
                    }

                    result += ")";
                }

            }

            return result;
        }

        /// <summary>
        /// Получение списка переменных, входящих в данную булеву функцию
        /// </summary>
        /// <param name="function">булева функция</param>
        /// <returns>массив переменных</returns>
        public char[] ListOfVariable(string function)
        {
            string buff_list_of_var = "";
            char[] char_function = function.ToCharArray();

            for (int i = 0; i < char_function.Length; i++)
            {
                for (int j = 2; j < letters.Length; j++)
                {
                    if (char_function[i] == letters[j])
                    {
                        if (buff_list_of_var.IndexOf(char_function[i]) == -1 && buff_list_of_var.IndexOf(char_function[i]) == -1)
                        {
                            buff_list_of_var = buff_list_of_var + new string(new char[] { char_function[i] });
                            break;
                        }
                    }
                }
            }
            buff_list_of_var = buff_list_of_var.ToUpper();

            buff_list_of_var = new string(buff_list_of_var.Distinct().ToArray());

            return buff_list_of_var.ToArray();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CorrectnessOfConditions())
            {
                if (textBox2.Text == string.Empty) //таблица истинности не задана пользователем
                {
                    if (textBox3.Text == string.Empty) //количество переменных не задано пользователем
                    {
                        //если пользователь не задал ни таблицы истинности, ни кол-ва переменных, 
                        //количество переменных генерируется автоматически, таблица истинности генерируется 
                        //случайным образом нужной длины (для сгенерированного количества переменных)
                        int num_of_var = rnd.Next(1, 10);
                        textBox1.Text = ConstructionOfConjunctiveNormalForm(GenRandomString("01", (int)Math.Pow(2, num_of_var)));
                    }
                    else
                    {
                        // таблица истинности генерируется случайным образом нужной длины (для заданного количества переменных)
                        textBox1.Text = ConstructionOfConjunctiveNormalForm(GenRandomString("01", Convert.ToInt32(textBox3.Text)));
                    }
                }
                else
                {
                    textBox1.Text = ConstructionOfConjunctiveNormalForm(textBox2.Text);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Generation();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button7.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Штрих Шеффера (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool NAND(bool a, bool b)
        {
            return !(a && b);
        }

        /// <summary>
        /// Штрих Шеффера (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool NAND(char a, char b)
        {
            return NAND(CharToBool(a), CharToBool(b));
        }

        /// <summary>
        /// Стрелка Пирса (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool NOR(bool a, bool b)
        {
            return !(a || b);
        }

        /// <summary>
        /// Стрелка Пирса (двухместная булева функция)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool NOR(char a, char b)
        {
            return NOR(CharToBool(a), CharToBool(b));
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Обработка_булевых_функций : Form
    {
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

        /// <summary>
        /// Строка, в которую записываются шаги выполнения метода
        /// </summary>
        static string message;


        public Обработка_булевых_функций(string param)
        {
            InitializeComponent();
            textBox1.Text = param;
        }

        private void Обработка_булевых_функций_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text += "Таблица истинности: " + new string(TruthTableBuilding(textBox1.Text)) + Environment.NewLine;
        }


        /// <summary>
        /// Подсчитывает количество переменных (разных) в данной булевой формуле
        /// </summary>
        /// <param name="param">булева формула</param>
        /// <returns>количество переменных</returns>
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

        //xml-комментарии
        /// <summary>
        ///  Построение таблицы истинности для заданной булевой формулы.
        /// </summary>
        /// <param name="example_function">булева формула, данная в виде string</param>
        /// <returns>Возвращает таблицу истинности в виде одномерного массива char[]</returns>
        static char[] TruthTableBuilding(string example_function)
        {
            message = string.Empty;

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

            return truth_table;

        }

        /// <summary>
        /// Вычисление булевой формулы с заданными значениями переменных
        /// </summary>
        /// <param name="param">булева формула</param>
        /// <param name="values">набор значений переменных</param>
        /// <returns>результат (истина или ложь) данной функции на данном наборе значений переменных</returns>
        static bool Computation(string param, char[] values)
        {
            message = message + Environment.NewLine + "Сейчас обрабатываю " + new string(values, 0, NumOfVar(param)) + Environment.NewLine;
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
            message = message + buff + Environment.NewLine;
            //Console.WriteLine(buff);

            for (int i = 0; i < (int)((double)NumberOfBrackets(param) / 2); )  
            {
                string buffbegin = buff; //
                int l = 0;
                for (int j = IndexOfInternalBracket(buff) + 1; j < buff.Length; j++) //дохожу от момента, где начинается самая внутренняя "(" 
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
                message = message + buff + Environment.NewLine;
            }
            if (NumberOfBrackets(buff) == 0)
            {
                //Console.WriteLine(Convert.ToChar(ComputationInBracket(buff)));
                message = message + ComputationInBracket(buff) + Environment.NewLine;
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
                char[] buffparam =

                param.ToCharArray();
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox4.Text += message + Environment.NewLine;
            message = string.Empty;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == NumOfVar(textBox1.Text))
                textBox2.Text += Computation(textBox1.Text, textBox3.Text.ToCharArray()) + Environment.NewLine;
            else
                MessageBox.Show("Неверное количество переменных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt.files (*.txt)|*.txt|*.Allfiles(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = saveFileDialog1.FileName;
                FileStream filestream = File.Open(FileName, FileMode.Create, FileAccess.Write);
                if (filestream != null)
                {
                    StreamWriter streamwriter = new StreamWriter(filestream);
                    streamwriter.Write(textBox2.Text);
                    streamwriter.Flush();
                    filestream.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Image.FromFile(@"C:\Программки\III семестр\Курсовые работы\Курсовая Михайлова\1459435733_trans-flag.gif", true);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (new string(TruthTableBuilding(textBox1.Text)).IndexOf('1') == -1)
            {
                textBox2.Text += "Формула является невыполнимой" + Environment.NewLine;
            }
            else if (new string(TruthTableBuilding(textBox1.Text)).IndexOf('0') == -1)
            {
                textBox2.Text += "Формула является тавтологией (тождественно истинной)" + Environment.NewLine;
            }
            else
            {
                textBox2.Text += "Формула является выполнимой" + Environment.NewLine;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text += "СДНФ: " + ConstructionOfDisjunctiveNormalForm(textBox1.Text) + Environment.NewLine;

        }

        /// <summary>
        /// Построение СДНФ
        /// </summary>
        /// <param name="function">булева функция, СДНФ которой необходимо построить</param>
        /// <returns></returns>
        public string ConstructionOfDisjunctiveNormalForm(string function)
        {
            // MessageBox.Show(new string(ListOfVariable(function)));

            char[] list_of_var = ListOfVariable(function);

            char[] truth_table = TruthTableBuilding(function);

            if (new string(truth_table).IndexOf("0") == -1)
                return "1";
            if (new string(truth_table).IndexOf("1") == -1)
                return "0";

            string result = "";
            bool is_begin = true;

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
        /// Построение СКНФ
        /// </summary>
        /// <param name="function">булева функция, СКНФ которой необходимо построить</param>
        /// <returns></returns>
        public string ConstructionOfConjunctiveNormalForm(string function)
        {
            // MessageBox.Show(new string(ListOfVariable(function)));

            char[] list_of_var = ListOfVariable(function);

            char[] truth_table = TruthTableBuilding(function);

            if (new string(truth_table).IndexOf("0") == -1)
                return "1";
            if (new string(truth_table).IndexOf("1") == -1)
                return "0";

            string result = "";

            bool is_begin = true;

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

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(0, 2));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text += "СКНФ: " + ConstructionOfConjunctiveNormalForm(textBox1.Text) + Environment.NewLine;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = FormatToSAT(textBox2.Text);
        }

        private string FormatToSAT(string text)
        {
            char[] list_of_var = ListOfVariable(textBox1.Text);

            for (int i = 0; i < list_of_var.Length; i++)
            {
                text = text.Replace(new string(new char[] {list_of_var[i]}), "x" + Convert.ToString(i,2));
            }

            text = text.Replace("&", "∧"); text = text.Replace("·", "∧");
            text = text.Replace("v", "∨"); text = text.Replace("V", "∨");
            text = text.Replace("-", "¬");

            return text;
        }
    }
}

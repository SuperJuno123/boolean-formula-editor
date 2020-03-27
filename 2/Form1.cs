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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Data.EventHandler = new Data.MyEvent(change_character);
            Data1.EventHandler = new Data1.MyEvent(Correct);
            Data2.EventHandler = new Data2.MyEvent(change_main_textbox);
            letters = "01AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuWwXxYyZz".ToCharArray();
            operations = "·&∧v∨V→⇒≡⇔↔=~ⴲ⊕↓|".ToCharArray();
        }

        /// <summary>
        /// Символы всех переменных (латинский алфавит). Первые два элемента массива - '0' и '1'
        /// </summary>
        char[] letters;
        /// <summary>
        /// Все возможные символы стандартных булевых функций
        /// </summary>
        char[] operations;

        string buff;
        bool flag;

        void change_main_textbox(string param)
        {
            richTextBox1.Text = param;
        }

        /// <summary>
        /// Метод для изменения символа операции. Символ меняется на кнопке в главной форме при нажатии на кнопку в меню "Настройки"
        /// </summary>
        /// <param name="num">Номер кнопки, с которой берется новый символ</param>
        /// <param name="param">Новый символ</param>
        void change_character(int num, string param) //обработчик (передаю данные методом делегатов)
        {
            switch (num)
            {
                case 5:
                    {
                        button5.Text = param;
                        break;
                    }
                case 6:
                    {
                        button6.Text = param;
                        break;
                    }
                case 8:
                    {
                        button8.Text = param;
                        break;
                    }
                case 7:
                    {
                        button7.Text = param;
                        break;
                    }
                case 9:
                    {
                        button9.Text = param;
                        break;
                    }
                case 12:
                    {
                        button12.Text = param;
                        break;
                    }

            }
        }

        /// <summary>
        /// Проверка корректности синтаксиса заданной булевой формулы 
        /// </summary>
        /// <param name="text">Булева формула</param>
        /// <returns>Правильна или неправильна формула</returns>
        public bool Correct(string text)
        {
            int num_of_parentheses = 0;

            for (int index = 0; index < text.Length; index++)
            {
                if (text.IndexOf("(", index) == index)
                {
                    num_of_parentheses++;
                }
                if (text.IndexOf(")", index) == index)
                {
                    num_of_parentheses--;
                }
            }
            if (num_of_parentheses != 0)
            {
                MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Проверьте количество скобок, на данный момент {0} скобкам не хватает пары", Math.Abs(num_of_parentheses)));
                return false;
            }

            char[] buff = richTextBox1.Text.Replace(" ", string.Empty).ToCharArray();

            //    bool f1, f2, f3; //f1 - найдена БУКВА, f2 - после буквы стоит знак ОПЕРАЦИИ, f3 - А*Б - после буквы+знак операции стоит БУКВА

            if (buff.Length >= 3)
            {
                for (int i = 0; i < buff.Length - 3; i++)
                {
                    bool f1 = false, f2 = false, f3 = false;
                    for (int j = 0; j < letters.Length; j++)
                        if (buff[i] == letters[j])
                        {
                            f1 = true;
                            break;
                        }
                    if (f1)
                    {
                        while (buff[i + 1] == '¬' || buff[i + 1] == '-' || buff[i + 1] == '(' || buff[i + 1] == ')')
                            i++;
                        for (int k = 0; k < letters.Length; k++)
                            if (buff[i + 1] == letters[k])
                            {
                                MessageBox.Show(String.Format("Ваша строка не является корректной формулой! В данный момент у вас пропущен знак операции. Помните, что для выражения конъюнкции необходимо использовать один из предложенных знаков. Отсутствие знака операции между двумя переменными является ошибкой"));
                                return false;
                            }
                        for (int k = 0; k < operations.Length; k++)
                            if (buff[i + 1] == operations[k])
                            {
                                f2 = true;
                                break;
                            }
                    }
                    if (f2)
                    {
                        while (buff[i + 2] == '¬' || buff[i + 2] == '-' || buff[i + 2] == '(' || buff[i + 2] == ')')
                            i++;
                        for (int t = 0; t < letters.Length; t++)
                            if (buff[i + 2] == letters[t])
                            {
                                f3 = true;
                                break;
                            }
                    }

                    if (!(f1 && f2 && f3) && (f1 && !f2 || f2 && !f3))
                    {
                        MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Возможные варианты решения проблемы: добавьте после переменной знак операции, либо добавьте после знака операции переменную"));
                        return false;
                    }
                }
                for (int j = 0; j < operations.Length; j++)
                {
                    if (buff[buff.Length - 1] == operations[j])
                    {
                        MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Вы забыли добавить переменную после последнего знака операции"));
                        return false;
                    }
                    if (buff[0] == operations[j])
                    {
                        MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Вы попытались начать формулу со знака операции"));
                        return false;
                    }
                    
                }
                for (int j = 0; j < letters.Length; j++)
                {
                    if (buff[buff.Length - 2] == letters[j])
                    {
                        for (int k = 0; k < letters.Length; k++)
                        {
                            if (buff[buff.Length - 1] == letters[k])
                            {
                                MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Не хватает знака операции между двумя переменными"));
                                return false;
                            }
                            if (buff[buff.Length - 1] == '-' || buff[buff.Length - 1] == '¬')
                            {
                                MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Не хватает переменной после знака отрицания"));

                            }
                        }
                    }
                }
            }
            else
            {
                if (buff.Length == 0)
                {
                    MessageBox.Show(String.Format("Вы забыли ввести формулу"));
                    return false;
                }
                if (buff.Length == 2)
                {
                    if (buff[0] == '-' || buff[0] == '¬')
                    {
                        for (int j = 0; j < letters.Length; j++)
                        {
                            if (buff[1] == letters[j])
                                return true;
                        }
                        MessageBox.Show("Ваша строка не является корректной формулой! После знака отрицания вы напечатали не переменную");
                        return false;
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Ваша строка не является корректной формулой! Вы попытались либо ввести две переменные без знака операции, либо забыли ввести вторую переменную после знака операции, либо ввели просто две скобки"));
                        return false;
                    }
                }
                if (buff.Length == 1)
                {
                    for (int j = 0; j < letters.Length; j++)
                    {
                        if (buff[0] == letters[j])
                            return true;
                    }
                    MessageBox.Show("Ваша строка не является корректной формулой! Вы можете ввести либо символы переменных, либо символы операций. Если вам нужна помощь, кликлите на кнопке ниже");
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Метод добавления символа с помощью клавиатуры. Если в тексте установлен курсор - символ вставляется в место установки курсора, если курсор в тексте не установлен, символ вставляется в конец строки
        /// </summary>
        /// <param name="character">Добавляемый символ</param>
        public void Apply_Character(string character) 
        {
            if (flag == true)
            {
                richTextBox1.Text = richTextBox1.Text.Insert(richTextBox1.SelectionStart, character);
                flag = false;
            }
            else
                richTextBox1.Text += character;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Correct(richTextBox1.Text))
            {
                Обработка_булевых_функций dialog1 = new Обработка_булевых_функций(richTextBox1.Text.Replace(" ", string.Empty));
                dialog1.Show();
            }
        }

        private void справкаОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа сделана в 2018 году\n\nАвтор Якимова Юлия Александровна", "О программе", MessageBoxButtons.OK);
        }

        private void инструкцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("", "Руководство пользователя");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.SelectionFont = fontDialog1.Font;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt.files (*.txt)|*.txt|*.Allfiles(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                FileStream filestream = File.Open(FileName, FileMode.Open, FileAccess.Read);
                if (filestream != null)
                {
                    StreamReader streamreader = new StreamReader(filestream, Encoding.Default);
                    richTextBox1.Text = streamreader.ReadToEnd();
                    filestream.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            saveFileDialog1.Filter = "txt.files (*.txt)|*.txt|*.Allfiles(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = saveFileDialog1.FileName;
                FileStream filestream = File.Open(FileName, FileMode.Create, FileAccess.Write);
                if (filestream != null)
                {
                    StreamWriter streamwriter = new StreamWriter(filestream);
                    streamwriter.Write(richTextBox1.Text);
                    streamwriter.Flush();
                    filestream.Close();
                }
            }
        }

        private void информацияОВсехБулевыхФункцияхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Информация_о_булевых_функциях dialog = new Информация_о_булевых_функциях(richTextBox1.Text);
            while (dialog.ShowDialog() == DialogResult.Retry)
            {
                dialog.Close();
                dialog = new Информация_о_булевых_функциях(richTextBox1.Text);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            buff = button12.Text;
            Apply_Character(buff);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            buff = button14.Text;
            Apply_Character(buff);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            buff = button10.Text;
            Apply_Character(buff);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            buff = button11.Text;
            Apply_Character(buff);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buff = button5.Text;
            Apply_Character(buff);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            buff = button8.Text;
            Apply_Character(buff);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            buff = button7.Text;
            Apply_Character(buff);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buff = button6.Text;
            Apply_Character(buff);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            buff = button9.Text;
            Apply_Character(buff);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            buff = button13.Text;
            Apply_Character(buff);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            buff = button15.Text;
            Apply_Character(buff);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            buff = button16.Text;
            Apply_Character(buff);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            buff = button17.Text;
            Apply_Character(buff);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            buff = button18.Text;
            Apply_Character(buff);
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            flag = true;
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength != 0)
            {
                if (flag == true)
                {
                    richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart - 1, 1);
                    flag = false;
                }
                else
                    richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.TextLength - 1, 1);
            }
        }

        private void генерацияБулевойФункцииПоВашимПараметрамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Генерация_функций dialog1 = new Генерация_функций();
            dialog1.ShowDialog();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Настройки dialog = new Настройки();
            dialog.Show();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите формулу, используя предложенные символы (вы также можете вводить символы с клавиатуры, но если в тексте встретится «неизвестный» символ, программа вас не поймёт). Вы можете изменить значки в настройках. Вы можете использовать пробелы и круглые скобки. Вы можете использовать в качестве переменных только буквы латинского алфавита (строчные или прописные), за исключением буквы V");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = SystemIcons.Application;

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Мяумур");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

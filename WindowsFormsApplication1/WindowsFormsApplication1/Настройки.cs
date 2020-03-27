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
    public partial class Настройки : Form
    {
        public Настройки()
        {
            InitializeComponent();

        }
        public string[] phrases = { "Ай!", "Ты хочешь поиграть?", "Эй, тыкать в поней - невежливо!", "Меня зовут Рэйнбоу Дэш, рада видеть тебя!", "Ты где-нибудь видел Флаттершай? Неужели она опять испугалась зайчонка...", "Я бы рада поболтать, но у меня дела", "Ой", "Хватить тыцкать меня", "Если тебе что-то непонятно, можешь обратиться в \"настройки\"", "Знаешь, а я вчера заняла первое место в гонках пегасов. Так-то!", "Интересно, каково это - быть единорогом?", "Привет!", "Ай-ай-ай!", "Это больно!", "И-го-го", "Я хочу стать самым быстрым пегасом!", "Не забудь про свои формулы!", "Интересно, как пегасы могут летать с такими маленькими крыльями? Искорка говорит, что все пони владеют магией, и пегасы тоже... Это помогает им удерживаться в воздухе"};

        private void button2_Click(object sender, EventArgs e)
        {
            Data.EventHandler(5, button2.Text); // (int, string) - (номер кнопки, в которую запишется новый знак, новый знак (берется с кнопки))

        }

        private void button12_Click(object sender, EventArgs e)
        {
            Data.EventHandler(9, button12.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Data.EventHandler(5, button3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Data.EventHandler(8, button4.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Data.EventHandler(8, button5.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Data.EventHandler(8, button6.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Data.EventHandler(7, button7.Text);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Data.EventHandler(7, button8.Text);

        }

        private void button9_Click(object sender, EventArgs e)
        {
        //    Data.EventHandler(7, button9.Text);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Data.EventHandler(6, button10.Text);

        }

        private void button11_Click(object sender, EventArgs e)
        {
            Data.EventHandler(6, button11.Text);

        }

        private void button13_Click(object sender, EventArgs e)
        {
        //    Data.EventHandler(9, button13.Text);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            Data.EventHandler(9, button14.Text);

        }

        private void button17_Click(object sender, EventArgs e)
        {
            Data.EventHandler(12, button17.Text);

        }

        private void button16_Click(object sender, EventArgs e)
        {
        //    Data.EventHandler(12, button16.Text);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Data.EventHandler(12, button18.Text);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Data.EventHandler(12, button19.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Data.EventHandler(12, button15.Text);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data.EventHandler(8, "∧"); Data.EventHandler(7, "∨");
            Data.EventHandler(9, "→"); Data.EventHandler(12, "≡");
            Data.EventHandler(6, "ⴲ"); Data.EventHandler(5, "¬");
        }

        private void button20_Click(object sender, EventArgs e)
        {
             
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Data.EventHandler(7, button9.Text);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            MessageBox.Show(phrases[rnd.Next(phrases.Length)]);
        }

        private void Настройки_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

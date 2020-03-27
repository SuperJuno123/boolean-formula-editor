using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Информация_о_булевых_функциях : Form
    {
        string path = System.IO.Directory.GetCurrentDirectory();
        public Информация_о_булевых_функциях(string param)
        {
            InitializeComponent();
            func_from_main_form = param;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        string func_from_main_form;
        
        private void Информация_о_булевых_функциях_Load(object sender, EventArgs e1)
        {
            LinkedList<string>functions = new LinkedList<string>();
            try
            {
                // создаем объект BinaryReader
                using (BinaryReader reader = new BinaryReader(File.Open(path + "\\database.bin", FileMode.Open)))
                {
                    // пока не достигнут конец файла
                    // считываем каждое значение из файла
                    while (reader.PeekChar() > -1)
                    {
                        functions.Add(reader.ReadString());
                    }

                    Threesome[] a = new Threesome[functions.Count];
                    int i = 0;

                    foreach (string s in functions)
                    {
                        a[i].func = new TextBox()
                        {
                            Name = "TextBox" + i.ToString(),
                            Location = new Point(15, 68 + 30 * i),
                            Text = s,
                            Size = new Size(600, 26),
                            Font = new Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)))
                        };
                        this.Controls.Add(a[i].func);

                        a[i].delete = new Button()
                        {
                            Name = "DeleteTextBox" + i.ToString(),
                            Location = new Point(15 + 600 + 10, 68 + 30 * i),
                            Size = new Size(100, 26),
                            Text = "Удалить из БД",
                        };
                        this.Controls.Add(a[i].delete);
                        a[i].delete.Click += Delete;
                        
                        a[i].copy = new Button()
                        {
                            Name = "CopyTextBox" + i.ToString(),
                            Location = new Point(15 + 600 + 10 + 100 + 10, 68 + 30 * i),
                            Size = new Size(100, 26),
                            Text = "Скопировать"
                        };
                        this.Controls.Add(a[i].copy);
                        a[i].copy.Click += Copy;
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path + "\\database.bin", FileMode.Append)))
            {
                if (Data1.EventHandler(func_from_main_form))
                {
                    writer.Write(func_from_main_form);
                }
                else
                    MessageBox.Show("Невозможно добавить запись в БД, так как в главной форме записана не формула");
            }
            Информация_о_булевых_функциях_Load(null, null); 
        }

        private void Delete(object sender, EventArgs e1)
        {
            var button = (Button)sender;
           
            LinkedList<string>functions = new LinkedList<string>();

            try
            {
                // создаем объект BinaryReader
                using (BinaryReader reader = new BinaryReader(File.Open(path + "\\database.bin", FileMode.Open)))
                {
                    // пока не достигнут конец файла
                    // считываем каждое значение из файла
                    while (reader.PeekChar() > -1)
                    {
                        functions.Add(reader.ReadString());
                    }  
                }
                File.Delete(path + "\\database.bin");
                using (BinaryWriter writer = new BinaryWriter(File.Open(path + "\\database.bin", FileMode.CreateNew)))
                {
                    int i = 0;
                    foreach (string s in functions)
                    {
                        if (i != Convert.ToInt32(button.Name.Replace("DeleteTextBox", string.Empty)))
                            writer.Write(s);
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            this.DialogResult = DialogResult.Retry;
        }

        private void Copy(object sender, EventArgs e1)
        {
            var button = (Button)sender;
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path + "\\database.bin", FileMode.Open)))
                {
                    for (int i = 0; reader.PeekChar() > -1; i++)
                    {
                        if (i == Convert.ToInt32(button.Name.Replace("CopyTextBox", string.Empty)))
                        {
                            Data2.EventHandler(reader.ReadString());
                            break;
                        }
                        reader.ReadString();
                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
    public struct Threesome
    {
        public TextBox func;
        public Button copy;
        public Button delete;
    }
    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }
    public class LinkedList<T> : IEnumerable<T>  // односвязный список
    {
        Node<T> head; // головной/первый элемент
        Node<T> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        // добавление элемента
        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (head == null)
                head = node;
            else
                tail.Next = node;
            tail = node;

            count++;
        }
        // удаление элемента
        public bool Remove(T data)
        {
            Node<T> current = head;
            Node<T> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        // убираем узел current, теперь previous ссылается не на current, а на current.Next
                        previous.Next = current.Next;

                        // Если current.Next не установлен, значит узел последний,
                        // изменяем переменную tail
                        if (current.Next == null)
                            tail = previous;
                    }
                    else
                    {
                        // если удаляется первый элемент
                        // переустанавливаем значение head
                        head = head.Next;

                        // если после удаления список пуст, сбрасываем tail
                        if (head == null)
                            tail = null;
                    }
                    count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }
            return false;
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }
        // очистка списка
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
        ////// реализация интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }

}

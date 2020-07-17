using EntityProject.DB;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EntityProject
{
    public partial class AdminForm : Form
    {
        BuyGamesEntities ctx = new BuyGamesEntities();//контекст для обращения к эл бд
        public AdminForm()
        {
            InitializeComponent();
        }
        private void AdminForm_Load(object sender, EventArgs e)
        {
            Type[] typelist = Assembly.GetExecutingAssembly()//получаем корневую директорию файлов
                                      .GetTypes()//получаем типы (таблицы) в ней
                                      .Where(t => t.Namespace == "EntityProject.DB")//только те что в папке DB
                                      .ToArray();
            //заполняем комбобокс названиями таблиц
            for(int i=0;i<typelist.Length-2; i++){
                //ингорируем класс контекста
                if(typelist[i].Name != ctx.GetType().Name){
                    comboBox1.Items.Add(typelist[i].Name);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)//выводим таблицу
        {
            if(dataGridView1.Columns.Count>0) dataGridView1.Columns[0].ReadOnly = false;//запрещаем менять айди
            ctx = new BuyGamesEntities();//обновляем контекст
            if (comboBox1.SelectedItem == null) { labelChange(false); return; }//если не выбрана ни одна из таблиц - ошибка
            switch (comboBox1.SelectedItem)//для получения данных и заполнения грида
            {
                case "buy":
                    ctx.buy.Load();
                    dataGridView1.DataSource = ctx.buy.Local.ToBindingList();
                    break;
                case "clients":
                    ctx.clients.Load();
                    dataGridView1.DataSource = ctx.clients.Local.ToBindingList();
                    break;
                case "game_price":
                    ctx.game_price.Load();
                    dataGridView1.DataSource = ctx.game_price.Local.ToBindingList();
                    break;
                case "games":
                    ctx.games.Load();
                    dataGridView1.DataSource = ctx.games.Local.ToBindingList();
                    break;
                case "genres":
                    ctx.genres.Load();
                    dataGridView1.DataSource = ctx.genres.Local.ToBindingList();
                    break;
            }
            dataGridView1.Columns[0].ReadOnly = true;//запрещаем менять айди
            //скрываем лишние элементы
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                if (comboBox1.Items.Contains(dataGridView1.Columns[i].HeaderText))
                    dataGridView1.Columns[i].Visible = false;
            labelChange(true);//сообщаем об успешном выполнении
        }
        private void button2_Click(object sender, EventArgs e)//сохраняем изменения в бд 
        {
            if (comboBox1.SelectedItem == null) return;//если нет инменений выходим
            try
            {
                dataGridView1.EndEdit();//заканчиваем работу с таблицей
                ctx.SaveChanges();//сохраняем изменения
                labelChange(true);//сообщаем об успешном выполнении
            }
            catch { labelChange(false); }
        }
        private void labelChange(bool a)//вывод собщения о результате выполнения действия 
        {
            if (a)
            {
                label1.Text = "Статус выполнения: done";
                label1.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                label1.Text = "Статус выполнения: error";
                label1.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}

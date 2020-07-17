using EntityProject.DB;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EntityProject
{
    public partial class AnaliticForm : Form
    {
        BuyGamesEntities ctx = new BuyGamesEntities();//контекст для обращения к эл бд
        bool wrongdate = false;//статус даты
        public AnaliticForm()
        {
            InitializeComponent();
        }
        private void AnaliticForm_Load(object sender, EventArgs e)
        {
            var genres = ctx.genres.Select(c => c.genre_name).ToList();//выгрузка списка жанров
            foreach (var a in genres)//выгрузка их в комбобокс
                comboBox1.Items.Add(a.ToString());
            //выгрузка цены, названия, жанра и разработчика игры

            //select games.game_name, games.dev, game_price.price, genres.genre_name from games 
            //join genres on games.game_id = genres.game_id 
            //join game_price on(game_price.game_id = games.game_id)
            var res = ctx.games//выбераем таблицу игр
                .Join(
                    ctx.genres,//добавляем жанры
                    game => game.game_id,
                    genre => genre.game_id,
                    (game, genre) => new {//объеденяем
                        GameID = game.game_id,
                        Game = game.game_name,
                        Genre = genre.genre_name,
                        Dev = game.dev
                    })
                .Join(
                    ctx.game_price,//и к результату добавляем еще цену
                    game => game.GameID,
                    gp => gp.game_id,
                    (game, gp) => new {
                        Game = game.Game,
                        Genre = game.Genre,
                        Dev = game.Dev,
                        Price = gp.price
                    })
                .OrderBy(c => c.Price);//сортировка по цене
            dataGridView2.DataSource = res.ToList();//вывод результатов запроса
        }
        private void button1_Click(object sender, EventArgs e)//поиск между датами
        {
            wrongdate = false;
            ctx = new BuyGamesEntities();//обновляем контекст (внутреннюю информацию)
            DateTime D1 = GetDate(textBox1.Text);//в GetDate проверяем дату на корректность
            DateTime D2 = GetDate(textBox2.Text);
            if (wrongdate){
                label5.Text = "Статус выполнения: wrong date";
                label5.ForeColor = Color.Red;
                return;
            }
            //поск цены игры между датами
            var result = (from tb in ctx.buy
                          join tg in ctx.games on tb.game_id equals tg.game_id
                          join tp in ctx.game_price on tg.game_id equals tp.game_id
                          where tb.buy_date > D1 && tb.buy_date < D2//сравнение
                          select new{
                              game_name = tg.game_name,
                              price = tp.price,
                              date = tb.buy_date
                          });
            dataGridView1.DataSource = result.ToList();//вывод результат запроса
            label5.Text = "Статус выполнения: done";
            label5.ForeColor = Color.Blue;
        }
        public DateTime GetDate(string dateorg)//проверка корректности даты
        {
            DateTime date = DateTime.Today;
            if (DateTime.TryParse(dateorg, out date)) { return date; }
            else { wrongdate = true; return date; }
        }
        private void Wrong(bool a)//вывод собщения о результате выполнения действия
        {
            if (a){
                label6.Text = "Статус выполнения: done";
                label6.ForeColor = Color.Blue;
            }
            else{
                label6.Text = "Статус выполнения: error";
                label6.ForeColor = Color.Red;
            }
        }
        private void button2_Click(object sender, EventArgs e)//поиск по жанру, сортировки, кол-во строк на вывод
        {
            int number = 1000;//ограничение по выводу
            if (comboBox1.SelectedItem == null) { Wrong(false); return; }//выбран ли жанр
            if (!int.TryParse(textBox3.Text, out number)) { number = 1000; }//если ввели некор число выведется 1000 строк
            if (number <= 0) { Wrong(false); return; }//если <=0 ошибка
            //поиск по жанру
            var res = ctx.games
                .Join(
                    ctx.genres,
                    game => game.game_id,
                    genre => genre.game_id,
                    (game, genre) => new {
                        GameID = game.game_id,
                        Game = game.game_name,
                        Genre = genre.genre_name,
                        Dev = game.dev
                    })
                .Join(
                    ctx.game_price,
                    game => game.GameID,
                    gp => gp.game_id,
                    (game, gp) => new {
                        Game = game.Game,
                        Genre = game.Genre,
                        Dev = game.Dev,
                        Price = gp.price
                    })
                .Where(c => c.Genre == comboBox1.SelectedItem.ToString());//только те кто по жанру подходят
            switch (comboBox2.SelectedItem){
                case "По цене(возр)":
                    dataGridView2.DataSource = res.OrderBy(c => c.Price).Take(number).ToList();
                    break;
                case "По цене(убыв)":
                    dataGridView2.DataSource = res.OrderByDescending(c => c.Price).Take(number).ToList();//обратная сорт
                    break;
                case "По алфавиту":
                    dataGridView2.DataSource = res.OrderBy(c => c.Game).Take(number).ToList();
                    break;
                default:
                    dataGridView2.DataSource = res.Take(number).ToList();
                    break;
            }
            Wrong(true);//сообщяем что все выполнено
        }
    }
}

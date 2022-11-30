using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lab_7_Comp.gr.Var._5_
{
    public partial class Form1 : Form
    {
        // Битовая картинка pictureBox
        Bitmap pictureBoxBitMap;

        // Битовая картинка динамического изображения
        Bitmap spriteBitMap;
        
        // Битовая картинка для временного хранения области экрана
        Bitmap cloneBitMap;
        
        // Графический контекст picturebox
        Graphics g_pictureBox;
        
        // Графический контекст спрайта
        Graphics g_sprite;
        
        int x, y, y2; // Координаты ракеты
        int width =335, height = 367; // Ширина и высота ракеты
        int count = 0; // Счетчик (ракета запущена или нет)
        
        Timer timer = new Timer();
        
        public Form1()
        {
            InitializeComponent();
        }

        // Функция рисования спрайта (ракеты)
        void DrawSprite()
        {

            SolidBrush myNos = new SolidBrush(Color.AliceBlue);
            // Задаем серебряный цвет для топливных баков

            SolidBrush myBak = new SolidBrush(Color.Silver);
            
            // Задаем морской и темно-синий цвет для корпуса ракеты
            SolidBrush myShip = new SolidBrush(Color.Aqua);
            SolidBrush myLine = new SolidBrush(Color.DarkBlue);

            // Задаем желтый и оранжевый цвет для пламени
            SolidBrush myFire1 = new SolidBrush(Color.Yellow);
            SolidBrush myFire2 = new SolidBrush(Color.Orange);
            
            // Рисуем два прямоугольника
            g_sprite.FillRectangle(myBak, 179, 115, 26, 175);
            g_sprite.FillRectangle(myBak, 275, 115, 26, 175);
            
            // Сверху каждого прямоугольника рисуем по треугольнику
            g_sprite.FillPolygon(myBak, new Point[] {
                new Point(179,115),new Point(192,100),
                new Point(192,100),new Point(205,115),
                new Point(205,115),new Point(179,115)   });

            g_sprite.FillPolygon(myBak, new Point[] {
                new Point(275,115),new Point(287,100),
                new Point(287,100),new Point(301,115),
                new Point(301,115),new Point(275,115)   });

            // ************* 2 - Рисуем нос ракеты ***************

            g_sprite.FillPolygon(myNos, new Point[] {
                new Point(205,90),new Point(240,60),
                new Point(240,60),new Point(275,90),
                new Point(275,90),new Point(275,290),
                new Point(275,290),new Point(205,290),
                new Point(205,290),new Point(205,90)    });

            // ******** 3 - Рисуем нижнюю часть ракеты ************
            
            g_sprite.FillPolygon(myLine, new Point[] {
                new Point(130,300),new Point(240,260),
                new Point(240,260),new Point(345,300),
                new Point(345,300),new Point(130,300)   });

            // ******** 4 - Рисуем часть ракеты ниже носа **********
            
            g_sprite.FillPolygon(myLine, new Point[] {
                new Point(204,145),new Point(240,115),
                new Point(240,115),new Point(276,145),
                new Point(276,145),new Point(204,145)   });

            // ********** 5 - Рисуем корпус ракеты белым цветом *****
            g_sprite.FillRectangle(myShip, 204 + count, 145, 72, 155);
            
            // ******* 6 - Рисуем серую полосу на корпусе ракеты *****
            g_sprite.FillRectangle(myLine, 204, 185, 72, 50);
            
            // *********** 7 - Рисуем пламя из сопла ракеты *********
            
            // Запущена ли ракета
            if (count > 0)
            {
                GraphicsPath myGraphicsPath = new GraphicsPath();
            
                Pen p = new Pen(Brushes.Red, 1);
                
                // Задаем координаты точек первой кривой (внутреннее пламя)
                
                Point[] myPointArray1 = { 
                    new Point(210, 300),
                    new Point(210, 330), new Point(240, 360),
                    new Point(270, 330), new Point(270, 300)    };

                // Добавляем кривую в контейнер
                myGraphicsPath.AddCurve(myPointArray1);

                // Выводим внутренню часть пламени, закрашенную желтым цветом
                g_sprite.FillPath(myFire1, myGraphicsPath);
                
                // Выводим закрашенную оранжевым цветом область
                g_sprite.FillPath(myFire2, myGraphicsPath);
                
                // Рисуем границы кривых
                g_sprite.DrawPath(p, myGraphicsPath);

            }
        }

        // Функция сохранения части изображения шириной
        void SavePart(int xt, int yt)
        {
            Rectangle cloneRect = new Rectangle(xt, yt, width, height);
            System.Drawing.Imaging.PixelFormat format =

            pictureBoxBitMap.PixelFormat;
            
            // Клонируем изображение, заданное прямоугольной областью
            cloneBitMap = pictureBoxBitMap.Clone(cloneRect, format);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Вызов метода отрисовки ракеты
            DrawRocket();

            // Создаём таймер с интервалом 100 миллисекунд
            timer = new Timer{ Interval = 100 };
            timer.Tick += new EventHandler(Timer1_Tick);
        }

        private void DrawRocket()
        {
            // Задаем задний фон
            pictureBox.Image = Image.FromFile(@"bg1.jpg");
            pictureBoxBitMap = new Bitmap(pictureBox.Image);
            g_pictureBox = Graphics.FromImage(pictureBox.Image);
            
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            
            // Рисуем ракету, солнце и платформу
            SolidBrush grow = new SolidBrush(Color.LightGoldenrodYellow);
            SolidBrush sun = new SolidBrush(Color.Yellow);
            DrawSprite();

            g_pictureBox.FillEllipse(sun, 20, 20, 40, 40);
            
            g_pictureBox.FillRectangle(grow, 0, pictureBox.Height - 40, pictureBox.Width+1, pictureBox.Height);
            g_pictureBox.FillRectangle(new SolidBrush(Color.Gray), pictureBox.Width / 2 + 102, 170, 30, 207);
            g_pictureBox.FillRectangle(new SolidBrush(Color.Gray), pictureBox.Width / 2 + 77, 220, 50, 20);

            // Создаём Bitmap для временного хранения части изображения
            cloneBitMap = new Bitmap(width, height);
            
            // Задаем начальные координаты вывода движущегося объекта
            x = pictureBox.Width / 2 - 150; y = 100; y2 = 100;
            
            // Сохраняем область экрана перед первым выводом объекта
            SavePart(x, y);
            
            // Выводим ракету на графический контекст g_pictureBox
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            
            // Перерисовываем pictureBox1
            pictureBox.Invalidate();
        }

        // Обрабатываем событие от таймера
        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            count++;
            // Если ракета запущена перерысовываем Bitmap уже с пламенем (там стоит проверка на count)
            if(count == 1)
                DrawRocket();

            // Восстанавливаем затёртую область статического изображения
            g_pictureBox.DrawImage(cloneBitMap, x, y);
            
            // Изменяем координаты для следующего вывода ракеты
            y -= 6;
            
            // Проверяем на выход изображения автобуса за правую границу
            if (y < 0) {
                y = y2;
                count = 1;
            }

            // Сохраняем область экрана перед первым выводом ракеты
            SavePart(x, y);
            
            // Выводим ракету
            g_pictureBox.DrawImage(spriteBitMap, x, y);

            // Перерисовываем pictureBox
            pictureBox.Invalidate();
        }

        // Включаем таймер по нажатию на кнопку
        private void ButtonStart_Click(object sender, EventArgs e) { timer.Enabled = true; }
        
    }
}
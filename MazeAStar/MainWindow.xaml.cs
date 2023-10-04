using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MazeAStar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Nodo puntoFinalAnterior;
        Nodo puntoInicialSegundaVuelta;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Nodo.Tablero = new bool[9, 9];
            //Nodo.Tablero[1, 5] = true;
            //Nodo.Tablero[2, 4] = true;
            //Nodo inicial = new Nodo
            //{
            //    Col = 1,
            //    Ren = 0,
            //    G = 0
            //};
            //Nodo final = new Nodo { Col = 8, Ren = 7 };
            //AStar aStar = new AStar();
            //var solucion = aStar.Buscar(inicial, final);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        Rectangle[,] cuadritos;
        Nodo inicial;
        Nodo final;
        ImageBrush imgBrush = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri(@"apple.png", UriKind.Relative))
        };
        ImageBrush snakeHead = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri(@"snake.png", UriKind.Relative))
        };
        private async void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                puntoFinalAnterior = final;

                if (puntoInicialSegundaVuelta == null)
                {
                    await quesemueva();
                    puntoInicialSegundaVuelta = final;
                }
                else
                {
                    final = puntoInicialSegundaVuelta; 
                    await quesemueva();
                }

                await Task.Delay(200); 
            }

        }
        private async Task quesemueva()
        {
            int filas = 15;
            int columnas = 15;
            int obstaculos = 0;
            int velocidadMovimiento = 500; 

            tablero.Children.Clear();
            tablero.Rows = filas;
            tablero.Columns = columnas;
            cuadritos = new Rectangle[columnas, filas];
            Nodo.Tablero = new bool[columnas, filas];

            for (int i = 0; i < columnas; i++)
            {
                for (int j = 0; j < filas; j++)
                {
                    cuadritos[i, j] = new Rectangle()
                    {
                        Stroke = Brushes.Black
                    };
                    tablero.Children.Add(cuadritos[i, j]);
                }
            }

            Random r = new Random();
            for (int i = 0; i < obstaculos; i++)
            {
                int fila = r.Next(filas);
                int columna = r.Next(columnas);
                cuadritos[columna, fila].Fill = Brushes.DarkSlateGray;
                Nodo.Tablero[columna, fila] = true;
            }

            if (puntoFinalAnterior == null)
            {
                inicial = new Nodo();
                do
                {
                    int fila = r.Next(filas);
                    int columna = r.Next(columnas);
                    inicial.Col = columna;
                    inicial.Ren = fila;
                } while (Nodo.Tablero[inicial.Col, inicial.Ren]);
            }
            else
            {
                inicial = puntoFinalAnterior; 
            }

            final = new Nodo();
            do
            {
                int fila = r.Next(filas);
                int columna = r.Next(columnas);
                final.Col = columna;
                final.Ren = fila;
            } while (Nodo.Tablero[inicial.Col, inicial.Ren]);
            cuadritos[final.Col, final.Ren].Fill = imgBrush;

            cuadritos[inicial.Col, inicial.Ren].Fill = snakeHead;
            AStar astar = new AStar();

            foreach (var movimiento in astar.Buscar(inicial, final))
            {
                await Task.Delay(velocidadMovimiento);
                cuadritos[inicial.Col, inicial.Ren].Fill = null;
                inicial = movimiento;
                cuadritos[inicial.Col, inicial.Ren].Fill = snakeHead;
            }
        }

         
    }
}

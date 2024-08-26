using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Snake
    {
        // Variable para guardar el tamaño del tablero (por ejemplo, de un tablero 9x9 se guardará 9)
        static int length;
        // Array de caracteres donde se guardará el rastro, la posición actual o el simbolo (Ø) de colisión con el rastro
        static char[,] tablero;
        // En esta varaible guardaremos las coordenadas en la matriz de la posición actual 
        static int [] coordsCasillaActual = new int[2];
        // Variable para indicar si el juego ha terminado o no
        static bool finJuego = false;
        // Variable para guardar las coordenadas de las posiciones de la serpiente
        static List<int[]> snakePositions = new List<int[]>();

        enum eTipoPuntoEnergia
        {
            comida,
            muerte
        }

        enum eSimbolo
        {
            rastro = 'X',
            cuerpo = 'O',
            haColisionado = 'Ø',
            comida = '■',
            muerte = '×'
        }

        static void Main(string[] args)
        {
            SnakeGame();
        }

        static void SnakeGame()
        {
            int opcion = SelectLevel();
            
            switch (opcion)
            {
                case 1:
                    Level1();
                    break;

                case 2:
                    Level2();
                    break;
            }
        }

        static int SelectLevel()
        {
            while (true)
            {
                Console.WriteLine("Selecciona un nivel (1 o 2):");
                string strOpcion = Console.ReadLine();
                int opcion;

                bool correctLevel = int.TryParse(strOpcion, out opcion);

                if (correctLevel)
                {
                    if (opcion == 1 || opcion == 2)
                        return opcion;
                    else
                        MostrarMensajeError("Opción incorrecta");
                }
                else
                    MostrarMensajeError("Opción incorrecta");
            }
        }

        static void MostrarMensajeError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);

            Console.ResetColor();
        }

        static void Level1()
        {
            // Asignamos el tamaño del tablero seleccionado
            length = SelectLength();

            Console.WriteLine("Bienvenido al juego de la serpiente, el tablero es de " + length + "x" + length);

            // Creamos el tablero con el tamaño escogido
            tablero = new char[length, length];

            InicializarMatrizNivel1();

            // Bucle principal del juego
            while (!finJuego)
            {
                DibujarTablero();

                Console.WriteLine("Pulsa una de las flechas para mover la serpiente");

                EsperarTeclaCorrectaNivel1();

                Console.WriteLine();
            }

            // Volvemos a mostrar el tablero por última vez
            DibujarTablero();

            Console.WriteLine("Pulsa una tecla para salir");
            Console.ReadKey();
        }

        static int SelectLength()
        {
            while (true)
            {
                Console.WriteLine("Selecciona el tamaño del tablero: ");
                string strLengthBoard = Console.ReadLine();
                int lengthBoard;

                bool correctLength = int.TryParse(strLengthBoard, out lengthBoard);

                if (correctLength)
                {
                    if (lengthBoard > 1)
                        return lengthBoard;
                    else
                        Console.WriteLine("Tamaño incorrecto");
                }
                else
                    Console.WriteLine("Este valor no es númerico");
            }
        }

        static void InicializarMatrizNivel1()
        {
            int contadorMitad = 0;
            int mitad;

            // Dependiendo de si el tamaño es par o impar, la mitad será una o otra
            if (length % 2 == 0 && length > 3)
                // Ya que la mitad en los tamaños pares no existe, lo colocaremos mas o menos en una posición centrada 
                mitad = ((length * length) / 2) + ((length / 2) - 1);
            else
                mitad = (length * length) / 2;

            // Lo inicializamos, ponemos espacios en cada posicion de la matriz
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (contadorMitad == mitad)
                    {
                        tablero[i, j] = (char) eSimbolo.cuerpo;
                        // Asignamos la casilla inicial
                        coordsCasillaActual[0] = i;
                        coordsCasillaActual[1] = j;
                    }
                    else
                        tablero[i, j] = ' ';

                    contadorMitad++;
                }
            }
        }

        static void EsperarTeclaCorrectaNivel1()
        {
            bool teclaCorrecta = false;

            // Hasta que no pulse una de las flechas no se continua
            while (!teclaCorrecta)
            {
                // Leeemos la tecla pulsada
                ConsoleKeyInfo result = Console.ReadKey();

                switch (result.Key)
                {
                    case ConsoleKey.UpArrow:
                        DesplazarseArriba();
                        teclaCorrecta = true;
                        break;

                    case ConsoleKey.DownArrow:
                        DesplazarseAbajo();
                        teclaCorrecta = true;
                        break;

                    case ConsoleKey.LeftArrow:
                        DesplazarseIzquierda();
                        teclaCorrecta = true;
                        break;

                    case ConsoleKey.RightArrow:
                        DesplazarseDerecha();
                        teclaCorrecta = true;
                        break;

                    default:
                        Console.Write("\b \b");
                        break;
                }
            }
        }

        static void DibujarTablero()
        {
            int numCasilla = 0;

            // Bucle para las filas, el tamaño se debe multiplicar por 2 y sumarle uno para que las líneas horizontales lleguen al final del cuadrado
            for (int i = 0; i < (length * 2) + 1; i++)
            {
                // Bucle para las columnas
                for (int j = 0; j < length; j++)
                {
                    // Si la fila es par dibujamos líneas horizontales, si no, dibujaremos líneas verticales o lo que haya en la casilla
                    if (i % 2 == 0)
                    {
                        Console.Write("────");
                        
                        // Si es el último caracter de la línea le añadimos otra raya para que el cuadrado quede bien 
                        if (j == length - 1)
                            Console.Write("─");
                    }
                    else
                    {
                        Console.Write("│");

                        PintarCasilla(numCasilla);

                        // Si es la última casilla dibujamos una línea vertical para cerrar el cuadrado
                        if (j == length - 1)
                            Console.Write("│");

                        numCasilla++;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void PintarCasilla(int numCasilla)
        {
            // Obtenemos las coordenadas para la matriz del número de la casilla
            int[] coords = IndexToMatrixCoords(numCasilla);
            char caracter = tablero[coords[0], coords[1]];

            switch (caracter)
            {
                // Simboliza el rastro
                case (char) eSimbolo.rastro:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;

                // Simboliza la posición actual y el cuerpo de la serpiente
                case (char) eSimbolo.cuerpo:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;

                // Simboliza que ha colisionado con el rastro/cuerpo o contra una pared y ha muerto
                case (char) eSimbolo.haColisionado:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;

                // Simboliza la comida
                case (char) eSimbolo.comida:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                // Simboliza el punto de muerte
                case (char) eSimbolo.muerte:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.Write(" " + caracter + " ");
            Console.ResetColor();
        }

        // Método para convertir el número de la casilla en las coordenadas de la casilla en la matriz, devuelve las coordenadas de la casilla en la matriz
        static int[] IndexToMatrixCoords(int index)
        {
            int rowInMatrix = index / length;
            int columnInMatrix = index % length;

            return new int[] { rowInMatrix, columnInMatrix };
        }

        static void DesplazarseArriba()
        {
            // Condición para ver si podemos desplazarnos hacia arriba
            if (coordsCasillaActual[0] > 0)
            {
                // Asignamos al tablero el caracter 'X' en la posición actual, 'X' simboliza el rastro
                tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.rastro;

                // Cambiamos la coordenada
                coordsCasillaActual[0]--;

                if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char) eSimbolo.rastro)
                    FinalJuego();
                else
                {
                    // Cambiamos el caracter de la nueva posición actual
                    tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.cuerpo;
                }
            }
            else
                MostrarMensajeError("\nNo puedes moverte hacia arriba");
        }

        static void DesplazarseAbajo()
        {
            // Condición para ver si podemos desplazarse hacia abajo
            if (coordsCasillaActual[0] < (length - 1))
            {
                tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.rastro;

                coordsCasillaActual[0]++;

                if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char) eSimbolo.rastro)
                    FinalJuego();
                else
                    tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.cuerpo;
            }
            else
                MostrarMensajeError("\nNo puedes moverte hacia abajo");
        }

        static void DesplazarseIzquierda()
        {
            // Condición para ver si podemos movernos hacia la izquierda
            if (coordsCasillaActual[1] > 0)
            {
                tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.rastro;

                coordsCasillaActual[1]--;

                if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char) eSimbolo.rastro)
                    FinalJuego();
                else
                    tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.cuerpo;
            }
            else
                MostrarMensajeError("\nNo puedes moverte hacia la izquierda");
        }

        static void DesplazarseDerecha()
        {
            // Condición para poder movernos hacia la derecha
            if (coordsCasillaActual[1] < (length - 1))
            {
                tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.rastro;

                coordsCasillaActual[1]++;

                if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char) eSimbolo.rastro)
                    FinalJuego();
                else
                    tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.cuerpo;
            }
            else
                MostrarMensajeError("\nNo puedes moverte hacia la derecha");
        }

        static void Level2()
        {
            length = SelectLength();

            Console.WriteLine("Bienvenido al nivel 2 del juego de la serpiente, el tablero es de " + length + "x" + length);

            // Creamos el tablero con el tamaño escogido
            tablero = new char[length, length];

            InicializarMatrizNivel2();

            GenerarPuntoEnergia(eTipoPuntoEnergia.comida);

            // Si el tamaño del tablero es mayor que 2x2 generaremos tantos puntos de muerte como el tamaño seleccionado
            if (length > 2)
            {
                for (int i = 0; i < length; i++)
                    GenerarPuntoEnergia(eTipoPuntoEnergia.muerte);
            }
            else
                GenerarPuntoEnergia(eTipoPuntoEnergia.muerte);

            // Bucle principal del juego
            while (!finJuego)
            {
                DibujarTablero();

                Console.WriteLine("Pulsa una de las flechas para mover la serpiente");

                EsperarTeclaCorrectaNivel2();

                Console.WriteLine();
                Console.WriteLine();
            }

            // Volvemos a mostrar el tablero por última vez
            DibujarTablero();

            Console.WriteLine("Pulsa una tecla para salir");
            Console.ReadKey();
        }

        static void InicializarMatrizNivel2()
        {
            int contadorMitad = 0;
            int mitad;

            // Dependiendo de si el tamaño es par o impar, la mitad será una o otra
            if (length % 2 == 0 && length > 3)
                // Ya que la mitad en los tamaños pares no existe, lo colocaremos mas o menos en una posición centrada 
                mitad = ((length * length) / 2) + ((length / 2) - 1);
            else
                mitad = (length * length) / 2;

            // Lo inicializamos, ponemos espacios en cada posicion de la matriz
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (contadorMitad == mitad)
                    {
                        tablero[i, j] = (char) eSimbolo.cuerpo;
                        // Asignamos la casilla inicial
                        coordsCasillaActual[0] = i;
                        coordsCasillaActual[1] = j;

                        // Lo añadimos a la lista de la serpiente
                        snakePositions.Add(new int[] { i, j });
                    }
                    else
                        tablero[i, j] = ' ';

                    contadorMitad++;
                }
            }
        }

        static void GenerarPuntoEnergia(eTipoPuntoEnergia tipo)
        {
            Random rnd = new Random();

            // Generamos un número aleatorio para seleccionar una casilla
            int randomNumberX = rnd.Next(length);
            int randomNumberY = rnd.Next(length);

            // Comprobamos si las coordenadas generadas son iguales a las de algún elemento del juego
            while ((randomNumberX == coordsCasillaActual[0] && randomNumberY == coordsCasillaActual[1])
                   || (tablero[randomNumberX, randomNumberY] == (char) eSimbolo.comida)
                   || (tablero[randomNumberX, randomNumberY] == (char) eSimbolo.muerte)
                   || (tablero[randomNumberX, randomNumberY] == (char) eSimbolo.cuerpo))
            {
                randomNumberX = rnd.Next(length);
                randomNumberY = rnd.Next(length);
            }

            if (tipo == eTipoPuntoEnergia.comida)
                tablero[randomNumberX, randomNumberY] = (char) eSimbolo.comida;
            else if (tipo == eTipoPuntoEnergia.muerte)
                tablero[randomNumberX, randomNumberY] = (char) eSimbolo.muerte;
        }

        static void EsperarTeclaCorrectaNivel2()
        {
            bool teclaCorrecta = false;

            // Hasta que no pulse una de las flechas no se continua
            while (!teclaCorrecta)
            {
                // Leeemos la tecla pulsada
                ConsoleKeyInfo result = Console.ReadKey();

                switch (result.Key)
                {
                    case ConsoleKey.UpArrow:
                        DesplazarseArribaNivel2();
                        teclaCorrecta = true;
                        break;

                    case ConsoleKey.DownArrow:
                        DesplazarseAbajoNivel2();
                        teclaCorrecta = true;
                        break;

                    case ConsoleKey.LeftArrow:
                        DesplazarseIzquierdaNivel2();
                        teclaCorrecta = true;
                        break;

                    case ConsoleKey.RightArrow:
                        DesplazarseDerechaNivel2();
                        teclaCorrecta = true;
                        break;

                    default:
                        Console.Write("\b \b");
                        break;
                }
            }
        }

        static void DesplazarseArribaNivel2()
        {
            // Condición para ver si podemos desplazarnos hacia arriba
            if (coordsCasillaActual[0] > 0)
            {
                // Cambiamos la coordenada
                coordsCasillaActual[0]--;

                ComprobacionesJuegoNivel2();
            }
            else
            {
                // Final del juego, ha colisionado contra una pared
                FinalJuego();
            }
        }

        static void DesplazarseAbajoNivel2()
        {
            // Condición para ver si podemos desplazarse hacia abajo
            if (coordsCasillaActual[0] < (length - 1))
            {

                // Cambiamos la coordenada
                coordsCasillaActual[0]++;

                ComprobacionesJuegoNivel2();
            }
            else
                FinalJuego();
        }

        static void DesplazarseIzquierdaNivel2()
        {
            // Condición para ver si podemos movernos hacia la izquierda
            if (coordsCasillaActual[1] > 0)
            {
                // Cambiamos la coordenada
                coordsCasillaActual[1]--;

                ComprobacionesJuegoNivel2();
            }
            else
                FinalJuego();
        }

        static void DesplazarseDerechaNivel2() 
        {
            // Condición para poder movernos hacia la derecha
            if (coordsCasillaActual[1] < (length - 1))
            {
                // Cambiamos la coordenada
                coordsCasillaActual[1]++;

                ComprobacionesJuegoNivel2();
            }
            else
                FinalJuego();
        }

        static void ComprobacionesJuegoNivel2()
        {
            // Añadimos la cabeza de la serpiente
            snakePositions.Insert(0, new int[] { coordsCasillaActual[0], coordsCasillaActual[1] });

            // Si colisiona contra su cuerpo se termina el juego
            if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char) eSimbolo.cuerpo)
                FinalJuego();
            else
            {
                // Comprobamos si pasa por encima de la comida
                if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char)eSimbolo.comida)
                    GenerarPuntoEnergia(eTipoPuntoEnergia.comida);
                else
                    EliminarColaSerpiente();

                // Si colisiona contra un punto de muerte se termina el juego
                if (tablero[coordsCasillaActual[0], coordsCasillaActual[1]] == (char)eSimbolo.muerte)
                    FinalJuego();
                else
                {    
                    // Cambiamos el caracter de la nueva posición actual
                    tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.cuerpo;
                }
            }
        }

        static void EliminarColaSerpiente()
        {
            int[] cola = snakePositions[snakePositions.Count - 1];

            // Eliminamos la cola de la serpiente
            tablero[cola[0], cola[1]] = ' ';
            snakePositions.RemoveAt(snakePositions.Count - 1);
        }

        static void FinalJuego()
        {
            tablero[coordsCasillaActual[0], coordsCasillaActual[1]] = (char) eSimbolo.haColisionado;

            MostrarMensajeError("\nFIN DEL JUEGO");

            finJuego = true;
        }
    }
}
